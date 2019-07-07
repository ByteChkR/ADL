using System;
using System.IO;
using ADL.Configs;
using ADL.Streams;

namespace ADL.Network.Streams
{
    public class NetLogStream : LogStream
    {

        /// <summary>
        ///     Creates a NetworkLogStream that uses TCP Clients as input
        /// </summary>
        /// <param name="s"></param>
        /// <param name="mask"></param>
        /// <param name="mt"></param>
        /// <param name="hasTimestamp"></param>
        public NetLogStream(Stream s, int mask, MatchType mt, bool hasTimestamp) :
            base(s, mask, mt, hasTimestamp)
        {



        }




        public override void Write(Log log)
        {
            if (IsClosed) return;
            if (AddTimeStamp) log.Message = Utils.TimeStamp + log.Message;
            var buffer = log.Serialize();
            try
            {
                BaseStream.Write(buffer, 0, buffer.Length);
                Flush();
            }
            catch (Exception)
            {
                Close();
            }
}
    }
}