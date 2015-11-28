using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;



class Program
{
    static void Main(string[] args)
    {
        Admin Aktuz;
        Uzytkownik Aktu;
        int a;
        Uruchomienie U = new Uruchomienie();
        pom g = new pom();
        string slowo = null;
        int o = 0;
        int ile = 0;
        iwezel.create(ref g, "Root", 'k', "Root");
        iwezel.create(ref g, "Home", 'k', "Root");
        ZarzadcaProcesow zarzadcapr = new ZarzadcaProcesow();
        Pamiec pam = new Pamiec(16 * 16);
        proces init = new proces(1, 20);
        zarzadcapr.dodaj_doWsz(init);


        //           
        //           String wczytane;
        //           int o=0;
        //           string slowo=null;
        //           ZarzadcaProcesow zarzadcapr = new ZarzadcaProcesow();
        //           iwezel.create(ref g, "Root", 'k', "Root");
        //           iwezel.create(ref g, "Home", 'k', "Root");

        // początek logowania
        int bre = 0;
        while (bre != 1)
        {
            a = U.spr();
            if (a == 1)
            {
                string ha;

                Console.Write("Ustaw hasło do konta administratora:");

                ha = Console.ReadLine();
                Aktuz = new Admin("administrator", ha, g, o, slowo, zarzadcapr, ile, pam, init);
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Aktuz.sciezka, true))
                {
                    file.WriteLineAsync();
                    file.WriteLineAsync(ha);
                }

                bre = Aktuz.polecenia();

            }
            else
            {
                Logowanie S = new Logowanie();
                S.Log();
                while (a != 1 && a != 2)
                {
                    try
                    {
                        a = S.spr();
                    }
                    catch (System.IndexOutOfRangeException)
                    {
                        Console.Write("złe dane\npodaj je jeszcze raz\n");
                        S.Log();
                    }
                }
                if (a == 2)
                {
                    Aktuz = new Admin(S.login, S.haslo, g, o, slowo, zarzadcapr, ile, pam, init);
                    bre = Aktuz.polecenia();
                }
                if (a == 1)
                {
                    Aktu = new Uzytkownik(S.login, S.haslo, g, o, slowo, zarzadcapr, ile, pam, init);
                    bre = Aktu.polecenia();
                }
            }

        }

        // koniec logowania
        return;
    }
}
