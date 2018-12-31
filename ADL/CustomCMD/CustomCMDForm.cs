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
        /// <summary>
        /// The Tags and their corresponding colors
        /// </summary>
        Dictionary<string, Color> colorCoding = null;
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
        Color baseFontColor;

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
            ps.BlockLastReadBuffer = false;
            tr = new StreamReader(ps);
            richTextBox1.BackColor = Background;
            baseFontColor = BaseFontColor;
            if (colorCoding != null && colorCoding.Count != 0)
            {
                this.colorCoding = colorCoding;
                _hasColorCoding = true;
            }
            richTextBox1.Font = new Font(richTextBox1.Font.FontFamily, fontSize);
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
            Color ret =  baseFontColor;

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

                string[] logs = block.Split('\n');

                for (int i = 0; i < logs.Length; i++)
                {
                    test = logs[i];

                    logColor = GetColorFromLine(test);

                    if (test.Length + richTextBox1.Text.Length > richTextBox1.MaxLength && richTextBox1.Text.Length >= 4096)
                    {
                        richTextBox1.Text = richTextBox1.Text.Substring(richTextBox1.Text.Length - 4096, 4096);
                    }

                    if (test.Length != 0) richTextBox1.AppendText(test, logColor);

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
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
    }
}
