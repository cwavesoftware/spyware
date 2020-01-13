using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace Monitorizare
{
    static class Program
    {
        static Mutex singleton = new Mutex(true, "My App Name");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] argv)
        {
            // Avoid duplicate running
            if (!singleton.WaitOne(TimeSpan.Zero, true))
            {
                // There is already another instance running!
                return;
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
