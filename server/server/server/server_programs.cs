using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace server_programs
{
    class Server
    {
        public Server()
        {

        }
        public  void start( int _port)
        {
            try { 
                int port = _port;
                Byte[] bytes = new Byte[256];
                string data;
                TcpListener lissener = new TcpListener(IPAddress.Any, port);
                lissener.Start();

                while (true)
                {
                    Console.WriteLine("waiting for conection");
                    TcpClient client = lissener.AcceptTcpClient();
                    Console.WriteLine("conected to {0}", IPAddress.Parse(((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()));

                    data = null;

                    NetworkStream stream = client.GetStream();

                    int i = 0;

                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("receved:{0}", data);
                        // this is processing the data can be converted latter into something else 
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("sent:{0}", data);
                    }
                    client.Close();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("error:{0}", e);
            }
        }
    }
}
