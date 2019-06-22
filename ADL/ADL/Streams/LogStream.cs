using System;
using System.Runtime.Remoting;
using System.Threading;
using System.IO;

namespace ADL.Streams
{
    /// <summary>
    /// Log stream class that you can use with virtually any stream(as long as you handle the multithreading on your own if used)
    /// This class wraps around your supplied stream and interacts with the system. 
    /// Because the LogStream implements every override and passes it to the base stream,
    /// you can use your created base stream itself instead of the LogStream.
    /// </summary>
    public class LogStream : Stream
    {
        /// <summary>
        /// Mask that the system uses to filter logs
        /// </summary>
        private BitMask _mask = new BitMask(true);

        /// <summary>
        /// The match type(how the mask gets compared)
        /// </summary>
        private MatchType _matchType = MatchType.MATCH_ALL;

        /// <summary>
        /// Put a timestamp infront of the log.
        /// </summary>
        private bool _setTimeStamp = false;

        /// <summary>
        /// Is the stream closed?
        /// </summary>
        private bool _streamClosed = false;

        /// <summary>
        /// If true all the LogChannels/TimeStamp is ignored and only the message will get received.
        /// </summary>
        private bool _overrideLogChannels = false;

        /// <summary>
        /// Base stream
        /// </summary>
        protected Stream _baseStream = null;

        #region Properties

        /// <summary>
        /// If true all the LogChannels/TimeStamp is ignored and only the message will get received.
        /// </summary>
        public bool OverrideChannelTag
        {
            get => _overrideLogChannels;
            set => _overrideLogChannels = value;
        }


        /// <summary>
        /// The Mask that determines wether this stream will receive a log message
        /// </summary>
        public int Mask
        {
            get => _mask;
            set => _mask = value;
        }

        /// <summary>
        /// The match type(how the mask gets compared)
        /// </summary>
        public MatchType MatchType
        {
            get => _matchType;
            set => _matchType = value;
        }

        /// <summary>
        /// Put a timestamp infront of the log.
        /// </summary>
        public bool AddTimeStamp
        {
            get => _setTimeStamp;
            set => _setTimeStamp = value;
        }

        /// <summary>
        /// If this is true the Underlying Stream is closed and the Whole Object was destroyed.
        /// </summary>
        public bool IsStreamClosed => _streamClosed;

        public Stream BaseStream => _baseStream;

        #endregion

        #region Overrides

        #region Methods

        public override void Write(byte[] value, int start, int count)
        {
            var tmp = new byte[count];
            Array.Copy(value, 0, tmp, 0, count);
            _baseStream.Write(tmp, 0, count);
            Flush();
        }

        public override void Flush()
        {
            _baseStream.Flush();
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback,
            object state)
        {
            return _baseStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback,
            object state)
        {
            return _baseStream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Close()
        {
            _streamClosed = true;
            _baseStream.Close();
        }

        public override ObjRef CreateObjRef(Type requestedType)
        {
            return _baseStream.CreateObjRef(requestedType);
        }

        [Obsolete]
        protected override WaitHandle CreateWaitHandle()
        {
            throw new NotSupportedException();
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return _baseStream.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            _baseStream.EndWrite(asyncResult);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
            //return _baseStream.Equals(obj);
        }

        public override int GetHashCode()
        {
            return _baseStream.GetHashCode();
        }

        public override object InitializeLifetimeService()
        {
            return _baseStream.InitializeLifetimeService();
        }


        public override int ReadByte()
        {
            return _baseStream.ReadByte();
        }


        public override string ToString()
        {
            return _baseStream.ToString();
        }

        public override void WriteByte(byte value)
        {
            _baseStream.WriteByte(value);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _baseStream.Read(buffer, offset, count);
        }

        #endregion

        #region Properties

        public override bool CanRead => _baseStream.CanRead;

        public override bool CanSeek => _baseStream.CanSeek;
        public override bool CanWrite => _baseStream.CanWrite;

        public override long Length => _baseStream.Length;

        public override long Position
        {
            get => _baseStream.Position;
            set => _baseStream.Position = value;
        }

        public override bool CanTimeout => _baseStream.CanTimeout;

        public override int ReadTimeout
        {
            get => _baseStream.ReadTimeout;
            set => _baseStream.ReadTimeout = value;
        }


        public override int WriteTimeout
        {
            get => _baseStream.WriteTimeout;
            set => _baseStream.WriteTimeout = value;
        }

        #endregion

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _baseStream.SetLength(value);
        }

        #endregion


        /// <summary>
        /// Creates a Log stream based on the parameters supplied.
        /// </summary>
        /// <param name="baseStream"></param>
        /// <param name="mask"></param>
        /// <param name="matchType"></param>
        /// <param name="setTimeStamp"></param>
        public LogStream(Stream baseStream, int mask = ~0, MatchType matchType = MatchType.MATCH_ALL,
            bool setTimeStamp = false)
        {
            _mask = mask;
            _matchType = matchType;
            _setTimeStamp = setTimeStamp;
            _baseStream = baseStream;
        }

        /// <summary>
        /// Writes a log to the stream.
        /// </summary>
        /// <param name="log">the log to send</param>
        public virtual void Write(Log log)
        {
            if (_streamClosed) return;
            if (_setTimeStamp) log.Message = Utils.TimeStamp + log.Message;
            var buffer = log.Serialize();
            _baseStream.Write(buffer, 0, buffer.Length);
            Flush();
        }

        /// <summary>
        /// Wrapper to make code more readable
        /// </summary>
        /// <param name="mask"></param>
        /// <returns></returns>
        public bool IsContainedInMask(BitMask mask)
        {
            return BitMask.IsContainedInMask(_mask, mask, _matchType == MatchType.MATCH_ALL);
        }
    }
}