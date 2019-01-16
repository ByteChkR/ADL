using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Linq;
using ADL.Configs;
using ADL.Streams;
/// <summary>
/// Namespace ADL is the "Root" namespace of ADL. It contains the Code needed to use ADL. But also in sub namespaces you will find other helpful tools.
/// </summary>
namespace ADL
{
    /// <summary>
    /// Main Debug Class. No instanciation needed.
    /// </summary>
    public static class Debug
    {

        #region Private Variables

        /// <summary>
        /// On/Off switch
        /// </summary>
        private static bool _adlEnabled = true;

        /// <summary>
        /// Flag to check wether this is the first execution.
        /// </summary>
        private static bool _firstLog = true;

        /// <summary>
        /// Should ADL search for updates?
        /// </summary>
        private static bool _sendUpdateMsg = true;

        /// <summary>
        /// Should ADL send warning of potential wrong use?
        /// </summary>
        private static bool _sendWarnings = true;

        /// <summary>
        /// Dictionary of Prefixes for the corresponding Masks
        /// </summary>
        private static Dictionary<int, string> _prefixes = new Dictionary<int, string>();

        /// <summary>
        /// The mask that gets used when _sendUpdateMessage is true
        /// </summary>
        private static int _updateMask = new BitMask(true);

        /// <summary>
        /// The mask that gets used when ADL is sending warnings
        /// </summary>
        private static int _adlWarningMask = new BitMask(true);

        /// <summary>
        /// String Builder to assemble the log
        /// </summary>
        private static StringBuilder _stringBuilder = new StringBuilder();
        /// <summary>
        /// List of LogStreams that are active
        /// </summary>
        private static List<LogStream> _streams = new List<LogStream>();

        /// <summary>
        /// Contains the flags that determine the way prefixes get looked up
        /// </summary>
        private static PrefixLookupSettings _lookupMode = PrefixLookupSettings.ADDPREFIXIFAVAILABLE;
        /// <summary>
        /// The extracted flag if we should put tags at all
        /// </summary>
        private static bool _addPrefix;
        /// <summary>
        /// The extracted flag if we should deconstruct the mask to find potential tags
        /// </summary>
        private static bool _deconstructtofind;
        /// <summary>
        /// The extracted flag if we should end the lookup when one tag was found.(Does nothing if deconstruct flag is set to false)
        /// </summary>
        private static bool _onlyone;
        #endregion

        #region Public Properties
        /// <summary>
        /// Public property, used to disable ADl
        /// </summary>
        public static bool ADLEnabled { get { return _adlEnabled; } set { _adlEnabled = value; } }

        /// <summary>
        /// Public property, used to disable update check.(Saves ~500ms)
        /// </summary>
        public static bool SendUpdateMessageOnFirstLog { get { return _sendUpdateMsg; } set { _sendUpdateMsg = value; } }

        /// <summary>
        /// Determines if ADL should send warnings about potential wrong use to the log streams
        /// </summary>
        public static bool SendWarnings { get { return _sendWarnings; } set { _sendWarnings = value; } }

        /// <summary>
        /// Update Mask. This mask gets used when one of the ADL Components are checking for updates.
        /// To disable the update messages, simly change the mask to new Bitmask(false)
        /// </summary>
        public static BitMask UpdateMask { get { return _updateMask; } set { _updateMask = value; } }

        /// <summary>
        /// Warning Mask. This mask gets used when ADL sends warnings about (possible)wrong use.
        /// </summary>
        public static BitMask ADLWarningMask { get { return _adlWarningMask; } set { _adlWarningMask = value; } }

        /// <summary>
        /// The number of Streams that ADL writes to
        /// </summary>
        public static int LogStreamCount { get { return _adlEnabled ? _streams.Count : 0; } }

        public static PrefixLookupSettings PrefixLookupMode
        {
            get { return _lookupMode; }
            set
            {
                _addPrefix = BitMask.IsContainedInMask((int)value, (int)PrefixLookupSettings.ADDPREFIXIFAVAILABLE, false);
                _deconstructtofind = BitMask.IsContainedInMask((int)value, (int)PrefixLookupSettings.DECONSTRUCTMASKTOFIND, false);
                _onlyone = BitMask.IsContainedInMask((int)value, (int)PrefixLookupSettings.ONLYONEPREFIX, false);
                _lookupMode = value;
            }
        }
        #endregion

        #region Streams

        /// <summary>
        /// Adds another stream to the debug logs.
        /// </summary>
        /// <param name="stream">The stream you want to add</param>
        public static void AddOutputStream(LogStream stream)
        {
            if (!_adlEnabled)
            {
                Log(_adlWarningMask, "AddOutputStream(" + stream.Mask + "): ADL is disabled, you are adding an Output Stream while ADL is disabled.");

            }
            if (_streams.Contains(stream))
            {
                Log(_adlWarningMask, "AddOutputStream(" + stream.Mask + "): Supplied stream is already in the list. Aborting!");
                return;
            }
            _streams.Add(stream);
        }

        /// <summary>
        /// Removes the specified Stream.
        /// </summary>
        /// <param name="stream">The stream you want to remove</param>
        /// /// <param name="CloseStream">If streams should be closed upon removal from the system</param>
        public static void RemoveOutputStream(LogStream stream, bool CloseStream = true)
        {
            if (!_adlEnabled)
            {
                Log(_adlWarningMask, "RemoveOutputStream(" + stream.Mask + "): ADL is disabled, you are removing an Output Stream while while ADL is disabled.");
            }
            if (!_streams.Contains(stream))
            {
                Log(_adlWarningMask, "RemoveOutputStream(" + stream.Mask + "): Supplied stream is not in the list. Aborting!");
                return;
            }
            _streams.Remove(stream);
            if (CloseStream) stream.Close();

        }

        /// <summary>
        /// Removes all output streams from the list. Everything gets written to file.
        /// </summary>
        /// <param name="CloseStream">If streams should be closed upon removal from the system</param>
        public static void RemoveAllOutputStreams(bool CloseStream = true)
        {
            Log(-1, "Debug Queue Emptied");
            if (CloseStream)
                foreach (LogStream ls in _streams)
                {
                    ls.Close();
                }
            _streams.Clear();
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
            if (!_adlEnabled)
            {
                Log(_adlWarningMask, "AddPrefixForMask(" + mask + "): ADL is disabled, you are adding a prefix for a mask while ADL is disabled.");
            }
            if (!BitMask.IsUniqueMask(mask))
            {
                Log(_adlWarningMask, "AddPrefixForMask(" + mask + "): Adding Prefix: " + prefix + " for mask: " + mask + ". Mask is not unique.");
            }
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
            if (!_adlEnabled)
            {
                Log(_adlWarningMask, "RemovePrefixForMask(" + mask + "): ADL is disabled, you are removing a prefix for a mask while ADL is disabled.");
            }
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
            if (!_adlEnabled)
            {
                string info = "";
                prefixes.ToList().ForEach(x => info += x + ", ");
                Log(_adlWarningMask, "SetAllPrefixes(" + info + "): ADL is disabled, you are removing a prefix for a mask while ADL is disabled.");
            }
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
        public static Dictionary<int, string> GetAllPrefixes()
        {
            if (!_adlEnabled)
            {
                Log(_adlWarningMask, "GetAllPrefixes(): ADL is disabled, you are getting all prefixes while ADL is disabled.");
            }
            return _prefixes;
        }
        #endregion

        #region Logging

        /// <summary>
        /// Fire Log Messsage with desired level(flag) and message
        /// </summary>
        /// <param name="mask">the flag</param>
        /// <param name="message">the message</param>
        public static void Log(int mask, string message)
        {
            if (!_adlEnabled && (mask != ADLWarningMask || !_sendWarnings)) return;

            if (_firstLog)
            {
                _firstLog = false;

                if (_sendUpdateMsg)
                {
                    string msg = UpdateDataObject.CheckUpdate(Assembly.GetExecutingAssembly().GetName().Name, Assembly.GetExecutingAssembly().GetName().Version);

                    Log(UpdateMask, msg);

                }
            }

            string _msg = GetMaskPrefix(mask) + message + Utils.NEW_LINE;

            foreach (LogStream logs in _streams)
            {
                if (logs.IsContainedInMask(mask))
                {
                    logs.Write(new Log(mask, _msg));
                }
            }
        }

        /// <summary>
        /// Generic Version. T is your Enum
        /// </summary>
        /// <typeparam name="T">Enum</typeparam>
        /// <param name="mask">Enum Mask</param>
        /// <param name="message">Message</param>
        public static void LogGen<T>(T mask, string message) where T : struct
        {
            if (!_adlEnabled && (!_sendWarnings || (BitMask)Convert.ToInt32(mask) != ADLWarningMask))
            {
                return;
            }
            Log(Convert.ToInt32(mask), message);
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
            if (_prefixes.ContainsValue(prefix))
            {
                foreach (KeyValuePair<int, string> kvp in _prefixes)
                {
                    if (prefix == kvp.Value)
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
            if (!_addPrefix) return "";
            _stringBuilder.Length = 0;
            if (_prefixes.ContainsKey(mask))
            {
                //We happen to have a custom prefix for the level
                _stringBuilder.Append(_prefixes[mask]);

            }
            else if (_deconstructtofind) //We have no Prefix specified for this particular level
            {
                List<int> flags = BitMask.GetUniqueMasksSet(mask); //Lets try to split all the flags into unique ones
                for (int i = 0; i < flags.Count; i++) //And then we apply the prefixes.
                {
                    if (_prefixes.ContainsKey(flags[i]))
                    {
                        _stringBuilder.Insert(0, _prefixes[flags[i]]);
                        if (_onlyone) break;
                    }
                    else //If still not in prefix lookup table, better have a prefix than having just plain text.
                    {
                        _stringBuilder.Insert(0, "[Log Mask:" + flags[i] + "]");
                        if (_onlyone) break;
                    }
                }
            }
            return _stringBuilder.ToString();

        }
        #endregion

        #region Config
        /// <summary>
        /// Loads a supplied ADLConfig.
        /// </summary>
        /// <param name="config">Config to load</param>
        public static void LoadConfig(ADLConfig config)
        {
            ADLEnabled = config.ADLEnabled;
            SendUpdateMessageOnFirstLog = config.SendUpdateMessageOnFirstLog;
            UpdateMask = config.UpdateMask;
            ADLWarningMask = config.WarningMask;
            SendWarnings = config.SendWarnings;
            _prefixes = config.Prefixes.ToDictionary();
            PrefixLookupMode = config.PrefixLookupMode;
        }

        /// <summary>
        /// Loads the ADL Config from the file at the supplied path
        /// </summary>
        /// <param name="path">file path</param>
        public static void LoadConfig(string path = "adl_config.xml")
        {
            ADLConfig config = ConfigManager.ReadFromFile<ADLConfig>(path);
            LoadConfig(config);
        }

        /// <summary>
        /// Saves the configuration to the given file path
        /// </summary>
        /// <param name="config">config to save</param>
        /// <param name="path">file path</param>
        public static void SaveConfig(ADLConfig config, string path = "adl_config.xml")
        {
            ConfigManager.SaveToFile(path, config);
        }


        /// <summary>
        /// Saves the current configuration of ADL to the given file path
        /// </summary>
        /// <param name="path">File path.</param>
        public static void SaveConfig(string path = "adl_config.xml")
        {
            ADLConfig config = ADLConfig.Standard;
            config.ADLEnabled = ADLEnabled;
            config.SendUpdateMessageOnFirstLog = SendUpdateMessageOnFirstLog;
            config.UpdateMask = UpdateMask;
            config.WarningMask = ADLWarningMask;
            config.SendWarnings = SendWarnings;
            config.Prefixes = new SerializableDictionary<int, string>(_prefixes);
            config.PrefixLookupMode = PrefixLookupMode;
            SaveConfig(config, path);
        }
        #endregion

    }
}
