using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace WindowsStalker
{
    public class StateObject {
        // Client socket.
        public Socket workSocket = null;
        // Size of receive buffer.
        public const int BufferSize = 1;
        // Receive buffer.
        public byte[] buffer = new byte[BufferSize];
        // Received data string.
        public byte ACK;
    }

    public class Sender
    {
        private Socket _socket;
        private const int _reconnectTimeout = 1;
        private readonly IPEndPoint _remoteEP;
        private string response;
        private const int _sendTimeout = 5;
        private const int _ackTimeout = 5;
        private const int _connectionTimeout = 5;
        private int _sendData;
        private bool _ack;
        private bool _connected;

        // ManualResetEvent instances signal completion.
        private static ManualResetEvent connectDone =
            new ManualResetEvent(false);
        private static ManualResetEvent sendDone =
            new ManualResetEvent(false);
        private static ManualResetEvent receiveDone =
            new ManualResetEvent(false);

        public Sender(string ipAddres, int portNumber)
        {
            IPAddress ipAddress = IPAddress.Parse(ipAddres);
            _remoteEP = new IPEndPoint(ipAddress, portNumber);
        }

        public bool StartSender()
        {
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork,
                    SocketType.Stream, ProtocolType.Tcp );

                Connect();
                connectDone.WaitOne(_connectionTimeout * 1000);
                return _connected;
            }
            catch (Exception e)
            {
                Console.WriteLine("Exeption couldn't start a sender");
                return false;
            }
        }

        public bool SendFrame(byte[] message)
        {
            sendDone.Reset();
            _ack = false;
            _sendData = 0;

            _socket.BeginSend(message, 0, message.Length, 0, SendCallback, _socket);

            sendDone.WaitOne(_sendTimeout * 1000);
            if (_sendData == message.Length)
            {
                GetMessage();
                receiveDone.WaitOne(_ackTimeout * 1000);
                if (_ack)
                {
                    Console.WriteLine("Ack send back!");
                }
                else
                {
                    Console.WriteLine("Ack not sent back :< file not sent ...");
                }
                return _ack;
            }
            return false;
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket) ar.AsyncState;

                // Complete sending the data to the remote device.
                int bytesSent = client.EndSend(ar);
                _sendData = bytesSent;
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                sendDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exeption: Error when sending!");
            }
        }

        public void GetMessage()
        {
            try
            {
                receiveDone.Reset();
                _ack = false;

                StateObject state = new StateObject {workSocket = _socket};

                _socket.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, ReceiveCallback, state);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exeption: Error when getting a message");
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            try
            {
                StateObject state = (StateObject) ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device.
                int bytesRead = client.EndReceive(ar);

                if (bytesRead == 1) {
                    byte ack = state.buffer[0];
                    _ack = ack == 1;
                    receiveDone.Set();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exeption: Error when receiving a message(callback)");
            }
        }

        public void Connect()
        {
            try
            {
                Console.WriteLine("Connecting ...");
                _socket.BeginConnect(_remoteEP, ConnectCallback, _socket);
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}",ane);
            }
            catch (SocketException se)
            {
                Console.WriteLine("Sender lost");
            }
            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e);
            }
        }

        private void ConnectCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = (Socket) ar.AsyncState;
                socket.EndConnect(ar);

                Console.WriteLine("Socket connected to {0}", _socket.RemoteEndPoint);

                _connected = true;
                connectDone.Set();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void CloseConnection()
        {
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
    }
}