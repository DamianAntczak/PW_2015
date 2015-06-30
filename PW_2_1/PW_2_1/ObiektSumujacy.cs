using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PW_2_1
{

    class ObiektSumujacy
    {
        private const int rozmiarTablicy = 10000;
        private int suma = 0;
        private object m_SyncObject = new object();
        private int[] tab;
        private Stopwatch stopwatch = new Stopwatch();

        public void sumujTablice(int poczatek, int koniec)
        {
            int licznik = 0;
            for (var i = poczatek; i <= koniec; i++)
            {
                licznik += tab[i];
            }
            lock (m_SyncObject)
            {
                suma += licznik;
            }
        }


        public void Sumuj()
        {
            tab = new int[rozmiarTablicy];
            Random rnd = new Random();

            Thread watek1, watek2, watek3, watek4 , watek5, watek6;

            for (var i = 0; i < tab.Length; i++) // losowe wypelnianie tablicy
            {
                tab[i] = rnd.Next(0, 1000);
            }

            watek1 = new Thread(() => sumujTablice(0,9999));

            stopwatch.Start();

            watek1.Start();
            watek1.Join();

            stopwatch.Stop();
            

            Console.WriteLine("1 watkek suma- " + suma + " czas " + stopwatch.ElapsedTicks.ToString() + " taktow");

            stopwatch.Reset();
            suma = 0; //zerowanie wyniku

            watek1 = new Thread(() => sumujTablice(0, 5000));
            watek2 = new Thread(() => sumujTablice(5001, 9999));

            stopwatch.Start(); // start pomiaru czasu

            watek1.Start();
            watek2.Start();

            watek1.Join();
            watek2.Join();

            stopwatch.Stop(); // koniec pomiaru czasu

            Console.WriteLine("2 watki suma- " + suma + " czas " + stopwatch.ElapsedTicks.ToString() + " taktow");
            stopwatch.Reset();
            suma = 0;

            watek1 = new Thread(()=>sumujTablice(0,2500));
            watek2 = new Thread(()=>sumujTablice(2501,5000));
            watek3 = new Thread(() => sumujTablice(5001, 7500));
            watek4 = new Thread(() => sumujTablice(7501,9999));

            stopwatch.Start();

            watek1.Start();
            watek2.Start();
            watek3.Start();
            watek4.Start();

            watek1.Join();
            watek2.Join();
            watek3.Join();
            watek4.Join();

            stopwatch.Stop();

            Console.WriteLine("4 watki suma- " + suma + " czas " + stopwatch.ElapsedTicks.ToString() +" taktow");

        }
    }
}
