using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace Monitorizare
{
    static class WinAPI
    {
        #region Imported WinAPI functions
        //This Function is used to get Active Window Title...
        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hwnd, string lpString, int cch);

        //This Function is used to get Handle for Active Window...
        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern IntPtr GetForegroundWindow();

        //This Function is used to get Active process ID...
        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern Int32 GetWindowThreadProcessId(IntPtr hWnd, out Int32 lpdwProcessId);

        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern short GetKeyState(int vKey);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hSnapshot);
        #endregion

        #region User-defined Functions
        public static Int32 GetWindowProcessID(IntPtr hwnd)
        {
            //This Function is used to get Active process ID...
            Int32 pid;
            GetWindowThreadProcessId(hwnd, out pid);
            return pid;
        }
        
        public static string ActiveApplTitle()
        {
            //This method is used to get active application's title using GetWindowText() method present in user32.dll
            IntPtr hwnd = GetForegroundWindow();
            if (hwnd.Equals(IntPtr.Zero)) return "";
            string lpText = new string(' ', 100);
            int intLength = GetWindowText(hwnd, lpText, lpText.Length);
            if ((intLength <= 0) || (intLength > lpText.Length)) return "unknown";
            return lpText.Trim().Replace(Convert.ToChar(0x0).ToString(), "");
        }
        public static String GetWindowText(IntPtr hWnd)
        {
            string result=new string((char)0,100);
            int rLen = GetWindowText(hWnd, result, result.Length);
            return result;
        }
        #endregion
    }
}
