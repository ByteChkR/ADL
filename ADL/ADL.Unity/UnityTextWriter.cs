using System;
using System.IO;
using System.Collections.Generic;
using ADL.Streams;
namespace ADL.Unity
{
    /// <summary>
    /// Textwriter derived UnityTextWriter, Supporting Wanring/Error mask.
    /// </summary>
    public sealed class UnityTextWriter : Stream
    {

        #region Overrides

        ///<summary>
        ///When overridden in a derived class, sets the position within the current stream.
        ///</summary>
        ///<param name="offset">A byte offset relative to the origin parameter. </param>
        ///<param name="origin">A value of type System.IO.SeekOrigin indicating the reference point used to obtain the new position. </param>
        ///<returns>
        ///The new position within the current stream.
        ///</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        ///<summary>
        ///When overridden in a derived class, sets the length of the current stream.
        ///</summary>
        ///<param name="value">The desired length of the current stream in bytes. </param>
        public override void SetLength(long value)
        {
            throw new NotSupportedException();
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek { get { return false; } }
        public override bool CanWrite { get { return true; } }

        public override long Length { get { return _buffer.Count; } }

        public override long Position { get => 0; set => throw new NotImplementedException(); }



        #endregion

        #region Private Variables
        /// <summary>
        /// The Buffer of the TextWriter
        /// </summary>
        private List<byte> _buffer = new List<byte>();

        /// <summary>
        /// The Error Mask
        /// </summary>
        private readonly int _errorMask;
        /// <summary>
        /// The Warn Mask
        /// </summary>
        private readonly int _warnMask;

        #endregion

        

        /// <summary>
        /// Constructor that needs masks
        /// </summary>
        /// <param name="WarnMask">everything satisfying this mask will be printed as a UnityWarning</param>
        /// <param name="ErrorMask">everything satisfying this mask will be printed as a UnityError</param>
        public UnityTextWriter(BitMask WarnMask, BitMask ErrorMask)
        {
            _warnMask = WarnMask;
            _errorMask = ErrorMask;
        }

        /// <summary>
        /// Writes the Log to the UnityConsole.
        /// </summary>
        public override void Flush()
        {
            LogPackage lp = new LogPackage(_buffer.ToArray());
            foreach (Log l in lp.Logs)
            {
                if (l.Mask == _warnMask)
                {
                    UnityEngine.Debug.LogWarning(_buffer.ToString());
                }
                else if (l.Mask == _errorMask)
                {
                    UnityEngine.Debug.LogError(_buffer.ToString());
                }
                else
                {
                    UnityEngine.Debug.Log(_buffer.ToString());
                }
            }

            _buffer.Clear();
        }




        /// <summary>
        /// Fills Buffer
        /// </summary>
        /// <param name="value">Line</param>
        public override void Write(byte[] value, int start, int count)
        {
            byte[] tmp = new byte[count];
            Array.Copy(value, 0, tmp, 0, count);
            _buffer.AddRange(tmp);
            Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException();
        }

    }


}
