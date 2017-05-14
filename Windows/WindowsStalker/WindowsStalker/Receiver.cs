using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WindowsStalker
{
    public class Receiver
    {
//        private Socket _socket;
//        private int _reconnectTimeout = 1;
//        private IPEndPoint _remoteEP;
//        private String response;
//        // Thread signal.
//        public static ManualResetEvent allDone = new ManualResetEvent(false);
//        private static ManualResetEvent connectDone = new ManualResetEvent(false);
//        private static ManualResetEvent receiveDone = new ManualResetEvent(false);
//
//        public Receiver(string ipAddres, int portNumber)
//        {
//            IPAddress ipAddress = IPAddress.Parse(ipAddres);
//            _remoteEP = new IPEndPoint(ipAddress, portNumber);
//        }
//
//        public void StartListening()
//        {
//            try
//            {
//                _socket = new Socket(AddressFamily.InterNetwork,
//                    SocketType.Stream, ProtocolType.Tcp );
//
//                Connect();
//                connectDone.WaitOne();
//
//                Listen();
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine( e.ToString());
//                HandleDisconnection();
//            }
//        }
//
//        public void Connect()
//        {
//            try
//            {
//                Console.WriteLine("Connecting ...");
//                _socket.BeginConnect(_remoteEP, ConnectCallback, _socket);
//            }
//            catch (ArgumentNullException ane)
//            {
//                Console.WriteLine("ArgumentNullException : {0}",ane);
//                HandleDisconnection();
//
//            }
//            catch (SocketException se)
//            {
//                Console.WriteLine("Sender lost");
//                HandleDisconnection();
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("Unexpected exception : {0}", e);
//                HandleDisconnection();
//            }
//        }
//
//        private void ConnectCallback(IAsyncResult ar)
//        {
//            try
//            {
//                Socket socket = (Socket) ar.AsyncState;
//                socket.EndConnect(ar);
//
//                Console.WriteLine("Socket connected to {0}", _socket.RemoteEndPoint);
//
//                connectDone.Set();
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//            }
//        }
//
//        public void HandleDisconnection()
//        {
//            Console.WriteLine("Sender with sever lost... Reconnecting in " + _reconnectTimeout + " minutes.");
//            Thread.Sleep(_reconnectTimeout * 10 * 1000);
//            StartListening();
//        }
//
//        public void GetMessage()
//        {
//            try
//            {
//                StateObject state = new StateObject {workSocket = _socket};
//
//                _socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e);
//            }
//
//        }
//
//        private void ReceiveCallback(IAsyncResult ar)
//        {
//            try
//            {
//                StateObject state = (StateObject) ar.AsyncState;
//                Socket client = state.workSocket;
//
//                // Read data from the remote device.
//                int bytesRead = client.EndReceive(ar);
//
//                if (bytesRead > 0) {
//                    // There might be more data, so store the data received so far.
//                    ArrayList newBytes;
//                    state.messageBuilder.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
//
//                    // Get the rest of the data.
//                    client.BeginReceive(state.buffer,0,StateObject.BufferSize,0,
//                        new AsyncCallback(ReceiveCallback), state);
//                } else {
//                    // All the data has arrived; put it in response.
//                    if (state.messageBuilder.Length > 1) {
//                        response = state.messageBuilder.ToString();
//                    }
//                    // Signal that all bytes have been received.
//                    receiveDone.Set();
//                }
//            }
//            catch (Exception e)
//            {
//                Console.WriteLine("TODO handle disconnect");
//            }
//        }
//
//        public void Listen()
//        {
//            while (true)
//            {
//                try
//                {
//                    receiveDone.Reset();
//                    GetMessage();
//                    receiveDone.WaitOne(10000);
//                    Console.WriteLine("Już nie czekam XD");
//                    HandleMessage(response);
//                }
//                catch (SocketException se)
//                {
//                    Console.WriteLine(se);
//                    HandleDisconnection();
//                }
//            }
//        }
//
//        private void HandleMessage(String message)
//        {
//            //TODO implement
//            Console.WriteLine("Echoed test = {0}",
//                response);
//        }
    }
}