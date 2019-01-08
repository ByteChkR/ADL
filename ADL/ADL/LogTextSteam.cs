using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Remoting;
using System.Threading;

namespace ADL
{
    public class LogTextSteam : Stream
    {

        #region Overrides

        #region Methods

        /// <summary>
        /// Fills Buffer
        /// </summary>
        /// <param name="value">Line</param>
        public override void Write(byte[] value, int start, int count)
        {
            byte[] tmp = new byte[count];
            Array.Copy(value, 0, tmp, 0, count);
            _baseStream.Write(tmp, 0, count);
            Flush();
        }

        public override void Flush()
        {
            _baseStream.Flush();
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _baseStream.BeginRead(buffer, offset, count, callback, state);
        }

        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _baseStream.BeginWrite(buffer, offset, count, callback, state);
        }

        public override void Close()
        {
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
            return _baseStream.Equals(obj);
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

        public override bool CanRead
        {
            get { return _baseStream.CanRead; }
        }

        public override bool CanSeek { get { return _baseStream.CanSeek; } }
        public override bool CanWrite { get { return _baseStream.CanWrite; } }

        public override long Length { get { return _baseStream.Length; } }

        public override long Position { get => _baseStream.Position; set => _baseStream.Position = value; }

        public override bool CanTimeout => _baseStream.CanTimeout;

        public override int ReadTimeout { get => _baseStream.ReadTimeout; set => _baseStream.ReadTimeout = value; }


        public override int WriteTimeout { get => _baseStream.WriteTimeout; set => _baseStream.WriteTimeout = value; }



        #endregion

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
            return _baseStream.Seek(offset, origin);
        }

        ///<summary>
        ///When overridden in a derived class, sets the length of the current stream.
        ///</summary>
        ///<param name="value">The desired length of the current stream in bytes. </param>
        public override void SetLength(long value)
        {
            _baseStream.SetLength(value);
        }

      
        #endregion

        Stream _baseStream;


        public LogTextSteam(Stream baseStream)
        {
            _baseStream = baseStream;
        }

        
    }
}
