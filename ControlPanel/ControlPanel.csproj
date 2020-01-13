using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.AccessControl;
using System.Diagnostics;
using System.Threading;
using System.Security.Principal;
using System.ServiceProcess;
using System.Configuration.Install;
using System.Reflection;
using Monitorizare;

namespace ControlPanel
{
    public partial class ControlPanel : Form
    {
        public ControlPanel()
        {
            InitializeComponent();
            rootdir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Monitorizare");
            confFile = new FileInfo(rootdir.FullName + "\\Monitorizare.conf");
            fullusername = Machine.getInstance().getFullUsername();
            try
            {
                serviceagent = new ServiceController("winsvc-launcher");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("Exception in ServiceControler(winsvc-launcher): {0}:",ex.Message));
            }
            Process[] procs=Process.GetProcessesByName("winsvc");
            if (procs.Length != 0)
            {
                desktopagent = procs[0];
                try
                {
                    desktopagent.EnableRaisingEvents = true;
                }
                catch
                {
                    MessageBox.Show("You need to run application as administrator.","Control Panel",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    Environment.Exit(0);
                }
                desktopagent.Exited += desktopagent_Exited;
            }
            daStopped = new daStoppedDel(dastoppedMethod);
        }

        #region Fields def.
        DirectoryInfo rootdir;
        FileInfo confFile;
        string fullusername;
        bool adlocked=true;
        ServiceController serviceagent;
        Process desktopagent;
        delegate void daStoppedDel();
        daStoppedDel daStopped;
        #endregion

        private void ControlPanel_Load(object sender, EventArgs e)
        {
            LoadSettings();
            try
            {
                salabel.Text = "Status: " + serviceagent.Status;
            }
            catch
            {
                salabel.Text = "Status: NOT installed";
            }
            if (desktopagent == null)
            {
                if (!File.Exists(rootdir.FullName + "\\winsvc.exe"))
                {
                    dalabel.Text = "Status: NOT INSTALLED";
                    daButton.Enabled = false;
                }
                else
                {
                    dalabel.Text = "Status: NOT running";
                    daButton.Text = "Start";
                }
            }
            if (!IsLocked(rootdir))
            {
                adlabel.Text = "Status: Unlocked";
                adlocked = false;
                adButton.Text = "Lock";
            }
            checkInstallationToolStripMenuItem.PerformClick();
        }
        void desktopagent_Exited(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
                this.Invoke(daStopped);
        }
        private void screenshoterCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (screenshoterCheckBox.Checked)
                intervalDomainUpDown.Enabled = true;
        }
        bool IsLocked(DirectoryInfo directory)
        {
            try
            {
                FileInfo[] files = directory.GetFiles();
                return false;
            }
            catch
            {
                return true;
            }
        }        
        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(confFile.FullName, false))
                {
                    uint activemodules = (uint)LoadModules();
                    sw.WriteLine(activemodules);
                    sw.WriteLine(intervalDomainUpDown.Text);
                    sw.Close();
                }
                if (MessageBox.Show("Settings saved but will not take effect until you restart.\r\nRestart system now?", "Control Panel",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    WindowsController.ExitWindows(RestartOptions.Reboot, false);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(String.Format("Exception in SaveMethod() (ControlPanel): {0}", ex.Message));
            }
        }
        private void lockButton_Click(object sender, EventArgs e)
        {
            Locker.Lock(rootdir,fullusername);

        }
        private void unlockButton_Click(object sender, EventArgs e)
        {
            if (adlocked) // If directory is locked
            {
                // Unlock 
                Locker.Unlock(rootdir, fullusername);
                if (!IsLocked(rootdir)) // Check if operation succeded
                {
                    adlabel.Text = "Status: Unlocked";
                    adButton.Text = "Lock";
                    adlocked = false;
                    Process.Start("explorer.exe",rootdir.FullName);
                }
                else
                    MessageBox.Show("Could not unlock directory", "Control Panel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else // Then, directory is unlocked
            {
                // Lock
                Locker.Lock(rootdir, fullusername);
                if (IsLocked(rootdir)) // Check if operation succeded
                {
                    adlabel.Text = "Status: Locked";
                    adButton.Text = "Unlock";
                    adlocked = true;
                }
                else
                    MessageBox.Show("Could not lock directory", "Control Panel", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void LoadSettings()
        {
            Modules activemodules = 0;
            uint interval = 0;
            try
            {
                using (StreamReader sr = new StreamReader(confFile.FullName))
                {
                    activemodules = (Modules)uint.Parse(sr.ReadLine());
                    interval = uint.Parse(sr.ReadLine());
                    sr.Close();
                }
            }
            catch { }
            if ((activemodules & Modules.Machine) == Modules.Machine)
                machineCheckbox.Checked = true;
            if ((activemodules & Modules.User) == Modules.User)
                userCheckbox.Checked = true;
            if ((activemodules & Modules.Keystrokes) == Modules.Keystrokes)
                keystrokesCheckbox.Checked = true;
            if ((activemodules & Modules.RemDrives) == Modules.RemDrives)
                remDrivesCheckbox.Checked = true;
            if ((activemodules & Modules.FSOp) == Modules.FSOp)
                fsCheckbox.Checked = true;
            if ((activemodules & Modules.Clipboard) == Modules.Clipboard)
                clipboardCheckbox.Checked = true;
            if ((activemodules & Modules.Printer) == Modules.Printer)
                printerCheckbox.Checked = true;
            if ((activemodules & Modules.Screenshoter) == Modules.Screenshoter)
                screenshoterCheckBox.Checked = true;
            if ((activemodules & Modules.Power) == Modules.Power)
                powerCheckebox.Checked = true;
            intervalDomainUpDown.Text = interval.ToString();
        }
        private void daButton_Click(object sender, EventArgs e)
        {
            if (desktopagent != null)
            {
                StopDA();
            }
            else
            {
                StartDA();
            }
        }
        void StartDA()
        {
            // Start desktop agent
            desktopagent = Process.Start(rootdir.FullName + "\\winsvc.exe");
            if (desktopagent != null)
            {
                desktopagent.EnableRaisingEvents = true;
                desktopagent.Exited += desktopagent_Exited;
                dalabel.Text = "Status: Running";
                daButton.Text = "Stop";
            }
        }
        void StopDA()
        {// Stop desktop agent, if exists
            if (desktopagent != null)
            {
                if (!desktopagent.CloseMainWindow())
                {
                    desktopagent.Kill();
                    Debug.WriteLine(String.Format("Exception in CloseMainWindow(): {0}",
                                                  System.Runtime.InteropServices.Marshal.GetLastWin32Error()));
                }
                dastoppedMethod();
            }
        }            
        public void dastoppedMethod()
        {
            desktopagent = null;
            dalabel.Text = "Status: NOT running";
            daButton.Text = "Start";
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void checkInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Check check = new Check(rootdir);
            check.ShowDialog();
        }
        private void installToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            dialog.Description = "Select source files' location";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = Cursors.WaitCursor;
                // Creating folders structure
                rootdir = Directory.CreateDirectory(rootdir.FullName);
                DirectoryInfo logdir = Directory.CreateDirectory(rootdir.FullName + "\\Logs");
                DirectoryInfo screensdir = Directory.CreateDirectory(rootdir.FullName + "\\Screencaptures");
                // Creating configuration file
                using (StreamWriter sw = new StreamWriter(rootdir.FullName + "\\Monitorizare.conf", false))
                {
                    sw.WriteLine("1151");
                    sw.WriteLine("15");
                    sw.Close();
                }
                // Coping files
                uint filescopied = 0;
                DirectoryInfo sourcedir = new DirectoryInfo(dialog.SelectedPath);
                foreach (FileInfo fi in sourcedir.GetFiles("*.*", SearchOption.AllDirectories))
                {
                    if (fi.Name == "winsvc.exe")
                        filescopied += Copyfile(fi, rootdir);
                    if (fi.Name == "winsvc-launcher.exe")
                        filescopied += Copyfile(fi, rootdir);
                    if (fi.Name == "Monitorizare.dll")
                        filescopied += Copyfile(fi, rootdir);
                    if (filescopied == 3)
                        break;
                }
                if (filescopied < 2)
                    MessageBox.Show("All or some of source files could not be found\r\nInstallation incomplete.",
                                    "Control Panel", MessageBoxButtons.OK, MessageBoxIcon.Error);

                try
                {// Install service agent
                    ManagedInstallerClass.InstallHelper(new string[] { rootdir.FullName + "\\winsvc-launcher.exe" });
                    if (MessageBox.Show("Installation successful.\r\nSystem must be restarted\r\nRestart system now?", "Control Panel",
                                        MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        WindowsController.ExitWindows(RestartOptions.Reboot, false);
                }
                catch (InvalidOperationException ex)
                {
                    MessageBox.Show("You need to run application as administrator.", "Control Panel", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    if (rootdir.Exists)
                        rootdir.Delete(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.InnerException.Message, "Control Panel", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                this.Cursor = Cursors.Arrow;
            } // end if ShowDialog()
        }
        uint Copyfile(FileInfo source, DirectoryInfo destdir)
        {
            uint rez = 0, i = 0;
            while (i < 3)
            {
                try
                {
                    string newfilestr = destdir.FullName + "\\" + source.Name;
                    FileInfo fi = new FileInfo(newfilestr);
                    if (!fi.Exists)
                        File.Copy(source.FullName, newfilestr, false);
                    rez = 1;
                    i = 3;
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    System.Threading.Thread.Sleep(500);
                    i++;
                }
            }
            return rez;
        }
        Modules LoadModules()
        {
            Modules activeModules = Modules.None;
            if (machineCheckbox.Checked) activeModules |= Modules.Machine;
            if (powerCheckebox.Checked) activeModules |= Modules.Power;
            if (userCheckbox.Checked) activeModules |= Modules.User;
            if (keystrokesCheckbox.Checked) activeModules |= Modules.Keystrokes;
            if (remDrivesCheckbox.Checked) activeModules |= Modules.RemDrives;
            if (fsCheckbox.Checked) activeModules |= Modules.FSOp;
            if (clipboardCheckbox.Checked) activeModules |= Modules.Clipboard;
            if (printerCheckbox.Checked) activeModules |= Modules.Printer;
            if (cryptoCheckbox.Checked) activeModules |= Modules.Crypto;
            if (antivirCheckbox.Checked) activeModules |= Modules.Antiv;
            if (screenshoterCheckBox.Checked) activeModules |= Modules.Screenshoter;
            return activeModules;
        }
        private void uninstallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            Locker.Unlock(rootdir, fullusername);
            StopDA();

            // Uninstall service agent
            serviceagent.Close();
            serviceagent.Dispose();
            var domain = AppDomain.CreateDomain("MyDomain");
            try
            {
                using (AssemblyInstaller installer = domain.CreateInstance(typeof(AssemblyInstaller).Assembly.FullName, typeof(AssemblyInstaller).FullName, false, BindingFlags.Public | BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.ExactBinding, null, new Object[] { rootdir.FullName + "\\winsvc-launcher.exe", new String[] { } }, null, null, null).Unwrap() as AssemblyInstaller)
                {
                    installer.UseNewContext = true;
                    installer.Uninstall(null);
                }
            }
            catch { }
            AppDomain.Unload(domain);

            try
            {
                // Delete files
                if (rootdir.Exists)
                {
                    rootdir.Delete(true);
                }
                MessageBox.Show("Uninstall complete.", "Control Panel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Uninstall may be completed but some files must be mannualy deleted. Check application folder.",
                                "Control Panel", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.Cursor = Cursors.Arrow;
        }
    }
}
