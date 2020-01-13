using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ControlPanel
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
            StartupForm stf = new StartupForm();
            Login loginform = new Login();
            if (stf.ShowDialog() == DialogResult.Yes)
                if (loginform.ShowDialog() == DialogResult.OK)
                    Application.Run(new ControlPanel());
        }
    }
}
