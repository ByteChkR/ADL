using System;

namespace ADL
{
    /// <summary>
    /// Little Helper class to have less of a hassle with masks in int form
    /// </summary>
    public class BitMask
    {
        /// <summary>
        /// Auto Convert to Int
        /// </summary>
        /// <param name="mask"></param>
        public static implicit operator int(BitMask mask)
        {
            return mask._mask;
        }

        public static implicit operator BitMask(int mask)
        {
            return new BitMask(mask);
        }

        int _mask = -1;


        /// <summary>
        /// Creates an Empty mask
        /// </summary>
        /// <param name="wildcard">If true, its a wildcard mask(everything)</param>
        public BitMask(bool wildcard = false)
        {
            if (!wildcard) _mask = 0;
        }

        /// <summary>
        /// Creates a mask based on mask supplied
        /// </summary>
        /// <param name="mask"></param>
        public BitMask(int mask)
        {
            _mask = mask;
        }

        /// <summary>
        /// Creates a mask based on flags supplied
        /// </summary>
        /// <param name="flags"></param>
        public BitMask(params int[] flags): this(Utils.CombineMasks(flags)) {}


        /// <summary>
        /// Sets all flags discarding the flags from before
        /// </summary>
        /// <param name="newFlags"></param>
        public void SetAllFlags(int newFlags)
        {
            _mask = newFlags;
        }

        
        /// <summary>
        /// Sets a single(or multiple) flags
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="yes"></param>
        public void SetFlag(int flag, bool yes)
        {
            if (yes)
            {
                Utils.CombineMasks(_mask, flag);
            }
            else
            {
                Utils.RemoveFlags(_mask, flag);
            }
        }

        /// <summary>
        /// Returns true when this mask satisfies the flags
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="matchType"></param>
        /// <returns></returns>
        public bool HasFlag(int flags, MatchType matchType)
        {
            return Utils.IsContainedInMask(_mask, flags, matchType == MatchType.MATCH_ALL);
        }
    }





    /// <summary>
    /// Little Helper class to have less of a hassle with masks in int form
    /// </summary>
    public class BitMask<T> where T:struct
    {
        /// <summary>
        /// Auto Convert to Int
        /// </summary>
        /// <param name="mask"></param>
        public static implicit operator int(BitMask<T> mask)
        {
            return mask._mask;
        }

        public static implicit operator BitMask<T>(int mask)
        {
            return new BitMask<T>(mask);
        }

        int _mask = -1;


        /// <summary>
        /// Creates an Empty mask
        /// </summary>
        /// <param name="wildcard">If true, its a wildcard mask(everything)</param>
        public BitMask(bool wildcard = false)
        {
            if (!wildcard) _mask = 0;
        }

        /// <summary>
        /// Creates a mask based on mask supplied
        /// </summary>
        /// <param name="mask"></param>
        public BitMask(T mask) : this(Convert.ToInt32(mask)) { }

        /// <summary>
        /// Creates a mask based on mask supplied
        /// </summary>
        /// <param name="mask"></param>
        public BitMask(int mask)
        {
            _mask = mask;
        }




        /// <summary>
        /// Creates a mask based on flags supplied
        /// </summary>
        /// <param name="flags"></param>
        public BitMask(params T[] flags) : this(Utils.CombineMasks<T>(flags)) { }


        /// <summary>
        /// Sets all flags discarding the flags from before
        /// </summary>
        /// <param name="newFlags"></param>
        public void SetAllFlags(T newFlags)
        {
            _mask = Convert.ToInt32(newFlags);
        }


        /// <summary>
        /// Sets a single(or multiple) flags
        /// </summary>
        /// <param name="flag"></param>
        /// <param name="yes"></param>
        public void SetFlag(T flag, bool yes)
        {
            int f = Convert.ToInt32(flag);
            if (yes)
            {
                Utils.CombineMasks(_mask, f);
            }
            else
            {
                Utils.RemoveFlags(_mask, f);
            }
        }

        /// <summary>
        /// Returns true when this mask satisfies the flags
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="matchType"></param>
        /// <returns></returns>
        public bool HasFlag(T flags, MatchType matchType)
        {
            return Utils.IsContainedInMask(_mask, Convert.ToInt32(flags), matchType == MatchType.MATCH_ALL);
        }
    }

}
