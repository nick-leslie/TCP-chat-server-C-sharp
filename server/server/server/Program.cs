using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace server_programs
{
    class hub
    {
        static void Main(string[] args)
        {
            Console.WriteLine(getLocalIP());
            Console.ReadKey();
            //  int port = 1234;
            // Server s = new Server();
            //s.start(port);
            //  TestingAsynch test = new TestingAsynch();
            // test.start(port);
            packetCreator packer = new packetCreator();
            packetReader interpreter = new packetReader();
            interpreter.readPacet(packer.createPacet(1, "this is a test"));
            
        }
        public static string getLocalIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return ("cannt find adddress");
    }
    }
}
