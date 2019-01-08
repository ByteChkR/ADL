using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ADL.Streams
{
    public class LogTextStream : LogStream
    {

        public LogTextStream(Stream baseStream, int mask = ~0, MatchType matchType = MatchType.MATCH_ALL, bool setTimeStamp = false) : base(baseStream, mask, matchType, setTimeStamp) { }

        /// <summary>
        /// Fills Buffer
        /// </summary>
        /// <param name="value">Line</param>
        public override void Write(Log log)
        {
            byte[] tmp = Encoding.ASCII.GetBytes(log.Message);
            _baseStream.Write(tmp, 0, tmp.Length);
            Flush();
        }

    }
}
