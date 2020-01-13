using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;

namespace Monitorizare
{
    public delegate void ClipboardMonitorEventHandler(ClipboardFormat format, object data);

    public enum ClipboardFormat : byte
    {
        /// <summary>Specifies the standard ANSI text format. This static field is read-only.
        /// </summary>
        /// <filterpriority>1</filterpriority>
        Text,
        /// <summary>Specifies the standard Windows Unicode text format. This static field
        /// is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        UnicodeText,
        /// <summary>Specifies the Windows device-independent bitmap (DIB) format. This static
        /// field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        Dib,
        /// <summary>Specifies a Windows bitmap format. This static field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        Bitmap,
        /// <summary>Specifies the Windows enhanced metafile format. This static field is
        /// read-only.</summary>
        /// <filterpriority>1</filterpriority>
        EnhancedMetafile,
        /// <summary>Specifies the Windows metafile format, which Windows Forms does not
        /// directly use. This static field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        MetafilePict,
        /// <summary>Specifies the Windows symbolic link format, which Windows Forms does
        /// not directly use. This static field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        SymbolicLink,
        /// <summary>Specifies the Windows Data Interchange Format (DIF), which Windows Forms
        /// does not directly use. This static field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        Dif,
        /// <summary>Specifies the Tagged Image File Format (TIFF), which Windows Forms does
        /// not directly use. This static field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        Tiff,
        /// <summary>Specifies the standard Windows original equipment manufacturer (OEM)
        /// text format. This static field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        OemText,
        /// <summary>Specifies the Windows palette format. This static field is read-only.
        /// </summary>
        /// <filterpriority>1</filterpriority>
        Palette,
        /// <summary>Specifies the Windows pen data format, which consists of pen strokes
        /// for handwriting software, Windows Forms does not use this format. This static
        /// field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        PenData,
        /// <summary>Specifies the Resource Interchange File Format (RIFF) audio format,
        /// which Windows Forms does not directly use. This static field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        Riff,
        /// <summary>Specifies the wave audio format, which Windows Forms does not directly
        /// use. This static field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        WaveAudio,
        /// <summary>Specifies the Windows file drop format, which Windows Forms does not
        /// directly use. This static field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        FileDrop,
        /// <summary>Specifies the Windows culture format, which Windows Forms does not directly
        /// use. This static field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        Locale,
        /// <summary>Specifies text consisting of HTML data. This static field is read-only.
        /// </summary>
        /// <filterpriority>1</filterpriority>
        Html,
        /// <summary>Specifies text consisting of Rich Text Format (RTF) data. This static
        /// field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        Rtf,
        /// <summary>Specifies a comma-separated value (CSV) format, which is a common interchange
        /// format used by spreadsheets. This format is not used directly by Windows Forms.
        /// This static field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        CommaSeparatedValue,
        /// <summary>Specifies the Windows Forms string class format, which Windows Forms
        /// uses to store string objects. This static field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        StringFormat,
        /// <summary>Specifies a format that encapsulates any type of Windows Forms object.
        /// This static field is read-only.</summary>
        /// <filterpriority>1</filterpriority>
        Serializable,
    }

    public class ClipboardMonitor
    {
        public ClipboardMonitor()
        {
            DetectorForm df = new DetectorForm(this);
            df.Show();
            handletodf = df.Handle;// will be hidden immediatelly
        }
        
        public event ClipboardMonitorEventHandler OnClipboardChange;
        IntPtr handletodf;
        static readonly string[] formats = Enum.GetNames(typeof(ClipboardFormat));
        uint lastcsqn = 0;
        
        public void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_CLIPBOARDUPDATE:// Only for Vista and newer versions of Windows
                        ClipChanged();                   
                    break;
            }
        }
        public void Start()
        {
            AddClipboardFormatListener(handletodf);
        }
        public void Stop()
        {
            RemoveClipboardFormatListener(handletodf);
        }
        private void ClipChanged()
        {
            IDataObject entireData = null;
            object data = null;
            ClipboardFormat? format = null;
            uint csqn = GetClipboardSequenceNumber();
            try
            {
                entireData = Clipboard.GetDataObject();
                foreach (var f in formats)
                {
                    if (entireData.GetDataPresent(f))
                    {
                        format = (ClipboardFormat)Enum.Parse(typeof(ClipboardFormat), f);
                        break;
                    }
                }
                data = entireData.GetData(format.ToString());
            }
            catch (Exception ex)
            {
                MainForm.Debug(String.Format("Exception in Clipchanged(): ", ex.Message));
            }
            if (data == null || format == null)
                return;
            if (OnClipboardChange != null && lastcsqn != csqn)
            {
                OnClipboardChange((ClipboardFormat)format, data);
                lastcsqn = csqn;
            }
        }

        #region Imported WinAPI functions
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AddClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool RemoveClipboardFormatListener(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern uint GetClipboardSequenceNumber();

        #endregion
        #region Win32 constants
        const int WM_CLIPBOARDUPDATE = 0x031D;
        #endregion
    }
}
