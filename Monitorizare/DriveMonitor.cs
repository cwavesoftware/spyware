using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;             // required for Message
using System.Runtime.InteropServices;   // required for Marshal
using System.IO;
using Microsoft.Win32.SafeHandles; 

namespace Monitorizare
{
    public delegate void DriveDetectorEventHandler(Object sender, char e);
        
    public class DriveMonitor
    {     
        /// <summary>
        /// It will create hidden form for processing Windows messages about USB drives
        /// No need to override WndProc in our form.
        /// </summary>
        public DriveMonitor()
        {
            frm = new DetectorForm(this);
            frm.Show(); // will be hidden immediatelly      
            for (int i = 0; i < 25; i++)
                hDriveMask[i] = IntPtr.Zero;
            // Register removable drives that are already connected
            // when application is started (when computer is started)
            foreach (DriveInfo di in DriveInfo.GetDrives())
            {
                if (di.DriveType == DriveType.Removable)
                {
                    RegisterForNotif(di.Name[0]);
                }
            }
        }
        /// <summary>
        /// Events signalized to the client app.
        /// Add handlers for these events in your form to be notified of removable device events 
        /// </summary>
        public event DriveDetectorEventHandler DeviceArrived;
        public event DriveDetectorEventHandler DeviceRemoved;
        public event DriveDetectorEventHandler DeviceQuerriedRemove;
        DetectorForm frm;
        IntPtr[] hDriveMask = new IntPtr[25];
        /// <summary>
        /// Message handler which must be called from client form.
        /// Processes Windows messages and calls event handlers. 
        /// </summary>
        /// <param name="m"></param>
        public void WndProc(ref Message m)
        {
            int devType;
            char c;
            if (m.Msg == WM_DEVICECHANGE)
            {
                // WM_DEVICECHANGE can have several meanings depending on the WParam value...
                switch (m.WParam.ToInt32())
                {
                    #region DBT_DEVICEARRIVAL
                    case (int)Events.DBT_DEVICEARRIVAL:// New device has just arrived
                        devType = Marshal.ReadInt32(m.LParam, 4);
                        if (devType == (int)DevTypes.DBT_DEVTYP_VOLUME)
                        {  
                            // Extract data to structure
                            DEV_BROADCAST_VOLUME dbv;
                            dbv = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));
                            // Get the drive letter 
                            c = DriveMaskToLetter(dbv.dbcv_unitmask);
                            if (DeviceArrived != null)// if  has event handler
                            {
                                // Raise DeviceArrived event;
                                DeviceArrived(this, c);
                            }
                            // Also register to get notified about DBT_DEVICEQUERYREMOVE
                            RegisterForNotif(c);
                        }
                        break;
                    #endregion
                    #region DBT_DEVICEQUERYREMOVE
                    case (int)Events.DBT_DEVICEQUERYREMOVE:
                        devType = Marshal.ReadInt32(m.LParam, 4);
                        if (devType == (int)DevTypes.DBT_DEVTYP_HANDLE)
                        {
                            DEV_BROADCAST_HANDLE dbhQueryRemove;
                            dbhQueryRemove = (DEV_BROADCAST_HANDLE)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HANDLE));
                            for (int i=0;i<25;i++)
                                if (hDriveMask[i] == dbhQueryRemove.dbch_handle)
                                {
                                    if (DeviceQuerriedRemove != null)
                                        // Raise DeviceQueriedRemove
                                        DeviceQuerriedRemove(this, Convert.ToChar(i + 65));
                                }
                            // Release handlers
                            UnregisterDeviceNotification(dbhQueryRemove.dbch_hdevnotify);
                            WinAPI.CloseHandle(dbhQueryRemove.dbch_handle);
                        }
                        break;
                    #endregion
                    #region DBT_DEVICEREMOVECOMPLETE
                    case (int)Events.DBT_DEVICEREMOVECOMPLETE:// Device has been removed
                        devType = Marshal.ReadInt32(m.LParam, 4);
                        if (devType == (int)DevTypes.DBT_DEVTYP_VOLUME)
                        {
                            DEV_BROADCAST_VOLUME dbv;
                            dbv = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));
                            c = DriveMaskToLetter(dbv.dbcv_unitmask);
                            if (DeviceRemoved != null)
                            {
                                // Raise DeviceRemoved event
                                DeviceRemoved(this, c);
                            }
                        }
                        break;
                    #endregion
                    #region DBT_CUSTOMEVENT
                    case (int)Events.DBT_CUSTOMEVENT:// Same as DBT_DEVICEQUERYREMOVE, but sent when
                                                    // user right-click on drive -> Eject
                        System.Diagnostics.Debug.WriteLine("DBT_CUSTOMEVENT");
                        devType = Marshal.ReadInt32(m.LParam, 4);
                        if (devType == (int)DevTypes.DBT_DEVTYP_HANDLE)
                        {
                            DEV_BROADCAST_HANDLE dbhCustomEvent;
                            dbhCustomEvent = (DEV_BROADCAST_HANDLE)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_HANDLE));
                            // TODO check behaviour on win 8
                        }
                        break;
                    #endregion
                }
            }// end if m.Msg == WM_DEVICECHANGE
        }// end WndProc
        char DriveMaskToLetter(int mask)
        {
            char letter;
            string drives = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            // 1 = A
            // 2 = B
            // 4 = C...
            int cnt = 0;
            int pom = mask / 2;
            while (pom != 0)
            {
                // while there is any bit set in the mask
                // shift it to the righ...                
                pom = pom / 2;
                cnt++;
            }

            if (cnt < drives.Length)
                letter = drives[cnt];
            else
                letter = '?';
            return letter;
        }
        IntPtr GetHDrive(char driveletter)
        {
            IntPtr r = CreateFile(String.Format("\\\\.\\{0}:", driveletter.ToString().ToLower()), FileAccess.Read,
                                  FileShare.ReadWrite, IntPtr.Zero, FileMode.Open, 0, IntPtr.Zero);
            hDriveMask[Convert.ToInt32(driveletter) - 65] = r;
            return r;
        }
        void RegisterForNotif(char driveletter)
        {
            IntPtr hDrive = GetHDrive(driveletter);
            DEV_BROADCAST_HANDLE filter = new DEV_BROADCAST_HANDLE();
            filter.dbch_size = Marshal.SizeOf(filter);
            filter.dbch_devicetype = (int)DevTypes.DBT_DEVTYP_HANDLE;
            filter.dbch_handle = hDrive;
            IntPtr pfilter = Marshal.AllocHGlobal(Marshal.SizeOf(filter));
            Marshal.StructureToPtr(filter, pfilter, true);
            IntPtr hRDN = RegisterDeviceNotification(frm.Handle, pfilter, (int)DevNotify.DEVICE_NOTIFY_WINDOW_HANDLE);
        }
        #region Win32 constants
        private const int BROADCAST_QUERY_DENY = 0x424D5144;
        private const int WM_DEVICECHANGE = 0x0219;
        private enum Events : int
        {
            DBT_DEVICEARRIVAL = 0x8000, // system detected a new device
            DBT_DEVICEQUERYREMOVE = 0x8001, // Preparing to remove (any program can disable the removal)
            DBT_DEVICEREMOVECOMPLETE = 0x8004, // device removed
            DBT_CONFIGCHANGECANCELED=0x0019,
            DBT_CONFIGCHANGED=0x0018,
            DBT_CUSTOMEVENT=0x8006,
            DBT_DEVICEQUERYREMOVEFAILED=0x8002,
            DBT_DEVICEREMOVEPENDING=0x8003,
            DBT_DEVICETYPESPECIFIC=0x8005,
            DBT_DEVNODES_CHANGED=0x0007,
            DBT_QUERYCHANGECONFIG=0x0017,
            DBT_USERDEFINED=0xFFFF
        }
        private enum DevTypes : int
        {
            /// <summary>
            /// The DBT_DEVICEARRIVAL and DBT_DEVICEREMOVECOMPLETE events
            /// are automatically broadcast to all top-level windows for port devices.
            /// Therefore, it is not necessary to call RegisterDeviceNotification for ports,
            /// and the function fails if the dbch_devicetype member is DBT_DEVTYP_PORT.
            /// </summary>
            DBT_DEVTYP_PORT = 0x00000003,
            /// <summary>
            /// Volume notifications are also broadcast to top-level windows,
            /// so RegisterDeviceNotification fails if dbch_devicetype is DBT_DEVTYP_VOLUME.
            /// </summary>
            DBT_DEVTYP_VOLUME = 0x00000002,
            /// <summary>
            ///  OEM-defined devices are not used directly by the system, 
            ///  so RegisterDeviceNotification fails if dbch_devicetype is DBT_DEVTYP_OEM.
            /// </summary>
            DBT_DEVTYP_OEM = 0x00000000,                
            DBT_DEVTYP_DEVNODE = 0x00000001,            //Devnode number
            DBT_DEVTYP_NET = 0x00000004,                //Network resource
            DBT_DEVTYP_DEVICEINTERFACE = 0x00000005,    //Device interface class
            DBT_DEVTYP_HANDLE = 0x00000006              //File system handle
        }
        [Flags]
        private enum DevNotify : int
        {
            DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000,
            DEVICE_NOTIFY_SERVICE_HANDLE = 0x00000001,
            DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 0x00000004
        }
        #endregion
        #region Structs
        // Structure with information for RegisterDeviceNotification.
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DEV_BROADCAST_DEVICEINTERFACE
        {
            public Int32 dbcc_size;
            public Int32 dbcc_devicetype;
            public Int32 dbcc_reserved;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
            public byte[] dbcc_classguid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] dbcc_name;
        }
        // Structure with information about a file system handle.
        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_HANDLE
        {
            public int dbch_size;
            public int dbch_devicetype;
            public int dbch_reserved;
            public IntPtr dbch_handle;
            public IntPtr dbch_hdevnotify;
            public Guid dbch_eventguid;
            public long dbch_nameoffset;
            public byte dbch_data;
            public byte dbch_data1;
        }
        // Struct for parameters of the WM_DEVICECHANGE message
        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_VOLUME
        {
            public int dbcv_size;
            public int dbcv_devicetype;
            public int dbcv_reserved;
            public int dbcv_unitmask;
        }
        #endregion
        #region Imported functions
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr RegisterDeviceNotification(IntPtr hRecipient, IntPtr NotificationFilter, Int32 Flags);
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterDeviceNotification(IntPtr hHandle);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr CreateFile(
             [MarshalAs(UnmanagedType.LPTStr)] string filename,
             [MarshalAs(UnmanagedType.U4)] FileAccess access,
             [MarshalAs(UnmanagedType.U4)] FileShare share,
             IntPtr securityAttributes, // optional SECURITY_ATTRIBUTES struct or IntPtr.Zero
             [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition,
             [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes,
             IntPtr templateFile);
        #endregion
    }    
}
