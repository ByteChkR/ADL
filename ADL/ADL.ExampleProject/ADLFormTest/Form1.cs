using System;
using System.Collections.Generic;

using System.Windows.Forms;
using ADL;
using ADL.Streams;
using ADL.CustomCMD;
using ADL.Configs;
namespace ADLFormTest
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// How to set up ADL.
        /// </summary>
        private static void CreateADLConfig()
        {
            Debug.SetAllPrefixes("[General]", "[Log]", "[Warning]", "[Error]", "[Fatal]", "[ADL]");
            Debug.ADLWarningMask = 4;
            Debug.ADLEnabled = true;
            Debug.SendUpdateMessageOnFirstLog = true;
            Debug.SendWarnings = true;
            Debug.UpdateMask = 32;
            Debug.PrefixLookupMode = PrefixLookupSettings.ADDPREFIXIFAVAILABLE;
            Debug.SaveConfig(); //Not Needed to work, but for the next time we can just load the config
        }

        private static void CreateADLCustomCMDConfig()
        {

            SerializableDictionary<int, SerializableColor> colorCoding =
                new SerializableDictionary<int, SerializableColor>(
                    new Dictionary<int, SerializableColor>()
                    {
                        {8, System.Drawing.Color.Red }, //Every errror message should be drawn in red.
                        {4, System.Drawing.Color.Orange }, //Every warning is painted in orange
                        {32, System.Drawing.Color.Green }
                    });
            ADLCustomConsoleConfig config = ADLCustomConsoleConfig.Standard;
            config.FontSize = 13;
            config.ColorCoding = colorCoding;
            CMDUtils.SaveConfig(config);
        }

        void CloseDebug(object sender, EventArgs e)
        {
            if(debugFrm != null)
            {
                CheckForIllegalCrossThreadCalls = false;
                debugFrm.Close();
                CheckForIllegalCrossThreadCalls = true;
            }
        }

        Form debugFrm;

        public Form1()
        {
            InitializeComponent();
            this.FormClosing += CloseDebug;
            // Create Config if not there:
            CreateADLConfig();
            CreateADLCustomCMDConfig();
            //

            Debug.LoadConfig();

            PipeStream ps = new PipeStream();
            LogStream ls = new LogStream(ps, new BitMask(true));
            Debug.AddOutputStream(ls); //Custom Console
            debugFrm = CMDUtils.CreateCustomConsole(ps);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            Debug.Log(8, "Button Press");
        }
    }
}
