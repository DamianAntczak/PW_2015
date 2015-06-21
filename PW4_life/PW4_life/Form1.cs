using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PW4_life
{


    public partial class Form1 : Form
    {
        Panel panel;
        static GameMatrix gameMatrix = new GameMatrix(rozmiarPlanszy);
        Thread[,] watki;

        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();


        const int rozmiarPlanszy = 10;

        public Form1()
        {
            InitializeComponent();
            this.Height = 524;
            this.Width = 512;


            watki = new Thread[rozmiarPlanszy , rozmiarPlanszy];


            panel = new Panel();
            panel.Size = new System.Drawing.Size(500, 500);


            for (int i = 0; i < rozmiarPlanszy - 1; i++)
            {
                for (int j = 0; j < rozmiarPlanszy - 1 ; j++)
                {
                    ThreadStart starter = () => funkcjaWatku(i, j, gameMatrix);
                    watki[i,j] = new Thread(starter);

                    watki[i, j].Start();
                }
            }

            Controls.Add(panel);


            timer.Tick += new EventHandler(timer_Tick); // Everytime timer ticks, timer_Tick will be called
            timer.Interval = (10) * (1);              // Timer will tick evert second
            timer.Enabled = true;                       // Enable the timer
            timer.Start();  

        }

        private void Form1_Paint(object sender,PaintEventArgs e)
        {
            Pen pBlack = new Pen(Color.Black,1);
            Pen p2 = new Pen(Color.Red, 5);

            Graphics g = this.panel.CreateGraphics();
            g.Clear(Color.White);



            //g.DrawLine(pBlack,1,1,100,100);

            rysujKomorki(g, p2);

            rysujLinie(g, pBlack);



        }

        private void rysujLinie(Graphics g, Pen p)
        {
            //linie pionowe

            for (int i = this.panel.Width / rozmiarPlanszy; i < this.panel.Width; i += (this.panel.Width / rozmiarPlanszy))
            {
                g.DrawLine(p, i, 0, i, this.panel.Height);
            }

            //linie poziome

            for (int i = this.panel.Height / rozmiarPlanszy; i < this.panel.Height; i += (this.panel.Height / rozmiarPlanszy))
            {
                g.DrawLine(p, 0, i,this.panel.Width, i);
            }

            int szerokoscKomorki = this.panel.Width / rozmiarPlanszy;
            int wysokoscKomorki = this.panel.Height / rozmiarPlanszy;

            //g.DrawRectangle(p, 3 * szerokoscKomorki, 4 * wysokoscKomorki, szerokoscKomorki,  wysokoscKomorki);
        }

        private void rysujKomorki(Graphics g, Pen p)
        {
            int szerokoscKomorki = this.panel.Width / rozmiarPlanszy;
            int wysokoscKomorki = this.panel.Height / rozmiarPlanszy;

            SolidBrush b = new SolidBrush(Color.Red);

            for(int i = 0 ; i < rozmiarPlanszy ; i++)
            {
                for(int j = 0; j < rozmiarPlanszy ; j++){

                    if (gameMatrix.getState(i, j)== true)
                    {
                        
                        g.FillRectangle(b, i * szerokoscKomorki, j * wysokoscKomorki, szerokoscKomorki, wysokoscKomorki);
                    }
                }

            }

        }

        static void funkcjaWatku(int i, int j, GameMatrix gameMatrix )
        {
            int suma = 0;

            for (int k = -1; k <= 1; k++)
            {
                for (int w = -1; w <= 1; w++)
                {

                    if (i > 1 && i < rozmiarPlanszy - 1 && j > 1 && j < rozmiarPlanszy - 1)
                    {
                        if (k == 0 && w == 0)
                        {
                            gameMatrix.setState(i, j, false);
                        }
                        else
                        {
                            suma += sprawdzSasiada(i + k, j + w, gameMatrix);
                        }

                    }
                }

            }


            if (suma == 3)
            {
                gameMatrix.setState(i, j, true);

            }
            else
            {
                gameMatrix.setState(i, j, false);
            }



            Thread.Sleep(20);
        }

        static  int sprawdzSasiada(int i, int j , GameMatrix gameMatrix)
        {
            if (gameMatrix.getState(i, j) == true)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public void draw()
        {

                Pen pBlack = new Pen(Color.Black, 1);
                Pen p2 = new Pen(Color.Red, 5);

                Graphics g = this.panel.CreateGraphics();
                g.Clear(Color.White);



                //g.DrawLine(pBlack,1,1,100,100);

                rysujKomorki(g, p2);

                rysujLinie(g, pBlack);

                System.Console.Out.WriteLine("dsfsdf");

        }


        void timer_Tick(object sender, EventArgs e)
        {
            draw();
        }

    }
}
