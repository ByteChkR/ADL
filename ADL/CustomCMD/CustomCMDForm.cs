using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using ADL;

namespace ADL.CustomCMD
{
    

    public partial class CustomCMDForm : Form
    {
        TextReader tr;
        public CustomCMDForm(TextReader tr)
        {
            InitializeComponent();

            this.tr = tr;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Show();
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            string test = tr.ReadToEnd();
            
            

            if (test != null)
            {
                if (test.Length + richTextBox1.Text.Length > richTextBox1.MaxLength && richTextBox1.Text.Length >= 4096)
                {
                    richTextBox1.Text = richTextBox1.Text.Substring(richTextBox1.Text.Length - 4096, 4096);
                }
                
                if(test.Length != 0)richTextBox1.Text += test;
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
