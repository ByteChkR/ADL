using System;
using System.IO;
using ADL.Streams;
using UnityEngine;

namespace ADL.Unity
{
    /// <summary>
    ///     Class required by the Unity Component to Create a logstream.
    /// </summary>
    [Serializable]
    public class LogStreamParams
    {
        public bool CreateCustomConsole = false;

        [Tooltip("The file where the log should be saved\nNo effect on Console")]
        public string FilePath = "log.log";

        [Tooltip("The Mask. The levels you want to ..Spectate..")] [EnumFlagsAttribute]
        public int Mask = new BitMask(true);

        [Tooltip("How should the System treat the Mask?")]
        public MatchType MatchType = MatchType.MatchAll;

        [Tooltip("Adds Timestamps([hh:mm:ss]) to the Logs")]
        public bool SetTimeStamp = false;


        /// <summary>
        ///     Creates a LogStream. If CreateCustomConsole = true then its not initialized with filname.
        ///     Instead its initialized with a PipeStream to support the Custrom Console Window.
        /// </summary>
        /// <returns></returns>
        public LogStream ToLogStream()
        {
            return CreateCustomConsole ? new LogStream(new PipeStream(), Mask, MatchType, SetTimeStamp) : new LogTextStream(new FileStream(FilePath, FileMode.OpenOrCreate), Mask, MatchType, SetTimeStamp);
        }

        /// <summary>
        ///     Creates a Log Stream from specified stream. Discards some of the information in this object.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public LogStream ToLogStream(Stream stream)
        {
            return new LogStream(stream, Mask, MatchType, SetTimeStamp);
        }
    }
}