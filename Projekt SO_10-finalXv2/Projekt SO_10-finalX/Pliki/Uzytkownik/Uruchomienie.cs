using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


    class Uruchomienie
    {

        string sciezka = "D:\\SO\\logi.txt";
        public int spr()
        {
            string s = File.ReadAllText(sciezka);
            if (s == "administrator")
                return 1;
            else return 0;
        }
    
       
    }

