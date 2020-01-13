using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Monitorizare
{
    public delegate void CreatedDeletedModifiedEventHandler(object sender, CreatedDeletedModifiedEventArgs e);
    public delegate void OpenedEventHandler(object sender, OpenedEventArgs e);
    public delegate void fsm_RenamedEventHandler(object sender, CustomRenamedEventArgs e);

    public class CreatedDeletedModifiedEventArgs : System.IO.FileSystemEventArgs
    {
        public CreatedDeletedModifiedEventArgs(WatcherChangeTypes changeType, string directory, string name)
            : base(changeType, directory, name)
        {
        }
        public bool IsFileSystem()
        {
            string filepath = this.FullPath;
            string fullusername = "";
            string user = "";
            try
            {
                fullusername = System.IO.File.GetAccessControl(filepath).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();
                user = fullusername.Substring(fullusername.LastIndexOf('\\') + 1);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(String.Format("Source = winsvc, Exception in GetAccessControl({0}): {1}", filepath, ex.Message));
            }
            // Check file origin by user
            if (user != "" && user != "SYSTEM")
            {
                FileAttributes attributes = new FileAttributes();
                try
                {
                    attributes = File.GetAttributes(filepath);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(String.Format("Source = winsvc, Exception in GetAttributes({0}): {1}", filepath, ex.Message));
                }
                // Check file origin by attributes 
                if (attributes != 0)
                {

                    if (!(((attributes & FileAttributes.Temporary) == FileAttributes.Temporary)
                                                            ||
                   ((attributes & FileAttributes.NotContentIndexed) == FileAttributes.NotContentIndexed)
                                                            ||
                   ((attributes & FileAttributes.Hidden) == FileAttributes.Hidden)
                                                            ||
                   ((attributes & FileAttributes.System) == FileAttributes.System)))
                        // Then probably is a user-created file
                        return false;
                    else
                        return true;
                }
                else
                    return true;
            }// end if user!=SYSTEM
            else
                return true;
        }
        public bool PassExtensionFilter()
        {
            if (this.Name.Substring(this.Name.IndexOf("\\") + 1, 2) == "~$")
                return false;
            FileInfo fi = new FileInfo(this.FullPath);
            string[] filters = new string[] { ".txt", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx",
                                              ".rtf", ".mdb", ".mdbx", ".pps", ".ppsx", ".mp3", ".avi",
                                              ".pst", ".rar", ".zip", ".pdf", ".mp4", ".jpeg",
                                              ".png", ".bmp", ".contact", ".jnt", ".pub", "zipx"};
            foreach (string s in filters)
            {
                if (fi.Extension.ToLower() == s)
                    return true;
            }
            return false;
        }
    }
    public class CustomRenamedEventArgs : System.IO.RenamedEventArgs
    {
        public CustomRenamedEventArgs(WatcherChangeTypes changeType, string directory, string name, string oldname)
            : base(changeType, directory, name, oldname)
        {            
        }
        public bool IsFileSystem()
        {
            string filepath = this.FullPath;
            string fullusername = "";
            string user = "";
            try
            {
                fullusername = System.IO.File.GetAccessControl(filepath).GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();
                user = fullusername.Substring(fullusername.LastIndexOf('\\') + 1);
            }
            catch (Exception ex)
            {
                MainForm.Debug(String.Format("Source = winsvc, Exception in GetAccessControl({0}): {1}", filepath, ex.Message));
            }
            // Check file origin by user
            if (user != "" && user != "SYSTEM")
            {
                FileAttributes attributes = new FileAttributes();
                try
                {
                    attributes = File.GetAttributes(filepath);
                }
                catch (Exception ex)
                {
                    MainForm.Debug(String.Format("Source = winsvc, Exception in GetAttributes({0}): {1}", filepath, ex.Message));
                }
                // Check file origin by attributes 
                if (attributes != 0)
                {
                    if (!(((attributes & FileAttributes.Temporary) == FileAttributes.Temporary)
                                                                   ||
                          ((attributes & FileAttributes.NotContentIndexed) == FileAttributes.NotContentIndexed)
                                                                   ||
                          ((attributes & FileAttributes.System) == FileAttributes.System)))
                        // Then probably is a user-created file
                        return false;
                    else
                        return true;
                }
                else
                    return true;
            }// end if user!=SYSTEM
            else
                return true;
        }
        public bool PassExtensionFilter()
        {
            bool passold=false, passnew=false;
            FileInfo oldfi = new FileInfo(this.OldFullPath);
            FileInfo newfi = new FileInfo(this.FullPath);
            string[] filters = new string[] {".txt", ".doc", ".docx", ".ppt", ".pptx", ".xls", ".xlsx",
                                              ".rtf", ".mdb", ".mdbx", ".pps", ".ppsx", ".mp3", ".avi",
                                              ".pst", ".rar", ".zip", ".pdf", ".mp4", ".jpg", ".jpeg",
                                              ".png", ".bmp", ".contact", ".jnt", ".pub", "zipx"};
            foreach (string s in filters)
            {
                if (oldfi.Extension.ToLower() == s)
                    passold = true;
                if (newfi.Extension.ToLower() == s)
                    passnew = true;
                if (passnew && passold)
                    break;
            }
            return passold&passnew;
        }
    }
    public class OpenedEventArgs:EventArgs
    {
        public OpenedEventArgs(string file)
            : base()
        {
            File = file;
        }
        public string File;
    }

    public class FileSystemMonitor:System.IO.FileSystemWatcher
    {
        public FileSystemMonitor(string path)
        {
            this.Path = path;
            this.IncludeSubdirectories = true;
            this.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
            this.EnableRaisingEvents = true;
            this.Created += FileSystemMonitor_Created;
            this.Deleted += FileSystemMonitor_Deleted;
            this.Renamed += FileSystemMonitor_Renamed;
            this.Changed += FileSystemMonitor_Changed;
            pm = new ProcessMonitor();
            pm.ProcessStart += pm_ProcessStart;
            pm.Start();
            MainForm.driveFSMs[Convert.ToByte(path[0]) - 65] = this;
        }
        public event CreatedDeletedModifiedEventHandler FSMCreated;
        public event CreatedDeletedModifiedEventHandler FSMDeleted;
        public event CreatedDeletedModifiedEventHandler FSMModified;
        public event OpenedEventHandler FSMOpened;
        public event fsm_RenamedEventHandler FSMRenamed;
        ProcessMonitor pm;

        void FileSystemMonitor_Changed(object sender, FileSystemEventArgs e)
        {
            CreatedDeletedModifiedEventArgs mye = new CreatedDeletedModifiedEventArgs(e.ChangeType, e.FullPath.Substring(0, e.FullPath.IndexOf("\\")),
                                                                      e.Name);
            if (FSMModified != null && mye.PassExtensionFilter() && !mye.IsFileSystem())
                FSMModified(this, mye);
        }
        void FileSystemMonitor_Deleted(object sender, FileSystemEventArgs e)
        {
            CreatedDeletedModifiedEventArgs mye = new CreatedDeletedModifiedEventArgs(e.ChangeType, e.FullPath.Substring(0, e.FullPath.IndexOf("\\")),
                                                                      e.Name);
            if (FSMDeleted != null && mye.PassExtensionFilter())
                FSMDeleted(this, mye);
        }
        void FileSystemMonitor_Created(object sender, FileSystemEventArgs e)
        {
            CreatedDeletedModifiedEventArgs mye = new CreatedDeletedModifiedEventArgs(e.ChangeType, e.FullPath.Substring(0,
                                                                      e.FullPath.IndexOf("\\")), e.Name);
            if (FSMCreated != null && mye.PassExtensionFilter() && !mye.IsFileSystem())
                FSMCreated(this, mye);
        }
        void FileSystemMonitor_Renamed(object sender, RenamedEventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine("-=== CORE ===-\r\n" + e.FullPath + " renamed from " + e.OldFullPath);
            CustomRenamedEventArgs mye = new CustomRenamedEventArgs(e.ChangeType, e.OldFullPath.Substring(0, e.OldFullPath.IndexOf("\\")),
                                                                    e.Name, e.OldName);
            if (FSMRenamed != null && mye.PassExtensionFilter() && !mye.IsFileSystem())
            {
                FSMRenamed(this, mye);
                return;
            }
            FileInfo oldfi = new FileInfo(e.OldFullPath);
            FileInfo newfi = new FileInfo(e.FullPath);
            //if ((oldfi.Extension == ".tmp" || oldfi.Name.Contains("__rar_"))&& FSMModified != null)
            //{
                CreatedDeletedModifiedEventArgs mye2 = new CreatedDeletedModifiedEventArgs(WatcherChangeTypes.Changed,
                                                                           newfi.Directory.FullName,
                                                                           e.Name.Substring(e.Name.LastIndexOf("\\")));
                if (mye2.PassExtensionFilter())
                {
                    FSMModified(sender, mye2);
                    return;
                }
            //}
        }
        void pm_ProcessStart(object sender, ProcessMonitorEventArgs e)
        {
            switch (e.ProcessName)
            {
                case "WINWORD.EXE":
                    if (e.Arguments!=null && e.ActiveAppTitle != null)
                    {
                        string fname = e.ActiveAppTitle;
                        OpenedEventArgs foe = new OpenedEventArgs(fname);
                        if (FSMOpened != null)
                            FSMOpened(this, foe);
                    }
                    break;
                case "AcroRd32.exe":
                    if (e.Arguments!=null && e.Arguments.Contains(".pdf"))
                    {
                        string[] args = e.Arguments.Split(new string[] { "\" \"" }, StringSplitOptions.RemoveEmptyEntries);
                        if (args.Length == 2)
                        {
                            string fname = args[1];
                            OpenedEventArgs foea = new OpenedEventArgs(fname);
                            if (FSMOpened != null)
                            {
                                FSMOpened(this, foea);
                            }
                            break;
                        }
                    }
                    break;
            }
        }
    }
}
