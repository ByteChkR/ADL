using System;
using System.Collections.Generic;
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


    
