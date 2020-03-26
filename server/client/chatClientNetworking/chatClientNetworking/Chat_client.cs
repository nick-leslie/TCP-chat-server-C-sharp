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
        private static Mutex locker = new Mutex();
        public Chat_client(string ip,int _port)
        {
            p_port = _port;
            client = new TcpClient(ip, port);
        }
      
        public void Start(string UserName)
        { 
            packetCreator creator = new packetCreator();
            packetReader reader = new packetReader();
            Byte[] pac = creator.createPacet(0, 0, UserName);

            NetworkStream stream = client.GetStream();
            stream.Write(pac, 0, pac.Length);

            Byte[] recevedPAC = new Byte[256];
            stream.Read(recevedPAC,0,recevedPAC.Length);
            CMDInterpreter(reader.readCMD(recevedPAC), recevedPAC);
            Thread receve = new Thread(receveMessage);
            //set up threading for receving messagess
        }
        public void sendMessage(Byte[] pac)
        { 
            Console.WriteLine("sending message;");
           locker.WaitOne();// entering the mutex so the stream is not wroute to and read at same time
            NetworkStream stream = client.GetStream();
            Console.WriteLine("sending message mutex");
            stream.Write(pac,0,pac.Length);
            locker.ReleaseMutex();//exiting the mutex
        }
        void receveMessage()
        {
            while (true)
            {
                packetReader reader = new packetReader();
                locker.WaitOne();
                Byte[] buffer = new Byte[256];
                NetworkStream stream = client.GetStream();
                stream.Read(buffer, 0, buffer.Length);
                CMDInterpreter(reader.ReadUserID(buffer), buffer);
                locker.ReleaseMutex();
                Thread.Sleep(500);
            }
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
