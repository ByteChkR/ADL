using System.Reflection;
using ADL.CustomCMD;
using ADL.Streams;
using ADL.Unity.UnityConfig;
using UnityEngine;

namespace ADL.Unity
{
    /// <summary>
    ///     Component to set up and use ADL in unity
    /// </summary>
    public class DebugComponent : MonoBehaviour
    {
        public static string[] DebugLevel = new string[0];

        [Tooltip("Configuration of ADL")] public UnityAAdlConfig Configuration = new UnityAAdlConfig();

        [Tooltip("On What prefixes should the unity console log an Error")] [EnumFlagsAttribute]
        public int ConsoleErrorMask = 0;

        [Tooltip("Contains the Parameters for the Unity Console.")]
        public LogStreamParams ConsoleParams;

        [Tooltip("On What prefixes should the unity console log a Warning")] [EnumFlagsAttribute]
        public int ConsoleWarningMask = 0;

        [Tooltip("Configuration of ADL")]
        public UnityAdlCustomConsoleConfig CustomCmdConfig = new UnityAdlCustomConsoleConfig();


        [Tooltip("The streams that get hooked up to the debug when the game starts")]
        public LogStreamParams[] Streams;

        [Tooltip("Should the Debug Logs hook up to the unity console?")]
        public bool UseConsole = true;

        private void Awake()
        {
            Configuration.Prepare();
            CustomCmdConfig.Prepare();
            Debug.LoadConfig(Configuration);


            DontDestroyOnLoad(gameObject);
            foreach (var lsp in Streams)
            {
                LogStream ls;

                if (lsp.CreateCustomConsole)
                {
                    ls = lsp.ToLogStream(new PipeStream());
                    CmdUtils.CreateCustomConsoleNoReturn(ls.PBaseStream as PipeStream,
                        CustomCmdConfig); // Currently not working due to referencing problems with my compiled code(using System.Windows.Forms)
                    //Apparently Unity Editor dll loading capabilities were never meant to load system resources.(The error is that the windows forms code is not able to find System.Runtime.Interopservices.Marshal.ReadInt16)
                    //Probably dumb mistake by me. Otherwise i manage to poke some super old 16 bit code that is not supported on my 64bit machine.
                }
                else
                {
                    ls = lsp.ToLogStream();
                }

                Debug.AddOutputStream(ls);
            }

            if (UseConsole)
            {
                if (ConsoleParams.CreateCustomConsole)
                {
                    var ls = ConsoleParams.ToLogStream(new PipeStream());
                    Debug.AddOutputStream(ls);
                    CmdUtils.CreateCustomConsoleNoReturn(ls.PBaseStream as PipeStream, CustomCmdConfig);
                }
                else
                {
                    UnityUtils.CreateUnityConsole(ConsoleParams, ConsoleWarningMask, ConsoleErrorMask);
                }
            }

            if (Debug.CheckForUpdates) CheckForUpdates();
        }

        private void OnDestroy()
        {
            Debug.RemoveAllOutputStreams();
        }


        public void CheckForUpdates()
        {
            var msg = UpdateDataObject.CheckUpdate(typeof(DebugComponent));
            Debug.Log(Debug.UpdateMask, msg);
        }
    }
}