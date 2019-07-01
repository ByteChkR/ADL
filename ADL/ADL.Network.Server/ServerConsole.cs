using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ADL;

namespace ADL.Network.Server
{
    /// <summary>
    /// The Command line server console for the NetworkListener.
    /// </summary>
    public class ServerConsole
    {
        /// <summary>
        /// The Delegate used to create the different commands.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        public delegate void Command(int start, params string[] cmds);

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

        };

        /// <summary>
        /// type "setdebug" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static void CMD_SetDebugMode(int start, params string[] cmds)
        {
            if (cmds.Length > 1 && nl != null)
            {
                nl.DebugNetworking = cmds[1] == "true" || cmds[1] == "1";
                Debug.Log(0, "Set Debug Mode to: " + nl.DebugNetworking);
            }
            else if (cmds.Length > 1 && nl == null)
            {
                CMD_Init(0, cmds);
                CMD_SetDebugMode(0, cmds);
            }
            else CMD_Help(0, cmds);
        }

        /// <summary>
        /// type "init" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static void CMD_Init(int start, params string[] cmds)
        {
            if (nl == null || nl != null && !nl.isStarted)
            {
                nl = new NetworkListener(50);
                Debug.Log(0, "Initialized Sucessfully.");
            }
            else
                Debug.Log(0, "Server is started. can not initialize while the server is running.");
        }


        /// <summary>
        ///  type "start" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static void CMD_Start(int start, params string[] cmds)
        {
            if (nl != null && !nl.isStarted) nl.Start();
            else if (nl == null)
            {
                CMD_Init(0, cmds);
                CMD_Start(0, cmds);
            }
            else
            {
                Debug.Log(0, "Server Already Started.");
            }

        }

        /// <summary>
        /// type "stop" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static void CMD_Stop(int start, params string[] cmds)
        {
            if (nl != null && nl.isStarted) nl.Stop();
            else
                Debug.Log(0, "Server Already Stopped.");
        }


        /// <summary>
        /// type "exit" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static void CMD_Exit(int start, params string[] cmds)
        {
            run = false;
        }

        /// <summary>
        /// type "setmillis" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static void CMD_SetMillis(int start, params string[] cmds)
        {
            if (cmds.Length > 1 && nl != null && uint.TryParse(cmds[1], out uint val))
            {
                if (nl.isStarted)
                {
                    CMD_Stop(0, cmds);
                    nl.RefreshMillis = (int)val;
                    CMD_Start(0, cmds);
                }
                else nl.RefreshMillis = (int)val;
                Debug.Log(0, "Set RefreshMillis to: " + val);
            }
            else if(cmds.Length>1 && nl == null)
            {
                CMD_Init(0, cmds);
                CMD_SetMillis(0, cmds);
            }
            else CMD_Help(0, cmds);
        }

        /// <summary>
        /// type "setport" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static void CMD_SetPort(int start, params string[] cmds)
        {
            if (cmds.Length > 1 && nl != null && uint.TryParse(cmds[1], out uint val))
            {
                if (nl.isStarted)
                {
                    CMD_Stop(0, cmds);
                    nl.Port = (int)val;
                    CMD_Start(0, cmds);
                }
                else nl.Port = (int)val;

                Debug.Log(0, "Set Port to: " + val);
            }
            else if (cmds.Length > 1 && nl == null)
            {
                CMD_Init(0, cmds);
                CMD_SetPort(0, cmds);
            }
            else CMD_Help(0, cmds);
        }

        /// <summary>
        /// type "settimeformat" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static void CMD_SetTimeFormat(int start, params string[] cmds)
        {
            if (cmds.Length > 1 && nl != null)
            {
                if (nl.isStarted)
                {
                    CMD_Stop(0, cmds);
                    Debug.TimeFormatString = cmds[1];
                    CMD_Start(0, cmds);
                }
                else Debug.TimeFormatString = cmds[1];

                Debug.Log(0, "Set RefreshMillis to: " + cmds[1]);
            }
            else if (cmds.Length > 1 && nl == null)
            {
                CMD_Init(0, cmds);
                CMD_SetTimeFormat(0, cmds);
            }
            else CMD_Help(0, cmds);
        }

        /// <summary>
        /// type "help" to call this function
        /// </summary>
        /// <param name="start"></param>
        /// <param name="cmds"></param>
        static void CMD_Help(int start, params string[] cmds)
        {
            Debug.Log(0, "start - start listener");
            Debug.Log(0, "stop - stop listener");
            Debug.Log(0, "help - display this text");
            Debug.Log(0, "exit - exit console");
            Debug.Log(0, "setmillis <millis> - sets millisecond refresh timer");
            Debug.Log(0, "setport <port> - set port of listener");
            Debug.Log(0, "timeformat <format> - sets the time format string");
            Debug.Log(0, "setdebug <true|false>");
        }

        /// <summary>
        /// Console Wrapper to Call commands.
        /// </summary>
        /// <param name="cmd"></param>
        public static void RunCommand(string cmd)
        {
            string[] cmds = cmd.Split(' ');
            if (Commands.ContainsKey(cmds[0]))
            {
                Commands[cmds[0]](0, cmds);
            }
            else
            {
                Debug.Log(0, "Command not recognized.");
                CMD_Help(0, cmds);
            }
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



        /// <summary>
        /// Entry point for the Server Executable
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            RunServer();
        }
    }
}
