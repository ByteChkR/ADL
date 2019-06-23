using System;
using System.Collections.Generic;
using System.Drawing;

namespace ADL.Configs
{
    /// <summary>
    ///     Contains all fields that are configurable in the component CustomCMD
    /// </summary>
    [Serializable]
    public class AdlCustomConsoleConfig : IAdlConfig
    {
        /// <summary>
        ///     The background color of the log window
        /// </summary>
        public SerializableColor BackgroundColor;

        /// <summary>
        ///     The color coding of the log window
        /// </summary>
        public SerializableDictionary<int, SerializableColor> ColorCoding;

        /// <summary>
        ///     The font color of the log window
        /// </summary>
        public SerializableColor FontColor;

        /// <summary>
        ///     The font size of the log window
        /// </summary>
        public float FontSize;

        /// <summary>
        ///     The time in ms that console refreshes the log window
        /// </summary>
        public int FrameTime;

        /// <summary>
        ///     The standard configuration of ADLCustomConsoleConfig
        /// </summary>
        public static AdlCustomConsoleConfig Standard
        {
            get
            {
                var config = new AdlCustomConsoleConfig
                {
                    BackgroundColor = Color.Black,
                    FontColor = Color.White,
                    FontSize = 8.5f,
                    FrameTime = 250,
                    ColorCoding =
                        new SerializableDictionary<int, SerializableColor>(new Dictionary<int, SerializableColor>())
                };
                return config;
            }
        }


        /// <summary>
        ///     Returns the standard configuration
        /// </summary>
        /// <returns></returns>
        public IAdlConfig GetStandard()
        {
            return Standard;
        }
    }
}