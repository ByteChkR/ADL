using System;
using UnityEngine;

namespace ADL.Unity
{
    [Serializable]
    public sealed class LogStreamParams
    {
        [Tooltip("The file where the log should be saved\nNo effect on Console")]
        public string FilePath = "log.log";
        [Tooltip("How should the System treat the Mask?")]
        public MatchType MatchType = MatchType.MATCH_ALL;
        [Tooltip("Adds Timestamps([hh:mm:ss]) to the Logs")]
        public bool SetTimeStamp = false;
        [Tooltip("If used as a file, it will append instead of delete")]
        public bool AppendIfExists = false;
        [Tooltip("The Mask. The levels you want to ..Spectate..")]
        [EnumFlagsAttribute] public int Mask = -1;
        public bool CreateCustomConsole = false;

        /// <summary>
        /// Creates a LogStream. If CreateCustomConsole = true then its not initialized with filname.
        /// Instead its initialized with a PipeStream to support the Custrom Console Window.
        /// </summary>
        /// <returns></returns>
        public LogStream ToLogStream()
        {
            if (CreateCustomConsole)
            {
                return LogStream.CreateLogStreamFromStream(new PipeStream(), Mask, MatchType, SetTimeStamp);
            }
            return LogStream.CreateLogStreamFromFile(FilePath, Mask, MatchType, SetTimeStamp, AppendIfExists);
        }

        /// <summary>
        /// Creates a Log Stream from specified stream. Discards some of the information in this object.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public LogStream ToLogStream(System.IO.Stream stream)
        {
            return LogStream.CreateLogStreamFromStream(stream, Mask, MatchType, SetTimeStamp);
        }

        public LogStream ToUnityConsoleLogStream(UnityTextWriter utw)
        {
            LogStream ls = new LogStream(utw);
            ls.SetMask(Mask);
            ls.SetMatchingMode(MatchType);
            ls.SetTimeStampUsage(SetTimeStamp);
            return ls;
        }
    }
}
