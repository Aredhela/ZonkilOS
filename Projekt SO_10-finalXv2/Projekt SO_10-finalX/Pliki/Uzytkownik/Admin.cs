using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


class Admin
{
    public

          string login, haslo;
    pom p;
    int o;
    string slowo;
    string aktualnykatalog;
    ZarzadcaProcesow zarzadcapr;
    int ile;
    Pamiec pam;
    proces init;
    public string sciezka = "D:\\SO\\logi.txt";
    public string grupa = "D:\\SO\\grupy.txt";

    public
        Admin(string log, string has, pom x, int ilo, string fff, ZarzadcaProcesow zzz, int il, Pamiec papa, proces ini)
    {
        o = ilo;
        login = log;
        haslo = has;
        p = x;
        slowo = fff;
        aktualnykatalog = "Home";
        zarzadcapr = zzz;
        ile = il;
        pam = papa;
        init = ini;
    }
    public int exit()
    {
        return 10;
    }
    public void adduse(string n, string h, string gru)
    {
        //      string n, h;
        //       n = Console.ReadLine();
        //      h = Console.ReadLine();
        if (n == "administrator")
            Console.WriteLine("nie mozesz utwozyc uzytkownika o takej nazwie");
        else
        {

            string[] f = File.ReadAllLines(sciezka);
            int b = 0, c = f.Length;
            for (int i = 2; i < c - 2; i = i + 3)
                if (n == f[i])
                    b = 1;
            if (b == 1)
                Console.WriteLine("juz istnieje taki uzytkownik");
            else
            {
                string[] t = File.ReadAllLines(grupa);
                b = 0;
                c = t.Length;
                for (int i = 0; i < c; i++)
                    if (t[i] == gru)
                        b = 1;
                if (b == 1)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(sciezka, true))
                    {

                        file.WriteLineAsync(n);
                        file.WriteLineAsync(h);
                        file.WriteLineAsync(gru);

                    }


                    iwezel.create(ref p, n, 'k', "Home");
                }
                else
                    Console.WriteLine("nie istnieje taka grupa");
            }
        }
    }
    public int removeuse(string n)
    {
        int i = 2;

        string[] t = File.ReadAllLines(sciezka);
        int b = 0, c = t.Length;
        do
        {
            if (t[i] == n)
            {
                b = i;
            }
            i = i + 3;
            if (b != 0)
            {
                if (b < c - 2)
                {
                    for (int j = i; j < c - 2; j = j + 2)
                    {
                        t[j - 3] = t[j];
                        t[j - 2] = t[j + 1];
                        t[j - 1] = t[j + 2];
                    }
                    t[c - 2] = null;
                    t[c - 1] = null;
                }
                else
                {
                    t[b] = null;
                    t[b + 1] = null;
                }
                string[] k = new string[c - 2];
                for (int o = 0; o < c - 3; o++)
                    k[o] = t[o];
                System.IO.File.WriteAllLines(sciezka, k);
                return 1;
            }
        } while (i < c - 1);

        return 0;

    }
    public void dodajgru(string n)
    {
        string[] t = File.ReadAllLines(grupa);
        int b = 0, c = t.Length;
        for (int i = 0; i < c; i++)
            if (t[i] == n)
                b = 1;
        if (b == 0)
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(grupa, true))
            {

                file.WriteLineAsync(n);

            }
        else
            Console.WriteLine("taka grua juz istnieje");
    }



    public int removegru(string n)
    {
        int i = 0;

        string[] t = File.ReadAllLines(grupa);
        int b = 0, c = t.Length;
        do
        {
            if (t[i] == n)
            {
                b = i;
            }
            i++;
            if (b != 0)
            {
                if (b < c - 1)
                    for (int j = i; j < c; j++)
                    {
                        t[j - 1] = t[j];


                    }

                string[] k = new string[c - 1];
                for (int o = 0; o < c - 1; o++)
                    k[o] = t[o];
                System.IO.File.WriteAllLines(grupa, k);
                return 1;
            }
        } while (i < c);

        return 0;


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


                case "adduse":

                    try
                    {
                        adduse(comenda[1], comenda[2], comenda[3]);
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        Console.WriteLine("niepoprawne dane");
                    }
                    break;

                case "removeuse":

                    int g = 1;
                    try
                    {
                        g = removeuse(comenda[1]);
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        Console.WriteLine("niepoprawne dane");
                    }
                    if (g == 0)
                        Console.WriteLine("nie istnieje taki uzytkownik");


                    break;

                case "addgru":
                    {

                        dodajgru(comenda[1]);

                        break;

                    }

                case "removegru":
                    string[] t = File.ReadAllLines(sciezka);
                    int b = 0, c = t.Length;
                    for (int i = 4; i < c; i = i + 3)
                        if (t[i] == comenda[1])
                            b = 1;


                    if (b == 0)
                        removegru(comenda[1]);
                    else
                        Console.WriteLine("usuń najpierw uzytkownikow nalezących do grupy");
                    break;

                case "logout":
                    stop = wylogowanie();
                    break;



                case "exit":
                    bre = exit();
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
                            aktualnykatalog = comenda[1];
                    }
                    break;

                case "creatk":

                    iwezel.create(ref p, comenda[1], 'k', aktualnykatalog);


                    break;
                case "creatp":

                    iwezel.create(ref p, comenda[1], 'p', aktualnykatalog);
                    iwezel.open(ref p, comenda[1], flaga.wonly, ref o, ref slowo);

                    break;
                case "delete":

                    iwezel.delete(ref p, comenda[1], ref o);

                    break;

                case "open":
                    {
                        iwezel.open(ref p, comenda[1], flaga.ronly, ref o, ref slowo);
                        if (slowo != null)
                            Console.WriteLine("\nOdczytano z pliku następujące dane: \n" + slowo);

                        break;
                    }
                case "pisz":
                    iwezel.open(ref p, comenda[1], flaga.wonly, ref o, ref slowo);
                    break;

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

                case "proces":
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
                                {
                                    np = proc.main.dodajnowy(pr); // -> dodaje kolejne procesy do wykonania  [funkcja od Tadka]
                                    Console.WriteLine("Dodano nowy proces do kolejki priorytetowej!\n");
                                }
                                   
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
                //////////////////////////////////////////////////////////////////// KOMUNIKACJA ///////////////////////////////////////////////////////////

                case "com":

                    Console.WriteLine("Wybierz rodzaj Łącza ");
                    Console.WriteLine("N -> Utwórz Łącze Nazwane ");
                    Console.WriteLine("R -> Usuń Łącze Nazwane ");
                    Console.WriteLine("1 -> Komunikacja Nazwana ");
                    Console.WriteLine("2 -> Komunikacja Nienazwana ");
                    Console.WriteLine("3 -> Wypisz Dostepne Lacza ");

                    string s1 = Console.ReadLine();

                    if (s1 == "1")
                    {
                        Console.WriteLine("PID procesu przekazującego wartość");
                        string s2 = Console.ReadLine();
                        try
                        {

                            if (zarzadcapr.kolejka.k_wszystkich.Exists(z => z.PID == Convert.ToInt32(s2)))
                            {
                                Console.WriteLine("PID procesu przyjmującego wartość");
                                string s3 = Console.ReadLine();
                                Console.WriteLine("Napisz komunikat");
                                string kom = Console.ReadLine();

                                if (zarzadcapr.kolejka.k_wszystkich.Exists(z => z.PID == Convert.ToInt32(s3)))
                                {

                                    ServiceLocator.GetService<Lacza>().CommunicationNamed(zarzadcapr.kolejka.k_wszystkich.First(z => z.PID == Convert.ToInt32(s2)), zarzadcapr.kolejka.k_wszystkich.First(z => z.PID == Convert.ToInt32(s3)), kom);

                                }
                                else
                                {
                                    Console.WriteLine("Nie ma procesu o takim PID ");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nie ma procesu o takim PID ");
                            }
                        }
                        catch { }
                    }
                    else if (s1 == "2")
                    {
                        try
                        {
                            Console.WriteLine("Podaj PID procesu nadawcy");
                            string s2 = Console.ReadLine();

                            if (zarzadcapr.kolejka.k_wszystkich.Exists(z => z.PID == Convert.ToInt32(s2)))
                            {

                                Console.WriteLine("Podaj PID procesu odbiorcy");
                                string s3 = Console.ReadLine();
                                Console.WriteLine("Przysyłany komunikat");
                                string skom = Console.ReadLine();

                                if (zarzadcapr.kolejka.k_wszystkich.Exists(z => z.PID == Convert.ToInt32(s3)))
                                {
                                    // Sprawdź pokrewieństwo
                                    int wysylacz = Convert.ToInt32(s2);
                                    int odbieracz = Convert.ToInt32(s3);
                                    var pro = zarzadcapr.kolejka.k_wszystkich.Find(z => z.PID == wysylacz);
                                    if (pro.potomkowie.Exists(z => z.PID == odbieracz) == true)
                                    {
                                        var pro1 = zarzadcapr.kolejka.k_wszystkich.Find(z => z.PID == odbieracz);
                                        pro1.rozkazy.Add(skom);
                                        Console.WriteLine("Komunikat został przekazany");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Procesy nie są spokrewnione");
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("Nie ma procesu o takim PID. Czy chcesz utworzyć dziecko procesu o PID: " + s2 + " i przesłać komunikat ? (Y/N)");
                                    string dec = Console.ReadLine();
                                    if (dec == "Y" || dec == "y")
                                    {
                                        int rodzic = Convert.ToInt32(s2);
                                        int dziecko = zarzadcapr.kolejka.k_wszystkich.Max(z => z.PID) + 1;
                                        zarzadcapr.utwProDziecko(rodzic, dziecko, 20);

                                        var pro = zarzadcapr.kolejka.k_wszystkich.Find(z => z.PID == dziecko);
                                        pro.rozkazy.Add(skom);

                                        Console.WriteLine("Komunikat został przekazany dla potomka.");
                                    }
                                    else if (dec == "N" || dec == "n")
                                    { }
                                    else
                                        Console.WriteLine("Zły rozkaz");
                                }

                            }
                            else
                                Console.WriteLine("Nie ma procesu o takim PID ");
                        }
                        catch { }
                        break;
                    }
                    else if (s1 == "n" || s1 == "N")
                    {
                        Console.WriteLine("Podaj PID procesu nadawcy");
                        string s2 = Console.ReadLine();
                        try
                        {

                            if (zarzadcapr.kolejka.k_wszystkich.Exists(z => z.PID == Convert.ToInt32(s2)))
                            {
                                Console.WriteLine("Podaj PID procesu odbiorcy");
                                string s3 = Console.ReadLine();

                                if (zarzadcapr.kolejka.k_wszystkich.Exists(z => z.PID == Convert.ToInt32(s3)))
                                {
                                    ServiceLocator.GetService<Lacza>().UtworzLacze(zarzadcapr.kolejka.k_wszystkich.First(z => z.PID == Convert.ToInt32(s2)), zarzadcapr.kolejka.k_wszystkich.First(z => z.PID == Convert.ToInt32(s3)));
                                    Console.WriteLine("Utworzono Lacze");
                                }
                                else
                                {
                                    Console.WriteLine("Nie ma procesu o takim PID ");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Nie ma procesu o takim PID ");
                            }
                        }
                        catch { }
                    }

                    else if (s1 == "r" || s1 == "R")
                    {
                        Console.WriteLine("Podaj nazwe lacza postaci 1-2");
                        string nazwa = Console.ReadLine();
                        ServiceLocator.GetService<Lacza>().UsunLacze(nazwa);

                    }
                    else if (s1 == "3")
                    {
                        Console.WriteLine("\n Utworzone Łącza: \n");
                        for (int i = 0; i < ServiceLocator.GetService<Lacza>().ListaLaczy.Count; i++)
                            Console.Write(ServiceLocator.GetService<Lacza>().ListaLaczy[i] + "  ");
                        Console.WriteLine("");
                    }
                    break;

                case "usunrozkaz":
                    try
                    {
                        Console.WriteLine("Podaj PID procesu");
                        string s = Console.ReadLine();
                        if (zarzadcapr.kolejka.k_wszystkich.Exists(z => z.PID == Convert.ToInt32(s)))
                        {
                            Console.WriteLine("Podaj rozkaz");
                            string s2 = Console.ReadLine();
                            var proc = zarzadcapr.kolejka.k_wszystkich.Find(z => z.PID == Convert.ToInt32(s));
                            if (proc.rozkazy.Contains(s2))
                            {

                                ServiceLocator.GetService<Lacza>().ClearOrder(zarzadcapr.kolejka.k_wszystkich.Find(z => z.PID == Convert.ToInt32(s)), s2);
                                Console.WriteLine("Usunięto");
                            }
                            else
                            { Console.WriteLine("Proces nie posiada takiego rozkazu"); }
                        }
                        else
                        {
                            Console.WriteLine("Nie ma procesu o takim PID ");
                        }
                    }
                    catch { }
                    break;

                case "przekaz":
                    {
                        try
                        {
                            Console.WriteLine("Podaj PID procesu, z którego pobrany ma zostać rozkaz.");
                            string s = Console.ReadLine();
                            if (zarzadcapr.kolejka.k_wszystkich.Exists(z => z.PID == Convert.ToInt32(s)))
                            {
                                Console.WriteLine("Podaj PID procesu, do którego przekazany ma zostać rozkaz.");
                                string s4 = Console.ReadLine();


                                if (zarzadcapr.kolejka.k_wszystkich.Exists(z => z.PID == Convert.ToInt32(s4)))
                                {
                                    Console.WriteLine("Podaj numer rozkazu.");
                                    string numer = Console.ReadLine();
                                    ServiceLocator.GetService<Lacza>().przekazrozkaz(zarzadcapr.kolejka.k_wszystkich.Find(z => z.PID == Convert.ToInt32(s)), zarzadcapr.kolejka.k_wszystkich.Find(z => z.PID == Convert.ToInt32(s4)), Convert.ToInt32(numer));

                                }
                                else
                                { Console.WriteLine("Nie ma procesu o takim PID"); }
                            }
                            else
                            { Console.WriteLine("Nie ma procesu o takim PID lub nie istnieję łącze nazwane lub pokrewieństwo pomiędzy procesami"); }
                        }
                        catch { }
                    }
                    break;
                ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

