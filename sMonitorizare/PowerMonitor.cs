using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace sMonitorizare
{
    /// <summary>
    /// This class cannot be instantiated.
    /// It provides methods used in Service1.cs
    /// for handling OnPowerEvent.
    /// </summary>
    static class PowerMonitor
    {
        #region Imported WinAPI functions
        [DllImport("kernel32.dll")]
        static extern bool GetSystemPowerStatus(out SystemPowerStatus sps);
        #endregion

        #region Structs
        public struct SystemPowerStatus
        {
            public ACLineStatus _ACLineStatus;
            public BatteryFlag _BatteryFlag;
            public Byte _BatteryLifePercent;
            public Byte _Reserved1;
            public Int32 _BatteryLifeTime;
            public Int32 _BatteryFullLifeTime;
        }
        public enum ACLineStatus : byte
        {
            Offline = 0,
            Online = 1,
            Unknown = 255
        }
        public enum BatteryFlag : byte
        {
            High = 1,
            Low = 2,
            Critical = 4,
            Charging = 8,
            NoSystemBattery = 128,
            Unknown = 255
        }
        #endregion

        public static bool ACPowerPluggedIn()
        {
            SystemPowerStatus SPS = new SystemPowerStatus();
            GetSystemPowerStatus(out SPS);
            if (SPS._ACLineStatus == ACLineStatus.Online)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static Int32 BatteryCharge()
        {
            SystemPowerStatus SPS = new SystemPowerStatus();
            GetSystemPowerStatus(out SPS);
            return (Int32)SPS._BatteryLifePercent;
        }
    }
}
