using System;
using System.Management;
using System.Threading;

namespace Monitorizare
{
    public delegate void ProcessMonitorEventHandler (object sender, ProcessMonitorEventArgs e);

    public class ProcessMonitorEventArgs:EventArgs
    {
        public ProcessMonitorEventArgs(object processName,object executablePath, UInt32 pid, object args,string activeapptitle):base()
        {
            ProcessName = (string)processName;
            ExecutablePath = (string)executablePath;
            PID = Int32.Parse(pid.ToString());
            Arguments = (string)args;
            ActiveAppTitle = activeapptitle;
        }
        public string ProcessName, ExecutablePath, Arguments, ActiveAppTitle;
        public Int32 PID;
    }
    class ProcessMonitor
    {
        public ProcessMonitor()
        {
            // Create event query to be notified within 1 second of 
            // a change in a service
            query = new WqlEventQuery("__InstanceCreationEvent", new TimeSpan(0, 0, 1), "TargetInstance isa \"Win32_Process\"");
            watcher.Query = query;
        }

        Thread pmThread;
        public event ProcessMonitorEventHandler ProcessStart;
        WqlEventQuery query;
        // Initialize an event watcher and subscribe to events 
        // that match this query
        ManagementEventWatcher watcher = new ManagementEventWatcher();

        public void Start()
        {          
            pmThread=new Thread(new ThreadStart(ThStartMethod));
            pmThread.IsBackground = true;
            pmThread.Start();
        }
        public void Stop()
        {
            //Cancel the subscription
            watcher.Stop();
        }
        void ThStartMethod()
        {
            // Block until the next event occurs 
            // Note: this can be done in a loop if waiting for more than one occurrence
            ManagementBaseObject mbo = watcher.WaitForNextEvent();

            // Wait for UI application (if any) to open
            // so we can get its title
            Thread.Sleep(500);
            UInt32 pid = UInt32.Parse((((ManagementBaseObject)mbo["TargetInstance"])["ProcessId"]).ToString());
            string aat = new string(' ', 100);
            if (pid == WinAPI.GetWindowProcessID(WinAPI.GetForegroundWindow()))
                aat = WinAPI.ActiveApplTitle();
            else
                aat = null;
            ProcessMonitorEventArgs e = new ProcessMonitorEventArgs(((ManagementBaseObject)mbo["TargetInstance"])["Name"],
                                                                 ((ManagementBaseObject)mbo["TargetInstance"])["ExecutablePath"],pid,
                                                                 ((ManagementBaseObject)mbo["TargetInstance"])["CommandLine"],aat);
            // Rise ProcessStart event
            if (ProcessStart != null)
            {
                ProcessStart(this, e);
            }
            ThStartMethod();
        }
    }
}
