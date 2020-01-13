using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;

namespace sMonitorizare
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if DEBUG
            sMonitorizare smin1 = new sMonitorizare();
            smin1.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new sMonitorizare() 
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
