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
        public static string[] _DebugLevel = new string[0];

        [Tooltip("Configuration of ADL")] public UnityADLConfig Configuration = new UnityADLConfig();

        [Tooltip("On What prefixes should the unity console log an Error")] [EnumFlagsAttribute]
        public int ConsoleErrorMask = 0;

        [Tooltip("Contains the Parameters for the Unity Console.")]
        public LogStreamParams ConsoleParams;

        [Tooltip("On What prefixes should the unity console log a Warning")] [EnumFlagsAttribute]
        public int ConsoleWarningMask = 0;

        [Tooltip("Configuration of ADL")]
        public UnityADLCustomConsoleConfig CustomCMDConfig = new UnityADLCustomConsoleConfig();


        [Tooltip("The streams that get hooked up to the debug when the game starts")]
        public LogStreamParams[] Streams;

        [Tooltip("Should the Debug Logs hook up to the unity console?")]
        public bool UseConsole = true;

        private void Awake()
        {
            Configuration.Prepare();
            CustomCMDConfig.Prepare();
            Debug.LoadConfig(Configuration);


            DontDestroyOnLoad(gameObject);
            foreach (var lsp in Streams)
            {
                LogStream ls;

                if (lsp.CreateCustomConsole)
                {
                    ls = lsp.ToLogStream(new PipeStream());
                    CMDUtils.CreateCustomConsoleNoReturn(ls.BaseStream as PipeStream,
                        CustomCMDConfig); // Currently not working due to referencing problems with my compiled code(using System.Windows.Forms)
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
                    CMDUtils.CreateCustomConsoleNoReturn(ls.BaseStream as PipeStream, CustomCMDConfig);
                }
                else
                {
                    UnityUtils.CreateUnityConsole(ConsoleParams, ConsoleWarningMask, ConsoleErrorMask);
                }
            }

            if (Debug.SendUpdateMessageOnFirstLog) CheckForUpdates();
        }

        private void OnDestroy()
        {
            Debug.RemoveAllOutputStreams();
        }


        public void CheckForUpdates()
        {
            var msg = UpdateDataObject.CheckUpdate(Assembly.GetExecutingAssembly().GetName().Name,
                Assembly.GetExecutingAssembly().GetName().Version);
            Debug.Log(Debug.UpdateMask, msg);
        }
    }
}