using System;

namespace chatClientNetworking
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
            Chat_client client = new Chat_client(, 1234);
            packetCreator PacketHandler = new packetCreator();
            Console.Write("enter username ");
            client.Start(Console.ReadLine());
            while(true)
            {
                Console.Write("enter message");
                client.sendMessage(PacketHandler.createPacet(client.UserID, 2, Console.ReadLine()));
            }
                
        }
    }
}
