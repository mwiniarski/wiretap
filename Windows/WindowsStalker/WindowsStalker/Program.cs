using System;
using System.Diagnostics;
using System.Management;
using System.Threading;

namespace WindowsStalker
{
    public class Program
    {
        private Thread _scheduleThread;

        public static void Main(string[] args)
        {
            Scheduler scheduler = new Scheduler();
            scheduler.StartScheduling();

            Stopwatch s = new Stopwatch();
            s.Start();

            while (true)
            {
                if (s.Elapsed > TimeSpan.FromSeconds(45))
                {
                    scheduler.UpdateConfiguration(15, 15);
                    s.Stop();
                    break;
                }
            }
        }

        public static string GetComputerID()
        {
            ManagementObjectCollection mbsList = null;
            ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_processor");
            mbsList = mbs.Get();
            string id = "";
            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorID"].ToString();
            }

            return id;
        }

        public void ApplyNewConfig()
        {

        }
    }
}