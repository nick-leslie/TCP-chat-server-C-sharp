using System;

namespace chatClientNetworking
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            Chat_client client = new Chat_client("10.1.1.48", 1234);
            packetCreator PacketHandler = new packetCreator();
            client.Start("swordcom36");
            while(true)
            {
                client.sendMessage(PacketHandler.createPacet(client.UserID, 2, Console.ReadLine()));
            }
                
        }
    }
}
