using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace ADL
{
    public class LogFileStream :FileStream
    {

        public LogFileStream(string path, FileMode mode) : base(path, mode) { }

        public override void Write(byte[] array, int offset, int count)
        {
            offset = sizeof(int) * 2;
            count = array.Length - offset;
            base.Write(array, offset, count);
        }

    }
}
