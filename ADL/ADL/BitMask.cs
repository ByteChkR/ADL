using System;
using System.Collections.Generic;
using System.Linq;

namespace ADL
{
    /// <summary>
    ///     Little Helper class to have less of a hassle with masks in int form
    /// </summary>
    public class BitMask
    {
        public static int Empty = 0;
        public static int WildCard = ~0;

        protected int Mask;

        /// <summary>
        ///     Sets all flags discarding the flags from before
        /// </summary>
        /// <param name="newMask">new Mask</param>
        public void SetAllFlags(int newMask)
        {
            Mask = newMask;
        }


        /// <summary>
        ///     Sets a single(or multiple) flags
        /// </summary>
        /// <param name="flag">flag or entire mask</param>
        /// <param name="yes">value you want to assign</param>
        public void SetFlag(int flag, bool yes)
        {
            Mask = yes ? CombineMasks(MaskCombineType.BitOr, Mask, flag) : RemoveFlags(Mask, flag);
        }

        /// <summary>
        ///     Returns true when this mask satisfies the flags
        /// </summary>
        /// <param name="flags">Mask or flag</param>
        /// <param name="matchType">Matching type you want to test against</param>
        /// <returns></returns>
        public bool HasFlag(int flags, MatchType matchType)
        {
            return IsContainedInMask(Mask, flags, matchType == MatchType.MatchAll);
        }

        /// <summary>
        ///     Flips every flag in the mask.
        /// </summary>
        public void Flip()
        {
            Mask = FlipMask(Mask);
        }

        #region MaskOperations

        /// <summary>
        ///     Returns true if the specified flag is also set in the mask
        /// </summary>
        /// <param name="mask">the mask</param>
        /// <param name="flag">the flag</param>
        /// <param name="matchType">if false, it will return true if ANY flag is set on both sides.</param>
        /// <returns></returns>
        public static bool IsContainedInMask(int mask, int flag, bool matchType)
        {
            if (mask == 0 && flag == 0)
                return true; //Hidden Channel
            if (mask == 0 || flag == 0) return false; //Anti-Wildcard
            if (matchType) //If true it compares the whole mask with the whole flag(if constructed from different flags)
                return (mask & flag) == flag;

            var a = GetUniqueMasksSet(flag);
            foreach (var f in a)
                if ((mask & f) == f)
                    return true;

            return false;
        }

        /// <summary>
        ///     Splits up parameter mask into Unique Flags(power of 2 numbers)
        /// </summary>
        /// <param name="mask">the mask you want to split</param>
        /// <returns></returns>
        public static List<int> GetUniqueMasksSet(int mask)
        {
            if (IsUniqueMask(mask)) return new List<int> {mask};
            var ret = new List<int>();
            for (var i = 0; i < sizeof(int) * Utils.ByteSize; i++)
            {
                var f = 1 << i;
                if (IsContainedInMask(mask, f, true)) ret.Add(f);
            }

            return ret;
        }

        /// <summary>
        ///     Checks if the specified mask is unique(e.g. a power of 2 number)
        /// </summary>
        /// <param name="mask">mask to test</param>
        /// <returns></returns>
        public static bool IsUniqueMask(int mask)
        {
            return mask != 0 && (mask & (mask - 1)) == 0;
        }

        /// <summary>
        ///     Combines the specified masks together
        /// </summary>
        /// <param name="combineType">How the Masks should be compared with each other</param>
        /// <param name="masks">the array of masks. SHOULD BE POWER OF 2 NUMBERS</param>
        /// <returns></returns>
        public static int CombineMasks(MaskCombineType combineType = MaskCombineType.BitOr, params int[] masks)
        {
            if (masks.Length == 0) return 0;
            var mask = masks[0];
            for (var i = 1; i < masks.Length; i++)
                mask = combineType == MaskCombineType.BitOr ? mask | masks[i] : mask & masks[i];
            return mask;
        }

        /// <summary>
        ///     Removes the specified flags from the mask
        /// </summary>
        /// <param name="mask">mask to remove from</param>
        /// <param name="flags">flag to remove</param>
        /// <returns></returns>
        public static int RemoveFlags(int mask, int flags)
        {
            return mask & ~flags;
        }

        /// <summary>
        ///     Completely Inverts the mask.
        /// </summary>
        /// <param name="mask">Mask to invert</param>
        /// <returns>Inverted Mask</returns>
        public static int FlipMask(int mask)
        {
            return ~mask;
        }

        #endregion

        #region Operator Overrides

        /// <summary>
        ///     Auto Convert to Int
        /// </summary>
        /// <param name="mask">This object</param>
        public static implicit operator int(BitMask mask)
        {
            return mask.Mask;
        }

        /// <summary>
        ///     Auto Convert from Int
        /// </summary>
        /// <param name="mask">Int to Convert</param>
        public static implicit operator BitMask(int mask)
        {
            return new BitMask(mask);
        }

        #endregion

        #region Constructors

        /// <summary>
        ///     Creates an Empty mask
        /// </summary>
        /// <param name="wildcard">If true, its a wildcard mask(everything)</param>
        public BitMask(bool wildcard = false)
        {
            if (wildcard) Mask = ~0;
        }

        /// <summary>
        ///     Creates a mask based on mask supplied
        /// </summary>
        /// <param name="mask">Mask to create a bitmask obj around.</param>
        public BitMask(int mask)
        {
            Mask = mask;
        }

        /// <summary>
        ///     Creates a mask based on flags supplied
        /// </summary>
        /// <param name="flags">all the flags you want to be set.</param>
        public BitMask(params int[] flags) : this(CombineMasks(MaskCombineType.BitOr, flags))
        {
        }

        #endregion
    }


    /// <summary>
    ///     Little Helper class to have less of a hassle with masks in int form
    /// </summary>
    /// <typeparam name="T">Type of enum you want to use</typeparam>
    public class BitMask<T> : BitMask where T : struct
    {
        #region MaskOperations

        /// <summary>
        ///     Generic Version. T is your Enum.
        /// </summary>
        /// <typeparam name="T">Enum Type</typeparam>
        /// <param name="combineType">The way how the masks are combined</param>
        /// <param name="masks">the masks to combine</param>
        /// <returns></returns>
        public static int CombineMasks(MaskCombineType combineType = MaskCombineType.BitOr, params T[] masks)
        {
            return CombineMasks(combineType, masks.Select(x => Convert.ToInt32(x)).ToArray());
        }

        #endregion

        /// <summary>
        ///     Sets all flags discarding the flags from before
        /// </summary>
        /// <param name="newFlags">New Flags</param>
        public void SetAllFlags(T newFlags)
        {
            Mask = Convert.ToInt32(newFlags);
        }


        /// <summary>
        ///     Sets a single(or multiple) flags
        /// </summary>
        /// <param name="flag">Flag or Mask you want to be set</param>
        /// <param name="yes">Value you want to set</param>
        public void SetFlag(T flag, bool yes)
        {
            var f = Convert.ToInt32(flag);
            if (yes)
                CombineMasks(MaskCombineType.BitOr, Mask, f);
            else
                RemoveFlags(Mask, f);
        }

        /// <summary>
        ///     Returns true when this mask satisfies the flags
        /// </summary>
        /// <param name="flags">Flag or Mask</param>
        /// <param name="matchType">Matchtype you want to check against</param>
        /// <returns></returns>
        public bool HasFlag(T flags, MatchType matchType)
        {
            return IsContainedInMask(Mask, Convert.ToInt32(flags), matchType == MatchType.MatchAll);
        }


        #region Operator Overrides

        /// <summary>
        ///     Auto Convert to Int
        /// </summary>
        /// <param name="mask">This Object</param>
        public static implicit operator int(BitMask<T> mask)
        {
            return mask.Mask;
        }

        /// <summary>
        ///     Auto Convert from Int
        /// </summary>
        /// <param name="mask">Int to Convert</param>
        public static implicit operator BitMask<T>(int mask)
        {
            return new BitMask<T>(mask);
        }

        ///// <summary>
        ///// Auto Convert to T
        ///// </summary>
        ///// <param name="mask">This Object</param>
        //public static implicit operator T(BitMask<T> mask)
        //{
        //    List<int> masks = BitMask.GetUniqueMasksSet(mask);

        //    T ret = (T)Enum.Parse(typeof(T), Enum.GetName(typeof(T), masks[0]));
        //    for(int i = 1; i < masks.Count; i++)
        //    {
        //        T item = (T)Enum.Parse(typeof(T), Enum.GetName(typeof(T), masks[1]));
        //        ret = CombineMasks(MaskCombineType.BIT_AND, ret, item);
        //    }
        //    return ret;
        //}

        /// <summary>
        ///     Auto Convert from T
        /// </summary>
        /// <param name="mask">T to Convert</param>
        public static implicit operator BitMask<T>(T mask)
        {
            return new BitMask<T>(mask);
        }

        #endregion


        #region Constructors

        /// <summary>
        ///     Creates an Empty mask
        /// </summary>
        /// <param name="wildcard">If true, its a wildcard mask(everything)</param>
        public BitMask(bool wildcard = false)
        {
            if (wildcard) Mask = ~0;
        }

        /// <summary>
        ///     Creates a mask based on mask supplied
        /// </summary>
        /// <param name="mask">Enum Values you want to Cast</param>
        public BitMask(T mask) : this(Convert.ToInt32(mask))
        {
        }

        /// <summary>
        ///     Creates a mask based on mask supplied
        /// </summary>
        /// <param name="mask">Integer Mask</param>
        public BitMask(int mask)
        {
            Mask = mask;
        }

        /// <summary>
        ///     Creates a mask based on flags supplied
        /// </summary>
        /// <param name="flags">Flags you want to be set</param>
        public BitMask(params T[] flags) : this(CombineMasks(MaskCombineType.BitOr, flags))
        {
        }

        #endregion
    }
}