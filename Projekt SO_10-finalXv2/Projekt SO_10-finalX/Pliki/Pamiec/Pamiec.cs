using System;
using System.Collections.Generic;
using System.Text;

    internal class Pamiec
    {
        public const int RozmiarBloku = StronaPamieci.Rozmiar;
        private readonly byte[] _pamiecFizyczna;
        // Ta lista przechowuje info o wszystkich do tej pory utworzonych stronach
        // nawet jeśl zostaly swapowane na dysk
        private readonly List<StronaPamieci> _strony;
        private readonly Queue<int> _stronyZmapowane;
        private readonly PlikStronnicowania _swap;
        private readonly bool[] _zajeteStronyFizyczne;
        private BlokPamieci _zajeteBlokiPamieci;

        public Pamiec(int rozmiar)
        {
            _pamiecFizyczna = new byte[rozmiar];
            _zajeteStronyFizyczne = new bool[rozmiar/RozmiarBloku];
            _strony = new List<StronaPamieci>();
            _stronyZmapowane = new Queue<int>();
            _swap = new PlikStronnicowania();
        }

        /// <summary>
        ///     Mapuje strone do pamieci fizycznej
        /// </summary>
        /// <param name="numerStrony"></param>
        /// <returns>Adres w pamieci fizycznej w ktorej strona zostala zmapowana</returns>
        private int ZmapujStrone(int numerStrony)
        {
            var adres = -1;
            var strona = _strony[numerStrony];
            // Znajdz wolna strone pamieci
            for (var i = 0; i < _zajeteStronyFizyczne.Length; i++)
            {
                if (_zajeteStronyFizyczne[i] == false)
                {
                    adres = i*RozmiarBloku;
                    _zajeteStronyFizyczne[i] = true;
                    break;
                }
            }

            // Jesli nie znaleziono wolnej strony, swapuj jedna na dysk zeby zwolnic miejsce
            if (adres == -1)
            {
                var nrStronyDoUsuniecia = _stronyZmapowane.Dequeue();
                _strony[nrStronyDoUsuniecia].JestZmapowanaWPamieciFizycznej = false;
                _strony[nrStronyDoUsuniecia].JestNaDysku = true;
                var daneDoZapisania = new byte[RozmiarBloku];
                Array.Copy(_pamiecFizyczna, _strony[nrStronyDoUsuniecia].AdresPoczatkowy, daneDoZapisania, 0,
                    RozmiarBloku);

                _swap.Zapisz((short) nrStronyDoUsuniecia, daneDoZapisania);
                adres = _strony[nrStronyDoUsuniecia].AdresPoczatkowy;
            }

            if (strona.JestNaDysku)
            {
                var daneZDysku = _swap.Odczytaj((short) numerStrony);

                daneZDysku.CopyTo(_pamiecFizyczna, adres);
            }
            else
            {
                // Strona istnieje, ale nigdy wczesniej nie byla uzywana.
                // Mapujemy ja w pamieci i zerujemy
                for (var i = 0; i < RozmiarBloku; i++)
                {
                    _pamiecFizyczna[adres + i] = 0;
                }
            }

            _stronyZmapowane.Enqueue(numerStrony);
            strona.JestZmapowanaWPamieciFizycznej = true;
            strona.AdresPoczatkowy = adres;
            return adres;
        }

        /// <summary>
        ///     Przelicza adres wirtualny na fizyczny.
        ///     Zwraca -1 jesli adres jest bledny.
        ///     Jesli zmapujStrone jest True, a adres nie jest aktualnie zmapowany,
        ///     to sprobuje go zmapowac (rzuca wyjatek jesli odpowiednia strona nie istnieje na dysku)
        /// </summary>
        /// <returns>
        ///     Indeks w tablicy pamiecFizyczna odpowiadajacy podanemu adresowi wirtualnemu
        ///     -1 jezeli podany adres jest bledny
        /// </returns>
        private int AdresWirtualnyNaFizyczny(int adresWirtualny)
        {
            // Adres wirtualny traktujemy jako rozmiarStrony * numerStrony + offset wewnatrz strony do bajtu w pamieci
            var numerStrony = adresWirtualny/StronaPamieci.Rozmiar;
            if (numerStrony >= _strony.Count)
            {
                // Strona nie istnieje
                return -1;
            }

            var strona = _strony[numerStrony];

            var offset = adresWirtualny%StronaPamieci.Rozmiar;
            if (strona.JestZmapowanaWPamieciFizycznej)
            {
                return strona.AdresPoczatkowy + offset;
            }
            return ZmapujStrone(numerStrony) + offset;
        }

        public byte Odczytaj(int adresWirtualny)
        {
            var adresFizyczny = AdresWirtualnyNaFizyczny(adresWirtualny);
            if (adresFizyczny == -1)
            {
                // TODO: blad, proba odczytu spod niezaalokowanego adresu
                return 0;
            }
            return _pamiecFizyczna[adresFizyczny];
        }

        public void Zapisz(int adresWirtualny, byte wartosc)
        {
            var adresFizyczny = AdresWirtualnyNaFizyczny(adresWirtualny);
            if (adresFizyczny == -1)
            {
                return;
            }
            _pamiecFizyczna[adresFizyczny] = wartosc;
        }

        /// <summary>
        ///     Alokuje pamiec metodą First-Fit.
        ///     Zaalokowane strony NIE SA natychmiast mapowane w pamieci fizycznej.
        /// </summary>
        /// <param name="iloscBlokow">
        ///     Ilość blokow do zaalokowania.
        ///     Kazdy blok ma rozmiar Pamiec.RozmiarBloku
        /// </param>
        /// <returns>
        ///     Adres poczatku zaalokowanego bloku w pamieci wirtualnej
        /// </returns>
        public int Zaalokuj(int iloscBlokow)
        {
            BlokPamieci nowyBlok;
            if (_zajeteBlokiPamieci == null)
            {
                nowyBlok = new BlokPamieci(0, iloscBlokow);
                _zajeteBlokiPamieci = nowyBlok;
            }
            else
            {
                var lookup = _zajeteBlokiPamieci;
                // szukamy pierwszej przerwy miedzy blokami zajetego miejsca, 
                // w ktora da sie zmiescic iloscBlokow

                while (lookup.Nastepny != null &&
                       lookup.Nastepny.StronaPoczatkowa - lookup.StronaPoczatkowa + lookup.Blokow > iloscBlokow)
                {
                    lookup = lookup.Nastepny;
                }

                // dodajemy nowy blok po znalezionym
                nowyBlok = new BlokPamieci(lookup.StronaPoczatkowa + lookup.Blokow, iloscBlokow)
                {
                    Nastepny = lookup.Nastepny
                };
                lookup.Nastepny = nowyBlok;
            }

            while (_strony.Count < nowyBlok.StronaPoczatkowa + nowyBlok.Blokow)
            {
                _strony.Add(new StronaPamieci());                
            }

            return nowyBlok.StronaPoczatkowa * RozmiarBloku;
        }

        public void Zwolnij(int adresPoczatkowy)
        {
            int numerStrony = adresPoczatkowy/RozmiarBloku;
            BlokPamieci doZwolnienia = null;
            BlokPamieci poprzedniBlok = null;   // Blok przed blokiem do zwolnienia

            // Szukamy strony zaczynajacej sie pod podanym adresem
            BlokPamieci lookup = _zajeteBlokiPamieci;
            while (lookup != null)
            {
                if (lookup.StronaPoczatkowa == numerStrony)
                {
                    doZwolnienia = lookup;
                    break;
                }
                poprzedniBlok = lookup;
                lookup = lookup.Nastepny;
            }

            if (doZwolnienia == null)
            {
                // Dostalismy zly adres, albo blok zostal juz zwolniony
                // nie robimy nic
                return;
            }

            if (poprzedniBlok != null) poprzedniBlok.Nastepny = doZwolnienia.Nastepny;
            else _zajeteBlokiPamieci = doZwolnienia.Nastepny;

            for (int i = 0; i < doZwolnienia.Blokow; i++)
            {
                StronaPamieci stronaDoZwolnienia = _strony[doZwolnienia.StronaPoczatkowa + i];
                stronaDoZwolnienia.JestZmapowanaWPamieciFizycznej = false;
                stronaDoZwolnienia.JestNaDysku = false;
                // Strona pozostanie w swap i w pamięci fizycznej, ale zostanie wyczyszczona i nadpisana po nastepnym zapisie do niej
            }
        }

        public void ListaZajetychBlokow()
        {
            Console.Write("Lista zajetych blokow:\n");
            BlokPamieci blok = _zajeteBlokiPamieci;
            while (blok != null)
            {
                Console.Write(blok);
                Console.Write("\n");
                blok = blok.Nastepny;
            }
        }

       public void StronyPamieci()
        {
            Console.WriteLine("Strony pamieci:\n");
            foreach (var strona in _strony)
            {
                Console.WriteLine(strona);
                Console.WriteLine("\n");
            }
        }

        public void PamiecFizyczna()
        {
            int komorkiNaLinie = 16;
            for (var row = 0; row < _pamiecFizyczna.Length / komorkiNaLinie; row++)
            {
                for (var col = 0; col < komorkiNaLinie; col++)
                {
                    Console.Write(string.Format("{0:000}", _pamiecFizyczna[row * komorkiNaLinie + col]));
                    Console.Write(" ");
                }
                Console.Write("\n");
            }
        }


        public override string ToString()
        {
            var s = new StringBuilder();
            s.Append("Lista zajetych blokow:\n");
            BlokPamieci blok = _zajeteBlokiPamieci;
            while (blok != null)
            {
                s.Append(blok);
                s.Append("\n");
                blok = blok.Nastepny;
            }

            s.Append("Strony pamieci:\n");
            foreach (var strona in _strony)
            {
                s.Append(strona);
                s.Append("\n");
            }

            s.Append("Pamiec fizyczna:\n");
            int komorkiNaLinie = 16;
            for (var row = 0; row < _pamiecFizyczna.Length / komorkiNaLinie; row++)
            {
                for (var col = 0; col < komorkiNaLinie; col++)
                {
                    s.Append(string.Format("{0:000}", _pamiecFizyczna[row * komorkiNaLinie + col]));
                    s.Append(" ");
                }
                s.Append("\n");
            }
            return s.ToString();
        }
    }