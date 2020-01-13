using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Monitorizare
{
    public class DetectorForm:System.Windows.Forms.Form
    {
        DriveMonitor driveMonitor = null;
        ClipboardMonitor clipboardMonitor = null;
        public DetectorForm(DriveMonitor monitor)
        {
            InitializeComponent();
            driveMonitor = monitor;
        }
        public DetectorForm(ClipboardMonitor monitor)
        {
            InitializeComponent();
            clipboardMonitor = monitor;
        }
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (driveMonitor != null)
                driveMonitor.WndProc(ref m);
            if (clipboardMonitor != null)
                clipboardMonitor.WndProc(ref m);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(10, 10);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DetectorForm";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.ResumeLayout(false);

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
    }
}
