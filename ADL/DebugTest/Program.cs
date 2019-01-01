using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ADL.Unity;
using ADL;
namespace DebugTest
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
            GENERICTEST = 32
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
            LogStream logStream = LogStream.CreateLogStreamFromStream(
                Console.OpenStandardOutput(), //The Stream we want to send the Logs to.
                bMaskGenericCustom, //Lets use the generic custom 
                MatchType.MATCH_ONE, //We want to make the logs pass when there is at least one tag that is included in the filter.
                true //Get that fancy timestamp infront of the log.
                );

            Debug.AddOutputStream(logStream); //Now we have Created the stream, just add it to the system.

            //Maybe we even want to have Custom Tags on the logs, depending what mask they have.
            Debug.SetAllPrefixes("[General]", "[Log]", "[Warning]", "[Error]", "[Fatal]", "[GENERIC]");

            //This is a test.
            for (int i = 0; i < 63; i++) //63 because its the highest value the current enum can take(every bit beeing 1)
            {
                int mask = i;
                Debug.Log(mask, "Test with mask " + mask);
            }

            Debug.Log<LoggingTypes>(LoggingTypes.LOG, "Finished the Console Out Test.");

            //Now we want to remove the stream from the system.
            Debug.RemoveOutputStream(logStream, true); //We want to remove a single one.
            //But we can remove all Streams in one go
            Debug.RemoveAllOutputStreams(true);

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
            LogStream logStream = LogStream.CreateLogStreamFromStream(
                pipeStream, //The Stream we want to send the Logs to.
                bMaskGenericWildcard, //Lets use the generic wildcard(you can set the mask dynamically when using a custom console.
                MatchType.MATCH_ONE, //We want to make the logs pass when all tags are included in the filter.
                true //Get that fancy timestamp infront of the log.
                );

            Debug.AddOutputStream(logStream); //Now we have Created the stream, just add it to the system.



            Dictionary<string, System.Drawing.Color> colorCoding = new Dictionary<string, System.Drawing.Color>()
            {
                {"[Error]", System.Drawing.Color.Red }, //Every errror message should be drawn in red.
                {"[Warning]", System.Drawing.Color.Orange } //Every warning is painted in orange
            };

            //After Creating the log Stream we want to create a custom Cmd window
            System.Windows.Forms.Form ccmd = //ADL.CustomCMD.CMDUtils.CreateCustomConsole(pipeStream); //Creates a basic Custom cmd with no visual adjustments
                ADL.CustomCMD.CMDUtils.CreateCustomConsole(pipeStream, colorCoding, 13); //Creates a custom cmd with color coding and custom font size.



            //Maybe we even want to have Custom Tags on the logs, depending what mask they have.
            Debug.SetAllPrefixes("[General]", "[Log]", "[Warning]", "[Error]", "[Fatal]", "[GENERIC]");

            //This is a test.
            for (int i = 0; i < 63; i++) //63 because its the highest value the current enum can take(every bit beeing 1)
            {
                int mask = i;
                Debug.Log(mask, "Test with mask " + mask);
            }

            Debug.Log<LoggingTypes>(LoggingTypes.LOG, "Finished the CustomConsole Out Test.");

            //Now we want to remove the stream from the system.
            Debug.RemoveOutputStream(logStream, true); //We want to remove a single one.
            //But we can remove all Streams in one go
            Debug.RemoveAllOutputStreams(true);
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
            BitMask<LoggingTypes> bMaskGenericCustom = new BitMask<LoggingTypes>(LoggingTypes.ERROR, LoggingTypes.LOG);


            //Then we want to create a LogStream that receives the Messages
            //Important: Its much easier to use CreateLogStreamFromFile than setting everything manually
            LogStream logStream = LogStream.CreateLogStreamFromFile(
                "test.log", //The Stream we want to send the Logs to.(in this case the file name)
                bMaskGenericCustom, //Lets use the generic custom 
                MatchType.MATCH_ONE, //We want to make the logs pass when there is at least one tag that is included in the filter.
                true, //Get that fancy timestamp infront of the log.
                true //We append the log if it already exists.
                );

            Debug.AddOutputStream(logStream); //Now we have Created the stream, just add it to the system.


            //Maybe we even want to have Custom Tags on the logs, depending what mask they have.
            Debug.SetAllPrefixes("[General]", "[Log]", "[Warning]", "[Error]", "[Fatal]", "[GENERIC]");

            //This is a test.
            for (int i = 0; i < 63; i++) //63 because its the highest value the current enum can take(every bit beeing 1)
            {
                int mask = i;
                Debug.Log(mask, "Test with mask " + mask);
            }

            Debug.Log<LoggingTypes>(LoggingTypes.LOG, "Finished the Logfile Out Test.");
            Console.WriteLine("Finished the Logfile Out Test.");

            //Now we want to remove the stream from the system.
            Debug.RemoveOutputStream(logStream, true); //We want to remove a single one.
            //But we can remove all Streams in one go
            Debug.RemoveAllOutputStreams(true);

            //In this case we want to delete the created log file to prevent logs piling up in the test project.
            System.IO.File.Delete("test.log");
        }



        static void Main(string[] args)
        {
            TestConsoleOut();

            TestCustomConsoleOut();

            TestLogFileOut();

            Console.Read();

            System.Windows.Forms.Application.Exit(); //Forces the custom console to close.

        }
    }
}
