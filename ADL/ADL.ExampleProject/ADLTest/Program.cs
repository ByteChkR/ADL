using System;
using System.Collections.Generic;
using ADL;
using ADL.Streams;
using ADL.CustomCMD;
using ADL.Configs;
namespace ADLTest
{
    class Program
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

        static void Main(string[] args)
        {
            // Create Config if not there:
            CreateADLConfig();
            CreateADLCustomCMDConfig();
            //

            Debug.LoadConfig();


            LogStream lts = new LogTextStream(Console.OpenStandardOutput());
            Debug.AddOutputStream(lts); //Console

            PipeStream ps = new PipeStream();
            LogStream ls = new LogStream(ps, new BitMask(true), MatchType.MATCH_ONE, true);
            Debug.AddOutputStream(ls); //Custom Console
            CMDUtils.CreateCustomConsole(ps);

            

            Test();
        }


        static void Test()
        {
            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(100);
                Debug.Log(32, "TEST");
            }
        }
    }
}
