using  System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;



    public class proces
    {
        public proces rodzic;
        public int PID;
        public List<proces> potomkowie = new List<proces>();
        public int stan_procesu;
        public uint[] rej_stalo = new uint[4];
        // wskaznik na program wykonujący (kod programu)
        public int użytkownik;
        public int[] priorytet = new int[3];
        public int l_rozk;
        // Adres początkowy stosu dla tego procesu
        public int heapAddrStart = 0;
        // Adres końcowy stosu dla tego procesu
        public int heapAddrEnd = 0;
        // ArrayList MemoryPages, które są związane ze stosem dla tego procesu
        public ArrayList heapPageTable = new ArrayList();
        public List<String> rozkazy = new List<String>();

        
        public proces(int pid)
        {
            PID = pid;
        }

        public proces(int pid, int fpriorytet)
        {
            PID = pid;
            priorytet[0] = fpriorytet;

        }

    }






    public class ZarzadcaProcesow
    {
      

        public Kolejki kolejka = new Kolejki();

        public proces utworz(proces rodzic, int name, int priorytet)
        {
            bool bExist;

            bExist = kolejka.k_wszystkich.Exists(oElement => oElement.PID.Equals(name));
            if (bExist == true)
            {
                throw Exception();
            }

            proces dziecko = new proces(name);
            dziecko.rodzic = rodzic;
            dziecko.stan_procesu = rodzic.stan_procesu;
            dziecko.priorytet[1] = rodzic.priorytet[1] = priorytet;
            dziecko.priorytet[2] = rodzic.priorytet[2];
            dziecko.priorytet[0] = rodzic.priorytet[0] = priorytet;
            dziecko.l_rozk = rodzic.l_rozk;
            rodzic.potomkowie.Add(dziecko);
            kolejka.k_nowy.Add(dziecko);
            kolejka.k_wszystkich.Add(dziecko);
            return dziecko;


        }
        public proces utwProDziecko(int PPID, int PID, int fprio)
        {
            int x1 = 0;
            for (int i = 1; i < kolejka.k_wszystkich.Count; i++)
            {
                if (kolejka.k_wszystkich[i].PID == PPID)
                {
                    x1 = i;
                }

            }
            proces rodzic = kolejka.k_wszystkich[x1];
            bool bExist;

            bExist = kolejka.k_wszystkich.Exists(oElement => oElement.PID.Equals(PID));
            if (bExist == true)
            {
                throw Exception();
            }

            proces dziecko = new proces(PID);
            dziecko.rodzic = rodzic;
            dziecko.stan_procesu = rodzic.stan_procesu;
            dziecko.priorytet[1] = rodzic.priorytet[1] = fprio;
            dziecko.priorytet[2] = rodzic.priorytet[2];
            dziecko.priorytet[0] = rodzic.priorytet[0] = fprio;
            dziecko.l_rozk = rodzic.l_rozk;
            rodzic.potomkowie.Add(dziecko);
            kolejka.k_nowy.Add(dziecko);
            kolejka.k_wszystkich.Add(dziecko);
            return dziecko;

        }

        public void exec(proces pr)
        {

            
            
               kolejka.k_nowy.Remove(pr);
                kolejka.k_gotowy.Add(pr);
                zmiensta(pr,1);
            
            
               
            


        }


        private void zmiensta(proces pr, int sta) //zmienia stan procesu
        {
            int pop_stan; // stan poprzedni
            pop_stan = pr.stan_procesu;
            if (sta > -1 && sta < 5)
            {
                pr.stan_procesu = sta;

            }
            else throw Exception();
        }

        public void wypisz(proces pr)
        {
            Console.WriteLine(pr.PID);
            foreach (proces a in pr.potomkowie)
                Console.WriteLine(a.PID);
        }
        public void wypisz2()
        {
            Console.Write("Proces PID( INIT ): " + kolejka.k_wszystkich[0].PID + "\t\tKom:");
            for (int j = 0; j < kolejka.k_wszystkich[0].rozkazy.Count; j++)
                Console.Write(kolejka.k_wszystkich[0].rozkazy[j] + ", ");
            Console.Write("\n");
            for (int i = 1; i < kolejka.k_wszystkich.Count; i++)
            {
              
                Console.Write("Proces PID: " + kolejka.k_wszystkich[i].PID +"\t Rodzic PPID: " + kolejka.k_wszystkich[i].rodzic.PID + "\tKom:" );
                for (int j = 0; j < kolejka.k_wszystkich[i].rozkazy.Count; j++)
                Console.Write(kolejka.k_wszystkich[i].rozkazy[j] + ", ");
                Console.Write("\n");
            }
        }
        private System.Exception Exception()
        {
            Console.WriteLine("błąd tworzenia procesu");
            throw new NotImplementedException();
            
        }


           public void dodaj_doWsz(proces proPCB)
        {
            kolejka.k_wszystkich.Add(proPCB);
        }
         

           public void usun_z_k(int fPID)//usuwa z kol wszyst(jeden)
           {
              
               int x1=-1;
               int x2 = -2;
               for (int i = 0; i < kolejka.k_gotowy.Count; i++)
               {
                   if (kolejka.k_gotowy[i].PID == fPID)
                   {
                       x2 = i;
                   }

               }


               kolejka.usunZKol2(x2);
               for (int i = 1; i < kolejka.k_wszystkich.Count; i++)
               {
                   if (kolejka.k_wszystkich[i].PID == fPID)
                   {
                       x1 = i; 
                   }
                   
               }
               if (x1 == -1)
               {
                   Console.WriteLine("Błąd nie ma takiego procesu");
               }
               else
               {
                   
                   kolejka.usunZKol(x1);
                   
               }
               for (int i = 1; i < kolejka.k_wszystkich.Count; i++)
               {
                   if (kolejka.k_wszystkich[i].rodzic.PID==fPID)
                   {
                       kolejka.k_wszystkich[i].rodzic.PID = 1;
                   }  
 
               }

             

               
             

           }

           public proces znajdz(int fPID)//usuwa z kol wszyst(jeden)
           {


               int x1 = -1;
               for (int i = 1; i < kolejka.k_wszystkich.Count; i++)
               {
                   if (kolejka.k_wszystkich[i].PID == fPID)
                   {
                       x1 = i;
                   }

               }
             

                  return kolejka.k_wszystkich[x1];

               
              

           }
    }

    public class Kolejki
    {


        public List<proces> k_wszystkich = new List<proces>();
        public List<proces> k_nowy = new List<proces>();
        public List<proces> k_gotowy = new List<proces>();
        public List<proces> k_oczekuj = new List<proces>();



        public proces wyslijproces()
        {
            /*
            proces pr = null;

            if (k_gotowy[0] == null)
            {
                pr = k_wszystkich[0];
            }

            pr = k_gotowy[0];
                k_gotowy.Remove(k_gotowy[0]);
              return pr;
            */


            proces pr = null;
            if (k_gotowy.Count == 0)
                return pr;
            pr = k_gotowy[0];
            k_gotowy.Remove(k_gotowy[0]);
            return pr;
        }
        public void wypiszkolejki()
        {
            for (int i = 0; i < k_wszystkich.Count; i++)
            {
                Console.Write("Kolejka wszystkich:");
                Console.WriteLine(" " + k_wszystkich[i].PID + " , ");
            }

            for (int i = 0; i < k_gotowy.Count; i++)
            {
                Console.Write("Kolejka gotowych:");
                Console.WriteLine(" " + k_gotowy[i].PID + " , ");
            }
            for (int i = 0; i < k_oczekuj.Count; i++)
            {
                Console.Write("Kolejka oczekujących:");
                Console.WriteLine(" " + k_oczekuj[i].PID + " , ");
            }

        }

        public void usunZKol(int a)
        {
            k_wszystkich.RemoveAt(a);

        }
        public void usunZKol2(int a)
        {
            k_gotowy.RemoveAt(a);

        }
        public void usunZKolOczekuj(int a)
        {
            k_oczekuj.RemoveAt(a);
        }

        public void dodajdoOczekuj(proces a)
        {
            k_oczekuj.Add(a);
        }
        public List<proces> nazwa_kol(proces proces)
        {
            int stan;
            //List nazwakolejki = "NULL";
            stan = proces.stan_procesu;
            if (stan == 0)
            {
                return k_nowy;
            }
            if (stan == 1)
            {
                return k_gotowy;
            }
            if (stan == 2)
            {
                return k_gotowy;
            }
            else
            {
                return k_oczekuj;
            }

        }

        public List<proces> GOTOWE()
        {
            return k_gotowy;
        }
    }



