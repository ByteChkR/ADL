using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ADL.Configs
{
    /// <summary>
    /// A Struct that has implicit conversion to System.Drawing.Color.
    /// </summary>
    [Serializable]
    public struct SerializableColor
    {
        /// <summary>
        /// RGB Values
        /// </summary>
        public byte R,G,B;

        /// <summary>
        /// Converts a System.Drawing.Color into a SerializableColor
        /// </summary>
        /// <param name="color"></param>
        public SerializableColor(Color color)
        {
            R = color.R;
            G = color.G;
            B = color.B;
        }
        


        /// <summary>
        /// Implicit operator override to directly convert to System.Drawing.Color
        /// </summary>
        /// <param name="color">Color to convert</param>
        public static implicit operator Color(SerializableColor color)
        {
            return color.ToColor();
        }
        /// <summary>
        /// Implicit operator override to directly convert to SerializableColor
        /// </summary>
        /// <param name="color">Color to convert</param>
        public static implicit operator SerializableColor(Color color)
        {
            return new SerializableColor(color);
        }

        /// <summary>
        /// Returns the System.Drawing.Color representation of this object.
        /// </summary>
        /// <returns>System Color Representation</returns>
        public Color ToColor()
        {
            return Color.FromArgb(R, G, B);
        }

    }
}
