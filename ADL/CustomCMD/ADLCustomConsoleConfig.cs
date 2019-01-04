
using System;
using System.Drawing;
using System.Collections.Generic;

namespace ADL.CustomCMD
{

    /// <summary>
    /// Contains all fields that are configurable in the component CustomCMD
    /// </summary>
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
        public SerializableDictionary<string, SerializableColor> ColorCoding;


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
                ADLCustomConsoleConfig config = new ADLCustomConsoleConfig();
                config.BackgroundColor = Color.Black;
                config.FontColor = Color.White;
                config.FontSize = 8.5f;
                config.ColorCoding = new SerializableDictionary<string, SerializableColor>(new Dictionary<string, SerializableColor>());
                return config;
            }
        }
    }
}
