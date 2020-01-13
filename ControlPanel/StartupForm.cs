using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.AccessControl;
using System.IO;
using System.Diagnostics;
using Monitorizare;

namespace ControlPanel
{
    public partial class StartupForm : Form
    {
        public StartupForm()
        {
            InitializeComponent();
            rootdir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Monitorizare");
            int y = DateTime.Now.Date.Year;
            int M = DateTime.Now.Date.Month;
            int d = DateTime.Now.Date.Day;
            machine = Environment.MachineName;
            string date4fname = String.Format("{0}.{1:00}.{2:00}", y, M, d);
            Logfile = new FileInfo(String.Format("{0}\\Logs\\{1}_{2}.log", rootdir.FullName, machine, date4fname));
            fullusername = Machine.getInstance().getFullUsername();            
        }

        string fullusername,machine;
        DirectoryInfo rootdir;
        public static FileInfo Logfile;

        private void extractorButton_Click(object sender, EventArgs e)
        {
            if (extractorLocationDialog.ShowDialog() == DialogResult.OK)
            {
                #region Grab failed logons
                EventLog evl;
                evl = new EventLog("Security");
                try
                {
                    foreach (EventLogEntry entry in evl.Entries)
                    {
                        if (entry.InstanceId == 4625)
                        {
                            string details = "";
                            if (entry.UserName != null)
                                details = entry.UserName + " tried to logon but entered a wrong password.";
                            LogEntry le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.UserFailureLogon, details, entry.TimeGenerated, machine);
                            evl.Clear();
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(System.Security.SecurityException))
                    {
                        MessageBox.Show("You need to run application as administrator.", "Control Panel", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Environment.Exit(0);
                    }
                }
                #endregion
                #region Files extrection
                uint filescopied = 0;
                DirectoryInfo logdestdir = Directory.CreateDirectory(extractorLocationDialog.SelectedPath + "\\Monitorizare\\Logs");
                DirectoryInfo logsourcedir = new DirectoryInfo(rootdir.FullName + "\\Logs");
                FileInfo[] files = logsourcedir.GetFiles();
                //int y = DateTime.Now.Date.Year;
                //int M = DateTime.Now.Date.Month;
                //int d = DateTime.Now.Date.Day;
                //string date4fname = String.Format("{0}.{1:00}.{2:00}", y, M, d);
                if (files.Length > 0)
                    foreach (FileInfo fi in files)
                    {
                        if (fi.CreationTime.Date != System.DateTime.Now.Date)
                            filescopied += Copyfile(fi, logdestdir);
                    }
                DirectoryInfo screensdestdir = Directory.CreateDirectory(extractorLocationDialog.SelectedPath + "\\Monitorizare\\Jpg");
                DirectoryInfo screenssourcedir = new DirectoryInfo(rootdir.FullName + "\\Screencaptures");
                FileInfo[] screens = screenssourcedir.GetFiles();
                if (screens.Length > 0)
                    foreach (FileInfo fi in screens)
                    {
                        if (fi.CreationTime.Date != System.DateTime.Now.Date)
                            filescopied += Copyfile(fi, screensdestdir);
                    }
                #endregion

                MessageBox.Show(String.Format("{0} files extracted.", filescopied), "Log extractor",
                                               MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        uint Copyfile(FileInfo source, DirectoryInfo destdir)
        {
            uint rez=0, i = 0;
            while (i<3)
            {
                try
                {
                    //string ext = source.Extension;
                    string newfilestr = destdir.FullName +"\\"+ source.Name;
                    File.Copy(source.FullName, newfilestr, true);
                    FileInfo fi = new FileInfo(newfilestr);
                    //fi.Attributes = FileAttributes.Normal;
                    rez = 1;
                    i = 3;
                }
                catch
                {
                    System.Threading.Thread.Sleep(500);
                    i++;
                }
            }
            return rez;
        }

        private void extractorButton_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip((Button)sender, "Log Extractor");
        }

        private void CPButton_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip((Button)sender, "Control Center");
        }
    }
}
