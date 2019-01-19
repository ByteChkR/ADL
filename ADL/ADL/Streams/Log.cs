using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADL.Streams
{
    /// <summary>
    /// Struct that can serialize and deserialize to be sent over a stream
    /// </summary>
    public struct Log
    {
        /// <summary>
        /// The mask of the log.
        /// </summary>
        public int Mask;
        /// <summary>
        /// The message that has been sent
        /// </summary>
        public string Message;


        /// <summary>
        /// Creates a valid Log Struct
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="message"></param>
        public Log(int mask, string message)
        {
            Mask = mask;
            Message = message;
        }

        /// <summary>
        /// Turns this object in to a byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] Serialize()
        {
            List<byte> ret = BitConverter.GetBytes(Mask).ToList(); //Mask
            ret.AddRange(BitConverter.GetBytes(Message.Length));//Message Length
            ret.AddRange(Encoding.ASCII.GetBytes(Message)); //Message
            return ret.ToArray();
        }

        /// <summary>
        /// Creates an Log object from a byte stream.
        /// </summary>
        /// <param name="buffer">the buffer where the object is in</param>
        /// <param name="startIndex">e.g. offset</param>
        /// <param name="bytesRead">0 = no object found(end of stream), -1 = no object found.(nothing there)</param>
        /// <returns></returns>
        public static Log Deserialize(byte[] buffer, int startIndex, out int bytesRead)
        {
            bytesRead = 0;
            if (buffer.Length < startIndex + sizeof(int) * 2 + 1) return new Log();


            int mask = BitConverter.ToInt32(buffer, startIndex);
            int msgLength = BitConverter.ToInt32(buffer, startIndex + sizeof(int));
            if (msgLength == 0)
            {
                bytesRead = -1;
                return new Log();
            }
            string message = "";
            if (msgLength > buffer.Length - startIndex - sizeof(int) * 2)
            {
                return new Log();
            }

            message = Encoding.ASCII.GetString(buffer, startIndex + sizeof(int) * 2, msgLength);

            bytesRead = sizeof(int) * 2 + msgLength;

            return new Log(mask, message);
        }
    }
}

