using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Security.AccessControl;
using System.Net;
using System.Net.Mail;
using Monitorizare;

namespace sMonitorizare
{
    public partial class sMonitorizare : ServiceBase
    {
        public sMonitorizare()
        {
            InitializeComponent();
            base.CanHandlePowerEvent = true;
            base.CanHandleSessionChangeEvent = true;
            base.CanShutdown = true;
        }

        ApplicationLauncher.PROCESS_INFORMATION procInfo;
        //public static List<LogEntry> logList = new List<LogEntry>();
        public static uint logCount = 0;
        string username="",fullusername="",machine="";
        public static DirectoryInfo Rootdir;
        public static FileInfo Logfile;
        LogEntry le;
        Modules activemodules=(Modules)1135;
        Process desktopagent;

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            int y = DateTime.Now.Date.Year;
            int M = DateTime.Now.Date.Month;
            int d = DateTime.Now.Date.Day;
            string date4fname = String.Format("{0}.{1:00}.{2:00}", y, M, d);
            Rootdir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Local");
            Rootdir.Attributes |= FileAttributes.Hidden;
            machine = Environment.MachineName;
            //Logfile = new FileInfo(String.Format("{0}\\{1}_{2}.dll", Rootdir.FullName, machine, date4fname));
            Logfile = new FileInfo(String.Format("{0}\\winsvc.dll", Path.GetTempPath()));
            //LoadSettings(ref activemodules);
            if ((activemodules & Modules.Machine) == Modules.Machine)
            {
                le = new LogEntry(Logfile.FullName,(uint)LogEntryIDs.MachineTurnOn,"",machine,username);
                foreach (DriveInfo di in DriveInfo.GetDrives())
                    if (di.DriveType == DriveType.Removable && di.IsReady)
                    {
                        string details = String.Format("Drive {0} was plugged in.  Label: {1}  Type: {2}  File format:" +
                                             " {3}  Capacity: {4}  Free space: {5}", di.Name, di.VolumeLabel,
                                             di.DriveType, di.DriveFormat, di.TotalFreeSpace, di.AvailableFreeSpace);
                        le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.RemovablePlug,details, machine, username);
                    }
            }
            #region Relevant just for debuging
            try
            {
                desktopagent = Process.GetProcessesByName("winsvc")[0];
                desktopagent.EnableRaisingEvents = true;
                desktopagent.Exited += desktopagent_Exited;
            }
            catch { }
            #endregion
        }
        
        protected override void OnStop()
        {
            
        }
        protected override void OnSessionChange(SessionChangeDescription changeDescription) 
        {
            switch (changeDescription.Reason)
            {
                case SessionChangeReason.SessionLogon:
                    if ((activemodules & Modules.User) == Modules.User)
                    {
                        // Retrieve the username of the user who had just logged on
                        username = Machine.getInstance().getUsername();
                        le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.UserLogon, "",machine, username);
                    }

                    fullusername = Machine.getInstance().getFullUsername();
                    //Locker.Lock(Rootdir, fullusername);
                    // Launching Monitorizare
                    String applicationName = Rootdir.FullName + "\\winsvc.exe";
                    bool appLaunched = ApplicationLauncher.StartProcessAndBypassUAC(String.Format("{0}", applicationName), out procInfo);
                    Debug(applicationName + " successfully launched = " + appLaunched);
                    if (appLaunched)
                    {
                        le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.DesktopAgentStarted, "This is an automated application action. This log is for debug purposes.",
                                          machine, username);
                        desktopagent = Process.GetProcessesByName("winsvc")[0];
                        desktopagent.EnableRaisingEvents = true;
                        desktopagent.Exited += desktopagent_Exited;
                    }
                    break;
                case SessionChangeReason.SessionLogoff:
                    if ((activemodules & Modules.User) == Modules.User)
                        le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.UserLogoff, "",machine, username);
                    break;
            }
            // Inform the base class of the change as well
            base.OnSessionChange(changeDescription);
        }// end of OnSessionChange

        protected override void OnShutdown()
        {
            if ((activemodules & Modules.Machine) == Modules.Machine)
                le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.MachineTurnOff, "",machine, username);
            base.OnShutdown();
        }

        void desktopagent_Exited(object sender, EventArgs e)
        {
            le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.DesktopAgentStopped, "This MAY BE an automated application action." +
                              " If this event isn't followed by an user logoff or machine turn off event, then desktop agent was killed." +
                              " This case requires further investigation.", machine, username);
        } 

        protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
        {
            if ((activemodules & Modules.Power) == Modules.Power)
            {
                string details = "";
                switch (powerStatus)
                {
                    case PowerBroadcastStatus.BatteryLow:
                        details = String.Format("WARNING Baterry low, {0}% remaining.", PowerMonitor.BatteryCharge());
                        le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.BatteryLow, details,machine, username);
                        break;
                    case PowerBroadcastStatus.PowerStatusChange:
                        if (PowerMonitor.ACPowerPluggedIn())
                        {
                            details = String.Format("Power Status Changed: System runs on AC power.");
                            le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.PowerChangedOnAC,details, machine, username);
                        }
                        else
                        {
                            details = String.Format("Power Status Changed: System runs on battery.");
                            le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.PowerChangedOnBattery, details, machine, username);
                        }
                        break;
                }
            }
            return base.OnPowerEvent(powerStatus);
        }
        public static void Debug(string log)
        {
            string s = DateTime.Now + ": " + log;
            System.Diagnostics.Debug.WriteLine(s);
            //try
            //{
            //    using (StreamWriter sw = new StreamWriter(Rootdir.FullName + "\\ServiceAgent.log", true))
            //    {
            //        sw.WriteLine(s);
            //        sw.Close();
            //    }
            //}
            //catch { }
        }
        //void LoadSettings(ref Modules activemodules)
        //{
        //    try
        //    {
        //        using (StreamReader sr = new StreamReader(Rootdir.FullName + "\\Monitorizare.conf"))
        //        {
        //            activemodules = (Modules)uint.Parse(sr.ReadLine());
        //            sr.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug(String.Format("Exception in LoadSettings() (sMOnitorizare): {0}", ex.Message));
        //    }
        //}
    }
}
