using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADL.Streams
{
    /// <summary>
    /// Wraps around all the logs that were sent over the stream.
    /// </summary>
    public struct LogPackage
    {
        /// <summary>
        /// all the logs read from the stream
        /// </summary>
        public List<Log> Logs;
        /// <summary>
        /// constructor doing all the deserializing for you.
        /// </summary>
        /// <param name="buffer"></param>
        public LogPackage(byte[] buffer)
        {
            List<Log> logs = new List<Log>();
            int bytesRead = 0;
            int totalBytes = 0;
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
    }
}


    
