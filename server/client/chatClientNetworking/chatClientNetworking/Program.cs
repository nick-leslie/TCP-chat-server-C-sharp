using System;

namespace chatClientNetworking
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ReadLine();
<<<<<<< HEAD
            Chat_client client = new Chat_client(, 1234);
=======
            Chat_client client = new Chat_client("10.1.1.48", 1234);
>>>>>>> 2201d7e8642f06d6e4fb82d9da00f00abc6d0eab
            packetCreator PacketHandler = new packetCreator();
            Console.Write("enter username ");
            client.Start(Console.ReadLine());
            while(true)
            {
                Console.WriteLine("enter message or command");
                string clientInput = Console.ReadLine();
                if (clientInput != "quit")
                {
                    if (clientInput != "change usernames")
                    {
                        client.sendMessage(PacketHandler.createPacet(client.UserID, 2, clientInput));
                    } else
                    {
                        client.sendMessage(PacketHandler.createPacet(client.UserID, 1, clientInput));
                    }
                }
                else
                {
                    client.disconect(client.client);
                }
            }
                
        }
    }
}
