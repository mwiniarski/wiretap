using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WindowsStalker
{
    public class Connection
    {
        public static void StartClient()
        {
            byte[] bytes = new byte[1024];

            try
            {
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress,11000);

                Socket socket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp );

                try
                {
                    socket.Connect(remoteEP);

                    Console.WriteLine("Socket connected to {0}",
                        socket.RemoteEndPoint.ToString());

                    byte[] msg = Encoding.ASCII.GetBytes("This is a test<EOF>");
                    int bytesSent = socket.Send(msg);
                    int bytesRec = socket.Receive(bytes);
                    Console.WriteLine("Echoed test = {0}",
                        Encoding.ASCII.GetString(bytes,0,bytesRec));

                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}",ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}",se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine( e.ToString());
            }
        }
    }
}