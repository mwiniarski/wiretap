using System;
using System.Collections.Generic;

namespace WindowsStalker
{
    public class Serializer
    {
        private Sender _sender;

        public void SendFile(String path)
        {

        }

        public List<byte[]> SplitFile()
        {
            List<byte[]> toSend = new List<byte[]>();
            byte[] partOne = {1, 0, 1, 3};
            byte[] partTwo = {120, 100, 100};

            toSend.Add(partOne);
            toSend.Add(partTwo);

            return toSend;
        }

        public bool SendSplitedFile(List<byte[]> splitedFile)
        {
            _sender = new Sender("192.168.0.101", 11000);
            bool started = _sender.StartSender();
            if (!started)
            {
                Console.WriteLine("Connection not established");
                return false;
            }

            bool sent = true;
            foreach (byte[] part in splitedFile)
            {
                sent = _sender.SendFrame(part);
                if(!sent) break;
            }
            if (sent)
            {
                Console.WriteLine("File sent!");
                return true;
            }
            Console.WriteLine("File file failed to sent!");
            return false;
        }
    }
}