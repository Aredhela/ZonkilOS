using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


class Logowanie
{
    public
    string login, haslo;
    string sciezka = "D:\\SO\\logi.txt";
    public
        Logowanie()
    {
    }
    public void Log()
    {
        Console.Write("podaj login:");
        login = Console.ReadLine();
        Console.Write("podaj hasło:");
        haslo = Console.ReadLine();

    }
    public int spr()
    {
        int i = 2;
        string[] t = File.ReadAllLines(sciezka);
        if (t[0] == login && t[1] == haslo)
            return 2;
        do
        {

            if (t[i] == login && t[i + 1] == haslo)
            { return 1; }
            i = i + 3;
        } while (true);


    }
}



