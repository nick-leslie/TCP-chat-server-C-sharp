using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.Generic;
namespace chatClientNetworking
{
    public class Chat_client
    {
        private int p_port = 0;
        public int port { get => p_port; set { if (value != 0) { p_port = value; } } }
        public int UserID;
        public Dictionary<int, string> users = new Dictionary<int, string>();
        public TcpClient client;
        public NetworkStream stream;
        private static Mutex locker = new Mutex();
        public Chat_client(string ip,int _port)
        {
            p_port = _port;
            client = new TcpClient(ip, port);
            NetworkStream stream = client.GetStream();
        }
        public void MicrosoftConect(String server, Byte[] packet)
        {
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = 1234;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.

                Byte[] data = new Byte[256];

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(packet, 0, packet.Length);

                Console.WriteLine("Sent: {0}", packet);
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
        public void Start(string UserName)
        { 
            packetCreator creator = new packetCreator();
            packetReader reader = new packetReader();
            Byte[] pac = creator.createPacet(0, 0, UserName);


            stream.Write(pac, 0, pac.Length);

            Byte[] recevedPAC = new Byte[256];
            stream.Read(recevedPAC,0,recevedPAC.Length);
            CMDInterpreter(reader.readCMD(recevedPAC), recevedPAC);
            Thread receve = new Thread(receveMessage);
            //set up threading for receving messagess
        }
        void sendMessage(Byte[] pac)
        {
           locker.WaitOne();// entering the mutex so the stream is not wroute to and read at same time
            stream.Write(pac,0,pac.Length);
            locker.ReleaseMutex();//exiting the mutex
        }
        void receveMessage()
        {
            packetReader reader = new packetReader();
            locker.WaitOne();
            Byte[] buffer = new Byte[256];
            stream.Read(buffer, 0, buffer.Length);
            CMDInterpreter(reader.ReadUserID(buffer), buffer);
            locker.ReleaseMutex();
        }
        void addUser(Byte[] pac)
        {
            // pac musct contain the new users ID and user name
            packetReader reader = new packetReader();
            users.Add(reader.ReadUserID(pac), reader.ReadMessage(pac, reader.readHeader(pac)));
        }
        void CMDInterpreter(int cmd,Byte[] Pac)
        {
            packetReader reader = new packetReader();
            if (cmd <= 3)
            {
                //  Console.WriteLine("entered the cmd structue");
                switch (cmd)
                {
                    case 0:
                        if (UserID == 0)
                        {
                            UserID = reader.ReadUserID(Pac);
                            Console.WriteLine(UserID + " the seting up a user ID is a scuckseess");
                        } else
                        {
                            Console.WriteLine("adding a new user"); 
                            addUser(Pac);
                        }
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
            }
            else
            {
                Console.WriteLine("invaled comand");
            }
        }

    }
}
