using System;
using System.Collections.Generic;
using ADL.Configs;
using ADL.Streams;
namespace ADL.DebugTest
{
    /// <summary>
    /// Test programm to verify.
    /// </summary>
    class Program
    {
        enum LoggingTypes : int
        {
            GENERAL = 1,
            LOG = 2,
            WARNING = 4,
            ERROR = 8,
            FATAL = 16,
            ADL = 32,
            CSVLOGGING = 64
        }


        static void TestConsoleOut()
        {
            //First we want to Specify what Messages we want to let trough the LogStream we are creating in a minute.

            //You have multiple ways to create a Bitmask without getting cancer from bitwise operations
            BitMask bMaskWildCard = new BitMask(true); //Creates a Wildcard(Everything will be let through)
            BitMask bMaskNone = new BitMask(); //Creates the opposite of Wildcard(Nothing will be let through)

            //There are also more readable ways to create a mask.
            //For example with enums. The Implementation will stay the same.
            BitMask<LoggingTypes> bMaskGenericWildcard = new BitMask<LoggingTypes>(true);
            BitMask<LoggingTypes> bMaskGenericNone = new BitMask<LoggingTypes>();

            //But you can do masks easier now.
            //This bitmask only lets through logs and errors
            BitMask<LoggingTypes> bMaskGenericCustom = new BitMask<LoggingTypes>(LoggingTypes.ERROR, LoggingTypes.LOG);




            //Then we want to create a LogStream that receives the Messages
            //Important: Its much easier to use CreateLogStreamFromStream than setting everything manually
            LogStream logStream = new LogTextStream(
                Console.OpenStandardOutput(),
                bMaskGenericCustom, //Lets use the generic custom 
                MatchType.MATCH_ONE, //We want to make the logs pass when there is at least one tag that is included in the filter.
                true //Get that fancy timestamp infront of the log.
                );

            Debug.AddOutputStream(logStream); //Now we have Created the stream, just add it to the system.




            //This is a test.
            for (int i = 1; i < 64; i++) //63 because its the highest value the current enum can take(every bit beeing 1)
            {
                int mask = i;
                //Debug.Log(mask, "Test with mask " + mask);
            }

            Debug.LogGen<LoggingTypes>(LoggingTypes.LOG, "Finished the Console Out Test.");

            //Now we want to remove the stream from the system.
            //Debug.RemoveOutputStream(logStream, true); //We want to remove a single one.
            //But we can remove all Streams in one go
            //Debug.RemoveAllOutputStreams(true);

        }

        static void TestCustomConsoleOut()
        {
            //First we want to Specify what Messages we want to let trough the LogStream we are creating in a minute.

            //You have multiple ways to create a Bitmask without getting cancer from bitwise operations
            BitMask bMaskWildCard = new BitMask(true); //Creates a Wildcard(Everything will be let through)
            BitMask bMaskNone = new BitMask(); //Creates the opposite of Wildcard(Nothing will be let through)

            //There are also more readable ways to create a mask.
            //For example with enums. The Implementation will stay the same.
            BitMask<LoggingTypes> bMaskGenericWildcard = new BitMask<LoggingTypes>(true);
            BitMask<LoggingTypes> bMaskGenericNone = new BitMask<LoggingTypes>();

            //But you can do masks easier now.
            //This bitmask only lets through logs and errors
            BitMask<LoggingTypes> bMaskGenericCustom = new BitMask<LoggingTypes>(LoggingTypes.ERROR, LoggingTypes.LOG);

            //We want to Create a PipeStream that our logstream is basing on(pipe streams are threadsave streams in a single sender/single receiver situation)
            PipeStream pipeStream = new PipeStream(); //Create a new instance


            //Then we want to create a LogStream that receives the Messages
            //Important: Its much easier to use CreateLogStreamFromStream than setting everything manually
            LogStream logStream = new LogStream(
                pipeStream, //The Stream we want to send the Logs to.
                bMaskGenericWildcard, //Lets use the generic wildcard(you can set the mask dynamically when using a custom console.
                MatchType.MATCH_ONE, //We want to make the logs pass when all tags are included in the filter.
                true //Get that fancy timestamp infront of the log.
                );

            Debug.AddOutputStream(logStream); //Now we have Created the stream, just add it to the system.




            //After Creating the log Stream we want to create a custom Cmd window
            System.Windows.Forms.Form ccmd = //ADL.CustomCMD.CMDUtils.CreateCustomConsole(pipeStream); //Creates a basic Custom cmd with no visual adjustments
                ADL.CustomCMD.CMDUtils.CreateCustomConsole(pipeStream); //Creates a custom cmd with color coding and custom font size.

            //This is a test.
            for (int i = 1; i < 64; i++) //63 because its the highest value the current enum can take(every bit beeing 1)
            {
                int mask = i;
                //Debug.Log(mask, "Test with mask " + mask);
            }

            Debug.LogGen<LoggingTypes>(LoggingTypes.LOG, "Finished the CustomConsole Out Test.");

            //Now we want to remove the stream from the system.
            //Debug.RemoveOutputStream(logStream, true); //We want to remove a single one.
            //But we can remove all Streams in one go
            //Debug.RemoveAllOutputStreams(true);
        }

        static void TestLogFileOut()
        {
            //First we want to Specify what Messages we want to let trough the LogStream we are creating in a minute.

            //You have multiple ways to create a Bitmask without getting cancer from bitwise operations
            BitMask bMaskWildCard = new BitMask(true); //Creates a Wildcard(Everything will be let through)
            BitMask bMaskNone = new BitMask(); //Creates the opposite of Wildcard(Nothing will be let through)

            //There are also more readable ways to create a mask.
            //For example with enums. The Implementation will stay the same.
            BitMask<LoggingTypes> bMaskGenericWildcard = new BitMask<LoggingTypes>(true);
            BitMask<LoggingTypes> bMaskGenericNone = new BitMask<LoggingTypes>();

            //But you can do masks easier now.
            //This bitmask only lets through logs and errors
            BitMask<LoggingTypes> bMaskGenericCustom = new BitMask<LoggingTypes>(LoggingTypes.CSVLOGGING);


            //Then we want to create a LogStream that receives the Messages
            //Important: Its much easier to use CreateLogStreamFromFile than setting everything manually
            LogStream logStream = new LogTextStream(new System.IO.FileStream("test.log", System.IO.FileMode.OpenOrCreate),
                bMaskGenericCustom, //Lets use the generic custom 
                MatchType.MATCH_ALL, //We want to make the logs pass when there is at least one tag that is included in the filter.
                false //Get that fancy timestamp infront of the log.
                );
            logStream.OverrideChannelTag = true; //Forces the LogStream to discard all Tag and timestamps and ONLY save the actual log.

            Debug.AddOutputStream(logStream); //Now we have Created the stream, just add it to the system.

            //This is a test.
            for (int i = 1; i < 64; i++) //63 because its the highest value the current enum can take(every bit beeing 1)
            {
                int mask = i;
                //Debug.Log(mask, "Test with mask " + mask);
            }

            Debug.LogGen<LoggingTypes>(LoggingTypes.LOG, "Finished the Logfile Out Test.");
            Console.WriteLine("Finished the Logfile Out Test.");

            //Now we want to remove the stream from the system.
            //Debug.RemoveOutputStream(logStream, true); //We want to remove a single one.
            //But we can remove all Streams in one go
            //Debug.RemoveAllOutputStreams(true);

            //In this case we want to delete the created log file to prevent logs piling up in the test project.
            //System.IO.File.Delete("test.log");
        }



        static void Main(string[] args)
        {
            CreateADLConfig();
            CreateADLCustomCMDConfig();
            Debug.LoadConfig(); //Using the standard path
            //Runtime Config Changes(not Getting saved):
            Debug.SendWarnings = true;
            Debug.SendUpdateMessageOnFirstLog = true;
            Debug.ADLEnabled = true;
            Debug.PrefixLookupMode = PrefixLookupSettings.ADDPREFIXIFAVAILABLE | 
                                    PrefixLookupSettings.DECONSTRUCTMASKTOFIND | 
                                    PrefixLookupSettings.BAKEPREFIXES; //If you have int.minvalue to int.maxvalue channels this is not really advisable. (Config files can be bloated by baked prefixes thus getting a huge size.)

            TestCustomConsoleOut();

            TestConsoleOut();

            TestLogFileOut();

            Random rnd = new Random();
            int mul;
            long msLastTime = 0;
            float avg = 0;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            
            for (int i = 0; i < 100000; i++)
            {

                System.Threading.Thread.Sleep(50);
                sw.Start();
                Debug.LogGen(LoggingTypes.CSVLOGGING, rnd.NextDouble().ToString() +",");
                sw.Stop();
                msLastTime = sw.ElapsedTicks;
                avg = (avg + msLastTime) / 2;
                sw.Reset();


                
            }



            //System.Windows.Forms.Application.Exit(); //Forces the custom console to close.

        }

        /// <summary>
        /// How to set up ADL.
        /// </summary>
        private static void CreateADLConfig()
        {
            Debug.SetAllPrefixes("[General]", "[Log]", "[Warning]", "[Error]", "[Fatal]", "[ADL]");
            Debug.SendWarnings = false;
            Debug.ADLWarningMask = 4;
            Debug.AddPrefixForMask(new BitMask(true), "[GLOBAL]");
            Debug.ADLEnabled = false;
            Debug.SendUpdateMessageOnFirstLog = true;
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
            config.FrameTime = 50;
            config.ColorCoding = colorCoding;
            ADL.CustomCMD.CMDUtils.SaveConfig(config);
        }
    }
}
