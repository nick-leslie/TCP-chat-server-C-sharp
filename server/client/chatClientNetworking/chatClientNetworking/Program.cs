using System;

namespace chatClientNetworking
{
    class Program
    {
        static void Main(string[] args)
        {
            Chat_client client = new Chat_client(1234);
            packetCreator PacketHandler = new packetCreator();
            //for (int i = 0; i < 4 ; i++)
            //{
                client.Start("10.1.1.48","swordcom36");
            //}
        }
    }
}
