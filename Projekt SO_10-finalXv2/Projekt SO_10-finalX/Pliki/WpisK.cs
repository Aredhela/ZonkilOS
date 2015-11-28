using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


    class WpisK
    {

        public string nazwa;
        public byte nriwezla;
       
        public WpisK(string str , byte wezel )
        {
            nazwa = str;
            nriwezla = wezel;
        }


        public static void wyswk1(string nazwa, ref pom x)     // wyswietla zawartosc jednego katalogu o podanej nazwie
        {
            bool ok = false;
            int o = 0;
            for (int i = 0; i < x.LK.Count(); i++)
            {
                if (x.LK[i][0].nazwa == nazwa)
                { ok = true; o = i; break; }

            }
            if (ok == true)
            {
                Console.WriteLine("Katalog o nazwie:  " + x.LK[o][0].nazwa + "  posiada nastepujące wpisy:");
                foreach (WpisK wpis in x.LK[o])
                {
                    Console.WriteLine("Nazwa: " + wpis.nazwa + "\t numer i-wezla:" + wpis.nriwezla);
                }



            }
            if (ok == false)
            {
                Console.WriteLine("Nie ma takiego katalogu");

            }
       }

        public static void wyswk2(ref pom x)     // wyswietla zawartosc wszystkich katalogow
        {
            int c;

            for (int i = 0; i < x.LK.Count(); i++)
            {   c=0;
                Console.Write("Katalog o nazwie:  " + x.LK[i][0].nazwa);
                for (int j = 0; j < x.LK[i].Count(); j++)
                {
                    if (j == 0)
                    { Console.Write(" posiada nastepujące wpisy:\n"); c = 1; }

                    Console.WriteLine("Nazwa: " + x.LK[i][j].nazwa + "\t numer i-wezla: "+x.LK[i][j].nriwezla);

                }
                if (c == 0)
                    Console.Write(" nie posiada wpisów katalogowych");
                Console.WriteLine();
            }
            

            
        }

    }
