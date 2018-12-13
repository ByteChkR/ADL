﻿using System;
using System.Collections.Generic;
using System.IO;

namespace ADL
{
    /// <summary>
    /// Log Stream class, use this when you want to add a new stream to the debug system.
    /// </summary>
    public sealed class LogStream
    {

        private TextWriter _stream;
        private int _mask = -1;
        private bool _matchAllFlags = true;
        private bool _setTimeStamp = false;
        private bool _streamClosed = false;

        /// <summary>
        /// Base Constructor. For file streams or similar
        /// </summary>
        /// <param name="stream"></param>
        public LogStream(Stream stream) : this(new StreamWriter(stream)) { }

        /// <summary>
        /// Base Constructor. For streams that have a TextWriter already(e.x.: Console.Out)
        /// </summary>
        /// <param name="stream"></param>
        public LogStream(TextWriter stream)
        {
            _stream = stream;
        }

        /// <summary>
        /// Flushes the stream and frees its resources
        /// </summary>
        public void CloseStream()
        {
            if (_streamClosed) return;
            _stream.Close();
            _stream.Dispose();
            _streamClosed = true;
        }

        #region Options

        /// <summary>
        /// Sets the Matching Mode between MatchAll and MatchOne
        /// </summary>
        /// <param name="MatchAll"></param>
        public void SetMatchingMode(MatchType matchType)
        {
            _matchAllFlags = matchType == MatchType.MATCH_ALL;
        }

        /// <summary>
        /// Sets the level mask of the log stream. Set to -1 to get every level
        /// </summary>
        /// <param name="newMask"></param>
        public void SetMask(int newMask)
        {
            _mask = newMask;
        }


        /// <summary>
        /// Displays the time when the message was logged.
        /// </summary>
        /// <param name="useTimeStamps"></param>
        public void SetTimeStampUsage(bool useTimeStamps)
        {
            _setTimeStamp = useTimeStamps;
        }

        #endregion

        /// <summary>
        /// Wrapper to Leave out mask ID in the parameters
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        public bool IsContainedInMask(int mask)
        {
            return Utils.IsContainedInMask(_mask, mask, _matchAllFlags);
        }

        /// <summary>
        /// Logs a Message(Note: Prefixes already applied.)
        /// </summary>
        /// <param name="message"></param>
        public void Log(int mask, string message)
        {
            if (_streamClosed) return;
            if (_setTimeStamp) message = Utils.TimeStamp + message;

            _stream.WriteLine(message, mask);
        }


        ///// <summary>
        ///// Writes te last bit of the Message buffer before flushing. After that it Flushes the Stream 
        ///// </summary>
        //public void FlushStream()
        //{
        //    _stream.Flush();
        //}



        #region LogStreamHelperFunctions

        /// <summary>
        /// Wrapper function that makes the creation and setting up a log stream a no brainer
        /// </summary>
        /// <param name="path">filepath</param>
        /// <param name="mask">debug level mask</param>
        /// <param name="matchType">should the log only fire if all the flags are in the mask</param>
        /// <param name="minimumWriteAmount">amount of characters requires to issue another Stream.Write call</param>
        /// <param name="setTimestamp">Put fancy timestamp infront of each line</param>
        /// <param name="appendIfExists">If the log you are trying to open is already existing append to it</param>
        /// <returns></returns>
        public static LogStream CreateLogStreamFromFile(string path, int mask = -1, MatchType matchType = MatchType.MATCH_ALL, bool setTimestamp = false, bool appendIfExists = false)
        {
            LogStream ret = new LogStream(new System.IO.StreamWriter(path, appendIfExists));
            ret.SetMask(mask);
            ret.SetMatchingMode(matchType);
            ret.SetTimeStampUsage(setTimestamp);
            return ret;
        }

        /// <summary>
        /// Wrapper function that makes the creation and setting up a log stream a no brainer
        /// </summary>
        /// <param name="stream">stream</param>
        /// <param name="mask">debug level mask</param>
        /// <param name="matchType">should the log only fire if all the flags are in the mask</param>
        /// <param name="minimumWriteAmount">amount of characters requires to issue another Stream.Write call</param>
        /// <param name="setTimestamp">Put fancy timestamp infront of each line</param>
        /// <returns></returns>
        public static LogStream CreateLogStreamFromStream(System.IO.Stream stream, int mask = -1, MatchType matchType = MatchType.MATCH_ALL, bool setTimestamp = false)
        {
            LogStream ret = new LogStream(stream);
            ret.SetMask(mask);
            ret.SetMatchingMode(matchType);
            ret.SetTimeStampUsage(setTimestamp);
            return ret;
        }

        /// <summary>
        /// Wrapper function that makes the creation and setting up a log stream a no brainer
        /// </summary>
        /// <param name="stream">textwriter</param>
        /// <param name="mask">debug level mask</param>
        /// <param name="matchType">should the log only fire if all the flags are in the mask</param>
        /// <param name="minimumWriteAmount">amount of characters requires to issue another Stream.Write call</param>
        /// <param name="setTimestamp">Put fancy timestamp infront of each line</param>
        /// <returns></returns>
        public static LogStream CreateLogStreamFromStream(System.IO.TextWriter stream, int mask = -1, MatchType matchType = MatchType.MATCH_ALL, bool setTimestamp = false)
        {
            LogStream ret = new LogStream(stream);
            ret.SetMask(mask);
            ret.SetMatchingMode(matchType);
            ret.SetTimeStampUsage(setTimestamp);
            return ret;
        }

        #endregion



    }

}
