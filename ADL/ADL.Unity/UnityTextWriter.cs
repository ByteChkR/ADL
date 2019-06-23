using System;
using System.Collections.Generic;
using System.IO;
using ADL.Streams;

namespace ADL.Unity
{
    /// <summary>
    ///     Textwriter derived UnityTextWriter, Supporting Wanring/Error mask.
    /// </summary>
    public class UnityTextWriter : Stream
    {
        /// <summary>
        ///     Constructor that needs masks
        /// </summary>
        /// <param name="warnMask">everything satisfying this mask will be printed as a UnityWarning</param>
        /// <param name="errorMask">everything satisfying this mask will be printed as a UnityError</param>
        public UnityTextWriter(BitMask warnMask, BitMask errorMask)
        {
            _warnMask = warnMask;
            _errorMask = errorMask;
        }

        /// <summary>
        ///     Writes the Log to the UnityConsole.
        /// </summary>
        public override void Flush()
        {
            var lp = new LogPackage(_buffer.ToArray());
            foreach (var l in lp.Logs)
                if (l.Mask == _warnMask)
                    UnityEngine.Debug.LogWarning(l.Message);
                else if (l.Mask == _errorMask)
                    UnityEngine.Debug.LogError(l.Message);
                else
                    UnityEngine.Debug.Log(l.Message);

            _buffer.Clear();
        }


        /// <summary>
        ///     Fills Buffer
        /// </summary>
        /// <param name="value">Line</param>
        /// <param name="start">Start index</param>
        /// <param name="count">Bytes to write</param>
        public override void Write(byte[] value, int start, int count)
        {
            var tmp = new byte[count];
            Array.Copy(value, 0, tmp, 0, count);
            _buffer.AddRange(tmp);
            Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

        #region Overrides

        /// <summary>
        ///     When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the origin parameter. </param>
        /// <param name="origin">
        ///     A value of type System.IO.SeekOrigin indicating the reference point used to obtain the new
        ///     position.
        /// </param>
        /// <returns>
        ///     The new position within the current stream.
        /// </returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        ///     When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes. </param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead => false;

        public override bool CanSeek => false;
        public override bool CanWrite => true;

        public override long Length => _buffer.Count;

        public override long Position
        {
            get => 0;
            set => throw new NotImplementedException();
        }

        #endregion

        #region Private Variables

        /// <summary>
        ///     The Buffer of the TextWriter
        /// </summary>
        private readonly List<byte> _buffer = new List<byte>();

        /// <summary>
        ///     The Error Mask
        /// </summary>
        private readonly int _errorMask;

        /// <summary>
        ///     The Warn Mask
        /// </summary>
        private readonly int _warnMask;

        #endregion
    }
}