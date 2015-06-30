using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PW_4_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        TimeSpan time, addTime;
        ScaleTransform st = new ScaleTransform(0.0,1);

        int wynik;
        bool koniec = false;
        int licz = 0;
        int sleep = 100;
        double start = 0.0;

        public MainWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            time = new TimeSpan(0, 0, 0);
            addTime = new TimeSpan(0, 0, 1);
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(TimeSpan.TicksPerSecond);
            
            
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            time -= addTime;
            lCzasu.Content = "Koniec obliczen za: " + time.TotalSeconds;
        }

        public void LiczSilnie(int n)
        {
            wynik = 1;
            if (n == 1 || n == 0)
                wynik = 1;
            else
            {
                for (int i = n; i >= 1; i--)
                {
                    wynik *= i;
                }
            }
            while (true)
            {
                if (koniec)
                {
                    Silnia.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal,
                     new Action(() => Silnia.Text = wynik.ToString() + " " + licz));
                    timer.Stop();
                    break;
                }
                else
                {
                    Thread.Sleep(sleep);

                }

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            koniec = false;
            licz = 0;
            time = TimeSpan.Zero;
            start = 0.0;
            wynik = 0;
            st.ScaleX = start;
            int n = Convert.ToInt32(Silnia.Text);
            Task t2 = Task.Run(() => LiczSilnie(n));

            while (wynik == 0) ;
            double s = (double)wynik / 10.0;
            time = new TimeSpan(0, 0, (int)Math.Ceiling(s));
            timer.Start();
            Task t1 = Task.Run(() =>
            {
                double x = 1.0 / wynik;

            });

        }
    }
}
