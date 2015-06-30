using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;

namespace PW1
{
    class Program
    {
        static void metodaWatku()
        {
            //nie robi nic
        }

        static void Main(string[] args)
        {

            Stopwatch pomiarUtworzenia = new Stopwatch();
            Stopwatch pomiarZakonczenia = new Stopwatch();

            int sredniaUtworzenia = 0, sredniaZakonczenia = 0, sumaUtworzenia = 0, sumaZakonczenia = 0;

            int iloscProb = 100;

            for (int i = 0; i < iloscProb; i++)
            {
                Thread thread = new Thread(metodaWatku);

                pomiarUtworzenia.Start();
                thread.Start();
                pomiarUtworzenia.Stop();

                pomiarZakonczenia.Start();
                thread.Abort();
                pomiarZakonczenia.Stop();


                sumaUtworzenia += Convert.ToInt32(pomiarUtworzenia.ElapsedMilliseconds);
                sumaZakonczenia += Convert.ToInt32(pomiarZakonczenia.ElapsedMilliseconds);
            }


            sredniaUtworzenia = sumaUtworzenia / iloscProb;
            sredniaZakonczenia = sumaZakonczenia / iloscProb;

            Console.WriteLine("Sredni czas utworzenia watku : " + sredniaUtworzenia + " ms");
            Console.WriteLine("Srednia czas zakonczenia watku : " + sredniaZakonczenia + "ms");
        }
    }
}
