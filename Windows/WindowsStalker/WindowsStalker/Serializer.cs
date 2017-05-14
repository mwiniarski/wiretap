using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WindowsStalker
{
    public class Serializer
    {
        private const int _packetSize = 256;
        private Sender _sender;

        public List<byte[]> SendFile(string path, byte dataType)
        {
            using(var fileStream = new FileStream(path, FileMode.Open))
            {
                List<byte[]> splitedFile = new List<byte[]>();
                var buffer = new byte[_packetSize];
                var bytesRead = 0;
                int framesCount = 0;
                int lastFrameSize = 0;
                while((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (bytesRead == _packetSize)
                    {
                        splitedFile.Add((byte[]) buffer.Clone());
                    }
                    else
                    {
                        lastFrameSize = bytesRead;
                        splitedFile.Add((byte[]) buffer.Clone());
                    }
                    framesCount++;
                }
                byte byteCount = (byte) (framesCount % _packetSize);
                byte byteCount2 = (byte) (framesCount/_packetSize);

                Console.WriteLine("frame count: " + framesCount);
                int filesize = byteCount + (byteCount2 * 256);
                Console.WriteLine("File size: " + filesize);

                byte[] firstFrame = {dataType, byteCount2, byteCount, (byte)lastFrameSize};
                splitedFile.Insert(0, firstFrame);

                return splitedFile;
            }
        }

        public List<byte[]> Test()
        {
            List<byte[]> send = new List<byte[]>();
            byte[] frame = {1, 2, 3, 4};
            send.Add(frame);
            return send;
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
            _sender = new Sender("192.168.0.199", 8888);
            bool started = _sender.StartSender();
            if (!started)
            {
                Console.WriteLine("Connection not established");
                return false;
            }

            bool sent = true;
            int counter = 0;
            foreach (byte[] part in splitedFile)
            {
                sent = _sender.SendFrame(part);
                counter++;
                if(!sent) break;
            }
            if (sent)
            {
                Console.WriteLine("File sent!");
                return true;
            }
            Console.WriteLine("File file failed to sent!");
            Console.WriteLine("Packets send: " + counter);
            return false;
        }
    }
}