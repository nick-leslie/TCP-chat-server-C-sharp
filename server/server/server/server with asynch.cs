using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace server_programs
{
    class TestingAsynch
    {
        private int _headerlength;
        public int headerlength { get => head; }
        public void start(int _port)
        {
            int port = _port;

            TcpListener lissener = new TcpListener(IPAddress.Any, port);
            lissener.Start();
            Console.WriteLine("lissing for conection");
            while (true)
            {
                lissener.BeginAcceptTcpClient(onAccept, lissener);
            }
        }
        void onAccept(IAsyncResult result)
        {
            TcpListener listener = (TcpListener)result.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(result);
            string data;
            Byte[] bytes = new Byte[256];


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
}
