using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PW4_life
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            Form1 myForm = new Form1();
            Application.Idle += new EventHandler(Application_Idle);
            Application.Run(myForm);
        }



        static void Application_Idle(object sender, EventArgs e)
        {

        }

    }
}
