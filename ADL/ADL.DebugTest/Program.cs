using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using ADL.Configs;
using ADL.CustomCMD;
using ADL.Network;
using ADL.Network.Streams;
using ADL.Streams;

namespace ADL.DebugTest
{
    /// <summary>
    ///     Test programm to verify.
    /// </summary>
    internal class Program
    {
        private static void TestConsoleOut()
        {
            //First we want to Specify what Messages we want to let trough the LogStream we are creating in a minute.

            //You have multiple ways to create a Bitmask without getting cancer from bitwise operations
            var bMaskWildCard = new BitMask(true); //Creates a Wildcard(Everything will be let through)
            var bMaskNone = new BitMask(); //Creates the opposite of Wildcard(Nothing will be let through)

            //There are also more readable ways to create a mask.
            //For example with enums. The Implementation will stay the same.
            var bMaskGenericWildcard = new BitMask<LoggingTypes>(true);
            var bMaskGenericNone = new BitMask<LoggingTypes>();

            //But you can do masks easier now.
            //This bitmask only lets through logs and errors
            var bMaskGenericCustom = new BitMask<LoggingTypes>(LoggingTypes.Error, LoggingTypes.Log);


            //Then we want to create a LogStream that receives the Messages
            //Important: Its much easier to use CreateLogStreamFromStream than setting everything manually
            LogStream logStream = new LogTextStream(
                Console.OpenStandardOutput(),
                bMaskGenericCustom, //Lets use the generic custom 
                MatchType.MatchOne, //We want to make the logs pass when there is at least one tag that is included in the filter.
                true //Get that fancy timestamp infront of the log.
            );

            Debug.AddOutputStream(logStream); //Now we have Created the stream, just add it to the system.


            //This is a test.
            for (var i = 1;
                i < 64;
                i++) //63 because its the highest value the current enum can take(every bit beeing 1)
            {
                var mask = i;
                //Debug.Log(mask, "Test with mask " + mask);
            }

            Debug.LogGen(LoggingTypes.Log, "Finished the Console Out Test.");

            //Now we want to remove the stream from the system.
            //Debug.RemoveOutputStream(logStream, true); //We want to remove a single one.
            //But we can remove all Streams in one go
            //Debug.RemoveAllOutputStreams(true);
        }

        private static void TestCustomConsoleOut()
        {
            //First we want to Specify what Messages we want to let trough the LogStream we are creating in a minute.

            //You have multiple ways to create a Bitmask without getting cancer from bitwise operations
            var bMaskWildCard = new BitMask(true); //Creates a Wildcard(Everything will be let through)
            var bMaskNone = new BitMask(); //Creates the opposite of Wildcard(Nothing will be let through)

            //There are also more readable ways to create a mask.
            //For example with enums. The Implementation will stay the same.
            var bMaskGenericWildcard = new BitMask<LoggingTypes>(true);
            var bMaskGenericNone = new BitMask<LoggingTypes>();

            //But you can do masks easier now.
            //This bitmask only lets through logs and errors
            var bMaskGenericCustom = new BitMask<LoggingTypes>(LoggingTypes.Error, LoggingTypes.Log);

            //We want to Create a PipeStream that our logstream is basing on(pipe streams are threadsave streams in a single sender/single receiver situation)
            var pipeStream = new PipeStream(); //Create a new instance


            //Then we want to create a LogStream that receives the Messages
            //Important: Its much easier to use CreateLogStreamFromStream than setting everything manually
            var logStream = new LogStream(
                pipeStream, //The Stream we want to send the Logs to.
                bMaskGenericWildcard, //Lets use the generic wildcard(you can set the mask dynamically when using a custom console.
                MatchType.MatchOne, //We want to make the logs pass when all tags are included in the filter.
                true //Get that fancy timestamp infront of the log.
            );
            //logStream.OverrideChannelTag = false;
            Debug.AddOutputStream(logStream); //Now we have Created the stream, just add it to the system.


            //After Creating the log Stream we want to create a custom Cmd window
            var ccmd
                = //ADL.CustomCMD.CMDUtils.CreateCustomConsole(pipeStream); //Creates a basic Custom cmd with no visual adjustments
                CmdUtils.CreateCustomConsole(pipeStream); //Creates a custom cmd with color coding and custom font size.

            //This is a test.
            for (var i = 1;
                i < 64;
                i++) //63 because its the highest value the current enum can take(every bit beeing 1)
            {
                var mask = i;
                //Debug.Log(mask, "Test with mask " + mask);
            }

            Debug.LogGen(LoggingTypes.Log, "Finished the CustomConsole Out Test.");

            //Now we want to remove the stream from the system.
            //Debug.RemoveOutputStream(logStream, true); //We want to remove a single one.
            //But we can remove all Streams in one go
            //Debug.RemoveAllOutputStreams(true);
        }

        private static void TestLogFileOut()
        {
            //First we want to Specify what Messages we want to let trough the LogStream we are creating in a minute.

            //You have multiple ways to create a Bitmask without getting cancer from bitwise operations
            var bMaskWildCard = new BitMask(true); //Creates a Wildcard(Everything will be let through)
            var bMaskNone = new BitMask(); //Creates the opposite of Wildcard(Nothing will be let through)

            //There are also more readable ways to create a mask.
            //For example with enums. The Implementation will stay the same.
            var bMaskGenericWildcard = new BitMask<LoggingTypes>(true);
            var bMaskGenericNone = new BitMask<LoggingTypes>();

            //But you can do masks easier now.
            //This bitmask only lets through logs and errors
            var bMaskGenericCustom = new BitMask<LoggingTypes>(LoggingTypes.Csvlogging);


            //Then we want to create a LogStream that receives the Messages
            //Important: Its much easier to use CreateLogStreamFromFile than setting everything manually
            LogStream logStream = new LogTextStream(new FileStream("test.log", FileMode.OpenOrCreate),
                bMaskGenericCustom //Get that fancy timestamp infront of the log.
            );
            //logStream.OverrideChannelTag = true; //Forces the LogStream to discard all Tag and timestamps and ONLY save the actual log.

            Debug.AddOutputStream(logStream); //Now we have Created the stream, just add it to the system.

            //This is a test.
            for (var i = 1;
                i < 64;
                i++) //63 because its the highest value the current enum can take(every bit beeing 1)
            {
                var mask = i;
                //Debug.Log(mask, "Test with mask " + mask);
            }

            Debug.LogGen(LoggingTypes.Log, "Finished the Logfile Out Test.");
            Console.WriteLine("Finished the Logfile Out Test.");

            //Now we want to remove the stream from the system.
            //Debug.RemoveOutputStream(logStream, true); //We want to remove a single one.
            //But we can remove all Streams in one go
            //Debug.RemoveAllOutputStreams(true);

            //In this case we want to delete the created log file to prevent logs piling up in the test project.
            //System.IO.File.Delete("test.log");
        }

        //Server
        private static void TestNetworkListen(bool mt)
        {
            //NetworkConfig.Save("adl_network_config.xml", new NetworkConfig());
            var nwl = new NetworkListener(100, "adl_network_config.xml", mt);
            nwl.Start();
        }


        private static void TestNetworkOut()
        {
            var nc = NetworkConfig.Load("adl_network_config.xml");
            var lts = NetLogStream.CreateNetLogStream(nc, 1, Assembly.GetExecutingAssembly().GetName().Version);

            if (lts == null)
            {
                Debug.Log(-1, "No Server found on that port");
                return;
            }

            Debug.Log(-1, "Adding Stream.");
            Debug.AddOutputStream(lts);

            for (var i = 0; i < 100; i++)
            for (var j = 1;
                j < 64;
                j++) //63 because its the highest value the current enum can take(every bit beeing 1)
                Debug.Log(j, "Net Test with mask " + j);
        }


        private static void Main(string[] args)
        {
            //CreateADLConfig();
            //CreateADLCustomCMDConfig();
            Debug.LoadConfig(); //Using the standard path
            //Runtime Config Changes(not Getting saved):
            Debug.SendWarnings = true;
            Debug.SendUpdateMessageOnFirstLog = true;

            Debug.AdlEnabled = true;
            Debug.PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable |
                                     PrefixLookupSettings.Deconstructmasktofind |
                                     PrefixLookupSettings
                                         .Bakeprefixes; //If you have int.minvalue to int.maxvalue channels this is not really advisable. (Config files can be bloated by baked prefixes thus getting a huge size.)

            //DataObject<float> testf = new DataObject<float>(1, MatchType.MATCH_ONE);
            //DataObject<string> tests = new DataObject<string>(1, MatchType.MATCH_ONE);
            //testf.Add(1.234f);
            //tests.Add("Hwllo");


            //DataObject obj = testf.Cast();

            //byte[] buf = DataObject.Serialize(obj);

            //DataObject<float> r = DataObject.Deserialize<float>(buf);
            //buf = DataObject.Serialize(tests.Cast());
            //DataObject<string> r1 = DataObject.Deserialize(buf).Cast<string>();

            //TestCustomConsoleOut();

            //TestConsoleOut();
            if (args.Length > 0)
            {
                if (args[0] == "-local")
                {
                    TestNetworkListen(true);
                    while (true)
                    {
                        Console.ReadLine();
                        TestNetworkOut();
                    }
                }

                if (args[0] == "-server")
                    TestNetworkListen(false);
                else if (args.Length > 1 && args[0] == "-client")
                    TestNetworkOut();
                else
                    while (true)
                    {
                        Console.ReadLine();
                        TestNetworkOut();
                    }
            }

            Console.Read();


            //TestLogFileOut();

            var rnd = new Random();
            float avg = 0;
            var sw = new Stopwatch();

            for (var i = 0; i < 100000; i++)
            {
                Thread.Sleep(50);
                sw.Start();
                Debug.LogGen(LoggingTypes.Log, rnd.NextDouble().ToString(CultureInfo.CurrentCulture));
                sw.Stop();
                var msLastTime = sw.ElapsedTicks;
                avg = (avg + msLastTime) / 2;
                sw.Reset();
            }


            //System.Windows.Forms.Application.Exit(); //Forces the custom console to close.
        }

        /// <summary>
        ///     How to set up ADL.
        /// </summary>
        private static void CreateAdlConfig()
        {
            Debug.SetAllPrefixes("[General]", "[Log]", "[Warning]", "[Error]", "[Fatal]", "[ADL]");
            Debug.SendWarnings = false;
            Debug.AdlWarningMask = 4;
            Debug.AddPrefixForMask(new BitMask(true), "[GLOBAL]");
            Debug.AdlEnabled = false;
            Debug.SendUpdateMessageOnFirstLog = true;
            Debug.UpdateMask = 32;
            Debug.PrefixLookupMode = PrefixLookupSettings.Addprefixifavailable;
            Debug.SaveConfig(); //Not Needed to work, but for the next time we can just load the config
        }

        private static void CreateAdlCustomCmdConfig()
        {
            var colorCoding =
                new SerializableDictionary<int, SerializableColor>(
                    new Dictionary<int, SerializableColor>
                    {
                        {8, Color.Red}, //Every errror message should be drawn in red.
                        {4, Color.Orange}, //Every warning is painted in orange
                        {32, Color.Green}
                    });
            var config = AdlCustomConsoleConfig.Standard;
            config.FontSize = 13;
            config.FrameTime = 50;
            config.ColorCoding = colorCoding;
            CmdUtils.SaveConfig(config);
        }

        private enum LoggingTypes
        {
            General = 1,
            Log = 2,
            Warning = 4,
            Error = 8,
            Fatal = 16,
            Adl = 32,
            Csvlogging = 64
        }
    }
}