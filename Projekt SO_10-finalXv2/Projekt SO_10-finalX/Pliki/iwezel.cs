using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    enum flaga { ronly, wonly }



    struct pom
    {
        public blok[] dysk;
        public iwezel[] tabi;
        public List<List<WpisK>> LK;
    }


    class iwezel
    {
      public string IDuzytkownika;
      public string IDgrupy;
      public char Typ;
      public byte prawa1;
      public byte prawa2;
      public byte prawa3;
      public int rozmiar;
      public int pierwszyblok;
      public int drugiblok;
      public int blokindeksowy;
      public DateTime datautw;
        static int licz = 0;    

        public iwezel(ref pom x, char typ)
        {
            if (licz == 0)
            {
                x.dysk = new blok[64];
                for (int i = 0; i < x.dysk.Length; i++)
                {
                    x.dysk.SetValue(new blok(), i);           //inicjowanie tablicy iwezlow, dysku i listy wpisów katalogowych
                }                                                
                x.tabi = new iwezel[20];                               
                x.LK = new List<List<WpisK>>();
            }

            IDuzytkownika = "0";
            IDgrupy = "0";
            Typ = typ;
            prawa1 = 7;
            prawa2 = 3;
            prawa3 = 3;
            rozmiar = 0;
            pierwszyblok = -1;
            drugiblok = -1;
            blokindeksowy =-1;
            datautw = DateTime.Now;
            x.tabi[licz] = this;

        }

        
        public static void Wyswtabi(ref pom x)     //wyswietlenie przykladowych pól dla tablicy iwęzłów
        {
            foreach (iwezel wezel in x.tabi)
            {
                if (wezel == null)
                    break;

                Console.Write("Typ:" + wezel.Typ +"  Pierwszy blok:"+wezel.pierwszyblok+ "  rozmiar:" + wezel.rozmiar);
                Console.WriteLine("  Data utw: {0:dd/MM/yy} czas:{0: H:mm:ss }", wezel.datautw); 
            }

        }


        public static int wolnemiejsce(ref int o, ref pom x)     //funkcja służąca do losowania kolejnych bloków dla pliku
        {
            Random random = new Random();
            int y = 0;
            while (true)                                            
            {
                if (o == 64)                                         //jeśli wszystkie bloki zostaną zajęte 
                { Console.WriteLine("Brak miejsca na dysku"); return -1; }

                y = random.Next(0, 63);
                if (x.dysk[y].nast == -1 )
                { o++; x.dysk[y].nast = -2; return y; }

            }

        }

        public static int czyistnieje(ref pom x, string naz)     //funkcja zwwraca nr i-wezla dla pliku w tablicy iwęzłów
        {
            bool ok = false;
            int c = 0;
            try
            {
                for (int i = 0; i < x.LK.Count(); i++)
                {
                    for (int j = 0; j < x.LK[i].Count(); j++)
                    {
                        if (x.LK[i][j].nazwa == naz)
                        {
                            c = x.LK[i][j].nriwezla;
                            ok = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd: \n" + ex.Message.ToString());
                return -1;
            }
            if (ok == false && c == 0)
            {
                return -1;
            }

            return c;


        }




        public static int wyswzajblok(ref pom x, string naz)    //funkcja wyświetla indeksy zajętych bloków na dysku przez plik
        {
            int k = 0;
            int v = 0;
            k = iwezel.czyistnieje(ref x, naz);
            List<int> LBP = new List<int>();
            LBP = iwezel.listazblok(ref x, k);


            if (k!=-1 && x.tabi[k].Typ == 'k')
            {
                Console.WriteLine("Katalog nie ma przydzielonych bloków na dysku");
                return -1;
            }

            if (k == -1)
            {
                Console.WriteLine("Nie ma takiego pliku");
                return -1;
            }            
            else
            {
                if (LBP.Count > 0)
                {
                    Console.WriteLine("\nPlik zajmuje następujące bloki:");
                    for (int i = 0; i < LBP.Count; i++)
                    {
                        v = i + 1;
                        Console.WriteLine("Blok nr: " + v + " dla tego pliku jest w " + LBP.ElementAt(i) + " bloku na dysku");
                    }
                    Console.WriteLine();

                    if (x.tabi[k].blokindeksowy != -1)
                        Console.WriteLine("Blok indeksowy to:" + x.tabi[k].blokindeksowy);
                    else
                        Console.WriteLine("Nie posiada bloku indeksowego");
                }
                else
                    Console.WriteLine("\nPlik jest pusty");
            }
            return 0;
        }

        public static List<int> listazblok(ref pom x, int indeks)      //zwraca liste z zajetymi blokami z
                                                                       //której korzystam do odczytania danych z pliku
        {
            List<int> LBP = new List<int>();
            int b = 0;

            if (indeks == -1)
                return LBP;

            if (x.tabi[indeks].pierwszyblok != -1)
                LBP.Add(x.tabi[indeks].pierwszyblok);
            else 
                return LBP;

            if (x.tabi[indeks].drugiblok != -1)
                LBP.Add(x.tabi[indeks].drugiblok);
            else
                return LBP;

            if (x.tabi[indeks].blokindeksowy == -1)
                return LBP;

            while (b != 4)
            {
                if (x.dysk[x.tabi[indeks].blokindeksowy].bl[b] == 65)
                    break;
                LBP.Add((int)x.dysk[x.tabi[indeks].blokindeksowy].bl[b]);
                b++;
            }


            return LBP;
        }
        
        public static int open(ref pom x, string naz, flaga l, ref int o, ref string Out)  // o- ilość zajętych bloków na dysku 
        //Out-zmienna potrzebna do zapisu odczytanego pliku    l-flaga decyduje czy chcemy coś odczytać czy coś zapisać
        //funkcja służąca do odczytu lub zapisu
        
        {                                                                  
            bool ok = false;
            string odkod = null;
            Out = odkod;
            int v = 0, b = 0; 
            List<int> LBP = new List<int>();

            for (int i = 0; i < x.LK.Count(); i++)
            {
                for (int j = 0; j < x.LK[i].Count(); j++)
                {
                    if (x.LK[i][j].nazwa == naz)                      // znajdowanie pliku o nazwie zapisanej w parametrze 'naz'
                    { v = x.LK[i][j].nriwezla; b = x.LK[i][0].nriwezla; ok = true; break; }
                }
            }
            if (ok == false)
            {
                Console.WriteLine("Nie ma takiego pliku");
                return -1;
            }
            LBP = iwezel.listazblok(ref x, v);

            if (l == flaga.ronly && ok == true)             //funkcja odczytuje plik 
            {
                if (x.tabi[v].Typ == 'p' && ((x.tabi[v].prawa1 == 1) || (x.tabi[v].prawa1 == 3) || (x.tabi[v].prawa1 == 7) || (x.tabi[v].prawa1 == 5)))
                {
                    if (x.tabi[v].pierwszyblok == -1)
                    {                                            // domyślnie pierwszy blok ma -1 co oznacza ze plik jest pusty
                        Console.WriteLine("Plik jest pusty");
                        
                        return -1;
                    }

                    else
                    {
                        byte[] q = new byte[LBP.Count * 32];

                        int ileb = LBP.Count;
                        int nowa = 0;
                        int r = 0;
                        int[] taby = new int[ileb];
                        
                        for (int i = 0; i < ileb; i++)
                            taby[i] = LBP[i];

                        while (nowa != ileb)
                        {
                            if (nowa > 5)
                            {
                                Console.WriteLine("Plik może mieć maksymalnie 6 bloków!");
                                break;
                            }

                            for (int i = 0; i < 32; i++)
                            {
                                if (x.dysk[taby[nowa]].bl[i] == 0)
                                    break;
                                q[r] = x.dysk[taby[nowa]].bl[i];
                                r++;
                            }

                            
                                if (ileb - 1 == nowa)
                                    odkod = Encoding.ASCII.GetString(q, 0, r);
                            
                            
                            nowa++;
                        }

                        
                        Out = odkod;

                    }

                }

                if (x.tabi[v].Typ == 'k')
                {
                    WpisK.wyswk1(naz, ref x);       //open z flaga ronly dla katalogu wyswietla wpisy danego katalogu

                }

            }



            if (l == flaga.wonly && ok == true)
            {                                       //sprawdzamy czy jest to plik o odpowiednich prawach do ktorego mozemy cos zapisac
                if (x.tabi[v].Typ == 'p' && ((x.tabi[v].prawa1 == 2) || (x.tabi[v].prawa1 == 3) || (x.tabi[v].prawa1 == 7) || (x.tabi[v].prawa1 == 6)))
                {
                    Console.WriteLine("Pisz do pliku o nazwie: "+naz);
                    string dane;
                    dane = Console.ReadLine();

                    byte[] q;
                    q = Encoding.ASCII.GetBytes(dane);
                    int ileb = q.Length / 32;
                    int u = x.tabi[v].blokindeksowy;
                    int r = 0;
                    int nowa = 0,koncz=0;
                    

                    if (x.tabi[v].pierwszyblok == -1)                   //jeśli plik jest pusty to zapis przebiega tak
                    {
                        
                        if (q.Length % 32 == 0)
                        {
                            int[] taby = new int[ileb];
                            while (nowa != ileb)
                            {
                                if (koncz == 1 && r == 192)
                                {
                                    Console.WriteLine("Plik może mieć maksymalnie 6 bloków!");
                                    break;
                                }

                                if (koncz == 1)
                                    break;

                                taby[nowa] = iwezel.wolnemiejsce(ref o, ref x);

                                for (int i = 0; i < 32; i++)
                                {
                                    if (r == q.Length || r == 192)
                                    { koncz = 1; break; }
                                                              
                                    x.dysk[taby[nowa]].bl[i] = q[r];
                                    r++;
                                }
                                
                                if (nowa == 0)
                                {
                                    x.tabi[v].pierwszyblok = taby[nowa];

                                }

                                if (nowa == 1)
                                {
                                    x.tabi[v].drugiblok = taby[nowa];
                                    x.dysk[taby[nowa - 1]].nast = taby[nowa];

                                }
                                if (nowa == 2)
                                {
                                    u = iwezel.wolnemiejsce(ref o, ref x);
                                    x.dysk[taby[nowa - 1]].nast = u;
                                    x.tabi[v].blokindeksowy = u;

                                    for (int i = 0; i < 32; i++)
                                        x.dysk[u].bl[i] = 65;       //wypelnienie bloku indeksowego liczbą(indeksem) nie nazlezącą do dysku
                                                                    //potrzebne do odczytania z bloku indeksowego kolejnych zajmowanych bloków 
                                    x.dysk[u].bl[0] = (byte)taby[nowa];
                                }

                                if (nowa == 3)
                                {
                                   // x.dysk[taby[nowa - 1]].nast = taby[nowa];
                                    x.dysk[u].bl[1] = (byte)taby[nowa];

                                }

                                if (nowa == 4)
                                {
                                    //x.dysk[taby[nowa - 1]].nast = taby[nowa];
                                    x.dysk[u].bl[2] = (byte)taby[nowa];

                                }

                                if (nowa == 5)
                                {
                                   // x.dysk[taby[nowa - 1]].nast = taby[nowa];
                                    x.dysk[u].bl[3] = (byte)taby[nowa];
                                }
                                                                
                                nowa++;
                                if (r == 192)
                                    koncz = 1;

                            }
                        }
                        if (q.Length % 32 != 0)
                        {
                            
                            int[] taby2 = new int[ileb + 1];

                            while (nowa != ileb + 1)
                            {
                                if (koncz == 1 && r == 192)
                                {
                                    Console.WriteLine("Plik może mieć maksymalnie 6 bloków!");
                                    break;
                                }

                                if (koncz == 1)
                                { break; }

                                taby2[nowa] = iwezel.wolnemiejsce(ref o, ref x);

                                for (int i = 0; i < 32; i++)
                                {
                                    if (r == q.Length || r==192)
                                    { koncz = 1; break; }

                                    x.dysk[taby2[nowa]].bl[i] = q[r];
                                    r++;
                                }
                                
                                if (nowa == 0)
                                {
                                    x.tabi[v].pierwszyblok = taby2[nowa];

                                }

                                if (nowa == 1)                              //2 bloki danych 
                                {
                                    x.tabi[v].drugiblok = taby2[nowa];
                                    x.dysk[taby2[nowa - 1]].nast = taby2[nowa];



                                }
                                if (nowa == 2)
                                {
                                    u = iwezel.wolnemiejsce(ref o, ref x);
                                    x.dysk[taby2[nowa - 1]].nast = u;
                                    x.tabi[v].blokindeksowy = u;

                                    for (int i = 0; i < 32; i++)
                                        x.dysk[u].bl[i] = 65;       

                                    x.dysk[u].bl[0] = (byte)taby2[nowa];

                                }

                                if (nowa == 3)
                                {
                                    // x.dysk[taby[nowa - 1]].nast = taby[nowa];
                                    x.dysk[u].bl[1] = (byte)taby2[nowa];

                                }

                                if (nowa == 4)
                                {
                                    //x.dysk[taby[nowa - 1]].nast = taby[nowa];
                                    x.dysk[u].bl[2] = (byte)taby2[nowa];

                                }

                                if (nowa == 5)
                                {
                                    // x.dysk[taby[nowa - 1]].nast = taby[nowa];
                                    x.dysk[u].bl[3] = (byte)taby2[nowa];
                                }
                                if (r == 192)
                                    koncz = 1;
                                nowa++;
                            }
                        }

                        x.tabi[v].rozmiar = r;

                        x.tabi[b].rozmiar += x.tabi[v].rozmiar;
                    }






                    if (x.tabi[v].pierwszyblok != -1 && r == 0)      //jeśli chcemy coś dopisać do pliku
                    {
                        byte[] q2 = null;
                       
                        int ostbl = 0;
                        int poj = (LBP.Count - 1) * 32;
                        ostbl = LBP.Last();
                        nowa =LBP.Count;

                        q2 = x.dysk[ostbl].bl;

                        for (int i = 0; i < 32; i++)
                        {
                            if (q2[i] != 0)
                                poj++;

                            if (q2[i] == 0)
                            {
                                x.dysk[ostbl].bl[i] = q[r];
                                r++;
                                poj++;
                                if (r == q.Length || poj==192)
                                    break;
                            }
                        }

                        if (q.Length - r <= 0)
                            goto skok;

                        ileb = ((q.Length - r) / 32) ;
                          

                            if (q.Length-r % 32 == 0)
                            {
                                int m = 0;
                                int[] taby = new int[ileb];
                                while (nowa != ileb+nowa)
                                {
                                    if (poj == 192)
                                        koncz = 1;

                                    if (koncz == 1 && poj == 192)
                                    {
                                        Console.WriteLine("Plik może mieć maksymalnie 6 bloków!");
                                        break;
                                    }

                                    if (koncz == 1)
                                        break;

                                    taby[m] = iwezel.wolnemiejsce(ref o, ref x);

                                    for (int i = 0; i < 32; i++)
                                    {
                                        if (r == q.Length || poj==192)
                                        { koncz = 1; break; }

                                        x.dysk[taby[m]].bl[i] = q[r];
                                        r++;
                                        poj++;
                                    }

                                    if (nowa == 0)
                                    {
                                        x.tabi[v].pierwszyblok = taby[m];

                                    }

                                    if (nowa == 1)
                                    {
                                        x.tabi[v].drugiblok = taby[m];
                                        x.dysk[LBP.First()].nast = taby[m];

                                    }
                                    if (nowa == 2)
                                    {
                                        u = iwezel.wolnemiejsce(ref o, ref x);
                                        x.dysk[taby[m - 1]].nast = u;
                                        x.tabi[v].blokindeksowy = u;

                                        for (int i = 0; i < 32; i++)
                                            x.dysk[u].bl[i] = 65;       //wypelnienie bloku indeksowego liczbą(indeksem) nie nazlezącą do dysku
                                        //potrzebne do odczytania z bloku indeksowego kolejnych zajmowanych bloków 
                                        x.dysk[u].bl[0] = (byte)taby[m];
                                    }

                                    if (nowa == 3)
                                    {
                                        // x.dysk[taby[nowa - 1]].nast = taby[nowa];
                                        x.dysk[u].bl[1] = (byte)taby[m];

                                    }

                                    if (nowa == 4)
                                    {
                                        //x.dysk[taby[nowa - 1]].nast = taby[nowa];
                                        x.dysk[u].bl[2] = (byte)taby[m];

                                    }

                                    if (nowa == 5)
                                    {
                                        // x.dysk[taby[nowa - 1]].nast = taby[nowa];
                                        x.dysk[u].bl[3] = (byte)taby[m];
                                    }

                                    m++;
                                    nowa++;


                                }
                            }
                            if (q.Length-r % 32 != 0)
                            {   int m=0;
                                int[] taby2 = new int[ileb+1];
                                                                
                                while (nowa != ileb+1+nowa)
                                {
                                    if (poj == 192)
                                        koncz = 1;

                                    if (koncz==1 && poj==192)
                                    {
                                        Console.WriteLine("Plik może mieć maksymalnie 6 bloków!");
                                        break;
                                    }
                                    if (koncz == 1)
                                        break;

                                    taby2[m] = iwezel.wolnemiejsce(ref o, ref x);

                                    for (int i = 0; i < 32; i++)
                                    {
                                        if (r == q.Length || poj==192)
                                        { koncz = 1; break; }

                                        x.dysk[taby2[m]].bl[i] = q[r];
                                        r++;
                                        poj++;
                                    }
                                    
                                    if (nowa == 0)
                                    {
                                        x.tabi[v].pierwszyblok = taby2[m];

                                    }

                                    if (nowa == 1)                              //2 bloki danych 
                                    {
                                        x.tabi[v].drugiblok = taby2[m];
                                        x.dysk[LBP.First()].nast = taby2[m];
                                        
                                    }

                                    if (nowa == 2)
                                    {
                                        u = iwezel.wolnemiejsce(ref o, ref x);
                                        x.dysk[taby2[m - 1]].nast = u;
                                        x.tabi[v].blokindeksowy = u;

                                        for (int i = 0; i < 32; i++)
                                            x.dysk[u].bl[i] = 65;

                                        x.dysk[u].bl[0] = (byte)taby2[m];

                                    }

                                    if (nowa == 3)
                                    {
                                        // x.dysk[taby[nowa - 1]].nast = taby[nowa];
                                        x.dysk[u].bl[1] = (byte)taby2[m];

                                    }

                                    if (nowa == 4)
                                    {
                                        //x.dysk[taby[nowa - 1]].nast = taby[nowa];
                                        x.dysk[u].bl[2] = (byte)taby2[m];

                                    }

                                    if (nowa == 5)
                                    {
                                        // x.dysk[taby[nowa - 1]].nast = taby[nowa];
                                        x.dysk[u].bl[3] = (byte)taby2[m];
                                    }
                                    m++;
                                    nowa++;
                                }
                            }
                           
                        skok:
                            x.tabi[b].rozmiar += r;
                            x.tabi[v].rozmiar = x.tabi[v].rozmiar + r;
                            
                        
                    

                    }

                    //kon dopis



               }
            }
            return 0;
        }

        public static int create(ref pom x, string nazwa, char typ, string nazwfold)
            //funkcja służąca do utworzenia pliku o typie: "typ", o nazwie: "nazwa" w katalogu o nazwie: "nazwfold"
        {
            if (licz == 20)
            {
                Console.WriteLine("\nWięcej plików nie dodamy, ponieważ tablica i-węzłów może mieć maksymalnie 20 elementów ");
                return -1;
            }

            if (licz > 1)
            {
                int w,k;
                w = iwezel.czyistnieje(ref x, nazwa);
                k = iwezel.czyistnieje(ref x, nazwfold);

                if (w != -1)
                {
                    Console.WriteLine("Taka nazwa już istnieje");
                    return -1;
                }
                //sprawdzamy czy istnieje plik lub katalog o tej samej nazwie

                if (k == -1)
                {
                    Console.WriteLine("Nie ma takiego katalogu plik lub katalog nie zostanie utworzony");
                    return -1;

                }
            }


            if (typ == 'k')
            {
                iwezel z = new iwezel(ref x, typ);
                WpisK q = new WpisK(nazwa, (byte)licz);


                if(licz>=2)
                Console.WriteLine("Utworzono katalog: "+nazwa+" w katalogu: "+ nazwfold); 

                for (int i = 0; i < x.LK.Count(); i++)
                {
                    if (x.LK[i][0].nazwa == nazwfold)
                    {
                        x.LK[i].Add(q);
                        break;
                    }

                }
                x.LK.Add(new List<WpisK>());
                x.LK[x.LK.Count() - 1].Add(q);

                if (licz == 1)
                    Console.WriteLine("Utworzono katalog 'Root' oraz 'Home' \n"); 

            }


            if (typ == 'p')
            {
                bool ok = false;
                iwezel z = new iwezel(ref x, typ);
                WpisK q = new WpisK(nazwa, (byte)licz);

                
                for (int i = 0; i < x.LK.Count(); i++)
                {
                    for (int j = 0; j < x.LK[i].Count(); j++)
                    {
                        if (x.LK[i][j].nazwa == nazwa)
                            ok = true;
                    } //spr czy nie ma pliku o tej samej nazwie 
                }
                if (ok == true)
                {
                    Console.WriteLine("istnieje plik o takiej nazwie w tym katalogu!");
                    return -1;
                }

                Console.WriteLine("Utworzono plik: " + nazwa + " w katalogu: " + nazwfold); 

                for (int i = 0; i < x.LK.Count(); i++)
                {
                    if (x.LK[i][0].nazwa == nazwfold && ok == false)
                    { x.LK[i].Add(q); break; }
                }

            }

            licz++;
            return 0;
        }


        public static int delete(ref pom x, string nazwa, ref int o)     //funkcja służąca do usuwania pliku 
        {            
            int pom = iwezel.czyistnieje(ref x, nazwa);
            if (pom == -1)
            {
                Console.WriteLine("Nie ma takiego pliku");
                return -1;
            }

            if (pom == 1 || pom == 0)
            {
                Console.WriteLine("Nie można usunąć tego ważnego folderu");
                return -1;
            }

            bool ok = false;
            int m = 0,u=0,k=0;
            List<int> LBP = new List<int>();

            for (int i = 0; i < x.LK.Count(); i++)
            {
                for (int j = 0; j < x.LK[i].Count(); j++)
                {
                    if (x.LK[i][j].nriwezla == pom)
                    {
                        u = x.LK[i][0].nriwezla;
                        
                    }

                }
            }
            
            for (int i = 0; i < x.LK.Count(); i++)
            {
                if (m == 1)
                {
                    while (i != x.LK.Count())
                    {                        
                            for (int l = x.LK[i].Count() - 1; l > 0; l--)
                            {
                                x.LK[i][l].nriwezla -= 1;                       //poprawa wpisow katalogowych w następnych katalogach
                            }
                        
                        i++;
                    }


                    break;
                }

                for (int j = 0; j < x.LK[i].Count(); j++)
                {
                    if (m == 1)
                        break;

                    if (x.tabi[pom].Typ == 'p')
                    {
                        if (x.LK[i][j].nazwa == nazwa && x.LK[i][j].nriwezla >= u)
                        {
                            if (x.LK[i].ElementAt(x.LK[i].Count - 1) != x.LK[i][j])
                            {
                                for (int l = x.LK[i].Count() - 1; l >= x.LK[i][j].nriwezla; l--)
                                {
                                    x.LK[i][l].nriwezla -= 1;
                                }
                            }
                            //poprawa wpisow katalogowych jeśli usuwamy plik
                            x.LK[i].RemoveAt(j);
                            ok = true;
                            m = 1;
                            Console.WriteLine("usunięto plik o nazwie:" + nazwa);
                            break;
                        }
                    }

                    if (x.tabi[pom].Typ == 'k')
                    {
                        if (x.LK[i][0].nazwa == nazwa && x.LK[i][j].nriwezla >= u)
                        {
                            
                            for (int l = x.LK[i].Count() - 1; l > 0; l--)
                            {
                                iwezel.delete(ref x, x.LK[i][l].nazwa, ref o);
                            }

                            x.LK[i].Clear();
                            x.LK.RemoveAt(i);
                            Console.WriteLine("usunięto  katalog o nazwie:" + nazwa);
                            ok = true;
                            m = 1;
                            break;
                        }
                    }

                }

            }

            if (ok == false)
            {
                Console.WriteLine("Kazałeś usunąć ważny katalog!");
                return -1;
            }

            LBP = iwezel.listazblok(ref x, pom);

            if (x.tabi[pom].Typ == 'k')
            {
               
                for (int i = 0; i < x.LK.Count(); i++)
                {
                    for (int j = 0; j < x.LK[i].Count(); j++)
                    {
                        if (x.LK[i][j].nriwezla == pom)
                        {
                            x.LK[i].RemoveAt(j);
                        }

                    }
                }

                for (int i = 0; i < x.LK.Count(); i++)
                {
                    for (int j = 0; j < x.LK[i].Count(); j++)
                    {
                        if (x.LK[i][j].nriwezla > u)
                        {
                            x.LK[i][j].nriwezla--;
                        }

                    }
                }




                x.tabi[pom] = null;

                if (pom == licz - 1) //jeśli na końcu w tablicy to zmniejszamy licznik elementów w tablicy i wychodzimy
                {
                    licz--;
                    return 0;
                }
                if (pom < licz - 1)
                {
                    int pom2 = 0;
                    // w innym wypadku musimy przepisać obiekty do naszej tablicy za pomocą tablicy pomocniczej
                    iwezel[] tabpom = new iwezel[20];


                    for (int i = 0; i < x.tabi.Count(); i++)
                    {
                        if (x.tabi[i] != null)
                        {
                            tabpom[pom2] = x.tabi[i];
                            pom2++;

                        }

                    }

                    x.tabi = tabpom;

                }

            }

            if (x.tabi[pom].Typ == 'p')
            {

                byte[] tp = new byte[32];  

                for (int i = 0; i < LBP.Count; i++)
                {
                    o = o - 1;
                                                                     //zwolnienie zajetych bloków przez plik
                    x.dysk[LBP.ElementAt(i)].nast = -1;
                    x.dysk[LBP.ElementAt(i)].bl = tp;
                    
                }

                if (x.tabi[pom].blokindeksowy != -1)
                {
                    x.dysk[x.tabi[pom].blokindeksowy].nast = -1;
                    x.dysk[x.tabi[pom].blokindeksowy].bl = tp;
                    o--;
                }


                

                x.tabi[u].rozmiar -= x.tabi[pom].rozmiar;

                x.tabi[pom] = null;


                if (pom == licz - 1) //jeśli na końcu w tablicy to zmniejszamy licznik elementów w tablicy i wychodzimy
                {
                    licz--;
                    return 0;
                }
                if (pom < licz - 1)
                {
                    int pom2 = 0;
                    // w innym wypadku musimy przepisać obiekty do naszej tablicy za pomocą tablicy pomocniczej
                    iwezel[] tabpom = new iwezel[20];


                    for (int i = 0; i < x.tabi.Count(); i++)
                    {
                        if (x.tabi[i] != null)
                        {
                            tabpom[pom2] = x.tabi[i];
                            pom2++;

                        }

                    }

                    x.tabi = tabpom;

                }
            }
            
            licz--;
            return 0;

        }
        public static int chname(ref pom x, string co, string naco) //funkcja zmienia nazwę pliku z "co" na "naco"
        {
            bool ok = false;
            try
            {
                for (int i = 0; i < x.LK.Count(); i++)
                {
                    for (int j = 0; j < x.LK[i].Count(); j++)
                    {
                        if (x.LK[i][j].nazwa == co)
                        {
                            x.LK[i][j].nazwa.Replace(co, naco);
                            ok = true;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Błąd: \n", ex.Message);
                return -1;
            }
            if (ok == false)
                return -1;
            return 0;
        }

        public static int chacces(ref pom x, string nazwa, byte pd1, byte pd2, byte pd3)
            //funkcja służąca do zmiany praw dostępu do pliku
        {

            int z = 0;
            z = iwezel.czyistnieje(ref x, nazwa);

            if (z == -1 || z == 0)
            {
                return -1;
            }
            x.tabi[z].prawa1 = pd1;
            x.tabi[z].prawa2 = pd2;
            x.tabi[z].prawa3 = pd3;


            return 0;
        }

        public static int chidu(ref pom x, string nazwa, string idu) //funkcja służąca do zmiany UID
        {
            int z = 0;
            z = iwezel.czyistnieje(ref x, nazwa);

            if (z == -1 || z == 0)
            {
                return -1;
            }

            x.tabi[z].IDuzytkownika.Replace(x.tabi[z].IDuzytkownika, idu);


            return 0;
        }

        public static int chidg(ref pom x, string nazwa, string idg) //funkcja służąca do zmiany GID
        {
            int z = 0;
            z = iwezel.czyistnieje(ref x, nazwa);

            if (z == -1 || z == 0)
            {
                return -1;
            }

            x.tabi[z].IDgrupy.Replace(x.tabi[z].IDgrupy, idg);


            return 0;
        }
    }

