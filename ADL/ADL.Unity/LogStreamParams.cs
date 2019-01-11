using System;
using UnityEngine;
using ADL.Streams;
namespace ADL.Unity
{
    /// <summary>
    /// Class required by the Unity Component to Create a logstream.
    /// </summary>
    [Serializable]
    public sealed class LogStreamParams
    {
        [Tooltip("The file where the log should be saved\nNo effect on Console")]
        public string FilePath = "log.log";
        [Tooltip("How should the System treat the Mask?")]
        public MatchType MatchType = MatchType.MATCH_ALL;
        [Tooltip("Adds Timestamps([hh:mm:ss]) to the Logs")]
        public bool SetTimeStamp = false;
        [Tooltip("The Mask. The levels you want to ..Spectate..")]
        [EnumFlagsAttribute] public BitMask Mask = new BitMask(true);
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
                return new LogStream(new PipeStream(), Mask, MatchType, SetTimeStamp);
            }
            return new LogTextStream(new System.IO.FileStream(FilePath, System.IO.FileMode.OpenOrCreate), Mask, MatchType, SetTimeStamp);
        }

        /// <summary>
        /// Creates a Log Stream from specified stream. Discards some of the information in this object.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public LogStream ToLogStream(System.IO.Stream stream)
        {
            return new LogStream(stream, Mask, MatchType, SetTimeStamp);
        }

        
    }
}
