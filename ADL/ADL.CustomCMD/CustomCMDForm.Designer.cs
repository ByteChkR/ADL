namespace ADL.CustomCMD
{
    internal partial class CustomCmdForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomCmdForm));
            this.rtb_LogOutput = new System.Windows.Forms.RichTextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.clb_TagFilter = new System.Windows.Forms.CheckedListBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtb_LogOutput
            // 
            this.rtb_LogOutput.BackColor = System.Drawing.Color.Black;
            this.rtb_LogOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtb_LogOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtb_LogOutput.ForeColor = System.Drawing.Color.Lime;
            this.rtb_LogOutput.Location = new System.Drawing.Point(0, 0);
            this.rtb_LogOutput.Name = "rtb_LogOutput";
            this.rtb_LogOutput.ReadOnly = true;
            this.rtb_LogOutput.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.rtb_LogOutput.Size = new System.Drawing.Size(664, 450);
            this.rtb_LogOutput.TabIndex = 1;
            this.rtb_LogOutput.Text = "";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // clb_TagFilter
            // 
            this.clb_TagFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clb_TagFilter.FormattingEnabled = true;
            this.clb_TagFilter.Location = new System.Drawing.Point(0, 0);
            this.clb_TagFilter.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.clb_TagFilter.Name = "clb_TagFilter";
            this.clb_TagFilter.Size = new System.Drawing.Size(133, 450);
            this.clb_TagFilter.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.clb_TagFilter);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.rtb_LogOutput);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 133;
            this.splitContainer1.SplitterWidth = 3;
            this.splitContainer1.TabIndex = 4;
            // 
            // CustomCMDForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CustomCmdForm";
            this.Text = "ADL : Custom Console";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox rtb_LogOutput;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckedListBox clb_TagFilter;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

