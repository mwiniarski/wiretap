using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TestServer
{
    public class Server
    {
        // Incoming data from the client.
        public static string data = null;

        public static void StartListening() {
            // Data buffer for incoming data.
            byte[] bytes = new Byte[1024];
            int portNumber = 11000;
            Mode serverMode = Mode.ECHO;

            // Establish the local endpoint for the socket.
            // Dns.GetHostName returns the name of the
            // host running the application.
            IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, portNumber);


            // Create a TCP/IP socket.
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp );

            // Bind the socket to the local endpoint and
            // listen for incoming connections.
            try {
                listener.Bind(localEndPoint);
                listener.Listen(10);

            // Start listening for connections.

                Console.WriteLine("Waiting for a connection... at " + ipAddress + " " +  portNumber);
                // Program is suspended while waiting for an incoming connection.
                Socket handler = listener.Accept();
                Console.WriteLine("Connection accepted");
                data = null;

                // An incoming connection needs to be processed.
                while (true) {
                    if (serverMode == Mode.ECHO)
                    {
                        bytes = new byte[1024];
                        int bytesRec = handler.Receive(bytes);
                        data = Encoding.ASCII.GetString(bytes,0,bytesRec);

                        // Show the data on the console.
                        Console.WriteLine( "Text received : {0}", data);
                        // Echo the data back to the client.
                        byte[] msg = Encoding.ASCII.GetBytes(data);
                        handler.Send(msg);
                    }
                    else if (serverMode == Mode.DISCONNECT)
                    {
                        break;
                    }

                }
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }
    }
}