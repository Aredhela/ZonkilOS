using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


    public class Lacze
    {
        public string name { get; set; }
        public int source { get; set; }
        public int destination { get; set; }
        public string bufor;
    }


    class Lacza
    {

        //.////////////////////////////////////////////////////////  Metody Komentarzy (Rozkazów) ///////////////////////////////////////////


        public void ClearOrder(proces p, string r)
        {
            p.rozkazy.Remove(r);
        }
        public void przekazrozkaz(proces a, proces b, int ktory)
        {
            string pomnazwa = a.PID + "-" + b.PID;
            if (File.Exists(Projekt_SO_10_finalX.Properties.Settings.Default.Path + pomnazwa + ".xml") == true)
            {
                if (a.rozkazy[ktory - 1] != "")
                {
                    string rozkaz = a.rozkazy[ktory - 1];
                    b.rozkazy.Add(rozkaz);
                    a.rozkazy.Remove(rozkaz);
                    Console.WriteLine("Przekazano");
                }
                else
                { Console.WriteLine("Nie ma rozkazu o takim numerze."); }
            }
            else
            { CommunicationNoNamed(a, b, ktory); }
        }
        //.//////////////////////////////////////////////////////// Metody Komunikacji ///////////////////////////////////////////////////////


        //.//////////////// Komunikacja Nienazwana ///////////////

        public void CommunicationNoNamed(proces p1, proces p2, int ktory)
        {
            var pro = ServiceLocator.GetService<ZarzadcaProcesow>().kolejka.k_wszystkich.Find(z => z.PID == p1.PID);
            if (pro.potomkowie.Exists(z => z.PID == p2.PID) == true)
            {
                var pro1 = ServiceLocator.GetService<ZarzadcaProcesow>().kolejka.k_wszystkich.Find(z => z.PID == p2.PID);
                if (pro1.rozkazy[ktory - 1] != "")
                {
                    string rozkaz = pro1.rozkazy[ktory - 1];
                    pro1.rozkazy.Add(rozkaz);
                    pro.rozkazy.Remove(rozkaz);
                    Console.WriteLine("Przekazano");
                }
                else
                { Console.WriteLine("Nie ma rozkazu o takim numerze."); }

            }
            else
            {
                Console.WriteLine("Procesy nie są spokrewnione lub nie istnieje pomiedzy nimi łącze");
            }
        }

        //.//////////////// Komunikacja Nazwana /////////////////
        public List<String> ListaLaczy = new List<String>();

        public void UtworzLacze(proces p1, proces p2)
        {
            Lacze d = new Lacze();
            d.name = Convert.ToString(p1.PID) + "-" + Convert.ToString(p2.PID);
            d.source = p1.PID;
            d.destination = p2.PID;
            d.bufor = "";

            System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Lacze));
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(Projekt_SO_10_finalX.Properties.Settings.Default.Path + d.name + ".xml"))
            {
                writer.Serialize(file, d);
                file.Close();
            }

        }
        public void UsunLacze(string lacze)
        {
            if (File.Exists(Projekt_SO_10_finalX.Properties.Settings.Default.Path + lacze + ".xml") == true)
            {
                File.Delete(Projekt_SO_10_finalX.Properties.Settings.Default.Path + lacze + ".xml");
                Console.WriteLine("Usunieto");  
            }
            else
                Console.WriteLine("Nie ma takiego lacza");  

        }
        public void CommunicationNamed(proces p1, proces p2, string rozkaz)
        {
            string pomnazwa = p1.PID + "-" + p2.PID;

            Lacze d=new Lacze();
            if (File.Exists(Projekt_SO_10_finalX.Properties.Settings.Default.Path + pomnazwa + ".xml") == true)
            {
                d.name = Convert.ToString(p1.PID) + "-" + Convert.ToString(p2.PID);
                d.source = p1.PID;
                d.destination = p2.PID;
                d.bufor = rozkaz;

                System.Xml.Serialization.XmlSerializer writer = new System.Xml.Serialization.XmlSerializer(typeof(Lacze));
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Projekt_SO_10_finalX.Properties.Settings.Default.Path + d.name + ".xml"))
                {
                    writer.Serialize(file, d);
                    file.Close();
                }
                System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(Lacze));
                using (System.IO.StreamReader file1 = new System.IO.StreamReader(Projekt_SO_10_finalX.Properties.Settings.Default.Path + pomnazwa + ".xml"))
                {
                    d = (Lacze)reader.Deserialize(file1);
                    p2.rozkazy.Add(d.bufor);
                    d.bufor = "";
                    Console.WriteLine("Przekazano");               
                }



                d.name = Convert.ToString(p1.PID) + "-" + Convert.ToString(p2.PID);
                d.source = p1.PID;
                d.destination = p2.PID;
                d.bufor = "";

                System.Xml.Serialization.XmlSerializer writer1 = new System.Xml.Serialization.XmlSerializer(typeof(Lacze));
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(Projekt_SO_10_finalX.Properties.Settings.Default.Path + d.name + ".xml"))
                {
                    writer1.Serialize(file, d);
                    file.Close();
                }




            }
            else
            { Console.WriteLine("Nie ma takiego Łącza"); }

        }

    }

