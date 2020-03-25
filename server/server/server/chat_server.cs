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
        Dictionary<int, string> users = new Dictionary<int, string>();
        Dictionary<int, IPAddress> userIP = new Dictionary<int, IPAddress>();
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
            IPAddress clinetIp = IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());


            NetworkStream stream = client.GetStream();
            stream.Read(data, 0, data.Length);
            if (reader.readVerification(data))
            {
                comandInterpriter(reader.readCMD(data),data,clinetIp);
            }
            
        }
        void comandInterpriter (int cmd,Byte[] pac,IPAddress address)
        {
            //Console.WriteLine("entering the comand interpriter not enter safety mode yet");
            if (cmd <= 3)
            {
              //  Console.WriteLine("entered the cmd structue");
                switch (cmd)
                {
                    case 0:
                        startSetion(pac,address);
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
        void startSetion(Byte[] pac,IPAddress address)
        {
            Console.WriteLine("start setion comand called");
            packetReader reader = new packetReader();
            users.Add(reader.ReadUserID(pac), reader.ReadMessage(pac, reader.readHeader(pac)));
            userIP.Add(reader.ReadUserID(pac), address);
            foreach(KeyValuePair<int,String> kvp in users)
            {
                Console.WriteLine("key = {0}, value ={1}",kvp.Key,kvp.Value);
            }
            foreach (KeyValuePair<int, IPAddress> kvp in userIP)
            {
                Console.WriteLine("key = {0}, value ={1}", kvp.Key, kvp.Value);
            }
        }
        void Send(Byte[] pac)
        {
            packetReader reader = new packetReader();
            foreach (KeyValuePair<int,IPAddress> kvp in userIP)
            {
                if(kvp.Key != reader.ReadUserID(pac) )
                {
                    // write send code
                }
            }
        }
        void disconect()
        {

        }
        void Receve()
        {

        }
    }
}
