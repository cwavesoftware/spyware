using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.ServiceProcess;

namespace ControlPanel
{
    public partial class Check : Form
    {
        public Check(DirectoryInfo di)
        {
            InitializeComponent();
            rootdir = di;
        }
        DirectoryInfo rootdir;

        private void Check_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists(rootdir.FullName) || !Directory.Exists(rootdir.FullName + "\\Logs")
                                                    || !Directory.Exists(rootdir.FullName + "\\Screencaptures"))
                adlabel.Text = "Directory tree status: INVALID DIRECTORY TREE";
            else
                adlabel.Text = "Directory tree status: OK";

            if (File.Exists(rootdir.FullName + "\\winsvc.exe"))
                dalabel.Text = "Desktop agent status: OK";
            else
                dalabel.Text = "Desktop agent status: NOT PRESENT";

            ServiceController sc = new ServiceController("winsvc-launcher");
            bool scnull = false;
            try { string proba=sc.ServiceName; }
            catch { scnull = true; }
            if (!scnull && File.Exists(rootdir.FullName + "\\winsvc-launcher.exe"))
                salabel.Text = "Service agent status: OK";
            else
                if (scnull && !File.Exists(rootdir.FullName + "\\winsvc-launcher.exe"))
                    salabel.Text = "Service agent status: NOT PRESENT, NOT INSTALLED";
                else
                    if (scnull)
                        salabel.Text = "Service agent status: Present, NOT INSTALLED";
                    else
                        salabel.Text = "Service agent status: Installed, NOT PRESENT";
            sc.Close();
            sc.Dispose();
            if (!File.Exists(rootdir + "\\Monitorizare.conf"))
                cflabel.Text="Configuration file status: NOT FOUND";
            else
            {
                try
                {
                    using (StreamReader sr = new StreamReader(rootdir + "\\Monitorizare.conf"))
                    {
                        uint.Parse(sr.ReadLine());
                        uint.Parse(sr.ReadLine());
                        cflabel.Text = "Configuration file status: OK";
                        sr.Close();
                    }
                }
                catch
                {
                    cflabel.Text = "Configuration file status: INVALID";
                }
            }
        }

        private void Check_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Convert.ToByte(e.KeyChar) == 13)
                this.Close();
        }
    }
}
