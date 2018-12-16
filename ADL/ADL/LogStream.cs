using System.IO;
using System;
namespace ADL
{
    /// <summary>
    /// Log Stream class, use this when you want to add a new stream to the debug system.
    /// </summary>
    public sealed class LogStream
    {
        /// <summary>
        /// The Text Writer ADL uses to write logs
        /// </summary>
        private TextWriter _stream;
        /// <summary>
        /// The Mask
        /// </summary>
        private int _mask = -1;
        /// <summary>
        /// The Mask that determines wether this stream will receive a log message
        /// </summary>
        public int Mask {
            get
            {
                return _mask;
            }
            set
            {
                _mask = value;
            }
        }

        /// <summary>
        /// The Flag that adl uses
        /// </summary>
        private bool _matchAllFlags = true;
        /// <summary>
        /// The Matchtype that is determining what Checking algorithms to use
        /// </summary>
        public MatchType MatchType
        {
            get
            {
                return _matchAllFlags ? MatchType.MATCH_ALL : MatchType.MATCH_ONE;
            }
            set
            {
                _matchAllFlags = value == MatchType.MATCH_ALL;
            }
        }
        /// <summary>
        /// Flag for timestamps that adl uses
        /// </summary>
        private bool _setTimeStamp = false;
        /// <summary>
        /// Set this to true to Prepend a Timestamp in front of each logging message.
        /// </summary>
        public bool PrependTimeStamp
        {
            get
            {
                return _setTimeStamp;
            }
            set
            {
                _setTimeStamp = value;
            }
        }
        /// <summary>
        /// Flag that is preventing ADL to crash when the stream is closed.
        /// </summary>
        private bool _streamClosed = false;
        /// <summary>
        /// If this is true the Underlying Stream is closed and the Whole Object was destroyed.
        /// </summary>
        public bool IsStreamClosed
        {
            get
            {
                return _streamClosed;
            }
        }

        /// <summary>
        /// Underlying stream
        /// </summary>
        private readonly Stream _str = null;

        /// <summary>
        /// The Unterlying stream.
        /// </summary>
        public Stream TextStream { get { return _str; } }

        /// <summary>
        /// Base Constructor. For file streams or similar
        /// </summary>
        /// <param name="stream">Stream you want to create the Log on</param>
        public LogStream(Stream stream)
        {
            _str = stream;
            _stream = new StreamWriter(stream);

        }

        /// <summary>
        /// Constructor with just the Text Writer. leaving out assigning the actual stream.
        /// Used in the unity component(UnityTextWriter has no base stream).
        /// </summary>
        /// <param name="textReader">Text reader you want to create the Log on</param>
        public LogStream(TextWriter textReader)
        {
            _stream = textReader;
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

        [Obsolete("Old Function. Please use the properties", false)]
        /// <summary>
        /// Sets the Matching Mode between MatchAll and MatchOne
        /// </summary>
        /// <param name="matchType">Match Type</param>
        public void SetMatchingMode(MatchType matchType)
        {
            _matchAllFlags = matchType == MatchType.MATCH_ALL;
        }

        [Obsolete("Old Function. Please use the properties", false)]
        /// <summary>
        /// Sets the level mask of the log stream. Set to -1 to get every level
        /// </summary>
        /// <param name="newMask">New Mask</param>
        public void SetMask(int newMask)
        {
            _mask = newMask;
        }

        [Obsolete("Old Function. Please use the properties", false)]
        /// <summary>
        /// Displays the time when the message was logged.
        /// </summary>
        /// <param name="useTimeStamps">Use Timestamps</param>
        public void SetTimeStampUsage(bool useTimeStamps)
        {
            _setTimeStamp = useTimeStamps;
        }

        #endregion

        /// <summary>
        /// Wrapper to Leave out mask ID in the parameters
        /// </summary>
        /// <param name="flag">Flag you want to check against</param>
        /// <returns>If the flag is contained in the Mask</returns>
        public bool IsContainedInMask(int flag)
        {
            return BitMask.IsContainedInMask(_mask, flag, _matchAllFlags);
        }

        /// <summary>
        /// Logs a Message on this LogStream(bypassing tags and masks)
        /// </summary>
        /// <param name="mask">Mask to send trough the Stream(for the unity Text Reader)</param>
        /// <param name="message">Message you want to send.</param>
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
        /// <param name="setTimestamp">Put fancy timestamp infront of each line</param>
        /// <param name="appendIfExists">If the log you are trying to open is already existing append to it</param>
        /// <returns>The Created LogStream</returns>
        public static LogStream CreateLogStreamFromFile(string path, int mask = -1, MatchType matchType = MatchType.MATCH_ALL, bool setTimestamp = false, bool appendIfExists = false)
        {
            Stream s = new FileStream(path, appendIfExists? FileMode.Append: FileMode.Create);
            LogStream ret = new LogStream(s)
            {
                Mask = mask,
                MatchType = matchType,
                PrependTimeStamp = setTimestamp
            };
            return ret;
        }

        /// <summary>
        /// Wrapper function that makes the creation and setting up a log stream a no brainer
        /// </summary>
        /// <param name="stream">stream</param>
        /// <param name="mask">debug level mask</param>
        /// <param name="matchType">should the log only fire if all the flags are in the mask</param>
        /// <param name="setTimestamp">Put fancy timestamp infront of each line</param>
        /// <returns>The Created LogStream</returns>
        public static LogStream CreateLogStreamFromStream(System.IO.Stream stream, int mask = -1, MatchType matchType = MatchType.MATCH_ALL, bool setTimestamp = false)
        {
            LogStream ret = new LogStream(stream)
            {
                Mask = mask,
                MatchType = matchType,
                PrependTimeStamp = setTimestamp
            };
            return ret;
        }


        #endregion



    }

}
