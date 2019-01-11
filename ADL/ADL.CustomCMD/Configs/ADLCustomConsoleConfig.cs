
using System;
using System.Drawing;
using System.Collections.Generic;
using ADL.Configs;

namespace ADL.Configs
{

    /// <summary>
    /// Contains all fields that are configurable in the component CustomCMD
    /// </summary>
    [Serializable]
    public struct ADLCustomConsoleConfig : IADLConfig
    {
        /// <summary>
        /// The background color of the log window
        /// </summary>
        public SerializableColor BackgroundColor;
        /// <summary>
        /// The font color of the log window
        /// </summary>
        public SerializableColor FontColor;
        /// <summary>
        /// The font size of the log window
        /// </summary>
        public float FontSize;
        /// <summary>
        /// The color coding of the log window
        /// </summary>
        public SerializableDictionary<int, SerializableColor> ColorCoding;


        /// <summary>
        /// Returns the standard configuration
        /// </summary>
        /// <returns></returns>
        public IADLConfig GetStandard()
        {
            return Standard;
        }

        /// <summary>
        /// The standard configuration of ADLCustomConsoleConfig
        /// </summary>
        public static ADLCustomConsoleConfig Standard
        {
            get
            {
                ADLCustomConsoleConfig config = new ADLCustomConsoleConfig
                {
                    BackgroundColor = Color.Black,
                    FontColor = Color.White,
                    FontSize = 8.5f,
                    ColorCoding = new SerializableDictionary<int, SerializableColor>(new Dictionary<int, SerializableColor>())
                };
                return config;
            }
        }
    }
}
