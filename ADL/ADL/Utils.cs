using System;
using System.Linq;
namespace ADL
{
    /// <summary>
    /// Helpful functions are stored here.
    /// </summary>
    public static class Utils
    {

        public static readonly int BYTE_SIZE = 8;
        public static readonly char NEW_LINE = '\n';
        public static readonly int MASK_ALL = ~0;
        /// <summary>
        /// Returns the Enum Size for the specified enum
        /// </summary>
        /// <param name="enumType">typeof(enum)</param>
        /// <returns>bitwise length of enum.</returns>
        public static int GetEnumSize(Type enumType)
        {
            int i = Enum.GetValues(enumType).Cast<int>().Max(); //Maximum Value (32 for LoggingTypes)
            return i + i - 1; //Actual Bitwise Maximal value. from 000000(0) to 111111(63)
        }


        /// <summary>
        /// Computes basis by the power of exp
        /// </summary>
        /// <param name="basis">basis</param>
        /// <param name="exp">exponent</param>
        /// <returns></returns>
        public static int IntPow(int basis, int exp)
        {
            if (exp == 0) return 1;
            int ret = basis;
            for (int i = 1; i < exp; i++)
            {
                ret *= basis;
            }
            return ret;
        }

        #region TimeStamp
        /// <summary>
        /// Current Time Stamp based on DateTime.Now
        /// </summary>
        public static string TimeStamp
        {
            get
            {
                return "[" + NumToTimeFormat(DateTime.Now.Hour) + ":" + NumToTimeFormat(DateTime.Now.Minute) + ":" + NumToTimeFormat(DateTime.Now.Second) + "]";
            }
        }

        /// <summary>
        /// Makes 1-9 => 01-09
        /// </summary>
        /// <param name="time">Integer</param>
        /// <returns></returns>
        public static string NumToTimeFormat(int time)
        {
            return (time < 10) ? "0" + time : time.ToString();
        }

        #endregion

    }
}
