using System;
using System.Collections.Generic;
using System.Text;

namespace server
{
    class Packet
    {
        //this list is used so the buffer dose not have a fixed size it is latter converted into a byte array for reading
        private List<Byte> Buffer;
        //hense the readable buffer
        private Byte[] ReadableBuffer;
        //the location for reading the buffer
        private int readPos;

        //the pacet for sending to users
        Packet(int _userID)
        {
            Buffer = new List<byte>();
            readPos = 0;
        }
        //the pacet for reseving
        Packet(Byte[] data)
        {
            Buffer = new List<byte>();
            readPos = 0;

            write(data);
        }
        #region basic functions
        public byte[] toArray()
        {
            ReadableBuffer = Buffer.ToArray();
            return ReadableBuffer;
        }

        #endregion
        #region reading that data 
        public byte readByte(bool _moveReadPos = true)
        {
            byte _value = ReadableBuffer[readPos];
            if(Buffer.Count > readPos)
            {
                if(_moveReadPos)
                {
                    readPos += 1;
                }
                return _value;
            } else
            {
                // error handling
                throw new Exception("could not read bytes ");
            }
            
        }
        #endregion
        #region writing the data
        //adding singal byte
        void write(byte _value)
        {
            Buffer.Add(_value);
        }
        //adding byte array
        void write(Byte[] _values)
        {
            Buffer.AddRange(_values);
        }
        //adding int
        void write(int _value)
        {
            Buffer.AddRange(BitConverter.GetBytes(_value));
        }
        // adding a string
        void write(string _value)
        {
            Buffer.AddRange(Encoding.ASCII.GetBytes(_value));
        }
        #endregion
    }
}
