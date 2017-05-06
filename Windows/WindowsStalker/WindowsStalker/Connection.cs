using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WindowsStalker
{
    public class Connection
    {
        private Socket _socket;
        private int _reconnectTimeout = 1;
        private IPEndPoint _remoteEP;

        public Connection(string ipAddres, int portNumber)
        {
            IPAddress ipAddress = IPAddress.Parse(ipAddres);
            _remoteEP = new IPEndPoint(ipAddress, portNumber);
        }

        public void StartClient()
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp );

                Connect();
            }
            catch (Exception e)
            {
                Console.WriteLine( e.ToString());
                HandleDisconnection();
            }
        }

        public bool SendMessage(byte[] message)
        {
            try
            {
                int bytesSent = _socket.Send(message);

                if (bytesSent == message.Length)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when sending");
                return false;
            }
        }

        public byte[] GetMessage()
        {
            byte[] bytes = new byte[1024];
            //returns received bytes count
            _socket.Receive(bytes);

            return bytes;
        }

        public void Connect()
        {try
            {
                Console.WriteLine("Connecting ...");
                _socket.Connect(_remoteEP);
                Console.WriteLine("Socket connected to {0}",
                    _socket.RemoteEndPoint);

//                    byte[] sampleMessage = Encoding.ASCII.GetBytes("Hello man!");
//                    SendMessage(sampleMessage);
//
//                    CloseConnection();
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}",ane);
                HandleDisconnection();

            }
            catch (SocketException se)
            {
                Console.WriteLine("Connection lost");
                HandleDisconnection();
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e);
                HandleDisconnection();
            }
        }

        public void HandleDisconnection()
        {
            Console.WriteLine("Connection with sever lost... Reconnecting in " + _reconnectTimeout + " minutes.");
            Thread.Sleep(_reconnectTimeout * 10 * 1000);
            StartClient();
        }

        public void CloseConnection()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }

        public void Listen()
        {
            while (true)
            {
                try
                {
                    byte[] message = GetMessage();
                    HandleMessage(message);
                }
                catch (SocketException se)
                {
                    Console.WriteLine(se);
                    HandleDisconnection();
                }
            }
        }

        private void HandleMessage(byte[] message)
        {
            //TODO implement
            Console.WriteLine("Echoed test = {0}",
            Encoding.ASCII.GetString(message));
        }
    }
}