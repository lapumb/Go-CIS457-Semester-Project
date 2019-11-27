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
                NetworkStream stream = Instance.Client.GetStream();
                byte[] message = Encoding.ASCII.GetBytes(data);
                stream.Write(message, 0, message.Length);
            }
        }
    }
}
