using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADL.Configs
{
    /// <summary>
    /// Contains the Configurations of the main ADL.Debug class.
    /// </summary>
    [Serializable]
    public class ADLConfig : IADLConfig
    {
        /// <summary>
        /// Is ADL enabled when this config is loaded?
        /// </summary>
        public bool ADLEnabled;
        /// <summary>
        /// Should ADL Search for updates when the first log message gets writen to the streams.
        /// It will take ~300ms to communicate with the github server.
        /// </summary>
        public bool SendUpdateMessageOnFirstLog;
        /// <summary>
        /// The mask that gets used to give information about the Update Check
        /// </summary>
        public int UpdateMask;
        /// <summary>
        /// The mask that ADL uses to write warnings
        /// </summary>
        public int WarningMask;

        /// <summary>
        /// A flag to switch if adl should send warnings at all.
        /// </summary>
        public bool SendWarnings;
        /// <summary>
        /// The prefixes that are used when a log in a specific mask gets sent.
        /// </summary>
       
        public SerializableDictionary<int, string> Prefixes;

        /// <summary>
        /// Determines the Options on how much effort is put into finding the right tags
        /// </summary>
        public PrefixLookupSettings PrefixLookupMode;

        #region Lookup Presets

        public static PrefixLookupSettings LowestPerformance =
            PrefixLookupSettings.ADDPREFIXIFAVAILABLE |
            PrefixLookupSettings.DECONSTRUCTMASKTOFIND;

        public static PrefixLookupSettings LowPerformance =
            PrefixLookupSettings.ADDPREFIXIFAVAILABLE |
            PrefixLookupSettings.DECONSTRUCTMASKTOFIND |
            PrefixLookupSettings.ONLYONEPREFIX;

        public static PrefixLookupSettings MediumPerformance =
            PrefixLookupSettings.ADDPREFIXIFAVAILABLE |
            PrefixLookupSettings.DECONSTRUCTMASKTOFIND |
            PrefixLookupSettings.BAKEPREFIXES;

        public static PrefixLookupSettings HighPerformance =
            PrefixLookupSettings.ADDPREFIXIFAVAILABLE;

        public static PrefixLookupSettings HighestPerformance = 
            PrefixLookupSettings.NOPREFIX;

        #endregion

        /// <summary>
        /// Standard Confuguration
        /// </summary>
        /// <returns>The standard configuration of ADL</returns>
        public IADLConfig GetStandard()
        {
            return Standard;
        }

        /// <summary>
        /// The standard configuration of ADL
        /// </summary>
        public static ADLConfig Standard
        {
            get
            {
                ADLConfig std = new ADLConfig
                {
                    ADLEnabled = true,
                    SendUpdateMessageOnFirstLog = true,
                    UpdateMask = new BitMask(true),
                    WarningMask = new BitMask(true),
                    SendWarnings = true,
                    Prefixes = new SerializableDictionary<int, string>(new Dictionary<int, string>()),
                    PrefixLookupMode = PrefixLookupSettings.ADDPREFIXIFAVAILABLE | PrefixLookupSettings.DECONSTRUCTMASKTOFIND
                };
                return std;
            }
        }
    }
}
