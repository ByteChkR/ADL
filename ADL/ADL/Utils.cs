using System;
using System.Collections.Generic;
using System.Linq;

namespace ADL
{
    /// <summary>
    /// Helpful functions are stored here.
    /// </summary>
    public static class Utils
    {
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
