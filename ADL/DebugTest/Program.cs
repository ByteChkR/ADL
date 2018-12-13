using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ADL.Unity;


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

        static void Main(string[] args)
        {

            

            ADL.Debug.SetAllPrefixes(new string[] { "[General]", "[Log]", "[Warning]", "[Error]", "[Fatal]", "[GENERIC]" });

            ADL.LogStream ResultLog = ADL.LogStream.CreateLogStreamFromFile("benchmark.log", 0, true, true, true);

            ADL.Debug.AddOutputStream(ResultLog);

            for (int SampleSize = 0; SampleSize < 15; SampleSize++)
            {

                SetUpConsole(131072, 4096);


            }

            ADL.Debug.RemoveAllOutputStreams();

            ResultLog = ADL.LogStream.CreateLogStreamFromFile("benchmark.log", 0, true,  true, true);

            ADL.Debug.AddOutputStream(ResultLog);

            for (int SampleSize = 0; SampleSize < 15; SampleSize++)
            {

                SetUpLogFile(131072, 4096);
            }

            ADL.Debug.RemoveAllOutputStreams();

            Console.WriteLine("DebugStreams Closed");

            System.IO.Directory.GetFiles(".\\", "log*.log").ToList().ForEach(x => System.IO.File.Delete(x));

            Console.ReadLine();
        }

        static void SetUpLogFile(int WriteSize, int TestAmount)
        {
            int Mask = ADL.Utils.CombineMasks(); //WIldcard will receive all messages
            ADL.LogStream logStream = ADL.LogStream.CreateLogStreamFromFile(
                "log" + ADL.Debug.ListeningStreams + ".log", // The file
                Mask, //The Mask
                true, //If true = MatchAll if false = MatchOne
                true,//Timestamp?
                false);//If file exists, should the log just append?

            ADL.Debug.AddOutputStream(logStream);


            //Testing:
            Random rnd = new Random((int)DateTime.Now.Ticks);


            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            for (int i = 0; i < TestAmount; i++)
            {
                int r = rnd.Next(1, 64);
                ADL.Debug.Log(r, "File Log Test nr. " + i);
            }

            sw.Stop();


            ADL.Debug.Log(-1, "Time for " + TestAmount + " Logs @ "+ADL.Debug.ListeningStreams+" File Stream(" + WriteSize + ") + Random " + "Ticks(Millis): " + sw.ElapsedTicks + "(" + sw.ElapsedMilliseconds + ")");


            //ADL.Debug.RemoveOutputStream(logStream);
        }

        static void SetUpConsole(int WriteSize, int TestAmount)
        {
            int Mask = ADL.Utils.CombineMasks(); //WIldcard will receive all messages
            ADL.LogStream logStream = ADL.LogStream.CreateLogStreamFromStream(
                Console.Out, // The stream
                Mask, //The Mask
                true, //If true = MatchAll if false = MatchOne
                true); //Timestamp?
            ADL.Debug.AddOutputStream(logStream);


            //Testing:
            Random rnd = new Random((int)DateTime.Now.Ticks);


            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            for (int i = 0; i < TestAmount; i++)
            {
                int r = rnd.Next(1, 64);
                ADL.Debug.Log(r, "Console Log Test nr. " + i);
            }

            sw.Stop();


            ADL.Debug.Log(-1, "Time for " + TestAmount + " Logs @ " + ADL.Debug.ListeningStreams + " Console Stream(" + WriteSize + ") + Random " + "Ticks(Millis): " + sw.ElapsedTicks + "(" + sw.ElapsedMilliseconds + ")");

            //ADL.Debug.RemoveOutputStream(logStream);

        }

    }
}
