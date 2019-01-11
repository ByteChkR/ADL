using System.Threading;
using UnityEngine;
using System.Reflection;
using ADL.Streams;
using ADL.Configs;
namespace ADL.Unity
{
    /// <summary>
    /// Component to set up and use ADL in unity
    /// </summary>
    public sealed class DebugComponent : MonoBehaviour
    {
        [Tooltip("Configuration of ADL")]
        public ADLConfig Configuration = ADLConfig.Standard;

        [Tooltip("Configuration of ADL")]
        public ADLCustomConsoleConfig CustomCMDConfig = ADLCustomConsoleConfig.Standard;


        [Tooltip("The streams that get hooked up to the debug when the game starts")]
        public LogStreamParams[] Streams;
        [Tooltip("The Debug levels(Enum like)")]
        public string[] DebugLevel;
        public static string[] _DebugLevel = new string[0];
        [Tooltip("Should the Debug Logs hook up to the unity console?")]
        public bool UseConsole = true;
        [Tooltip("Contains the Parameters for the Unity Console.")]
        public LogStreamParams ConsoleParams;

        [Tooltip("On What prefixes should the unity console log a Warning")]
        [EnumFlagsAttribute] public int ConsoleWarningMask = 0;
        [Tooltip("On What prefixes should the unity console log an Error")]
        [EnumFlagsAttribute] public int ConsoleErrorMask = 0;
        void Awake()
        {
            Debug.LoadConfig(Configuration);
            

            DontDestroyOnLoad(gameObject);
            Debug.SetAllPrefixes(DebugLevel);
            foreach (LogStreamParams lsp in Streams)
            {
                LogStream ls = lsp.ToLogStream();
                Debug.AddOutputStream(ls);
                if (lsp.CreateCustomConsole)
                {
                    ADL.CustomCMD.CMDUtils.CreateCustomConsoleNoReturn(ls.BaseStream as PipeStream, CustomCMDConfig);// Currently not working due to referencing problems with my compiled code(using System.Windows.Forms)
                    //Apparently Unity Editor dll loading capabilities were never meant to load system resources.(The error is that the windows forms code is not able to find System.Runtime.Interopservices.Marshal.ReadInt16)
                    //Probably dumb mistake by me. Otherwise i manage to poke some super old 16 bit code that is not supported on my 64bit machine.
                }
            }
            if (UseConsole)
            {
                UnityUtils.CreateUnityConsole(ConsoleParams, ConsoleWarningMask, ConsoleErrorMask);
            }
            if (Debug.SendUpdateMessageOnFirstLog)
            {
                CheckForUpdates();
            }

        }

        void OnDestroy()
        {
            Debug.RemoveAllOutputStreams();
        }


        public void CheckForUpdates()
        {
            string msg = UpdateDataObject.CheckUpdate(Assembly.GetExecutingAssembly().GetName().Name, Assembly.GetExecutingAssembly().GetName().Version);
            Debug.Log(Debug.UpdateMask, msg);
        }


    }
}
