using System.Collections.Generic;

    class PlikStronnicowania
    {
        private struct StronaNaDysku
        {
            public short Numer;
            public byte[] Dane;
        }

        private readonly List<StronaNaDysku> _stronyNaDysku = new List<StronaNaDysku>();

        // TODO: dobrze by bylo zrobic lookup table dla tych blokow
        public byte[] Odczytaj(short numerStrony)
        {
            foreach (var strona in _stronyNaDysku)
            {
                if (strona.Numer != numerStrony) continue;
                var copy = new byte[Pamiec.RozmiarBloku];
                strona.Dane.CopyTo(copy, 0);
                return copy;
            }

            throw new System.Exception("Blad stronnicowania: Nie znaleziono strony na dysku");
        }

        public void Zapisz(short numerStrony, byte[] dane)
        {
            // Szukamy strony na dysku, jesli byla juz utworzona, to ja nadpisujemy
            foreach (var strona in _stronyNaDysku)
            {
                if (strona.Numer == numerStrony)
                {
                    dane.CopyTo(strona.Dane, 0);
                    return;
                }
            }

            // Ta strona nie byla jeszcze na dysku, tworzymy nowa
            var nowaStrona = new StronaNaDysku {Numer = numerStrony, Dane = new byte[Pamiec.RozmiarBloku]};
            dane.CopyTo(nowaStrona.Dane, 0);
            _stronyNaDysku.Add(nowaStrona);
        }
    }
