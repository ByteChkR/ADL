using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using ADL;
using ADL.Configs;

namespace ADL.Network.Server
{
    /// <summary>
    /// The Command line server console for the NetworkListener.
    /// </summary>
    public class ServerConsole
    {

        private static bool dontsearchUpdates = false;
        /// <summary>
        /// The Delegate used to create the different commands.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        public delegate int Command(int start, params string[] cmds);

        /// <summary>
        /// The network listener that gets manipulated.
        /// </summary>
        private static NetworkListener nl;

        /// <summary>
        /// List of commands
        /// </summary>
        public static Dictionary<string, Command> Commands = new Dictionary<string, Command>()
        {
            { "start",  CMD_Start},
            { "stop",  CMD_Stop},
            { "exit",  CMD_Exit},
            { "help",  CMD_Help},
            { "setmillis",  CMD_SetMillis},
            { "setport",  CMD_SetPort},
            { "settimeformat",  CMD_SetTimeFormat},
            { "setdebug", CMD_SetDebugMode },
            { "init",  CMD_Init},
            { "saveconfig" , CMD_SaveConfig},

        };

        static int CMD_SaveConfig(int start, params string[] cmds)
        {
            if (cmds.Length > start + 1 && nl != null && cmds[start + 1].StartsWith("c:"))
            {

                string confPath = cmds[start + 1].Substring(2);

                Debug.Log(0, "Saving Server config to path: " + confPath);
                try
                {
                    NetworkServerConfig.Save(confPath, nl.Config);

                }
                catch (Exception e)
                {

                    Debug.Log(0, "Invalid path: " + confPath + "\n" + e.ToString());

                }
                return 2;
            }
            else
            {

                Debug.Log(0, "Server not initialized or invalid path");
                return 1;
            }
        }


        /// <summary>
        /// type "setdebug" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static int CMD_SetDebugMode(int start, params string[] cmds)
        {
            if (cmds.Length > start + 1 && nl != null)
            {
                nl.DebugNetworking = cmds[start + 1] == "true" || cmds[start + 1] == "1";
                Debug.Log(0, "Set Debug Mode to: " + nl.DebugNetworking);
                return 2;
            }
            else if (cmds.Length > start + 1 && nl == null)
            {
                CMD_Init(start, cmds);
                CMD_SetDebugMode(start, cmds);
                return 2;
            }
            else
            {
                CMD_Help(start, cmds);
                return 1;
            }
        }

        /// <summary>
        /// type "init" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static int CMD_Init(int start, params string[] cmds)
        {
            int cmdsConsumed = 1;
            string configPath = "";
            if (cmds.Length > start + 1 && cmds[start + 1].StartsWith("c:"))
            {
                cmdsConsumed++;
                if (nl == null || nl != null && !nl.isStarted)
                {
                    configPath = cmds[start + 1].Substring(2);
                    nl = new NetworkListener(50, configPath, true, dontsearchUpdates);
                    dontsearchUpdates = true;
                    Debug.Log(0, "Initialized Sucessfully.");
                }
                else
                    Debug.Log(0, "Server is started. can not initialize while the server is running.");



            }
            else
            {
                if (nl == null || nl != null && !nl.isStarted)
                {
                    nl = new NetworkListener(50, "", true, dontsearchUpdates);
                    dontsearchUpdates = true;
                    Debug.Log(0, "Initialized Sucessfully.");
                }
                else
                    Debug.Log(0, "Server is started. can not initialize while the server is running.");
            }
            return cmdsConsumed;

        }


        /// <summary>
        ///  type "start" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static int CMD_Start(int start, params string[] cmds)
        {
            if (nl != null && !nl.isStarted) nl.Start();
            else if (nl == null)
            {
                CMD_Init(start, cmds);
                CMD_Start(start, cmds);
            }
            else
            {
                Debug.Log(0, "Server Already Started.");
            }
            Thread.Sleep(100);
            return 1;

        }

        /// <summary>
        /// type "stop" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static int CMD_Stop(int start, params string[] cmds)
        {
            if (nl != null && nl.isStarted) nl.Stop();
            else
                Debug.Log(0, "Server Already Stopped.");

            Thread.Sleep(100);
            return 1;
        }


        /// <summary>
        /// type "exit" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static int CMD_Exit(int start, params string[] cmds)
        {
            run = false;
            return 1;
        }

        /// <summary>
        /// type "setmillis" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static int CMD_SetMillis(int start, params string[] cmds)
        {
            if (cmds.Length > start + 1 && nl != null && uint.TryParse(cmds[start + 1], out uint val))
            {
                if (nl.isStarted)
                {
                    CMD_Stop(start, cmds);
                    nl.RefreshMillis = (int)val;
                    CMD_Start(start, cmds);
                }
                else nl.RefreshMillis = (int)val;
                Debug.Log(0, "Set RefreshMillis to: " + val);
                return 2;
            }
            else if (cmds.Length > start + 1 && nl == null)
            {
                CMD_Init(start, cmds);
                CMD_SetMillis(start, cmds);
                return 2;
            }
            else
            {
                CMD_Help(start, cmds);
                return 1;
            }
        }

        /// <summary>
        /// type "setport" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static int CMD_SetPort(int start, params string[] cmds)
        {
            if (cmds.Length > start + 1 && nl != null && uint.TryParse(cmds[start + 1], out uint val))
            {
                if (nl.isStarted)
                {
                    CMD_Stop(start, cmds);
                    nl.Port = (int)val;
                    CMD_Start(start, cmds);
                }
                else nl.Port = (int)val;

                Debug.Log(0, "Set Port to: " + val);
                return 2;
            }
            else if (cmds.Length > start + 1 && nl == null)
            {
                CMD_Init(start, cmds);
                CMD_SetPort(start, cmds);
                return 2;
            }
            else
            {
                CMD_Help(start, cmds);
                return 1;
            }
        }

        /// <summary>
        /// type "settimeformat" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static int CMD_SetTimeFormat(int start, params string[] cmds)
        {
            if (cmds.Length > start + 1 && nl != null)
            {
                if (nl.isStarted)
                {
                    CMD_Stop(start, cmds);
                    Debug.TimeFormatString = cmds[start + 1];
                    CMD_Start(start, cmds);
                }
                else Debug.TimeFormatString = cmds[start + 1];

                Debug.Log(0, "Set RefreshMillis to: " + cmds[1]);
                return 2;
            }
            else if (cmds.Length > start + 1 && nl == null)
            {
                CMD_Init(start, cmds);
                CMD_SetTimeFormat(start, cmds);
                return 2;
            }
            else
            {
                CMD_Help(start, cmds);
                return 1;
            }
        }

        /// <summary>
        /// type "help" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static int CMD_Help(int start, params string[] cmds)
        {
            Debug.Log(0, "init <c:configpath> - initialize server with optional settings.");
            Debug.Log(0, "start - start listener");
            Debug.Log(0, "stop - stop listener");
            Debug.Log(0, "help - display this text");
            Debug.Log(0, "exit - exit console");
            Debug.Log(0, "setmillis <millis> - sets millisecond refresh timer");
            Debug.Log(0, "setport <port> - set port of listener");
            Debug.Log(0, "timeformat <format> - sets the time format string");
            Debug.Log(0, "setdebug <true|false>");
            Debug.Log(0, "saveconf <c:configpath> - saves the config of the server to the specified path");
            return 1;
        }

        /// <summary>
        /// Console Wrapper to Call commands.
        /// </summary>
        /// <param name="cmd"></param>
        public static void RunCommand(string cmd)
        {
            string[] cmds = cmd.Split(' ');


            for (int i = 0; i < cmds.Length;)
            {
                if (!Commands.ContainsKey(cmds[i]))
                {
                    Debug.Log(0, "Invalid Command: " + cmds[i]);
                    CMD_Help(0, null);
                    i++;
                    continue;
                }
                i += Commands[cmds[i]](i, cmds);
            }

            //if (Commands.ContainsKey(cmds[0]))
            //{
            //    Commands[cmds[0]](0, cmds);
            //}
            //else
            //{
            //    Debug.Log(0, "Command not recognized.");
            //    CMD_Help(0, cmds);
            //}
        }


        /// <summary>
        /// Flag if the server loop should continue
        /// </summary>
        static bool run = true;

        /// <summary>
        /// The server loop that can be used to control the program in a command line.
        /// </summary>
        public static void RunServer()
        {
            ADL.Streams.LogTextStream lts = new ADL.Streams.LogTextStream(Console.OpenStandardOutput(), 0, MatchType.MatchAll, true);
            Debug.AddOutputStream(lts);

            while (run)
            {
                Console.Write(">");
                string cmd = Console.ReadLine().ToLower();
                RunCommand(cmd);
            }

            if (nl != null) nl.Stop();
        }


        public ServerConsole()
        {
            RunServer();
        }


        /// <summary>
        /// Entry point for the Server Executable
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            ServerConsole sc = new ServerConsole();
        }
    }
}
