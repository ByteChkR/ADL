using System;
using System.Collections.Generic;
using System.Drawing;

namespace ADL.Configs
{
    /// <summary>
    ///     Contains all fields that are configurable in the component CustomCMD
    /// </summary>
    [Serializable]
    public class AdlCustomConsoleConfig : AbstractAdlConfig
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
        ///     Returns the standard configuration
        /// </summary>
        /// <returns></returns>
        public override AbstractAdlConfig GetStandard()
        {
            return new AdlCustomConsoleConfig
            {
                BackgroundColor = Color.Black,
                FontColor = Color.White,
                FontSize = 8.5f,
                FrameTime = 250,
                ColorCoding =
                    new SerializableDictionary<int, SerializableColor>(new Dictionary<int, SerializableColor>())
            };
        }
    }
}