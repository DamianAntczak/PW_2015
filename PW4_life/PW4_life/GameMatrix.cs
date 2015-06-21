using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PW4_life
{
    class GameMatrix
    {
        bool[,] matrix;

        Mutex mut = new Mutex(false);

        Random rand;

        public GameMatrix(int szerokoscMacierzy)
        {
            rand = new Random();

            matrix = new bool[szerokoscMacierzy, szerokoscMacierzy];

            for (int i = 0; i < szerokoscMacierzy; i++)
            {
                for (int j = 0; j < szerokoscMacierzy; j++)
                {
                    matrix[i,j] = randInit();
                }
            }


        }

        private bool randInit()
        {
            

            if (rand.Next(1, 100) > 50)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool getState(int wiersz, int kolumna)
        {
            lock (this)
            {
                return matrix[wiersz, kolumna];

            }
        }

        public void setState(int wiersz, int kolumna, bool stan)
        {
            matrix[wiersz, kolumna] = stan;
        }
    }
}
