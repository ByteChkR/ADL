using System.IO;

namespace ADL.Streams
{
    public class LogTextStream : LogStream
    {
        /// <summary>
        ///     Constructor, passing the parameters to log stream
        /// </summary>
        /// <param name="baseStream"></param>
        /// <param name="mask"></param>
        /// <param name="matchType"></param>
        /// <param name="setTimeStamp"></param>
        public LogTextStream(Stream baseStream, int mask = ~0, MatchType matchType = MatchType.MatchAll,
            bool setTimeStamp = false) : base(baseStream, mask, matchType, setTimeStamp)
        {
        }

        /// <summary>
        ///     Fills Buffer with the string message only.(used when output is System.Console)
        /// </summary>
        /// <param name="log">Log</param>
        public override void Write(Log log)
        {
            if (AddTimeStamp) log.Message = Utils.TimeStamp + log.Message;
            var tmp = Debug.TextEncoding.GetBytes(log.Message);
            BaseStream.Write(tmp, 0, tmp.Length);
            Flush();
        }
    }
}