using System.Windows.Forms;
using System.Drawing;

namespace ADL.CustomCMD
{
    /// <summary>
    /// Extentions for WindowsForms.RichTextBox
    /// </summary>
    public static class RichTextBoxExtensions
    {
        private static int test = 0;
        /// <summary>
        /// Appends text in a specified color.
        /// </summary>
        /// <param name="box">this</param>
        /// <param name="text">text to append</param>
        /// <param name="color">color of the text</param>
        public static void AppendText(this RichTextBox box, string text, Color color)
        {

            
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;
            box.AppendText(text);
            box.SelectionColor = box.ForeColor;

        }
    }
}
