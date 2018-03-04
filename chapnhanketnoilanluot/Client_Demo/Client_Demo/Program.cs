using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client_Demo
{
    class ClientDemo
    {
        private TcpClient _client;
        private Boolean _isConnected;

        public ClientDemo(String ipAddress, int portNum)
        {
            _client = new TcpClient();
            _client.Connect(ipAddress, portNum);
            HandleCommunication();
        }
        public void HandleCommunication()
        {
            //_sReader = new StreamReader(_client.GetStream(), Encoding.ASCII);
            //_sWriter = new StreamWriter(_client.GetStream(), Encoding.ASCII);
            NetworkStream stream = _client.GetStream();
            _isConnected = true;
            String sData = null;
            Byte[] bytes = new Byte[256];
            int i;
            while (_isConnected)
            {
                // repuest
                Console.Write("Client 1 :");
                sData = Console.ReadLine();
                // send
                byte[] msg = System.Text.Encoding.ASCII.GetBytes(sData);
                stream.Write(msg,0,msg.Length);
                // reponse
                i = stream.Read(bytes, 0, bytes.Length);
                string str = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Server :"+str);
                if (str.Equals("bye"))
                {
                    Console.ReadKey();
                    break;
                }
                   
            }
            stream.Close();
            _client.Close();

        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Mutil Threaded TCP Server Demo");
            Console.WriteLine("Provide TCP: ");
            String ip = Console.ReadLine();
            Console.WriteLine("Provide Port:");
            int port = Int32.Parse(Console.ReadLine());
            ClientDemo client = new ClientDemo(ip, port);
        }
    }
}
