using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PW_2_5
{
    class Tablica
    {
        private int[] tab;
        private bool czyZnaleziono; 

        private object obiektSynchronizacji = new object();

        public void szukajElementu(int poczatek, int koniec, int element)
        {
            for (int i = poczatek; i <= koniec; i++)
            {
                if (tab[i] == element)
                {
                    lock(obiektSynchronizacji)
                    {
                        czyZnaleziono = true;
                    }
                    break;
                }

            }

        }

        public bool czyJestElement()
        {
            return czyZnaleziono;
        }

        public Tablica(int[] tab)
        {
            this.tab = tab;
        }

    }


    class Program
    {
        static void Main(string[] args)
        {
            int[] tab= { 2, 4, 4, 6, 7, 8, 12, 4, 15, 28,16, 17, 15 };
            int dlugosc_tab = tab.Length;

            int ilosc_watkow = 4;
            int krok = dlugosc_tab / ilosc_watkow;
            Console.Out.WriteLine(krok);

            Thread[] tablicaWatkow = new Thread[ilosc_watkow];


            Tablica tablica = new Tablica(tab);

            int element = 0;

            Console.Out.WriteLine("Podaj szukany element w tablicy");
            string podanaLiczba = Console.In.ReadLine();

            element = int.Parse(podanaLiczba);

            int ostatni = 0;

            for (int i = 0; i < ilosc_watkow  ; i++)
            {
                tablicaWatkow[i] = new Thread(delegate()
                {
                    if (dlugosc_tab % 2 == 0)
                    {
                        tablica.szukajElementu(ostatni, ostatni + krok, element);
                    }
                    else if (i == ilosc_watkow - 1)
                    {
                        tablica.szukajElementu(ostatni, ostatni + krok + 1, element);
                    }
                    ostatni += krok;
                });
                tablicaWatkow[i].Start();
            }



            foreach(Thread watek in tablicaWatkow){
                watek.Join();
            }

            if (tablica.czyJestElement())
            {
                Console.Out.WriteLine("Znaleziono element : " + element + " w tabicy");
            }
            else
            {
                Console.Out.WriteLine("Nie znalezionow elementu : " + element + " w tabicy");
            }
        }
    }



}
