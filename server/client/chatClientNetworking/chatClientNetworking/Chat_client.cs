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
        public NetworkStream stream;
        public Chat_client(string ip,int _port)
        {
            p_port = _port;
            client = new TcpClient(ip, port);
            stream = client.GetStream();
        }
      
        public void Start(string UserName)
        { 
            packetCreator creator = new packetCreator();
            packetReader reader = new packetReader();
            Byte[] pac = creator.createPacet(0, 0, UserName);

            stream.Write(pac, 0, pac.Length);
            stream.Flush();
            Byte[] recevedPAC = new Byte[256];
            stream.Read(recevedPAC,0,recevedPAC.Length);
            CMDInterpreter(reader.readCMD(recevedPAC), recevedPAC);
            Thread receve = new Thread(receveMessage);
            receve.Start();
            //set up threading for receving messagess
        }
        public void sendMessage(Byte[] pac)
        { 
            locker.WaitOne();// entering the mutex so the stream is not wroute to and read at same time

            NetworkStream stream = client.GetStream();
            stream.Write(pac,0,pac.Length);
            stream.Flush();

            locker.ReleaseMutex();//exiting the mute
            //exiting the mutex
        }
        void receveMessage()
        {
            while (true)
            {
                //Console.WriteLine("receve loop called");
                packetReader reader = new packetReader();
                Byte[] buffer = new Byte[256];
                stream.Read(buffer, 0, buffer.Length);
                //locker.ReleaseMutex();
                CMDInterpreter(reader.readCMD(buffer), buffer);
                stream.Flush();
            }
        }
        void addUser(Byte[] pac)
        {
            //pac musct contain the new users ID and username
          //  Console.WriteLine("add user called");
            packetReader reader = new packetReader();
            users.Add(reader.ReadUserID(pac), reader.ReadMessage(pac, reader.readHeader(pac)));
        }
        void CMDInterpreter(int cmd,Byte[] Pac)
        {
          //  Console.WriteLine("entered cmd reader");
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
                        } else
                        { 
                            addUser(Pac);
                        }
                        break;
                    case 1:
                        //Change username
                        Console.WriteLine("change username comand called");
                        break;
                    case 2:
                        //message sent
                        Console.WriteLine(users[reader.ReadUserID(Pac)]+ ":" +reader.ReadMessage(Pac,reader.readHeader(Pac)));
                       // Console.WriteLine("receve message comand called");
                        break;
                    case 3:
                        Console.WriteLine("disconect comand called");
                        disconectOther();
                        break;
                }
            }
            else
            {
                Console.WriteLine("invaled comand");
            }
        }
        public void disconectSelf(TcpClient client)
        {
            packetCreator creator = new packetCreator();
            Byte[] endPacket = creator.createPacet(UserID, 3,"");
            sendMessage(endPacket);
            client.Close();
        }
        void disconectOther()
        {

        }
    }
}
