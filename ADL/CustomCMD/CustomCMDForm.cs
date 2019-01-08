using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ADL.Configs;
using System.Linq;
using ADL.Streams;
using ADL;
namespace ADL.CustomCMD
{

    /// <summary>
    /// Form to Display the Content of a LogStream.
    /// Color coding supported.
    /// </summary>
    public partial class CustomCMDForm : Form
    {
        private int _maxConsoleTextLength = 25000;
        private int _minConsoleTextLength = 5000;
        private int _maxLogCountPerFrame = 250;
        private PipeStream ps;
        
        public int MaxConsoleLogCount
        {
            get
            {
                return _maxConsoleTextLength;
            }
            set
            {
                _maxConsoleTextLength = value;
            }
        }
        public int MinConsoleTextLength
        {
            get
            {
                return _minConsoleTextLength;
            }
            set
            {
                _minConsoleTextLength = value;
            }
        }

        public int MaxLogCountPerFrame
        {
            get
            {
                return _maxLogCountPerFrame;
            }
            set
            {
                _maxLogCountPerFrame = value;
            }
        }

        /// <summary>
        /// The Tags and their corresponding colors
        /// </summary>
        private Dictionary<int, SerializableColor> _colorCoding = null;

        ///// <summary>
        ///// Copies the Prefix Array from the Debug Class to the new thread of the CustomCMD
        ///// </summary>
        //Dictionary<int, string> _prefixes = null;


        /// <summary>
        /// Flag to check if running the color coding algorithm is useful
        /// </summary>
        private bool _hasColorCoding;
        /// <summary>
        /// Basis color. If no colorcoding or tag is not found
        /// </summary>

        public Color BackgroundColor
        {
            get
            {
                return rtb_LogOutput.BackColor;
            }
            set
            {
                rtb_LogOutput.BackColor = value;
            }
        }

        public Color FontColor
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

        public float FontSize
        {
            get
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
        public CustomCMDForm(PipeStream ps, Color Background, Color BaseFontColor, float fontSize, Dictionary<int, SerializableColor> colorCoding = null)
        {
            InitializeComponent();
            this.ps = ps;
            foreach (KeyValuePair<int, string> kvp in Debug.GetAllPrefixes()) //Copy all tags to the form. To Mitigate some access violation.
            {
                clb_TagFilter.Items.Add(kvp.Value);
                clb_TagFilter.SetItemChecked(clb_TagFilter.Items.IndexOf(kvp.Value), true);
            }
            ps.BlockLastReadBuffer = false; //Nothing in the stream? Nothing in the return.


            //Visual elements
            BackColor = Background;
            FontColor = BaseFontColor;
            FontSize = fontSize;

            if (colorCoding != null && colorCoding.Count != 0)
            {
                this._colorCoding = colorCoding;
                _hasColorCoding = true;
            }


        }

        /// <summary>
        /// Gets called when the form is fully initialized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            Show();

            //while (true)
            //{
            //    Application.DoEvents();
            //    RefreshTextBox();
            //}
            timer1.Start();
        }

        /// <summary>
        /// Filters the Tags and returns the right color
        /// </summary>
        /// <param name="line">entire log line</param>
        /// <returns></returns>
        Color GetColorFromMask(int mask)
        {
            Color ret = FontColor;

            if (_hasColorCoding)
            {
                if (_colorCoding.ContainsKey(mask)) return _colorCoding[mask];
            }
            return ret;
        }


        private string ConsoleTitleInfo = "ADL : Custom Console : Logs Received {0} : Logs Written {1} : Blocks Writen {2}(1:{3}) : Text Cleared {4}";
        private int _totalLogsReceived = 0;
        private int _logsWritten = 0;
        private int _blocksWritten = 0;
        private int _consoleCleared = 0;
        private float _avgLogsPerBlock = 0;
        private int _totalLogsWrittenInBlocks = 0;
        /// <summary>
        /// Refreshes text box with content from the log stream.
        /// </summary>
        void RefreshTextBox()
        {
            if (rtb_LogOutput.IsDisposed || rtb_LogOutput.Disposing) return;
            LogPackage block = ReadBlock();
            if (block.Logs.Count == 0) return;

            if (MaxConsoleLogCount < block.Logs.Count)
                ClearConsole(block.Logs.Count());

            ShowInConsole(block);
            Text = string.Format(ConsoleTitleInfo, _totalLogsReceived, _logsWritten, _blocksWritten, Math.Round(_avgLogsPerBlock, 3), _consoleCleared);

        }


        private LogPackage ReadBlock()
        {
            byte[] buffer = new byte[ps.Length];
            ps.Read(buffer, 0, (int)ps.Length);
            return new LogPackage(buffer);
        }



        /// <summary>
        /// Filters the Logs based on the Checked State of the tags in clb_tagFilter
        /// </summary>
        /// <param name="logPackage"></param>
        /// <returns></returns>
        private LogPackage FilterLogs(LogPackage logPackage)
        {
            bool[] result = new bool[logPackage.Logs.Count];
            bool containsOne = false;
            for (int i = 0; i < logPackage.Logs.Count; i++)
            {
                containsOne = false;
                BitMask mask;
                for (int j = 0; j < clb_TagFilter.CheckedItems.Count; j++)
                {
                    if (!Debug.GetPrefixMask(clb_TagFilter.CheckedItems[j].ToString(), out mask)) continue;
                    if (BitMask.IsContainedInMask(mask,  logPackage.Logs[i].Mask, false)) containsOne = true;
                }

                result[i] = containsOne;
            }

            for (int i = result.Length - 1; i >= 0; i--)
            {
                if (result[i]) continue;
                logPackage.Logs.RemoveAt(i);
            }
            result = null;
            return logPackage;
        }

        /// <summary>
        /// Splits the log string on new lines
        /// Filters them, and chooses the right way to append it to the console.
        /// </summary>
        /// <param name="logs">whole block of logs</param>
        private void ShowInConsole(LogPackage logs)
        {
            LogPackage llogs = FilterLogs(logs);
            if (llogs.Logs.Count == 0) return;
            _totalLogsReceived += llogs.Logs.Count;
            if (llogs.Logs.Count > MaxLogCountPerFrame) //Can not keep up with the amount of logs. Writing this whole block without color support and at one piece.
            {
                _totalLogsWrittenInBlocks += llogs.Logs.Count;
                _blocksWritten++;

                WriteToConsole(string.Join("", llogs.Logs.Select(x=>x.Message).ToArray()), FontColor); //Rejoin the filtered list.
            }
            else
            {
                _logsWritten += llogs.Logs.Count;
                Color fontColor;
                foreach (Log l in llogs.Logs)
                {
                    fontColor = GetColorFromMask(l.Mask);
                    WriteToConsole(l.Message, fontColor);
                }
            }
            rtb_LogOutput.ScrollToBottom();
            _avgLogsPerBlock = (float)_totalLogsWrittenInBlocks / _blocksWritten;
        }

        /// <summary>
        /// Writes the text to the RichTextBox
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <param name="textColor">Text Color</param>
        private void WriteToConsole(string text, Color textColor)
        {
            rtb_LogOutput.AppendText(text, textColor);
        }

        /// <summary>
        /// Clears the RichTextBox.Text part that is not on screen.
        /// </summary>
        /// <param name="nextLength"></param>
        private void ClearConsole(int nextLength)
        {

            if (rtb_LogOutput.TextLength < MinConsoleTextLength + nextLength) return;
            _consoleCleared++;
            
            int totalLength = rtb_LogOutput.Text.Length + nextLength;
            string txt = rtb_LogOutput.Text.Substring
                (rtb_LogOutput.TextLength - (MinConsoleTextLength + nextLength),
                MinConsoleTextLength + nextLength);
            rtb_LogOutput.Clear();
            rtb_LogOutput.AppendText(txt, FontColor);
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
    }
}
