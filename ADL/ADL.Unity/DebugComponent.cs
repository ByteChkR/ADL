using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using System.IO;
using System.Diagnostics;
namespace ADL.Unity
{
    public sealed class DebugComponent : MonoBehaviour
    {
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
        [EnumFlagsAttribute] public int ConsoleWarningMask = -1;
        [Tooltip("On What prefixes should the unity console log an Error")]
        [EnumFlagsAttribute] public int ConsoleErrorMask = -1;
        private Thread _consoleThread = null;
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Debug.SetAllPrefixes(DebugLevel);
            foreach (LogStreamParams lsp in Streams)
            {
                Debug.AddOutputStream(lsp.ToLogStream());
            }
            if (UseConsole)
            {
                SetUpConsole();
            }

        }

        void OnDestroy()
        {
            Debug.RemoveAllOutputStreams();
            if (_consoleThread != null) _consoleThread.Abort();
        }

        void SetUpConsole()
        {
            UnityTextWriter utw = new UnityTextWriter(ConsoleWarningMask, ConsoleErrorMask);
            Debug.AddOutputStream(ConsoleParams.ToLogStream(utw));
        }


    }
}
