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

        private void timer1_Tick(object sender, EventArgs e)
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

                    logColor = baseFontColor;

                    if (_hasColorCoding)
                    {
                        string[] s = test.Split(']');
                        foreach (string tag in s)
                        {

                            if (colorCoding.ContainsKey(tag + ']'))
                            {
                                logColor = colorCoding[tag + ']'];
                                break;
                            }
                        }
                    }

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

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            richTextBox1.ScrollToCaret();
        }
    }
}
