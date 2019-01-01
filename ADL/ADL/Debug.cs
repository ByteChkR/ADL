using System;
using System.Collections.Generic;
using System.Text;

namespace ADL
{
    /// <summary>
    /// Main Debug Class. No instanciation needed.
    /// </summary>
    public static class Debug
    {
        /// <summary>
        /// String Builder to assemble the log
        /// </summary>
        private static StringBuilder _stringBuilder = new StringBuilder();
        /// <summary>
        /// List of LogStreams that are active
        /// </summary>
        private static List<LogStream> _steams = new List<LogStream>();
        /// <summary>
        /// Dictionary of Prefixes for the corresponding Masks
        /// </summary>
        private static Dictionary<int, string> _prefixes = new Dictionary<int, string>();
        /// <summary>
        /// The number of Streams that ADL writes to
        /// </summary>
        public static int LogStreamCount { get { return _steams.Count; } }



        #region Streams

        /// <summary>
        /// Adds another stream to the debug logs.
        /// </summary>
        /// <param name="stream">The stream you want to add</param>
        public static void AddOutputStream(LogStream stream)
        {
            if (_steams.Contains(stream)) return;
            _steams.Add(stream);
        }

        /// <summary>
        /// Removes the specified Stream.
        /// </summary>
        /// <param name="stream">The stream you want to remove</param>
        /// /// <param name="CloseStream">If streams should be closed upon removal from the system</param>
        public static void RemoveOutputStream(LogStream stream, bool CloseStream = true)
        {
            if (!_steams.Contains(stream)) return;
            _steams.Remove(stream);
            if(CloseStream)stream.CloseStream();

        }

        /// <summary>
        /// Removes all output streams from the list. Everything gets written to file.
        /// </summary>
        /// <param name="CloseStream">If streams should be closed upon removal from the system</param>
        public static void RemoveAllOutputStreams(bool CloseStream = true)
        {
            Log(-1, "Debug Queue Emptied");
            if (CloseStream)
                foreach (LogStream ls in _steams)
                {
                    ls.CloseStream();
                }
            _steams.Clear();
        }

        #endregion

        #region Prefixes

        

        /// <summary>
        /// Adds a prefix for the specified level
        /// </summary>
        /// <param name="mask">flag combination</param>
        /// <param name="prefix">desired prefix</param>
        public static void AddPrefixForMask(BitMask mask, string prefix)
        {
            if (_prefixes.ContainsKey(mask))
                _prefixes[mask] = prefix;
            else
                _prefixes.Add(mask, prefix);
        }

        /// <summary>
        /// Removes Prefix from prefix lookup table
        /// </summary>
        /// <param name="mask"></param>
        public static void RemovePrefixForMask(BitMask mask)
        {
            if (!_prefixes.ContainsKey(mask)) return;
            _prefixes.Remove(mask);
        }

        /// <summary>
        /// Clears all Prefixes
        /// </summary>
        public static void RemoveAllPrefixes()
        {
            _prefixes.Clear();
        }

        /// <summary>
        /// Sets all Prefixes from the list from low to high.
        /// You can not specify the level, because it will fill the prefixes by power of 2. So prefixes[0] = level1 and prefixes[2] = level4 and so on
        /// </summary>
        /// <param name="prefixes">List of prefixes</param>
        public static void SetAllPrefixes(params string[] prefixes)
        {
            RemoveAllPrefixes();

            for (int i = 0; i < prefixes.Length; i++)
            {
                AddPrefixForMask(Utils.IntPow(2, i), prefixes[i]);
            }
        }

        /// <summary>
        /// Gets all Tags with corresponding masks.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<int, string> GetAllTags()
        {
            return _prefixes;
        }
        #endregion

        /// <summary>
        /// Fire Log Messsage with desired level(flag) and message
        /// </summary>
        /// <param name="mask">the flag</param>
        /// <param name="message">the message</param>
        public static void Log(BitMask mask, string message)
        {
            foreach (LogStream adls in _steams)
            {
                if (adls.IsContainedInMask(mask))
                {
                    adls.Log(mask, GetMaskPrefix(mask) + message);
                }
            }
        }

        /// <summary>
        /// Generic Version. T is your Enum
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="level">Enum Mask</param>
        /// <param name="message">Message</param>
        public static void Log<T>(T level, string message) where T : struct
        {
            Log((ADL.BitMask)Convert.ToInt32(level), message);
        }

        /// <summary>
        /// Gets the Mask of the Specified Prefix
        /// </summary>
        /// <param name="prefix">Prefix</param>
        /// <param name="mask">Mask returned by the function</param>
        /// <returns>True if mask is found in Dictionary</returns>
        public static bool GetPrefixMask(string prefix, out BitMask mask)
        {
            mask = 0;
            if (_prefixes.ContainsValue(prefix)){
                foreach (KeyValuePair<int, string> kvp in _prefixes)
                {
                    if(prefix == kvp.Value)
                    {
                        mask = kvp.Key;
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the concatenated string of all the Prefixes that are fallin in that mask.
        /// </summary>
        /// <param name="mask"></param>
        /// <returns>All Prefixes for specified mask</returns>
        public static string GetMaskPrefix(BitMask mask)
        {
            if (mask == -1) return "[GLOBAL]";
            _stringBuilder.Length = 0;
            if (_prefixes.ContainsKey(mask))
            {
                //We happen to have a custom prefix for the level
                _stringBuilder.Append(_prefixes[mask]);
                
            }
            else //We have no Prefix specified for this particular level
            {
                List<int> flags = BitMask.GetUniqueMasksSet(mask); //Lets try to split all the flags into unique ones
                for (int i = 0; i < flags.Count; i++) //And then we apply the prefixes.
                {
                    if (_prefixes.ContainsKey(flags[i]))
                    {
                        _stringBuilder.Insert(0, _prefixes[flags[i]]);
                    }
                    else //If still not in prefix lookup table, better have a prefix than having just plain text.
                    {
                        _stringBuilder.Insert(0, "[Log Mask:" + flags[i] + "]");
                    }
                }
            }
            return _stringBuilder.ToString();

        }




    }
}
