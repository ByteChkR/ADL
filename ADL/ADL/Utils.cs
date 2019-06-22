using System;
using System.Linq;
using System.Text;

namespace ADL
{
    /// <summary>
    ///     Helpful functions are stored here.
    /// </summary>
    public static class Utils
    {
        public static readonly int BYTE_SIZE = 8;
        public static readonly char NEW_LINE = '\n';

        /// <summary>
        ///     Returns the Enum Size for the specified enum
        /// </summary>
        /// <param name="enumType">typeof(enum)</param>
        /// <returns>bitwise length of enum.</returns>
        public static int GetEnumSize(Type enumType)
        {
            var i = Enum.GetValues(enumType).Cast<int>().Max(); //Maximum Value (32 for LoggingTypes)
            return i + i - 1; //Actual Bitwise Maximal value. from 000000(0) to 111111(63)
        }


        /// <summary>
        ///     Computes basis by the power of exp
        /// </summary>
        /// <param name="basis">basis</param>
        /// <param name="exp">exponent</param>
        /// <returns></returns>
        public static int IntPow(int basis, int exp)
        {
            if (exp == 0) return 1;
            var ret = basis;
            for (var i = 1; i < exp; i++) ret *= basis;
            return ret;
        }

        /// <summary>
        /// Converts a Byte Array to a Hexadecimal Encoded String for representation
        /// </summary>
        /// <param name="Bytes"></param>
        /// <returns></returns>
        public static string ByteArrayToHexString(byte[] Bytes)
        {
            var Result = new StringBuilder(Bytes.Length * 2);
            var HexAlphabet = "0123456789ABCDEF";

            foreach (var B in Bytes)
            {
                Result.Append(HexAlphabet[B >> 4]);
                Result.Append(HexAlphabet[B & 0xF]);
            }

            return Result.ToString();
        }

        /// <summary>
        /// Converts a Hexadecimal Encoded String into a byte array.
        /// </summary>
        /// <param name="Hex"></param>
        /// <returns></returns>
        public static byte[] HexStringToByteArray(string Hex)
        {
            var Bytes = new byte[Hex.Length / 2];
            int[] HexValue =
            {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05,
                0x06, 0x07, 0x08, 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F
            };

            for (int x = 0, i = 0; i < Hex.Length; i += 2, x += 1)
                Bytes[x] = (byte) ((HexValue[char.ToUpper(Hex[i + 0]) - '0'] << 4) |
                                   HexValue[char.ToUpper(Hex[i + 1]) - '0']);

            return Bytes;
        }

        #region TimeStamp

        /// <summary>
        ///     Current Time Stamp based on DateTime.Now
        /// </summary>
        public static string TimeStamp => "[" + NumToTimeFormat(DateTime.Now.Hour) + ":" +
                                          NumToTimeFormat(DateTime.Now.Minute) + ":" +
                                          NumToTimeFormat(DateTime.Now.Second) + "]";

        /// <summary>
        ///     Makes 1-9 => 01-09
        /// </summary>
        /// <param name="time">Integer</param>
        /// <returns></returns>
        public static string NumToTimeFormat(int time)
        {
            return time < 10 ? "0" + time : time.ToString();
        }

        #endregion
    }
}