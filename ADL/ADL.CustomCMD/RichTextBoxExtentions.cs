using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ADL.CustomCMD
{
    /// <summary>
    ///     Extentions for WindowsForms.RichTextBox
    /// </summary>
    public static class RichTextBoxExtensions
    {
        private const int WmVscroll = 277;
        private const int SbPagebottom = 7;

        /// <summary>
        ///     Appends text in a specified color.
        /// </summary>
        /// <param name="box">this</param>
        /// <param name="text">text to append</param>
        /// <param name="color">color of the text</param>
        public static void AppendText(this RichTextBox box, string text, Color color)
        {
            if (box.Disposing || box.IsDisposed) return;
            box.SelectionStart = box.TextLength;
            box.SelectionLength = 0;

            box.SelectionColor = color;


            box.AppendText(text);


            box.SelectionColor = box.ForeColor;
        }

        /// <summary>
        ///     Wrapper to extend the RichTextBox
        /// </summary>
        /// <param name="box"></param>
        public static void ScrollToBottom(this RichTextBox box)
        {
            ScrollToBtm(box);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        ///     Since winforms issues an Access violation exception I implemented a external function to cope with this
        /// </summary>
        /// <param name="myRichTextBox">The textbox that should be scrolled down</param>
        public static void ScrollToBtm(RichTextBox myRichTextBox)
        {
            SendMessage(myRichTextBox.Handle, WmVscroll, (IntPtr) SbPagebottom, IntPtr.Zero);
        }
    }
}