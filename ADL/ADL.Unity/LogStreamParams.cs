using System;
using UnityEditor;
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
        bool mA { get { return MatchType == MatchType.MATCH_ALL; } }
        [Tooltip("Adds Timestamps([hh:mm:ss]) to the Logs")]
        public bool SetTimeStamp = false;
        [Tooltip("If used as a file, it will append instead of delete")]
        public bool AppendIfExists = false;
        [Tooltip("The Mask. The levels you want to ..Spectate..")]
        [EnumFlagsAttribute] public int Mask = -1;

        /// <summary>
        /// Creates a LogStream from file
        /// </summary>
        /// <returns></returns>
        public LogStream ToLogStream()
        {
            return LogStream.CreateLogStreamFromFile(FilePath, Mask, mA, SetTimeStamp, AppendIfExists);
        }

        /// <summary>
        /// Creates a Log Stream from specified stream. Discards some of the information in this object.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public LogStream ToLogStream(System.IO.TextWriter stream)
        {
            return LogStream.CreateLogStreamFromStream(stream, Mask, mA, SetTimeStamp);

        }
    }
}
