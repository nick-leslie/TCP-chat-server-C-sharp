using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
namespace server
{
    class chat_server
    {
        void start(int _port)
        {
            int port = _port;
            TcpListener server = new TcpListener(IPAddress.Any,port);
            server.Start();
            Console.WriteLine("ready to reseve conections");
            server.BeginAcceptTcpClient(handdleConections,server);
     
        }
        void handdleConections(IAsyncResult result)
        {
            TcpListener server = (TcpListener)result.AsyncState;
            TcpClient client = server.EndAcceptTcpClient(result);
            
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
