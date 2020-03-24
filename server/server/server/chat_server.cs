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
            comandInterpriter(reader.readCMD(data));
            reader.readPacet(data);
        }
        void comandInterpriter (int cmd)
        {
            if (cmd <= 3)
            {
                switch (cmd)
                {
                    case 0:
                        //TODO start setion
                        Console.WriteLine("start setion comand called");
                        break;
                    case 1:
                        //Change username
                        Console.WriteLine("change username comand called");
                        break;
                    case 2:
                        //sending message
                        Console.WriteLine("send message comand called");
                        break;
                    case 3:
                        Console.WriteLine("disconect comand called");
                        //disconect
                        break;
                }
            } else
            {
                Console.WriteLine("invaled comand");
            }
            
        }
        void WelcomeUser()
        {

        }
        void Send()
        {

        }
        void disconect()
        {

        }
        void Receve()
        {

        }
    }
}
