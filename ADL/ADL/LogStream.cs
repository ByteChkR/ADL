using System.IO;
using System;
namespace ADL
{
    /// <summary>
    /// Log Stream class, use this when you want to add a new stream to the debug system.
    /// </summary>
    public sealed class LogStream
    {

        #region Private Variables

        /// <summary>
        /// The Text Writer ADL uses to write logs
        /// </summary>
        private StreamWriter _stream;
        /// <summary>
        /// The Mask
        /// </summary>
        private BitMask _mask = -1;
        /// <summary>
        /// The Flag that adl uses
        /// </summary>
        private bool _matchAllFlags = true;
        /// <summary>
        /// Flag for timestamps that adl uses
        /// </summary>
        private bool _setTimeStamp = false;
        /// <summary>
        /// Flag that is preventing ADL to crash when the stream is closed.
        /// </summary>
        private bool _streamClosed = false;
        /// <summary>
        /// Underlying stream
        /// </summary>
        private readonly Stream _str = null;



        #endregion

        #region Public Properties

        /// <summary>
        /// The Mask that determines wether this stream will receive a log message
        /// </summary>
        public int Mask
        {
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
        /// The Unterlying stream.
        /// </summary>
        public Stream Stream { get { return _str; } }


        #endregion


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
        public bool IsContainedInMask(BitMask flag)
        {
            return BitMask.IsContainedInMask(_mask, flag, _matchAllFlags);
        }

        /// <summary>
        /// Logs a Message on this LogStream(bypassing tags and masks)
        /// </summary>
        /// <param name="mask">Mask to send trough the Stream(for the unity Text Reader)</param>
        /// <param name="message">Message you want to send.</param>
        public void Log(BitMask mask, string message)
        {
            if (_streamClosed) return;
            if (_setTimeStamp) message = Utils.TimeStamp + message;

            if (_str != null)
            {
                byte[] b = new Log(mask, message).Serialize();
                _str.Write(b, 0, b.Length);
            }

        }

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
            Stream s = new LogFileStream(path, appendIfExists ? FileMode.Append : FileMode.Create);
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
        public static LogStream CreateLogStreamFromStream(Stream stream, int mask = -1, MatchType matchType = MatchType.MATCH_ALL, bool setTimestamp = false)
        {
            LogStream ret = new LogStream(stream)
            {
                Mask = mask,
                MatchType = matchType,
                PrependTimeStamp = setTimestamp
            };
            return ret;
        }

        public static LogStream CreateLogStreamFromConsoleStream(int mask = -1, MatchType matchType = MatchType.MATCH_ALL, bool setTimeStamp = false)
        {
            LogStream ret = new LogStream(new LogTextSteam(Console.OpenStandardOutput()))
            {
                Mask = mask,
                MatchType = matchType,
                PrependTimeStamp = setTimeStamp
            };
            return ret;
        }


        #endregion
    }

}
