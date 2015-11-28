using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

 class blok
    {
        public byte[] bl;
        public int nast;

        public blok()
        {
            bl = new byte[32];
            nast = -1;
        }

        public static void Wyswietblok(ref pom x, int nr,string slo)
        {
            int c=0;
            for (int i = 0; i < x.LK.Count(); i++)
            {
                for (int j = 1; j < x.LK[i].Count(); j++)
                {
                    if (x.LK[i][j].nazwa == slo)
                    {
                        c = x.LK[i][j].nriwezla; 
                    }

                }
            }

            List<int> LBP = new List<int>();
            LBP = iwezel.listazblok(ref x, c);


            if (c != 0 && LBP.Count>0 )
            {
                if (nr <= LBP.Count && nr!=0)
                {
                    Console.WriteLine("Blok nr: " + nr + " dla tego pliku, o indeksie nr: "+LBP.ElementAt(nr-1)+" na dysku wygląda tak:");

                    for (int i = 0; i < 32; i++)
                    {
                        Console.Write(x.dysk[LBP.ElementAt(nr-1)].bl[i] + " ");
                        if (i == 15)
                            Console.WriteLine();
                    }
                    Console.WriteLine();

                    Console.WriteLine("Indeks na następny to: " + x.dysk[LBP.ElementAt(nr - 1)].nast);
                }
               

                if (nr > LBP.Count)
                {
                    Console.WriteLine("Blok nr: " + nr + " dla tego pliku nie istnieje");
                    
                }

                if (nr == 0)
                {
                    Console.WriteLine("Blok indeksowy dla tego pliku, o indeksie nr: " + x.tabi[c].blokindeksowy + " na dysku wygląda tak:");

                    
                    for (int i = 0; i < 32; i++)
                    {
                        Console.Write(x.dysk[x.tabi[c].blokindeksowy].bl[i] + " ");
                        if (i == 15)
                            Console.WriteLine();
                    }
                    Console.WriteLine();
                    Console.WriteLine("Indeks na następny to: " + x.dysk[x.tabi[c].blokindeksowy].nast);
                }


            }

            if (c == 0 || LBP.Count == 0)
            {
                Console.WriteLine("Nie ma takiego pliku lub jest on pusty");
            }
        }

        public static void Wyswietbloklubdysk(ref pom x, int nr, string slowo)
        {

            if (slowo == "b" || slowo == "B")
            {
                Console.WriteLine("Blok nr:" + nr);
                for (int i = 0; i < 32; i++)
                {
                    Console.Write(x.dysk[nr].bl[i] + " ");
                    if (i == 15)
                        Console.WriteLine();
                }
                Console.WriteLine();

                Console.WriteLine("Indeks na następny to: " + x.dysk[nr].nast);
            }


            if (slowo == "d" || slowo == "D")
            {
                for (int j = 0; j < 64; j++)
                {
                    Console.WriteLine("Blok nr:" + j);
                    for (int i = 0; i < 32; i++)
                    {
                        Console.Write(x.dysk[j].bl[i] + " ");
                        if (i == 15)
                            Console.WriteLine();
                    }
                    Console.WriteLine();

                    Console.WriteLine("Indeks na następny to: " + x.dysk[j].nast);
                }

            }

        }


        public static void wolnebloki(ref pom x,ref int o)
        {
            int pom = 64;

            pom = pom - o;
            Console.WriteLine("Wolnych bloków jest tyle: "+pom);

            pom = 0;
               Console.WriteLine("Indeksy o nastepujących numerach są wolne:");
                for (int i = 0; i < 64; i++)
                {
                     if (x.dysk[i].nast == -1)
                     { Console.Write(" " + i); pom++; }
                     if (pom == 20 || pom == 40)
                         Console.Write("\n");

                }
                Console.WriteLine();

        }

        public static void zajetebloki(ref pom x, ref int o)
        {
            
            Console.WriteLine("Zajętych bloków jest tyle: " + o);

            if (o == 0)
                goto tu;

                Console.WriteLine("Indeksy o nastepujących numerach są zajęte:");
                for (int i = 0; i < 64; i++)
                {
                    if (x.dysk[i].nast != -1)
                        Console.Write(" " + i);
                }
            tu:
                Console.WriteLine();


        }

       

    }
