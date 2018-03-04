using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server_Demo
{
    class TcpServer
    {
        private TcpListener _server;
        private int count = 0;

        public TcpServer(int port)
        {
            _server = new TcpListener(IPAddress.Any, port);
            _server.Start();
            if(count<10)
            LoopClients();
        }
        public void LoopClients()
        {

            // wait for  client connection
               count++;
                TcpClient newclient = _server.AcceptTcpClient();
            // client found
            // create a thread to handle commuication                
            Thread t = new Thread((obj) => { HandleClient((TcpClient)obj); });
            t.Start(newclient);
        }
        public void HandleClient( Object obj)
        {
            // retrieve client from paramater pass to thread
            TcpClient client = (TcpClient)obj;
            // sets two streams
            NetworkStream stream = client.GetStream();
            
            // you could use the NetworkSteam to read and write
            // but there is no forcing flush, even when requested
            Console.WriteLine("Connected !");
            String sData = null;
         //   Boolean bClientConnected = true;
            Byte[] bytes = new Byte[256];
        
            while (client.Connected)
            {
                // read 
                int i = stream.Read(bytes, 0, bytes.Length);
             
                sData = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Client :" + sData);
                // send
                byte[] a;
                if (sData.Equals("exit"))
                {
                    a = System.Text.Encoding.ASCII.GetBytes("bye");
                    stream.Write(a, 0, a.Length);
                    LoopClients();
                    break;
                }
                
                 a = System.Text.Encoding.ASCII.GetBytes("Dong y"+count);
                stream.Write(a, 0, a.Length);        
            }
            stream.Close();
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Mutil Threaded TCP Server Demo:");
            TcpServer server = new TcpServer(5555);
        }
    }
}
