using System.Text;
using System.IO;

namespace ADL.Unity
{
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
        public UnityTextWriter(int WarnMask, int ErrorMask)
        {
            this.WarnMask = WarnMask;
            this.ErrorMask = ErrorMask;
        }
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
        /// <param name="value"></param>
        /// <param name="arg0"></param>
        public override void WriteLine(string value, object arg0)
        {
            FlushType = (int)arg0;
            Write(value+'\n');
        }

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

        public override void Write(char value)
        {
            buffer.Append(value);
            if (value == '\n')
            {
                Flush();
            }
        }

        public override void Write(char[] value, int index, int count)
        {
            Write(new string(value, index, count));
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }

   
}
