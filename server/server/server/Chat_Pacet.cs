using System;
using System.Collections.Generic;
using System.Text;

namespace server_programs
{
    class packetCreator
    {
        List<Byte> buffer;
        int headerSize = 10;
        int userIDLength = 2;
        int packetVarificationLength = 4;
        string packetVerification = "NL36";
       public packetCreator()
        {
            buffer = new List<byte>();
        }
       public Byte[] createPacet(int userID,int cmd,string Message)
        {
            writeCMD(cmd);
            writeUserID(userID);
            write(Message);
            Byte[] packet;
            //creaing the header
            int messagelength = buffer.Count - userIDLength;
            string messageHeader = messagelength.ToString().PadRight(headerSize);
            Console.WriteLine(messageHeader);
            buffer.InsertRange(0, Encoding.ASCII.GetBytes(messageHeader));
            // creatubg the conformation packet
            buffer.InsertRange(0, Encoding.ASCII.GetBytes(packetVerification));
            packet = buffer.ToArray();
            reset();
            return packet;
        }
       
         void write(Byte[] data)
        {
            buffer.AddRange(data);
        }
        void write(string s)
        {
            buffer.AddRange(Encoding.ASCII.GetBytes(s));
        }
        void writeUserID(int num)
        {
            if (num.ToString().Length <= 2)
            {
                buffer.AddRange(Encoding.ASCII.GetBytes(num.ToString().PadRight(userIDLength)));
            } else
            {
                Console.WriteLine("user Id out of scope");
                buffer.AddRange(Encoding.ASCII.GetBytes(0.ToString().PadRight(userIDLength)));
            }
        }
        void writeCMD(int _cmd)
        {
            if (_cmd <= 3)
            {
               buffer.AddRange(Encoding.ASCII.GetBytes(_cmd.ToString()));
            } else
            {
               buffer.AddRange(Encoding.ASCII.GetBytes(0.ToString()));
            }
        }
        void reset()
        {
            buffer.Clear();
        }
    }
    public class packetReader
    {
        int headerSize = 10;
        int userIDLength = 2;
        int Mesagelength;
        int packetVarificationLength = 4;
        int cmdByteNum = 1;
        string packetVerification = "NL36";
        public packetReader()
        {

        }
        public void readPacet(Byte[] pac)
        {
            if (readVerification(pac)==true)
            {
                Mesagelength = readHeader(pac);
                Console.WriteLine("Message Length:" + Mesagelength);
                Console.WriteLine("CMD inputed:" + readCMD(pac));
                Console.WriteLine("UserID:" + ReadUserID(pac));
                Console.WriteLine("Message:" + ReadMessage(pac, Mesagelength));
                Console.WriteLine("the full packet:" + Encoding.ASCII.GetString(pac));
            } else
            {
                Console.WriteLine("packet not corect verification code");
            }
        } 
        public bool readVerification(Byte[] pac)
        {
            Byte[] verByteAray = new Byte[packetVarificationLength];
            String verCode;
            for (int i = 0; i < packetVarificationLength; i++)
            {
                verByteAray[i] = pac[i];
            }
            verCode = Encoding.ASCII.GetString(verByteAray);
            if (verCode==packetVerification)
            {
                return true;
            } else
            {
                return false;
            }
            
        }
        public int readHeader(Byte[] pac)
        {
            Byte[] headerByteArray = new Byte[headerSize];
            String headerString;
            int messageLegth=0;
            for (int i = 0; i < headerSize; i++)
            {
                headerByteArray[i] = pac[packetVarificationLength+i];
            }
            headerString = Encoding.ASCII.GetString(headerByteArray);
            headerString.Trim();
            try
            {
                messageLegth = Int32.Parse(headerString);   
            } catch
            {
                Console.WriteLine("the header is coropted");
            }
            return messageLegth;
        }
        public int readCMD(Byte[] pac)
        {
            byte[] cmdByte = new Byte[cmdByteNum];
            string cmdString;
            int cmd;
            for (int i = 0; i < cmdByteNum; i++)
            {
             cmdByte[i] = pac[packetVarificationLength + headerSize + cmdByteNum + i];
            }
            cmdString = Encoding.ASCII.GetString(cmdByte);
            try
            {
                cmd = Int32.Parse(cmdString);
            } catch
            {
                Console.WriteLine("invaled cmd");
                return 0;
            }
            return cmd;
        }
        private int ReadUserID(Byte[] pac)
        {
            Byte[] userIDBytes = new Byte[userIDLength];
            String userIDString = "";
            int userID=0;
            for (int i = 0; i < userIDLength; i++)
            {
                userIDBytes[i] = pac[headerSize + packetVarificationLength + cmdByteNum + i];
            }
            userIDString = Encoding.ASCII.GetString(userIDBytes);
            userIDString.Trim();
            try
            {
                userID = Int32.Parse(userIDString);
            }
            catch
            {
                Console.WriteLine("the userID is coropted");
            }
            return userID;
        }
        private string ReadMessage(Byte[] pac,int messageLength)
        {
            Byte[] messageBytes = new Byte[messageLength];
            String Message;
            for (int i = 0; i < messageLength; i++)
            {
                messageBytes[i] = pac[packetVarificationLength + headerSize + cmdByteNum + userIDLength + i];
            }
            Message = Encoding.ASCII.GetString(messageBytes);
            return Message;
        }
    }
}
