using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace server_programs
{
    class chat_server
    {
        int HeaderSize;
        public void start(int _port)
        {
            int port = _port;
            TcpListener server = new TcpListener(IPAddress.Any,port);
            server.Start();
            Console.WriteLine("ready to reseve conections");
            while (true)
            {
                server.BeginAcceptTcpClient(handdleConections, server);
            }
     
        }
        void handdleConections(IAsyncResult result)
        {
            TcpListener server = (TcpListener)result.AsyncState;
            TcpClient client = server.EndAcceptTcpClient(result);
            packetReader reader = new packetReader();
            Byte[] data = new Byte[256];

            NetworkStream stream = client.GetStream();
            stream.Read(data, 0, data.Length);
            reader.readPacet(data);
        }
        void WelcomeUser()
        {

        }
        void Send()
        {

        }
        void Receve()
        {

        }
    }
}
