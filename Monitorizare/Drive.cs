using System;
using System.IO;

namespace Monitorizare
{
    /// <summary>
    ///  For now, this class isn't used anymore.
    ///  We keep it for eventualy future using.
    /// </summary>
    public class Drive
    {
        public Drive(char letter, bool beginmonitoring)
        {
            System.Threading.Thread.Sleep(5000);
            Name = letter + ":\\";
            if (beginmonitoring)
                Monitor.EnableRaisingEvents = true;
            else
                Monitor.EnableRaisingEvents = false;
            foreach (DriveInfo di in DriveInfo.GetDrives())
            {
                if (di.Name == Name)
                {
                    this.Label = di.VolumeLabel;
                    this.Freespace = di.TotalFreeSpace;
                    this.Capacity = di.TotalSize;
                    this.Fileformat = di.DriveFormat;
                    this.Type = di.DriveType;
                    break;
                }
            }
        }
        public string Name;
        public string Label;
        public long Freespace;
        public string Fileformat;
        public System.IO.DriveType Type;
        public Int64 Capacity;
        public FileSystemMonitor Monitor;
        public bool IsMonitored
        {
            get { return Monitor.EnableRaisingEvents; }
            set { Monitor.EnableRaisingEvents = value; }
        }
        public Drive Dispose()
        {
            try
            {
                this.Monitor.Dispose();
            }
            catch { }
            return null;
        }
    }
}
