using System;
using System.IO;
using System.Threading;

namespace Monitorizare
{
    public class LogEntry
    {
        public LogEntry(string logfilepath, uint id, string details, string machine, string user)
        {
            _id = id;
            _datetime = System.DateTime.Now;
            _details = details;
            _machine = machine;
            _user = user;
            _logfilepath = logfilepath;
            #region set title and category
            switch (id)
            {
                case (uint)LogEntryIDs.MachineTurnOn:
                    _title = "Machine turned on";
                    _category = "Machine";
                    break;
                case (uint)LogEntryIDs.MachineTurnOff:
                    _title = "Machine turned off";
                    _category = "Machine";
                    break;
                case (uint)LogEntryIDs.UserLogon:
                    _title = "Successful user logon";
                    _category = "Sessions";
                    break;
                case (uint)LogEntryIDs.UserLogoff:
                    _title = "User logoff";
                    _category = "Sessions";
                    break;
                case (uint)LogEntryIDs.UserFailureLogon:
                    _title = "Failed user logon";
                    _category = "Sessions";
                    break;
                case (uint)LogEntryIDs.FileOpen:
                    _title = "File opened";
                    _category = "File system";
                    break;
                case (uint)LogEntryIDs.FileCreate:
                    _title = "File created";
                    _category = "File system";
                    break;
                case (uint)LogEntryIDs.FileDelete:
                    _title = "File deleted";
                    _category = "File system";
                    break;
                case (uint)LogEntryIDs.FileRename:
                    _title = "File renamed";
                    _category = "File system";
                    break;
                case (uint)LogEntryIDs.FileModified:
                    _title = "File modified";
                    _category = "File system";
                    break;
                case (uint)LogEntryIDs.SnowbreakStart:
                    _title = "Snowbreak started";
                    _category = "Snowbreak";
                    break;
                case (uint)LogEntryIDs.RemovablePlug:
                    _title = "Removable device pluged in";
                    _category = "Removable";
                    break;
                case (uint)LogEntryIDs.RemovableUnplug:
                    _title = "Removable device unpluged";
                    _category = "Removable";
                    break;
                case (uint)LogEntryIDs.FilePrint:
                    _title = "Document print";
                    _category = "Printer";
                    break;
                case (uint)LogEntryIDs.ClipboardCopy:
                    _title = "Clipboard";
                    _category = "Clipboard";
                    break;
                case (uint)LogEntryIDs.BatteryLow:
                    _title = "Low battery";
                    _category = "Machine";
                    break;
                case (uint)LogEntryIDs.PowerChangedOnBattery:
                    _title = "Power source change";
                    _category = "Machine";
                    break;
                case (uint)LogEntryIDs.PowerChangedOnAC:
                    _title = "Power source change";
                    _category = "Machine";
                    break;
                case (uint)LogEntryIDs.Keystroke:
                    _title = "Keystroke";
                    _category = "Keystrokes";
                    break;
                case (uint)LogEntryIDs.DesktopAgentStarted:
                    _title = "Desktop agent started";
                    _category = "Application";
                    break;
                case (uint)LogEntryIDs.DesktopAgentStopped:
                    _title = "Desktop agent stopped";
                    _category = "Application";
                    break;
            }
            #endregion
            Thread t = new Thread(new ThreadStart(WriteToFile));
            t.Start();
        }
        public LogEntry(string logfilepath, uint id, string details, DateTime datetime, string machine)
        {
            _id = id;
            _datetime = datetime;
            _details = details;
            _machine = machine;
            _user = "";
            _logfilepath = logfilepath;
            #region set title and category
            switch (id)
            {
                case (uint)LogEntryIDs.MachineTurnOn:
                    _title = "Machine turned on";
                    _category = "Machine";
                    break;
                case (uint)LogEntryIDs.MachineTurnOff:
                    _title = "Machine turned off";
                    _category = "Machine";
                    break;
                case (uint)LogEntryIDs.UserLogon:
                    _title = "Successful user logon";
                    _category = "Sessions";
                    break;
                case (uint)LogEntryIDs.UserLogoff:
                    _title = "User logoff";
                    _category = "Sessions";
                    break;
                case (uint)LogEntryIDs.UserFailureLogon:
                    _title = "Failed user logon";
                    _category = "Sessions";
                    break;
                case (uint)LogEntryIDs.FileOpen:
                    _title = "File opened";
                    _category = "File system";
                    break;
                case (uint)LogEntryIDs.FileCreate:
                    _title = "File created";
                    _category = "File system";
                    break;
                case (uint)LogEntryIDs.FileDelete:
                    _title = "File deleted";
                    _category = "File system";
                    break;
                case (uint)LogEntryIDs.FileRename:
                    _title = "File renamed";
                    _category = "File system";
                    break;
                case (uint)LogEntryIDs.FileModified:
                    _title = "File modified";
                    _category = "File system";
                    break;
                case (uint)LogEntryIDs.SnowbreakStart:
                    _title = "Snowbreak started";
                    _category = "Snowbreak";
                    break;
                case (uint)LogEntryIDs.RemovablePlug:
                    _title = "Removable device pluged in";
                    _category = "Removable";
                    break;
                case (uint)LogEntryIDs.RemovableUnplug:
                    _title = "Removable device unpluged";
                    _category = "Removable";
                    break;
                case (uint)LogEntryIDs.FilePrint:
                    _title = "Document print";
                    _category = "Printer";
                    break;
                case (uint)LogEntryIDs.ClipboardCopy:
                    _title = "Clipboard";
                    _category = "Clipboard";
                    break;
                case (uint)LogEntryIDs.BatteryLow:
                    _title = "Low battery";
                    _category = "Machine";
                    break;
                case (uint)LogEntryIDs.PowerChangedOnBattery:
                    _title = "Power source change";
                    _category = "Machine";
                    break;
                case (uint)LogEntryIDs.PowerChangedOnAC:
                    _title = "Power source change";
                    _category = "Machine";
                    break;
                case (uint)LogEntryIDs.Keystroke:
                    _title = "Keystroke";
                    _category = "Keystrokes";
                    break;
                case (uint)LogEntryIDs.DesktopAgentStarted:
                    _title = "Desktop agent started";
                    _category = "Application";
                    break;
                case (uint)LogEntryIDs.DesktopAgentStopped:
                    _title = "Desktop agent stopped";
                    _category = "Application";
                    break;
            }
            #endregion
            Thread t = new Thread(new ThreadStart(WriteToFile));
            t.Start();
        }
        uint _id;
        DateTime _datetime;
        string _title, _details, _category, _machine, _user;
        string _logfilepath;
        void WriteToFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(this._logfilepath, true))
                {
                    sw.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6}", this._id, this._datetime,
                                               this._machine, this._user, this._category, this._title, this._details));
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                //MainForm.Debug(String.Format("Exception in WriteToFile() (Desktop Agent): {0}\r\nRetrying...", ex.Message));
                Thread.Sleep(300);
                WriteToFile();
            }
        }
    }
    public enum LogEntryIDs : uint
    {
        MachineTurnOn = 0x0001,
        MachineTurnOff = 0x0002,
        UserLogon = 0x0003,
        UserLogoff = 0x0004,
        UserFailureLogon = 0x0005,
        FileOpen = 0x0006,
        FileCreate = 0x0007,
        FileDelete = 0x0008,
        FileRename = 0x0009,
        FileModified = 0x000e,
        SnowbreakStart = 0x000a,
        RemovablePlug = 0x000b,
        RemovableUnplug = 0x000c,
        FilePrint = 0x000d,
        ClipboardCopy = 0x000f,
        BatteryLow = 0x0010,
        PowerChangedOnBattery = 0x0011,
        PowerChangedOnAC = 0x0012,
        Keystroke = 0x0013,
        DesktopAgentStopped = 0x0014,
        DesktopAgentStarted = 0x0015
    }

    [FlagsAttribute]
    public enum Modules : uint
    {
        None = 0,
        Machine = 1,
        User = 2,
        Keystrokes = 4,
        RemDrives = 8,
        FSOp = 16,
        Clipboard = 32,
        Printer = 64,
        Crypto = 128,
        Antiv = 256,
        Screenshoter = 512,
        Power = 1024
    }
}
