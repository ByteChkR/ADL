using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ADL.Streams
{
    /// <summary>
    /// Object to wrap all received logs into one object.
    /// </summary>
    public struct LogPackage
    {
        /// <summary>
        /// Logs that were deserialized
        /// </summary>
        public List<Log> Logs;

        /// <summary>
        /// Constructor that takes the output of the stream.
        /// </summary>
        /// <param name="buffer"></param>
        public LogPackage(byte[] buffer)
        {
            var logs = new List<Log>();
            var bytesRead = 0;
            var totalBytes = 0;
            Log l;
            do
            {
                l = Log.Deserialize(buffer, totalBytes, out bytesRead);
                if (bytesRead == -1) break; //Break manually when the logs end before the end of the buffer was reached.
                if (bytesRead != 0) logs.Add(l);

                totalBytes += bytesRead;
            } while (bytesRead != 0);

            Logs = logs;
        }

        public byte[] GetSerialized(bool setTimestamp)
        {
            var ret = new List<byte>();
            Log l;
            for (var i = 0; i < Logs.Count; i++)
            {
                l = Logs[i];
                if (setTimestamp) l.Message = Utils.TimeStamp + l.Message;
                ret.AddRange(l.Serialize());
            }

            return ret.ToArray();
        }

        public static bool ReadBlock(Stream s, out LogPackage package)
        {
            List<byte> pack = new List<byte>();
            byte[] buf = new byte[1024];
            int i = s.Read(buf, 0, buf.Length);
            bool hasData = i != 0;
            while(i != 0)
            {
                for (int j = 0; j < i; j++)
                {
                    pack.Add(buf[j]);
                }
                s.Read(buf, 0, buf.Length);
            }
            package = hasData ? new LogPackage(buf) : new LogPackage(new byte[0]);
            return hasData ? true : false;
        }

        

        public static LogPackage ReadBlock(Stream s, int length)
        {
            //Due to multithreading
            byte[] buffer = new byte[length];
            s.Read(buffer, 0, length);
            return new LogPackage(buffer);
        }
    }
}