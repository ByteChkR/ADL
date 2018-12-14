using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ADL.CustomCMD
{


    public partial class CustomCMDForm : Form
    {
        Dictionary<string, Color> colorCoding = null;
        StreamReader tr;
        bool _hasColorCoding;
        Color baseFontColor;

        /// <summary>
        /// Creates a new CustomCMD.
        /// </summary>
        /// <param name="ps"></param>
        /// <param name="Background"></param>
        /// <param name="BaseFontColor"></param>
        /// <param name="fontSize"></param>
        /// <param name="colorCoding"></param>
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

        private void Form1_Load(object sender, EventArgs e)
        {
            Show();
            timer1.Start();
        }

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshTextBox();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
    }
}
