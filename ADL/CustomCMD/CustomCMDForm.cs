using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ADL.CustomCMD
{

    /// <summary>
    /// Form to Display the Content of a LogStream.
    /// Color coding supported.
    /// </summary>
    public partial class CustomCMDForm : Form
    {
        public static readonly int MinConsoleTextLength = 4096;


        /// <summary>
        /// The Tags and their corresponding colors
        /// </summary>
        Dictionary<string, Color> colorCoding = null;

        ///// <summary>
        ///// Copies the Prefix Array from the Debug Class to the new thread of the CustomCMD
        ///// </summary>
        //Dictionary<int, string> _prefixes = null;

        /// <summary>
        /// The Stream reader thats is used to fill the textbox
        /// </summary>
        StreamReader tr;
        /// <summary>
        /// Flag to check if running the color coding algorithm is useful
        /// </summary>
        bool _hasColorCoding;
        /// <summary>
        /// Basis color. If no colorcoding or tag is not found
        /// </summary>

        Color BackgroundColor {
            get
            {
                return rtb_LogOutput.BackColor;
            }
            set
            {
                rtb_LogOutput.BackColor = value;
            }
        }

        Color FontColor
        {
            get
            {
                return rtb_LogOutput.ForeColor;
            }
            set
            {
                rtb_LogOutput.ForeColor = value;
            }
        }

        float FontSize { get
            {
                return rtb_LogOutput.Font.Size;
            }
            set
            {
                float v = value;
                rtb_LogOutput.Font = new Font(rtb_LogOutput.Font.FontFamily, v);
            }
        }

        /// <summary>
        /// Creates a new CustomCMD.
        /// </summary>
        /// <param name="ps">Pipe Stream to read from</param>
        /// <param name="Background">Background Color</param>
        /// <param name="BaseFontColor">Base Font Color. Acts as fallback if no color coding or tag not found</param>
        /// <param name="fontSize">font size of the logs</param>
        /// <param name="colorCoding">colorcoding</param>
        public CustomCMDForm(PipeStream ps, Color Background, Color BaseFontColor, float fontSize, Dictionary<string, Color> colorCoding = null)
        {
            InitializeComponent();
            FormClosed += CloseForm; //Makes the programm close when this console is closed.
            foreach (KeyValuePair<int, string> kvp in Debug.GetAllTags()) //Copy all tags to the form. To Mitigate some access violation.
            {
                clb_TagFilter.Items.Add(kvp.Value);
                clb_TagFilter.SetItemChecked(clb_TagFilter.Items.IndexOf(kvp.Value), true);
            }
            ps.BlockLastReadBuffer = false; //Nothing in the stream? Nothing in the return.

            tr = new StreamReader(ps);

            //Visual elements
            BackColor = Background;
            FontColor = BaseFontColor;
            FontSize = fontSize;

            if (colorCoding != null && colorCoding.Count != 0) 
            {
                this.colorCoding = colorCoding;
                _hasColorCoding = true;
            }


        }

        

        /// <summary>
        /// Completely Terminates the Window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void CloseForm(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Gets called when the form is fully initialized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            Show();
            timer1.Start();
        }

        /// <summary>
        /// Filters the Tags and returns the right color
        /// </summary>
        /// <param name="line">entire log line</param>
        /// <returns></returns>
        Color GetColorFromLine(string line)
        {
            Color ret = FontColor;

            if (_hasColorCoding)
            {
                string[] s = line.Split(']');
                foreach (string tag in s)
                {

                    if (colorCoding.ContainsKey(tag + ']'))
                    {
                        ret = colorCoding[tag + ']'];
                        break;
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Refreshes text box with content from the log stream.
        /// </summary>
        void RefreshTextBox()
        {
            string test = "";
            Color logColor;

            string block = tr.ReadToEnd();



            if (block != null)
            {

                string[] logs = block.Split(Utils.NEW_LINE);

                for (int i = 0; i < logs.Length; i++)
                {
                    test = logs[i];

                    foreach (string tag in clb_TagFilter.Items)
                    {
                        if (test.Contains(tag) && clb_TagFilter.GetItemCheckState(clb_TagFilter.Items.IndexOf(tag)) == CheckState.Unchecked)
                        {
                            return;
                        }
                    }

                    logColor = GetColorFromLine(test);



                    if (test.Length + rtb_LogOutput.Text.Length > rtb_LogOutput.MaxLength && rtb_LogOutput.Text.Length >= MinConsoleTextLength)
                    {
                        rtb_LogOutput.Text = rtb_LogOutput.Text.Substring(rtb_LogOutput.Text.Length - MinConsoleTextLength, MinConsoleTextLength);
                    }

                    if (test.Length != 0) rtb_LogOutput.AppendText(test+Utils.NEW_LINE, logColor);

                }

            }
            else
            {
                Application.Exit();
            }

        }


        /// <summary>
        /// Timer thats periodically refreshes the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            RefreshTextBox();
        }

        /// <summary>
        /// Makes the textbox scroll down when text changed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
            rtb_LogOutput.SelectionStart = rtb_LogOutput.Text.Length;
            rtb_LogOutput.ScrollToCaret();
        }
    }
}
