using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;

namespace Go.Model
{
    class Connection
    {
        private static Connection _instance;

        public static Connection Instance
        {
            get
            {
                if (_instance == null) _instance = new Connection();
                return _instance;
            }
        }
        public TcpClient Client { get; set; }

        public void Send(string data)
        {
            if (Instance == null)
            {
                Debug.WriteLine("C/S : , Instance is null. Returning.");
                return; 
            }
            else
            {
                try
                {
                    NetworkStream stream = Instance.Client.GetStream();
                    byte[] message = Encoding.UTF8.GetBytes(data);
                    stream.Write(message, 0, message.Length);
                } 
                catch (Exception e)
                {
                    Debug.WriteLine("C/S : Caught, " + e.Message);
                }
            }
        }

        public void ClosingSequence()
        {
            if (Client == null) return;

            if (Client.Connected)
            {
                Instance.Send("QUIT lapumb");
                Debug.WriteLine("Sent 'QUIT', receiving: " + Instance.Receive());
                Client.Close();
            }

            Client.Dispose();
            Instance.Client = null;
        }

        public string Receive()
        {
            StringBuilder myCompleteMessage = new StringBuilder();
            if (Instance == null)
            {
                Debug.WriteLine("C/R : , Instance is null. Returning.");
                return myCompleteMessage.ToString();
            }
            else
            {
                NetworkStream stream = Instance.Client.GetStream();
                // Check to see if this NetworkStream is readable.
                if (stream.CanRead)
                {
                    byte[] myReadBuffer = new byte[1024];
                    int numberOfBytesRead = 0;

                    // Incoming message may be larger than the buffer size.
                    do
                    {
                        numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);
                        myCompleteMessage.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));
                    }
                    while (stream.DataAvailable);
                    // Print out the received message to the console.
                    Console.WriteLine("You received the following message : " +
                                                 myCompleteMessage);
                }
                else
                {
                    Console.WriteLine("Sorry.  You cannot read from this NetworkStream.");
                }
            }
            return myCompleteMessage.ToString();
        }
    }
}
