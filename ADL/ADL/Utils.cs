using System;
using System.Collections.Generic;
using System.Linq;

namespace ADL
{
    public static class Utils
    {
        /// <summary>
        /// Computes basis by the power of exp
        /// </summary>
        /// <param name="basis"></param>
        /// <param name="exp"></param>
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
        /// <param name="time">S</param>
        /// <returns></returns>
        public static string NumToTimeFormat(int time)
        {
            return (time < 10) ? "0" + time : time.ToString();
        }

        #endregion


        #region FlagOperations
        /// <summary>
        /// Returns true if the specified flag is also set in the mask
        /// </summary>
        /// <param name="mask">the mask</param>
        /// <param name="flag">the flag</param>
        /// <param name="MatchAll">if false, it will return true if ANY flag is set on both sides.</param>
        /// <returns></returns>
        public static bool IsContainedInMask(int mask, int flag, bool MatchAll)
        {
            if (mask == -1 || flag == -1) return true; //Wildcard. When -1 you will get any message
            if (mask == 0 || flag == 0) return false; //Anti-Wildcard
            if (MatchAll) //If true it compares the whole mask with the whole flag(if constructed from different flags)
            {
                return (mask & flag) == flag;
            }
            else //if Match all is false, extract every single flag and compare them with the mask one by one and return true if there is at least one flag.
            {
                List<int> a = GetUniqueFlagsSet(flag);
                foreach (int f in a)
                {
                    if ((mask & f) == f) return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Splits up parameter flag into Unique Flags(power of 2 numbers)
        /// </summary>
        /// <param name="flag">the flag you want to split</param>
        /// <returns></returns>
        public static List<int> GetUniqueFlagsSet(int flag)
        {
            if (IsUniqueFlag(flag)) return new List<int>() { flag };
            List<int> ret = new List<int>();
            for (int i = 0; i < sizeof(int) * 8; i++)
            {
                int f = 1 << i;
                if (IsContainedInMask(flag, f, true))
                {
                    ret.Add(f);
                }
            }
            return ret;
        }

        /// <summary>
        /// Checks if the specified flag is unique(e.g. a power of 2 number)
        /// </summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public static bool IsUniqueFlag(int flag)
        {
            return flag != 0 && (flag & (flag - 1)) == 0;
        }
        #endregion



        #region MaskHelperFunctions


        /// <summary>
        /// Combines the specified masks together
        /// </summary>
        /// <param name="masks">the array of masks. SHOULD BE POWER OF 2 NUMBERS</param>
        /// <returns></returns>
        public static int CombineMasks(MaskCombineType combineType = MaskCombineType.BIT_OR, params int[] masks)
        {
            if (masks.Length == 0) return -1;
            int mask = masks[0];
            for (int i = 1; i < masks.Length; i++)
            {
                mask = combineType == MaskCombineType.BIT_OR ? mask | masks[i] : mask & masks[i];
            }
            return mask;
        }

        /// <summary>
        /// Removes the specified flags from the mask
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static int RemoveFlags(int mask, int flags)
        {
            return mask & ~flags;
        }

        /// <summary>
        /// Generic Version. T is your Enum.
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="masks">EnumValues</param>
        /// <returns></returns>
        public static int CombineMasks<T>(params T[] masks) where T : struct
        {
            return CombineMasks(masks.Select(x => Convert.ToInt32(x)).ToArray());
        }

        #endregion
    }
}
