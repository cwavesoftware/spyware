using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Timers;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace Monitorizare
{
    /// <summary>
    ///  ScreenCapturer will take printscreens at a time interval 
    ///  mentioned in <interval> param. Its static TakeShot() method
    ///  provides posibility for taking ocasional, single printscreens. 
    /// </summary>
    class ScreenCapturer
    {
        public ScreenCapturer(int interval)
        {
            Interval = interval;
        }
        public int Interval;

        public void Start()
        {
            Thread scapThread = new Thread(new ParameterizedThreadStart(scapThreadMethod));
            scapThread.IsBackground = true;
            scapThread.Start(Interval);
        }
        static void scapThreadMethod(object pause)
        {
            Fotographer.TakeShot();
            Thread.Sleep((int)pause);
            scapThreadMethod(pause);
        }

        public static class Fotographer
        {
            static Bitmap bmp;
            static Graphics g;
            static string fname;

            public static bool TakeShot()
            {
                try
                {
                    bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height,
                                                                                PixelFormat.Format32bppArgb);
                    g = Graphics.FromImage(bmp);
                    fname = "screen" + System.DateTime.Now.ToString().Replace(':', '-').Replace('/', '.') + ".jpeg";
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0,
                                        Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
                    bmp.Save(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)+
                                                       "\\Monitorizare\\Screencaptures\\"+fname);
                    return true;
                }
                catch (Exception ex)
                {
                    MainForm.Debug(String.Format("Source: winsvc, Exception in Fotographer.Takeshot(): {0}",ex.Message));
                    return false; 
                }
            }
        }        
    }
}
