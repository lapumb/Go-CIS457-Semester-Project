using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace CentralServer
{
    class Program
    {
        public static TcpClient Client { get; set; }
        private static TcpListener Listener { get; set; }
        public static string IpString { get; set; }

        static void Main(string[] args)
        {
            IPAddress[] localIp = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress address in localIp)
            {
                if (address.AddressFamily == AddressFamily.InterNetwork)
                {
                    IpString = address.ToString();
                }
            }

            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(IpString), 1234);
            Listener = new TcpListener(ep);
            Listener.Start();
            Console.WriteLine(@"    
            ===================================================    
                   Started listening requests at: {0}:{1}    
            ===================================================",
            ep.Address, ep.Port);
            Client = Listener.AcceptTcpClient();
            Console.WriteLine("Connected to client!" + " \n");

            while (Client.Connected)
            {
                try
                {
                    const int bytesize = 1024 * 1024;
                    byte[] buffer = new byte[bytesize];
                    string x = Client.GetStream().Read(buffer, 0, bytesize).ToString();
                    var data = ASCIIEncoding.ASCII.GetString(buffer);

                    // TO SEND : (I think) Client.Client.Send(...); 

                    if (data.ToUpper().Contains("SLP2"))
                    {
                        Console.WriteLine("Pc is going to Sleep Mode!" + " \n");
                    }
                }
                catch (Exception exc)
                {
                    Client.Dispose();
                    Client.Close();
                    Debug.WriteLine("P/M: Exception caught : " + exc.Message);
                }
            }
        }
    }
}
