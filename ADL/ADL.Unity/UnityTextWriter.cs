using System.Text;
using System.IO;

namespace ADL.Unity
{
    /// <summary>
    /// Textwriter derived UnityTextWriter, Supporting Wanring/Error mask.
    /// </summary>
    public sealed class UnityTextWriter : TextWriter
    {
        /// <summary>
        /// The Buffer of the TextWriter
        /// </summary>
        private StringBuilder buffer = new StringBuilder();
        /// <summary>
        /// The Log Type
        /// </summary>
        public int FlushType = 0;
        /// <summary>
        /// The Masks
        /// </summary>
        private int ErrorMask, WarnMask;
        /// <summary>
        /// Constructor that needs masks
        /// </summary>
        /// <param name="WarnMask">everything satisfying this mask will be printed as a UnityWarning</param>
        /// <param name="ErrorMask">everything satisfying this mask will be printed as a UnityError</param>
        public UnityTextWriter(int WarnMask, int ErrorMask)
        {
            this.WarnMask = WarnMask;
            this.ErrorMask = ErrorMask;
        }

        /// <summary>
        /// Writes the Log to the UnityConsole.
        /// </summary>
        public override void Flush()
        {
            if(FlushType == -1 || FlushType == 0)
            {
                UnityEngine.Debug.Log(buffer.ToString());
            }
            else if(BitMask.IsContainedInMask(FlushType, WarnMask, false))
            {

                UnityEngine.Debug.LogWarning(buffer.ToString());
            }
            else if(BitMask.IsContainedInMask(FlushType, ErrorMask, false)){

                UnityEngine.Debug.LogError(buffer.ToString());
            }
            else
            {

                UnityEngine.Debug.Log(buffer.ToString());
            }

            FlushType = 0;
            buffer.Length = 0;
        }

        
        /// <summary>
        /// Uses arg0 as Flush Type(actually pretty hacked)
        /// </summary>
        /// <param name="value">line</param>
        /// <param name="arg0">log mask</param>
        public override void WriteLine(string value, object arg0)
        {
            FlushType = (int)arg0;
            Write(value+'\n');
        }

        /// <summary>
        /// Fills Buffer
        /// </summary>
        /// <param name="value">Line</param>
        public override void Write(string value)
        {
            buffer.Append(value);
            if (value != null)
            {
                var len = value.Length;
                if (len > 0)
                {
                    var lastChar = value[len - 1];
                    if (lastChar == '\n')
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
            buffer.Append(value);
            if (value == '\n')
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
