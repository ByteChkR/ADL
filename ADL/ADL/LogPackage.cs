using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADL
{
    public struct LogPackage
    {
        public List<Log> Logs;
        public LogPackage(byte[] buffer)
        {
            List<Log> logs = new List<Log>();
            int bytesRead = 0;
            int totalBytes = 0;
            Log l;
            do
            {
                l = Log.Deserialize(buffer, totalBytes, out bytesRead);
                if (bytesRead != 0) logs.Add(l);
                totalBytes += bytesRead;
            } while (bytesRead != 0);
            Logs = logs;
        }
    }


    public struct Log
    {
        public BitMask Mask;
        public string Message;


        public Log(BitMask mask, string message)
        {
            Mask = mask;
            Message = message;
        }

        public byte[] Serialize()
        {
            
            List<byte> ret = BitConverter.GetBytes(Mask).ToList(); //Mask
            ret.AddRange(BitConverter.GetBytes(Message.Length));//Message Length
            ret.AddRange(Encoding.ASCII.GetBytes(Message)); //Message
            return ret.ToArray();
        }

        public static Log Deserialize(byte[] buffer, int startIndex, out int bytesRead)
        {
            bytesRead = 0;
            if (buffer.Length < startIndex + sizeof(int) * 2 + 1) return new Log();

            int mask = BitConverter.ToInt32(buffer, startIndex);
            int msgLength = BitConverter.ToInt32(buffer, startIndex + sizeof(int));
            string message = Encoding.ASCII.GetString(buffer, startIndex + sizeof(int) * 2, msgLength);
            bytesRead = sizeof(int) * 2 + msgLength;

            return new Log(mask, message);
        }
    }
}
