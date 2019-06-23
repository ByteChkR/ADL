using System.Collections.Generic;
using System.IO;

namespace ADL.Streams
{
    /// <summary>
    ///     Object to wrap all received logs into one object.
    /// </summary>
    public struct LogPackage
    {
        /// <summary>
        ///     Logs that were deserialized
        /// </summary>
        public List<Log> Logs;

        /// <summary>
        ///     Constructor that takes the output of the stream.
        /// </summary>
        /// <param name="buffer"></param>
        public LogPackage(byte[] buffer)
        {
            var logs = new List<Log>();
            int bytesRead;
            var totalBytes = 0;
            do
            {
                var l = Log.Deserialize(buffer, totalBytes, out bytesRead);
                if (bytesRead == -1) break; //Break manually when the logs end before the end of the buffer was reached.
                if (bytesRead != 0) logs.Add(l);

                totalBytes += bytesRead;
            } while (bytesRead != 0);

            Logs = logs;
        }


        /// <summary>
        ///     Turns this log package to a serialized byte buffer.
        /// </summary>
        /// <param name="setTimestamp"></param>
        /// <returns></returns>
        public byte[] GetSerialized(bool setTimestamp)
        {
            var ret = new List<byte>();
            foreach (var t in Logs)
            {
                var l = t;
                if (setTimestamp) l.Message = Utils.TimeStamp + l.Message;
                ret.AddRange(l.Serialize());
            }

            return ret.ToArray();
        }

        /// <summary>
        ///     Reads a block of Binary Data and turns it into a LogPackage
        /// </summary>
        /// <param name="s"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static LogPackage ReadBlock(Stream s, int length)
        {
            //Due to multithreading
            var buffer = new byte[length];
            s.Read(buffer, 0, length);
            return new LogPackage(buffer);
        }
    }
}