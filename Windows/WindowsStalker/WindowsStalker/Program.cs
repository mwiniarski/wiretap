using System.Management;
using System.Threading;

namespace WindowsStalker
{
    public class Program
    {
        private Thread _scheduleThread;
        private Thread _listeningThread;

        public static void Main(string[] args)
        {
            Scheduler s = new Scheduler();
            Thread sThread = new Thread(s.StartScheduling);
            sThread.Start();
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