using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


class Uzytkownik
{
    public string login, haslo, slowo, gru;

    pom p;
    int o;

    string aktualnykatalog;
    ZarzadcaProcesow zarzadcapr;
    int ile;
    Pamiec pam;
    proces init;
    public string sciezka = "C:\\Users\\Samsung\\Desktop\\Semestr III\\logi.txt";



    public Uzytkownik(string log, string has, pom x, int ilo, string fff, ZarzadcaProcesow zzz, int il, Pamiec papa, proces ini)
    {
        o = ilo;
        login = log;
        haslo = has;
        p = x;
        slowo = fff;

        zarzadcapr = zzz;
        ile = il;
        pam = papa;
        init = ini;
        aktualnykatalog = log;
        string[] t = File.ReadAllLines(sciezka);
        int c = t.Length;

        for (int i = 2; i < c - 2; i = i + 3)
            if (t[i] == login)
                gru = t[i + 2];
    }

    public int exit()
    {
        return 10;
    }

    public int wylogowanie()
    {
        return 10;

    }
    public int polecenia()
    {
        int stop = 0, bre = 0;
        while (true)
        {
            Console.Write("Komenda:");
            string com;
            com = Console.ReadLine();
            string[] comenda = com.Split(' ');
            switch (comenda[0])
            {
                case "logout":
                    stop = wylogowanie();
                    break;

                case "zmhaslo":

                    string[] t = File.ReadAllLines(sciezka);
                    int c = t.Length;

                    for (int i = 2; i < c - 2; i = i + 3)
                        if (t[i] == login)
                            t[i + 1] = comenda[1];
                    System.IO.File.WriteAllLines(sciezka, t);

                    break;

                case "zmienkat":
                    int a;

                    a = iwezel.czyistnieje(ref p, comenda[1]);
                    if (a == -1 || a == 0)
                        Console.WriteLine("nie istnieje taka nazwa");
                    else
                    {
                        if (p.tabi[a].Typ == 'p')
                            Console.WriteLine("to nie jest katalog");
                        else
                        {
                            if (p.tabi[a].IDuzytkownika == login)
                                aktualnykatalog = comenda[1];
                            else
                                if (p.tabi[a].IDgrupy == gru)
                                    if (p.tabi[a].prawa2 != 0)
                                        aktualnykatalog = comenda[1];
                                    else
                                        if (p.tabi[a].prawa3 != 0)
                                            aktualnykatalog = comenda[1];
                                        else
                                            Console.WriteLine("nie masz zadnych praw w tym katalogu");
                        }
                    }
                    break;
                case "open":
                    {
                        int ff = 1;
                        a = iwezel.czyistnieje(ref p, comenda[1]);
                        if (a == -1 || a == 0)
                            Console.WriteLine("nie istnieje taka nazwa");
                        else
                            if (p.tabi[a].IDuzytkownika == login)
                            {
                                iwezel.open(ref p, comenda[1], flaga.ronly, ref o, ref slowo);
                                ff = 0;
                            }
                        if (a != -1 && a != 0)
                            if (p.tabi[a].IDgrupy == gru && ff == 1)
                                if (p.tabi[a].prawa2 != 0)
                                {
                                    iwezel.open(ref p, comenda[1], flaga.ronly, ref o, ref slowo);

                                    ff = 0;
                                }
                        if (a != -1 && a != 0)
                            if (p.tabi[a].prawa3 != 0 && ff == 1)
                            {
                                iwezel.open(ref p, comenda[1], flaga.ronly, ref o, ref slowo);

                                ff = 0;
                            }
                        if (ff == 1)
                            Console.WriteLine("nie masz zadnych praw w tym pliku");



                        if (ff == 0 && slowo != null)
                            Console.WriteLine(slowo);
                    }

                    break;
                case "pisz":
                    {
                        int ff = 1;
                        a = iwezel.czyistnieje(ref p, comenda[1]);
                        if (a == -1 || a == 0)
                            Console.WriteLine("nie istnieje taka nazwa");
                        else
                            if (p.tabi[a].IDuzytkownika == login)
                            {
                                iwezel.open(ref p, comenda[1], flaga.wonly, ref o, ref slowo);
                                ff = 0;
                            }
                        if (a != -1 && a != 0)
                            if (p.tabi[a].IDgrupy == gru && ff == 1)
                                if (p.tabi[a].prawa2 == 2 || p.tabi[a].prawa2 == 3 || p.tabi[a].prawa2 == 6 || p.tabi[a].prawa2 == 7)
                                {
                                    iwezel.open(ref p, comenda[1], flaga.wonly, ref o, ref slowo);

                                    ff = 0;
                                }
                        if (a != -1 && a != 0)
                            if (p.tabi[a].prawa3 == 2 || p.tabi[a].prawa3 == 3 || p.tabi[a].prawa3 == 6 || p.tabi[a].prawa3 == 7)
                            {
                                iwezel.open(ref p, comenda[1], flaga.wonly, ref o, ref slowo);

                                ff = 0;
                            }
                        if (ff == 1)
                            Console.WriteLine("nie mozesz dopisywać");


                        break;
                    }
                case "exit":
                    bre = exit();
                    break;
                case "creatk":

                    iwezel.create(ref p, comenda[1], 'k', aktualnykatalog);
                    a = iwezel.czyistnieje(ref p, comenda[1]);
                    p.tabi[a].IDuzytkownika = login;
                    p.tabi[a].IDgrupy = gru;
                    break;
                case "creatp":

                    iwezel.create(ref p, comenda[1], 'p', aktualnykatalog);
                    iwezel.open(ref p, comenda[1], flaga.wonly, ref o, ref slowo);
                    a = iwezel.czyistnieje(ref p, comenda[1]);
                    p.tabi[a].IDuzytkownika = login;
                    p.tabi[a].IDgrupy = gru;
                    break;
                case "delete":

                    a = iwezel.czyistnieje(ref p, comenda[1]);
                    if (a == -1)
                        Console.WriteLine("nie istnieje taki plik");
                    else
                        if (p.tabi[a].IDuzytkownika == login)
                            iwezel.delete(ref p, comenda[1], ref o);
                        else
                            Console.WriteLine("Nie mozesz tego usunac");
                    break;
                case "zmprawa":
                    try
                    {
                        uint b = Convert.ToUInt32(comenda[2]);
                        uint d = Convert.ToUInt32(comenda[3]);
                        uint e = Convert.ToUInt32(comenda[3]);
                        byte f = Convert.ToByte(b);
                        byte g = Convert.ToByte(d);
                        byte h = Convert.ToByte(f);
                        if (f > 7 || g > 7 || h > 7)
                            Console.WriteLine("niepoprawna wartosc praw");

                        a = iwezel.czyistnieje(ref p, comenda[1]);
                        if (a == -1)
                            Console.WriteLine("nie istnieje taki plik");
                        else
                            if (p.tabi[a].IDuzytkownika == login)
                            {
                                iwezel.chacces(ref p, comenda[1], f, g, h);
                            }




                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        Console.WriteLine("niepoprawne dane");
                    }
                    break;

                case "Blokipam":
                    {
                        pam.ListaZajetychBlokow();
                        break;
                    }

                case "Stronypam":
                    {
                        pam.StronyPamieci();
                        break;
                    }

                case "Pamiecfiz":
                    {
                        pam.PamiecFizyczna();
                        break;
                    }

                case "Pamiec":
                    {
                        Console.WriteLine(pam);
                        break;
                    }
                case "pliki?":
                    {
                        Console.WriteLine();
                        Console.WriteLine("5. Tablica i-wezłów");
                        Console.WriteLine("6. Jakie bloki zajmuje plik");
                        Console.WriteLine("7. Który blok wyświetlić dla pliku");
                        Console.WriteLine("8. Wyświetlenie zawartości wszystkich katalogów");
                        Console.WriteLine("9. Jakie bloki na dysku są wolne, zajęte");
                        Console.WriteLine("10. Wyświetlenie całego dysku lub wybranego bloku");
                        Console.WriteLine();
                        break;
                    }

                case "5":
                    {
                        Console.WriteLine("\nTablica i-węzłów");
                        iwezel.Wyswtabi(ref p);
                        break;
                    }

                case "6":
                    {
                        Console.WriteLine("6. Jakie bloki zajmuje plik");
                        Console.WriteLine("\nPodaj nazwę plku");
                        string nazwa;
                        nazwa = Console.ReadLine();
                        iwezel.wyswzajblok(ref p, nazwa);

                        break;
                    }

                case "7":
                    {
                        Console.WriteLine("7. Który blok wyświetlić dla pliku");
                        int v = 0;
                        Console.WriteLine("\nPodaj nazwę plku");
                        string nazwa, nazwa2;
                        nazwa = Console.ReadLine();
                        int numer = -1;
                        Console.WriteLine("\nPodaj nr bloku, który chcesz wyświetlić");
                        do
                        {
                            if (v > 0)
                                Console.WriteLine("\nPodaj nr bloku jeszcze raz");

                            try
                            {
                                nazwa2 = Console.ReadLine();
                                numer = Int32.Parse(nazwa2);
                            }
                            catch (FormatException ex)
                            {
                                Console.WriteLine("Podaj liczbę!");
                            }

                            v++;
                        } while (numer < 0 || numer > 6);
                        blok.Wyswietblok(ref p, numer, nazwa);

                        break;
                    }

                case "8":
                    {
                        Console.WriteLine("8. Wyświetlenie zawartości wszystkich katalogów");
                        WpisK.wyswk2(ref p);

                        break;
                    }

                case "9":
                    {
                        string nazwa;
                        int v = 0;
                        Console.WriteLine("9. Jakie bloki na dysku są wolne, zajęte");
                        Console.WriteLine("\nKtóre wyświetlić?  Wolne(w) czy Zajęte(z)?");
                        do
                        {
                            if (v > 0)
                                Console.WriteLine("nie ma takiej akcji proszę podać jeszcze raz 'w' lub 'z'");

                            nazwa = Console.ReadLine();

                            if (nazwa == "w" || nazwa == "W")
                                blok.wolnebloki(ref p, ref o);


                            if (nazwa == "z" || nazwa == "Z")
                                blok.zajetebloki(ref p, ref o);
                            v++;

                        } while (nazwa != "w" && nazwa != "W" && nazwa != "z" && nazwa != "Z");

                        break;
                    }

                case "10":
                    {
                        string nazwa, nazwa2;
                        int v = 0, v1 = 0;
                        int numer = -1;
                        Console.WriteLine("10. Wyświetlenie całego dysku lub wybranego bloku");
                        Console.WriteLine("\nCo wyświetlić?  Blok(b) czy Dysk(d)?");
                        do
                        {
                            if (v > 0)
                                Console.WriteLine("nie ma takiej akcji proszę podać jeszcze raz 'b' lub 'd'");

                            nazwa = Console.ReadLine();

                            if (nazwa == "b" || nazwa == "B")
                            {

                                Console.WriteLine("Proszę podać nr bloku");
                                do
                                {
                                    if (v1 > 0)
                                        Console.WriteLine("Nie ma takiego bloku. Podaj jeszcze raz");

                                    try
                                    {
                                        nazwa2 = Console.ReadLine();
                                        numer = Int32.Parse(nazwa2);
                                    }
                                    catch (FormatException ex)
                                    {
                                        Console.WriteLine("Podaj liczbę!");
                                    }

                                    if (numer >= 0 && numer < 64)
                                        blok.Wyswietbloklubdysk(ref p, numer, nazwa);

                                    v1++;
                                } while (numer < 0 || numer > 63);
                            }


                            if (nazwa == "d" || nazwa == "D")
                                blok.Wyswietbloklubdysk(ref p, 0, nazwa);


                            v++;

                        } while (nazwa != "d" && nazwa != "D" && nazwa != "b" && nazwa != "B");

                        break;
                    }
                case "procesor":
                    {
                        ////////////////////////////////////////////////////////////////////////////////////////////////
                        Symulacja_procesora proc = new Symulacja_procesora(1, 4, 15);
                        Interpreter it = new Interpreter();

                        proc.main.dodajnowy(zarzadcapr.kolejka.wyslijproces()); //-> dodaje 1 proces na starcie systemu [wklejam funkcje od Tadka]

                        Console.WriteLine("\nStart Symulacji procesora!");

                        proc.main.info();
                        proces tmpp = proc.main.znajdz_zwroc_usun();
                        byte[] tabpam = new byte[4];
                        int z = 0;
                        while (true)
                        {
                            for (int i = 0; i < 4; i++)
                                tabpam[i] = pam.Odczytaj(tmpp.heapAddrStart + i + (tmpp.l_rozk * 4));

                            uint tmpppp = BitConverter.ToUInt32(tabpam, 0); // test -> jaki rozkaz

                            proc.blokada = it.wykonajInstrukcje(tmpp, tabpam); // -> wynonanie 1 instrukcji z podanego procesu [wklejam funkcje od Iwony]
                            proc.main.wypiszStanRejestruProcesu(tmpp);


                            Console.In.Read();
                            Console.In.Read();
                            if (proc.blokada)       // zakonczenie procesu
                            {
                                pam.Zwolnij(tmpp.heapAddrStart);

                                tmpp = proc.main.znajdz_zwroc_usun();

                                pam.ListaZajetychBlokow();
                                proc.blokada = false;
                            }

                            if (tmpp == null)       // koniec wykonywania wszystkich procesow
                            {
                                Console.Out.WriteLine("Ilosc taktow procesora: " + proc.takt);
                                Console.Out.WriteLine("Wszystkie procesy zostaly wykonane!");
                                break;
                            }

                            if (proc.takt % proc.rekalk == 0 && tmpp != null) // obliczenie priorytetu dla wszystkich procesow po 15 cyklach
                            {
                                proc.main.zmniejsziprzestaw();                                         // oblicza prio dla pozostalych procesow
                                tmpp.priorytet[2] = tmpp.priorytet[2] / 2;                          // oblicza prio dla 1 przetrymywanego w tmpp procesu
                                tmpp.priorytet[0] = tmpp.priorytet[1] + tmpp.priorytet[2];          //
                            }

                            if (tmpp.l_rozk % proc.wywlaszcz == 0 && tmpp != null) // wywlaszczenie pocesu po 4 cyklach proca    //EDIT
                            {
                                proc.main.przestaw(tmpp);

                                proc.main.info();
                                tmpp = proc.main.znajdz_zwroc_usun();
                            }
                            if (proc.takt % 1 == 0) // przychodzi proces
                            {
                                proces pr = zarzadcapr.kolejka.wyslijproces();
                                proces np = null;
                                if (pr != null)
                                    np = proc.main.dodajnowy(pr); // -> dodaje kolejne procesy do wykonania  [funkcja od Tadka]

                                if (proc.main.coswpadlo == true)
                                {
                                    if (np.priorytet[0] < tmpp.priorytet[0])
                                    {
                                        proc.main.przestaw(tmpp);
                                        proc.main.info();
                                        tmpp = proc.main.znajdz_zwroc_usun();
                                    }
                                    else { }
                                    proc.main.coswpadlo = false;
                                }
                            }


                            proc.takt++;
                        }
                        ile = 0;

                        break;
                    }



                case "proces":
                    {
                        int q = 0;
                        a = iwezel.czyistnieje(ref p, comenda[1]);
                        if (a == -1 || a == 0)
                            Console.WriteLine("nie istnieje taka nazwa");
                        else
                            if (p.tabi[a].IDuzytkownika == login)
                            {

                                q = 1;
                            }
                        if (a != -1 && a != 0)
                            if (p.tabi[a].IDgrupy == gru && q == 1)
                                if (p.tabi[a].prawa2 == 4 || p.tabi[a].prawa2 == 5 || p.tabi[a].prawa2 == 6 || p.tabi[a].prawa2 == 7)
                                {


                                    q = 1;
                                }
                        if (a != -1 && a != 0)
                            if (p.tabi[a].prawa3 == 4 || p.tabi[a].prawa3 == 5 || p.tabi[a].prawa3 == 6 || p.tabi[a].prawa3 == 7)
                            {


                                q = 1;
                            }
                        if (q == 0)
                            Console.WriteLine("nie mozesz tego zrobic");
                        else
                        {
                            Console.WriteLine("podaj PPID, PID i priorytet");
                            int ax1, bx1, cx1;
                            ax1 = int.Parse(Console.ReadLine());
                            bx1 = int.Parse(Console.ReadLine());
                            cx1 = int.Parse(Console.ReadLine());
                            proces ppp = zarzadcapr.utwProDziecko(ax1, bx1, cx1);
                            Console.WriteLine("Proces dodano");





                            zarzadcapr.exec(ppp);

                            Console.WriteLine("Podaj ile bloków powinien posiadac proces");
                            //wczytane = Console.ReadLine();     po 3 bloki na proces
                            ppp.heapAddrStart = pam.Zaalokuj(3);   // statycznie ustawiony na 3

                            iwezel.open(ref p, comenda[1], flaga.ronly, ref o, ref slowo);

                            //byte[] tabrozk;

                            string[] str = slowo.Split(' '); // stringa zarzutowac na uinta a uinta na bajty   // integer 

                            byte[] ttmp = new byte[4];
                            int y = 0;
                            for (int l = 0; l < str.Length; l++)
                            {
                                uint tmp = Convert.ToUInt32(str[l]);
                                ttmp = BitConverter.GetBytes(tmp);
                                uint tmppp = BitConverter.ToUInt32(ttmp, 0);
                                for (int x = 0; x < ttmp.Length; x++)
                                {
                                    pam.Zapisz(ppp.heapAddrStart + y, ttmp[x]);
                                    y++;
                                }
                            }

                            ile++;


                            //////////////////////////////////////////////////////////////////////////////////////////
                            Console.WriteLine(pam);
                        }
                        break;
                    }
                case "wy":
                    {
                        zarzadcapr.wypisz2();
                        break;
                    }
                case "wy2":
                    {
                        zarzadcapr.kolejka.wypiszkolejki();
                        break;
                    }
                case "kill":
                    {
                        Console.WriteLine("podaj PID");
                        int axc = int.Parse(Console.ReadLine());
                        proces kkk = zarzadcapr.znajdz(axc);
                        pam.Zwolnij(kkk.heapAddrStart);
                        zarzadcapr.usun_z_k(axc);
                        ile--;
                        break;
                    }
                default:
                    {
                        Console.WriteLine("niepoprawna komenda");
                        break;
                    }



            }
            if (stop == 10)
                return 0;
            if (bre == 10)
                return 1;

        }
    }
}


