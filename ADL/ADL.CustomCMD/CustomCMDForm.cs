using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ADL.Configs;
using ADL.Streams;

namespace ADL.CustomCMD
{
    /// <summary>
    ///     Form to Display the Content of a LogStream.
    ///     Color coding supported.
    /// </summary>
    internal partial class CustomCmdForm : Form
    {
        /// <summary>
        ///     The Tags and their corresponding colors
        /// </summary>
        private readonly Dictionary<int, SerializableColor> _colorCoding;

        ///// <summary>
        ///// Copies the Prefix Array from the Debug Class to the new thread of the CustomCMD
        ///// </summary>
        //Dictionary<int, string> _prefixes = null;


        /// <summary>
        ///     Flag to check if running the color coding algorithm is useful
        /// </summary>
        private readonly bool _hasColorCoding;

        private const int MaxLogCountPerBlock = 500;


        private const string ConsoleTitleInfo = "ADL : Custom Console : Logs Received {0} : Logs Written {1} : Blocks Writen {2}(1:{3}) : Text Cleared {4}";


        private readonly Queue<Log> _lastLogs = new Queue<Log>();
        private readonly PipeStream _ps;
        private float _avgLogsPerBlock;
        private int _blocksWritten;

        private int _consoleCleared;

        private int _logCount;
        private int _logsWritten;
        private int _totalLogsReceived;
        private int _totalLogsWrittenInBlocks;


        /// <summary>
        ///     Creates a new CustomCMD.
        /// </summary>
        /// <param name="ps">Pipe Stream to read from</param>
        /// <param name="background">Background Color</param>
        /// <param name="baseFontColor">Base Font Color. Acts as fallback if no color coding or tag not found</param>
        /// <param name="fontSize">font size of the logs</param>
        /// <param name="frameTime">the millseconds between checking for logs.</param>
        /// <param name="colorCoding">colorcoding</param>
        public CustomCmdForm(PipeStream ps, Color background, Color baseFontColor, float fontSize, int frameTime,
            Dictionary<int, SerializableColor> colorCoding = null)
        {
            InitializeComponent();
            _ps = ps;
            foreach (var kvp in Debug.GetAllPrefixes()) //Copy all tags to the form. To Mitigate some access violation.
            {
                clb_TagFilter.Items.Add(kvp.Value);
                clb_TagFilter.SetItemChecked(clb_TagFilter.Items.IndexOf(kvp.Value), true);
            }

            ps.BlockLastReadBuffer = false; //Nothing in the stream? Nothing in the return.

            timer1.Interval = frameTime;

            //Visual elements
            BackColor = background;
            FontColor = baseFontColor;
            FontSize = fontSize;

            if (colorCoding == null || colorCoding.Count == 0) return;
            _colorCoding = colorCoding;
            _hasColorCoding = true;
        }

        public sealed override Color BackColor
        {
            get => base.BackColor;
            set => base.BackColor = value;
        }

        public int MaxConsoleLogCount { get; set; } = 500;

        //public int MaxLogCountPerBlock { get; set; } = 200;

        public int MinConsoleLogCount { get; set; } = 23;


        /// <summary>
        ///     Basis color. If no colorcoding or tag is not found
        /// </summary>

        public Color BackgroundColor
        {
            get => rtb_LogOutput.BackColor;
            set => rtb_LogOutput.BackColor = value;
        }

        public Color FontColor
        {
            get => rtb_LogOutput.ForeColor;
            set => rtb_LogOutput.ForeColor = value;
        }

        public float FontSize
        {
            get => rtb_LogOutput.Font.Size;
            set
            {
                var v = value;
                rtb_LogOutput.Font = new Font(rtb_LogOutput.Font.FontFamily, v);
            }
        }

        /// <summary>
        ///     Gets called when the form is fully initialized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //while (true)
            //{
            //    Application.DoEvents();
            //    RefreshTextBox();
            //}
            timer1.Start();
        }

        /// <summary>
        ///     Filters the Tags and returns the right color
        /// </summary>
        /// <param name="mask">Mask</param>
        /// <returns></returns>
        private Color GetColorFromMask(int mask)
        {
            var ret = FontColor;

            if (!_hasColorCoding) return ret;
            if (_colorCoding.ContainsKey(mask))
                return _colorCoding[mask];
            if (!BitMask.IsContainedInMask((int) Debug.PrefixLookupMode,
                (int) PrefixLookupSettings.Deconstructmasktofind, true)) return ret;
            foreach (var m in BitMask.GetUniqueMasksSet(mask))
                if (_colorCoding.ContainsKey(m))
                    return _colorCoding[m];

            return ret;
        }

        /// <summary>
        ///     Refreshes text box with content from the log stream.
        /// </summary>
        private void RefreshTextBox()
        {
            if (rtb_LogOutput.IsDisposed || rtb_LogOutput.Disposing) return;
            var block = ReadBlock();
            if (block.Logs.Count == 0) return;

            if (MaxConsoleLogCount < _logCount)
                ClearConsole();
            _logCount += block.Logs.Count;
            ShowInConsole(block);
            Text = string.Format(ConsoleTitleInfo, _totalLogsReceived, _logsWritten, _blocksWritten,
                Math.Round(_avgLogsPerBlock, 3), _consoleCleared);
        }


        private LogPackage ReadBlock()
        {
            return LogPackage.ReadBlock(_ps, (int) _ps.Length);
        }


        /// <summary>
        ///     Filters the Logs based on the Checked State of the tags in clb_tagFilter
        /// </summary>
        /// <param name="logPackage"></param>
        /// <returns></returns>
        private LogPackage FilterLogs(LogPackage logPackage)
        {
            var result = new bool[logPackage.Logs.Count];
            for (var i = 0; i < logPackage.Logs.Count; i++)
            {
                var containsOne=false;
                foreach (var t in clb_TagFilter.CheckedItems)
                {
                    if (!Debug.GetPrefixMask(t.ToString(), out var mask)) continue;
                    if (BitMask.IsContainedInMask(mask, logPackage.Logs[i].Mask, false)) containsOne = true;
                }

                result[i] = containsOne;
            }

            for (var i = result.Length - 1; i >= 0; i--)
            {
                if (result[i]) continue;
                logPackage.Logs.RemoveAt(i);
            }
            
            return logPackage;
        }

        /// <summary>
        ///     Splits the log string on new lines
        ///     Filters them, and chooses the right way to append it to the console.
        /// </summary>
        /// <param name="logs">whole block of logs</param>
        private void ShowInConsole(LogPackage logs)
        {
            var llogs = logs; //FilterLogs(logs);
            if (llogs.Logs.Count == 0) return;
            _totalLogsReceived += llogs.Logs.Count;
            if (llogs.Logs.Count > MaxLogCountPerBlock)
                //Can not keep up with the amount of logs. Writing this whole block without color support and at one piece.
            {
                if (MaxLogCountPerBlock < llogs.Logs.Count)
                    return; // Thats not worth it. there is no way to write all of that in the console.
                llogs = FilterLogs(llogs); //If we write in blocks filter out the wrong logs
                Debug.Log(Debug.AdlWarningMask,
                    "CustomCMDForm.ShowInConsole(Logpackage Logs) : You are outputting to much logs. the Console can not keep up in that pace. Consider changing the mask for the console to achieve better performance.");
                _totalLogsWrittenInBlocks += llogs.Logs.Count;
                _blocksWritten++;

                WriteToConsole(string.Join("", llogs.Logs.Select(x => x.Message).ToArray()),
                    FontColor); //Rejoin the filtered list.
            }
            else
            {
                _logsWritten += llogs.Logs.Count;
                foreach (var l in llogs.Logs)
                {
                    //Do the FilterLogs() code in this loop to prevent another 2 for loops.
                    var containsOne = false;
                    for (var j = 0; j < clb_TagFilter.CheckedItems.Count; j++)
                    {
                        if (!Debug.GetPrefixMask(j.ToString(), out var mask)) continue;
                        if (BitMask.IsContainedInMask(mask, l.Mask, false)) containsOne = true;
                    }

                    if (!containsOne) break;


                    var fontColor = GetColorFromMask(l.Mask);

                    _lastLogs.Enqueue(l);
                    if (_lastLogs.Count > MinConsoleLogCount)
                        _lastLogs.Dequeue();
                    WriteToConsole(l.Message, fontColor);
                }
            }

            rtb_LogOutput.ScrollToBottom();
            _avgLogsPerBlock = (float) _totalLogsWrittenInBlocks / _blocksWritten;
        }

        /// <summary>
        ///     Writes the text to the RichTextBox
        /// </summary>
        /// <param name="text">Text to write</param>
        /// <param name="textColor">Text Color</param>
        private void WriteToConsole(string text, Color textColor)
        {
            rtb_LogOutput.AppendText(text, textColor);
        }

        /// <summary>
        ///     Clears the RichTextBox.Text part that is not on screen.
        /// </summary>
        private void ClearConsole()
        {
            _consoleCleared++;
            var logs = _lastLogs.ToList();
            _lastLogs.Clear();
            _logCount = 0;
            rtb_LogOutput.Clear();

            foreach (var log in logs) WriteToConsole(log.Message, GetColorFromMask(log.Mask));
        }


        /// <summary>
        ///     Timer thats periodically refreshes the textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer1_Tick(object sender, EventArgs e)
        {
            Application.DoEvents();
            RefreshTextBox();
        }
    }
}