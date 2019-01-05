using System.Text;
using System.IO;

namespace ADL.Unity
{
    /// <summary>
    /// Textwriter derived UnityTextWriter, Supporting Wanring/Error mask.
    /// </summary>
    public sealed class UnityTextWriter : TextWriter
    {

        #region Private Variables
        /// <summary>
        /// The Buffer of the TextWriter
        /// </summary>
        private StringBuilder _buffer = new StringBuilder();
        
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
        /// The Log Type
        /// </summary>
        public int FlushType = 0;

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
            if(FlushType == -1 || FlushType == 0)
            {
                UnityEngine.Debug.Log(_buffer.ToString());
            }
            else if(BitMask.IsContainedInMask(FlushType, _warnMask, false))
            {

                UnityEngine.Debug.LogWarning(_buffer.ToString());
            }
            else if(BitMask.IsContainedInMask(FlushType, _errorMask, false)){

                UnityEngine.Debug.LogError(_buffer.ToString());
            }
            else
            {

                UnityEngine.Debug.Log(_buffer.ToString());
            }

            FlushType = 0;
            _buffer.Length = 0;
        }

        
        /// <summary>
        /// Uses arg0 as Flush Type(actually pretty hacked)
        /// </summary>
        /// <param name="value">line</param>
        /// <param name="arg0">log mask</param>
        public override void WriteLine(string value, object arg0)
        {
            FlushType = (int)arg0;
            Write(value+ Utils.NEW_LINE);
        }

        /// <summary>
        /// Fills Buffer
        /// </summary>
        /// <param name="value">Line</param>
        public override void Write(string value)
        {
            _buffer.Append(value);
            if (value != null)
            {
                var len = value.Length;
                if (len > 0)
                {
                    var lastChar = value[len - 1];
                    if (lastChar == Utils.NEW_LINE)
                    {
                        Flush();
                    }
                }
            }
        }

        /// <summary>
        /// Write single char
        /// </summary>
        /// <param name="value">char</param>
        public override void Write(char value)
        {
            _buffer.Append(value);
            if (value ==  Utils.NEW_LINE )
            {
                Flush();
            }
        }

        /// <summary>
        /// Write an array of char
        /// </summary>
        /// <param name="value">array</param>
        /// <param name="index">start index to start reading from the array</param>
        /// <param name="count">how much from the array to read</param>
        public override void Write(char[] value, int index, int count)
        {
            Write(new string(value, index, count));
        }
        /// <summary>
        /// Specifies encoding of the textWriter
        /// </summary>
        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }

   
}
