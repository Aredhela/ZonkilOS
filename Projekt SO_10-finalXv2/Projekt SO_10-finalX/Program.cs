
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Symulacja_procesora
{
    public Symulacja_procesora(int ptakt, int twywlaszcz, int prekalk)
    {
        takt = ptakt;
        rekalk = prekalk;
        wywlaszcz = twywlaszcz;

        TablicaKolejek tl = new TablicaKolejek();
        for (int i = 0; i < 8; i++)
            tl.tablicaWszystkich[i] = new Kolejka();
        main = tl;
    }

    public int takt;
    public int rekalk; // potrzebne do przelicznia priorytetow wszystkich procesow
    public int wywlaszcz;
    public TablicaKolejek main; // pole przechowuje kolejke priorytetowa
    public Boolean blokada; // blokuje proca
};

class TablicaKolejek
{
    public Boolean[] wektorbitowy = new Boolean[8]; // sprawdza czy w kolejce jest jakis proces
    public Kolejka[] tablicaWszystkich = new Kolejka[8]; // dwuwymiarowa tablica list procesow (na bazie priorytetu)
    public Boolean coswpadlo = false; // zmienia sie na true, jezeli do listy prio zostanie dodany element
    // podczas funkcji licz musi zostac spowrotem przestawione na false (po 1 takcie zegara)



    public proces znajdz_zwroc_usun()// znajduje proces , zwraca proces i usuwa z kolejki priorytetowej
    {
        proces tmp;
        for (int i = 0; i < 8; i++)
        {
            if (wektorbitowy[i] == true)
            {
                tmp = tablicaWszystkich[i].listasp.First();
                tablicaWszystkich[i].listasp.Remove(tmp);
                if (tablicaWszystkich[i].listasp.Count == 0)
                    wektorbitowy[i] = false;
                return tmp;
            }
        }
        return null;
    }
    public proces znajdz_zworc() // zwraca proces o najwyzszym priorytecie, zwraca proces
    {
        proces tmp;
        for (int i = 0; i < 8; i++)
        {
            if (wektorbitowy[i] == true)
            {
                tmp = tablicaWszystkich[i].listasp.First();
                return tmp;
            }
        }
        return null;
    }
    public void zmniejsziprzestaw()// procedura dokonujaca przeliczenia wszystkich priorytetow w kolejce priorytetowej i ustawienie w odpowiednich kolejkach (wspolczynnik zaniku)
    {

        for (int i = 0; i < 8; i++)             ///// przelicznie priorytetu dynamicznego dla poszczegolnego procesu (wszystkie kolejki)
        {
            foreach (proces tmp in tablicaWszystkich[i].listasp)
            {
                tmp.priorytet[2] = tmp.priorytet[2] / 2;
                tmp.priorytet[0] = tmp.priorytet[1] + tmp.priorytet[2];
            }

        }

        List<proces> listatmp = new List<proces>(); /// utworzenie tymczasowej listy tmp do chwilowego przetrzymywanie procesow

        for (int i = 0; i < 8; i++)             //// tymczasowe kopieowanie do kolejki tmp
        {
            foreach (proces tmp in tablicaWszystkich[i].listasp)
            {
                listatmp.Add(tmp);
            }
        }

        for (int i = 0; i < 8; i++)             //// czyszczenie kolejek i wektora bitowego
        {
            tablicaWszystkich[i].listasp.Clear();
            wektorbitowy[i] = false;
        }


        dodaj(listatmp); //// ponowne kopiowanie na liste glowna
        listatmp.Clear(); //// resetowanie kolejki tmp

    }
    public void info()  // wyswietla w oknie konsoli kolejke priorytetowa
    {
        for (int i = 0; i < 8; i++)
        {
            Console.Write("(K" + i + ") ");
            if (wektorbitowy[i] == true)
                Console.Write(1 + ":\t");
            if (wektorbitowy[i] == false)
                Console.Write(0 + ":\t");

            foreach (proces tmp in tablicaWszystkich[i].listasp)
            {
                Console.Write("P:" + tmp.PID + " Prio:" + tmp.priorytet[0] + "\t");
            }
            Console.WriteLine();
        }
        Console.WriteLine();

    }
    public void przestaw(proces p)  // Procedura przestawiajaca proces po jednym cyklu proca (po przeliczeniu priorytetu) do odpowiedniej kolejki
    {
        proces tmp = new proces(100);
        tmp = p;

        przeliczdlaprocesu(tmp);
        dodaj(tmp);

    }

    public void przeliczdlaprocesu(proces p) // procedura przeliczajaca priorytet dla konkretnego procesu (wlasnie wywlaszczonego)
    {
        int prio;
        p.priorytet[2] = p.priorytet[2] / 2;
        prio = p.priorytet[1] + p.priorytet[2];    // prio = baza + l_rozk/2
        p.priorytet[0] = prio;
    }


    public void dodaj(List<proces> listaprocgot) // ?? pobiera procesy z kolejki, gotowych do umieszczenia na kolejce priorytetowej
    {
        foreach (proces tmp in listaprocgot)
        {
            dodaj(tmp);
        }
    }
    public void dodaj(proces proces) // dodaje proces podany jako parametr typu "proces" do kolejki priorytetowej
    {
        for (int i = 0; i < 8; i++)
        {

            if (i == proces.priorytet[0] / 4)
            {
                tablicaWszystkich[i].dodaj(proces);
                wektorbitowy[i] = true; // potrzebna modyfikacja bo przestawia sie na tru za kazdym dodaniem procesu
            }
        }
    }
    public proces dodajnowy(proces proces) // dodaje nowy proces podany jako parametr typu "proces" do kolejki priorytetowej
    {
        for (int i = 0; i < 8; i++)
        {

            if (i == proces.priorytet[0] / 4)
            {
                tablicaWszystkich[i].dodaj(proces);
                wektorbitowy[i] = true; // potrzebna modyfikacja bo przestawia sie na tru za kazdym dodaniem procesu
                coswpadlo = true;
            }
        }
        return proces;
    }

    public void usun(int kolejka) // usuwa ostatni element
    {

        for (int j = 0; j < kolejka; j++)
        {
            tablicaWszystkich[j].usun();
            if (tablicaWszystkich[j].listasp.Count == 0)
                wektorbitowy[j] = false;
        }

    }

    public void wypiszStanRejestruProcesu(proces p)
    {
        Console.WriteLine("Wkonywany proces P:" + p.PID + " Stan R1: " + p.rej_stalo[0] + " R2: " + p.rej_stalo[1] + " R3: " + p.rej_stalo[2] + " R4: " + p.rej_stalo[3] + " L. ROZK: " + p.l_rozk + "\n");
    }
}

class Kolejka
{
    public List<proces> listasp = new List<proces>(); // inicjuje wlasciwa liste

    public void dodaj(proces pro) // dodaje proces na liste
    {
        listasp.Add(pro);
    }

    public void usun() // usuwa ostatni element z listy
    {
        if (listasp.Count == 1)
        {
            foreach (proces tmp in listasp)
            {
                listasp.Remove(tmp);
            }
        }
    }
}

public class Interpreter
{
    //rejestry

    public uint MVI_R1 = 11, MVI_R2 = 12, MVI_R3 = 13, MVI_R4 = 14;         // wpisywanie do rejestrow
    public uint MOV_R1_R2 = 15, MOV_R1_R3 = 16, MOV_R1_R4 = 17;             // przenoszenie miedzy rejestrami (MOV "odbiorca" "nadawca") 
    public uint MOV_R2_R1 = 21, MOV_R2_R3 = 22, MOV_R2_R4 = 23;
    public uint MOV_R3_R1 = 24, MOV_R3_R2 = 25, MOV_R3_R4 = 26;
    public uint MOV_R4_R1 = 27, MOV_R4_R2 = 28, MOV_R4_R3 = 29;
    public uint ADD_R1 = 31, ADD_R2 = 32, ADD_R3 = 33, ADD_R4 = 34;         //Dodawnaie
    public uint ADD_R1_R2 = 35;
    public uint SUB_R1 = 41, SUB_R2 = 42, SUB_R3 = 43, SUB_R4 = 44;         //Odejmowanie
    public uint SUB_R1_R2 = 45;
    public uint MUL_R1 = 51, MUL_R2 = 52, MUL_R3 = 53, MUL_R4 = 54;         // MNOZENIE
    public uint MUL_R1_R2 = 55;
    public uint DIV_R1 = 61, DIV_R2 = 62, DIV_R3 = 63, DIV_R4 = 64;         // Dzielenie
    public uint DIV_R1_R2 = 65;
    public uint INC_R1 = 71, INC_R2 = 72, INC_R3 = 73, INC_R4 = 74;         // Inkrementacja rejestru
    public uint DEC_R1 = 81, DEC_R2 = 82, DEC_R3 = 83, DEC_R4 = 84;         // Dekrementacja rejestru
    public uint JMP = 91, JNZ = 92, RET = 99;                                // skok / skok jezeli nie zero w R2 / koniec(powrot) programu


    public bool wykonajInstrukcje(proces p, byte[] tab)
    {
        uint tmpinstrukcjarozkazu = BitConverter.ToUInt32(tab, 0);/// []byte na uint

        uint rozkaz = Convert.ToUInt32(tmpinstrukcjarozkazu.ToString().Substring(0, 2)); // na unit

        uint arguments;
        String tmparguments = tmpinstrukcjarozkazu.ToString().Substring(2, tmpinstrukcjarozkazu.ToString().Length - 2);
        if (tmparguments == "")
            arguments = 0;
        else
            arguments = Convert.ToUInt32(tmpinstrukcjarozkazu.ToString().Substring(2, tmpinstrukcjarozkazu.ToString().Length - 2)); // na unit

        ///Wypisywanie w konsoli rozkazu z argumentem
        Console.WriteLine("Wykonywany rozkaz to: " + rozkaz + " o argumencie: " + arguments);
        ///

        p.l_rozk++;                             // EDIT  
        p.priorytet[2]++;                       //

        //MVI
        if (MVI_R1 == rozkaz)
        {
            p.rej_stalo[0] = arguments;
        }
        if (MVI_R2 == rozkaz)
        {
            p.rej_stalo[1] = arguments;
        }
        if (MVI_R3 == rozkaz)
        {
            p.rej_stalo[2] = arguments;
        }
        if (MVI_R4 == rozkaz)
        {
            p.rej_stalo[3] = arguments;
        }

        //MOV_R1_
        if (MOV_R1_R2 == rozkaz)
        {
            p.rej_stalo[0] = p.rej_stalo[1];
        }
        if (MOV_R1_R3 == rozkaz)
        {
            p.rej_stalo[0] = p.rej_stalo[2];
        }
        if (MOV_R1_R4 == rozkaz)
        {
            p.rej_stalo[0] = p.rej_stalo[3];
        }
        //MOV_R2_
        if (MOV_R2_R1 == rozkaz)
        {
            p.rej_stalo[1] = p.rej_stalo[0];
        }
        if (MOV_R2_R3 == rozkaz)
        {
            p.rej_stalo[1] = p.rej_stalo[2];
        }
        if (MOV_R2_R4 == rozkaz)
        {
            p.rej_stalo[1] = p.rej_stalo[3];
        }
        //MOV_R3_
        if (MOV_R3_R1 == rozkaz)
        {
            p.rej_stalo[2] = p.rej_stalo[0];
        }
        if (MOV_R3_R2 == rozkaz)
        {
            p.rej_stalo[2] = p.rej_stalo[1];
        }
        if (MOV_R3_R4 == rozkaz)
        {
            p.rej_stalo[2] = p.rej_stalo[3];
        }
        //MOV_R4_
        if (MOV_R4_R1 == rozkaz)
        {
            p.rej_stalo[3] = p.rej_stalo[0];
        }
        if (MOV_R4_R2 == rozkaz)
        {
            p.rej_stalo[3] = p.rej_stalo[1];
        }
        if (MOV_R4_R3 == rozkaz)
        {
            p.rej_stalo[3] = p.rej_stalo[2];
        }

        //ADD
        if (ADD_R1 == rozkaz)
        {
            p.rej_stalo[0] += arguments;
        }
        if (ADD_R2 == rozkaz)
        {
            p.rej_stalo[1] += arguments;
        }
        if (ADD_R3 == rozkaz)
        {
            p.rej_stalo[2] += arguments;
        }
        if (ADD_R4 == rozkaz)
        {
            p.rej_stalo[3] += arguments;
        }
        if (ADD_R1_R2 == rozkaz)
        {
            p.rej_stalo[0] += p.rej_stalo[1];
        }

        //SUB
        if (SUB_R1 == rozkaz)
        {
            p.rej_stalo[0] -= arguments;
        }
        if (SUB_R2 == rozkaz)
        {
            p.rej_stalo[1] -= arguments;
        }
        if (SUB_R3 == rozkaz)
        {
            p.rej_stalo[2] -= arguments;
        }
        if (SUB_R4 == rozkaz)
        {
            p.rej_stalo[3] -= arguments;
        }
        if (SUB_R1_R2 == rozkaz)
        {
            p.rej_stalo[0] -= p.rej_stalo[1];
        }

        //MUL
        if (MUL_R1 == rozkaz)
        {
            p.rej_stalo[0] *= arguments;
        }
        if (MUL_R2 == rozkaz)
        {
            p.rej_stalo[1] *= arguments;
        }
        if (MUL_R3 == rozkaz)
        {
            p.rej_stalo[2] *= arguments;
        }
        if (MUL_R4 == rozkaz)
        {
            p.rej_stalo[3] *= arguments;
        }
        if (MUL_R1_R2 == rozkaz)
        {
            p.rej_stalo[0] *= p.rej_stalo[1];
        }

        //DIV
        if (DIV_R1 == rozkaz)
        {
            p.rej_stalo[0] /= arguments;
        }
        if (DIV_R2 == rozkaz)
        {
            p.rej_stalo[1] /= arguments;
        }
        if (DIV_R3 == rozkaz)
        {
            p.rej_stalo[2] /= arguments;
        }
        if (DIV_R4 == rozkaz)
        {
            p.rej_stalo[3] /= arguments;
        }
        if (DIV_R1_R2 == rozkaz)
        {
            p.rej_stalo[0] /= p.rej_stalo[1];
        }

        //INC
        if (INC_R1 == rozkaz)
        {
            p.rej_stalo[0]++;
        }
        if (INC_R2 == rozkaz)
        {
            p.rej_stalo[1]++;
        }
        if (INC_R3 == rozkaz)
        {
            p.rej_stalo[2]++;
        }
        if (INC_R4 == rozkaz)
        {
            p.rej_stalo[3]++;
        }

        //DEC
        if (DEC_R1 == rozkaz)
        {
            p.rej_stalo[0]--;
        }
        if (DEC_R2 == rozkaz)
        {
            p.rej_stalo[1]--;
        }
        if (DEC_R3 == rozkaz)
        {
            p.rej_stalo[2]--;
        }
        if (DEC_R4 == rozkaz)
        {
            p.rej_stalo[3]--;
        }

        //JMP
        if (JMP == rozkaz)
        {
            p.l_rozk = (int)arguments;
        }

        //JNZ
        if (JMP == rozkaz)
        {
            if (p.rej_stalo[1] != 0)
            {
                p.l_rozk = (int)arguments;
            }
        }
        //RET
        if (RET == rozkaz)
        {
            return true;
        }


        return false;
    }

}


//class program
//{
//    static void Main(string[] args)
//    {
//        /// tadek
//        /// 

//        pom p = new pom();
//        int ile = 0;
//        String wczytane;
//        int o = 0;
//        string slowo = null;
//        ZarzadcaProcesow zarzadcapr = new ZarzadcaProcesow();
//        iwezel.create(ref p, "Root", 'k', "Root");
//        iwezel.create(ref p, "Home", 'k', "Root");

//        Console.Write("Podaj ile procesow ma zostac utworzonych: ");

//        wczytane = Console.ReadLine();
//        ile = Convert.ToInt32(wczytane);

//        proces init = new proces(1, 20);
//        pamiec pam = new pamiec(16 * 16);

        //int j;
        //for (int i = 0; i < ile; i++)
        //{

        //    Console.WriteLine("\nPodaj nazwę katalogu lub pliku");
        //    string nazwa;
        //    nazwa = Console.ReadLine();

        //    Console.WriteLine("\nPodaj nazwę katalogu w którym ma być przechowywany plik lub katalog:");
        //    string nazwa2;
        //    nazwa2 = Console.ReadLine();
        //    char p1 = 'p';
        //    iwezel.create(ref p, nazwa, p1, nazwa2);

        //    // zapisz rozkazow do plikow
        //    iwezel.open(ref p, nazwa, flaga.wonly, ref o, ref slowo);



//            j = i + 1;
//            Console.Write("Podaj jaki priorytet ma posiadać proces: " + j + " PRIO: ");

//            wczytane = Console.ReadLine();

//            proces ppp = zarzadcapr.utworz(init, i + 1, Convert.ToInt32(wczytane));
//            zarzadcapr.exec(ppp);

//            Console.WriteLine("Podaj ile bloków powinien posiadac proces");
//            //wczytane = Console.ReadLine();     po 3 bloki na proces
//            ppp.heapAddrStart = pam.Zaalokuj(3);   // statycznie ustawiony na 3

//            iwezel.open(ref p, nazwa, flaga.ronly, ref o, ref slowo);

//            //byte[] tabrozk;

//            string[] str = slowo.Split(' '); // stringa zarzutowac na uinta a uinta na bajty   // integer 

//            byte[] ttmp = new byte[4];
//            int y = 0;
//            for (int l = 0; l < str.Length; l++)
//            {
//                uint tmp = Convert.ToUInt32(str[l]);
//                ttmp = BitConverter.GetBytes(tmp);
//                uint tmppp = BitConverter.ToUInt32(ttmp, 0);
//                for (int x = 0; x < ttmp.Length; x++)
//                {
//                    pam.Zapisz(ppp.heapAddrStart + y, ttmp[x]);
//                    y++;
//                }
//            }

//        }

//        //////////////////////////////////////////////////////////////////////////////////////////
//        Console.WriteLine(pam);

//        ////////////////////////////////////////////////////////////////////////////////////////////////
//        Symulacja_procesora proc = new Symulacja_procesora(1, 4, 15);
//        Interpreter it = new Interpreter();

//        proc.main.dodajnowy(zarzadcapr.kolejka.wyslijproces()); //-> dodaje 1 proces na starcie systemu [wklejam funkcje od Tadka]

//        Console.WriteLine("\nStart Symulacji procesora!");

//        proc.main.info();
//        proces tmpp = proc.main.znajdz_zwroc_usun();
//        byte[] tabpam = new byte[4];
//        int z = 0;
//        while (true)
//        {
//            for (int i = 0; i < 4; i++)
//                tabpam[i] = pam.Odczytaj(tmpp.heapAddrStart + i + (tmpp.l_rozk * 4));

//            uint tmpppp = BitConverter.ToUInt32(tabpam, 0); // test -> jaki rozkaz

//            proc.blokada = it.wykonajInstrukcje(tmpp, tabpam); // -> wynonanie 1 instrukcji z podanego procesu [wklejam funkcje od Iwony]
//            proc.main.wypiszStanRejestruProcesu(tmpp);


//            Console.In.Read();
//            Console.In.Read();
//            if (proc.blokada)       // zakonczenie procesu
//            {
//                tmpp = proc.main.znajdz_zwroc_usun();
//                proc.blokada = false;
//            }

//            if (tmpp == null)       // koniec wykonywania wszystkich procesow
//            {
//                Console.Out.WriteLine("Ilosc taktow procesora: " + proc.takt);
//                Console.Out.WriteLine("Wszystkie procesy zostaly wykonane!");
//                break;
//            }

//            if (proc.takt % proc.rekalk == 0 && tmpp != null) // obliczenie priorytetu dla wszystkich procesow po 15 cyklach
//            {
//                proc.main.zmniejsziprzestaw();                                         // oblicza prio dla pozostalych procesow
//                tmpp.priorytet[2] = tmpp.priorytet[2] / 2;                          // oblicza prio dla 1 przetrymywanego w tmpp procesu
//                tmpp.priorytet[0] = tmpp.priorytet[1] + tmpp.priorytet[2];          //
//            }

//            if (tmpp.l_rozk % proc.wywlaszcz == 0 && tmpp != null) // wywlaszczenie pocesu po 4 cyklach proca    //EDIT
//            {
//                proc.main.przestaw(tmpp);

//                proc.main.info();
//                tmpp = proc.main.znajdz_zwroc_usun();
//            }
//            if (proc.takt % 1 == 0) // przychodzi proces
//            {
//                proces pr = zarzadcapr.kolejka.wyslijproces();
//                proces np = null;
//                if (pr != null)
//                    np = proc.main.dodajnowy(pr); // -> dodaje kolejne procesy do wykonania  [funkcja od Tadka]

//                if (proc.main.coswpadlo == true)
//                {
//                    if (np.priorytet[0] < tmpp.priorytet[0])
//                    {
//                        proc.main.przestaw(tmpp);
//                        proc.main.info();
//                        tmpp = proc.main.znajdz_zwroc_usun();
//                    }
//                    else { }
//                    proc.main.coswpadlo = false;
//                }
//            }


//            proc.takt++;
//        }

//        Console.In.Read();

//    }
//}
