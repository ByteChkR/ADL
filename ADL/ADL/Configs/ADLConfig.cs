using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ADL.Configs
{
    /// <summary>
    ///     Contains the Configurations of the main ADL.Debug class.
    /// </summary>
    [Serializable]
    public class AdlConfig : IAdlConfig
    {
        /// <summary>
        ///     Is ADL enabled when this config is loaded?
        /// </summary>
        public bool AdlEnabled;

        /// <summary>
        ///     The prefixes that are used when a log in a specific mask gets sent.
        /// </summary>
        public SerializableDictionary<int, string> Prefixes;

        /// <summary>
        ///     Determines the Options on how much effort is put into finding the right tags
        /// </summary>
        public PrefixLookupSettings PrefixLookupMode;

        /// <summary>
        ///     Should ADL Search for updates when the first log message gets writen to the streams.
        ///     It will take ~300ms to communicate with the github server.
        /// </summary>
        public bool SendUpdateMessageOnFirstLog;

        /// <summary>
        ///     A flag to switch if adl should send warnings at all.
        /// </summary>
        public bool SendWarnings;

        [XmlIgnore]
        public Encoding TextEncoding;

        /// <summary>
        ///     The mask that gets used to give information about the Update Check
        /// </summary>
        public int UpdateMask;

        /// <summary>
        ///     The mask that ADL uses to write warnings
        /// </summary>
        public int WarningMask;

        /// <summary>
        ///     The standard configuration of ADL
        /// </summary>
        public static AdlConfig Standard
        {
            get
            {
                var std = new AdlConfig
                {
                    AdlEnabled = true,
                    SendUpdateMessageOnFirstLog = true,
                    UpdateMask = new BitMask(true),
                    WarningMask = new BitMask(true),
                    SendWarnings = true,
                    Prefixes = new SerializableDictionary<int, string>(new Dictionary<int, string>()),
                    PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable |
                                       PrefixLookupSettings.Deconstructmasktofind,
                    TextEncoding = Encoding.ASCII
                };
                return std;
            }
        }

        /// <summary>
        ///     Standard Confuguration
        /// </summary>
        /// <returns>The standard configuration of ADL</returns>
        public IAdlConfig GetStandard()
        {
            return Standard;
        }

        #region Lookup Presets

        public static PrefixLookupSettings LowestPerformance =
            PrefixLookupSettings.Addprefixifavailable |
            PrefixLookupSettings.Deconstructmasktofind;

        public static PrefixLookupSettings LowPerformance =
            PrefixLookupSettings.Addprefixifavailable |
            PrefixLookupSettings.Deconstructmasktofind |
            PrefixLookupSettings.Onlyoneprefix;

        public static PrefixLookupSettings MediumPerformance =
            PrefixLookupSettings.Addprefixifavailable |
            PrefixLookupSettings.Deconstructmasktofind |
            PrefixLookupSettings.Bakeprefixes;

        public static PrefixLookupSettings HighPerformance =
            PrefixLookupSettings.Addprefixifavailable;

        public static PrefixLookupSettings HighestPerformance =
            PrefixLookupSettings.Noprefix;

        #endregion
    }
}