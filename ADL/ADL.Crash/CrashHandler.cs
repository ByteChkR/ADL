using System;
using System.Collections;
using System.Reflection;
using System.Text;
using ADL.Configs;

namespace ADL.Crash
{
    [Serializable]
    public class CrashConfig : AbstractAdlConfig
    {
        public int CrashMask;
        public override AbstractAdlConfig GetStandard()
        {
            return new CrashConfig()
            {
                CheckForUpdates = true
            };
        }
    }

    public static class CrashHandler
    {
        private static bool initialized = false;

        private static CrashConfig config = ConfigManager.GetDefault<CrashConfig>();

        public static void SaveCurrentConfig(string configPath = "adl_crash.xml")
        {
            ConfigManager.SaveToFile(configPath, config);
        }
        public static void Initialize(string configPath = "adl_crash.xml")
        {
            Initialize(ConfigManager.ReadFromFile<CrashConfig>(configPath));
        }
        public static void Initialize(CrashConfig config)
        {
            Initialize(config.CrashMask, config.CheckForUpdates);
        }
        public static void Initialize(BitMask crashMask, bool CheckUpdates = true)
        {
            config.CheckForUpdates = CheckUpdates;
            config.CrashMask = crashMask;

            if (CheckUpdates)
            {
                var msg = UpdateDataObject.CheckUpdate(typeof(CrashHandler));
                Debug.Log(Debug.UpdateMask, msg);
            }


            initialized = true;
        }

        public static void Log(Exception exception, BitMask crashNotes = null, bool includeInner = true)
        {
            if (!initialized)
            {
                Debug.Log(-1, "Crash handler was not initialized");
                return;
            }
            if (crashNotes != null)
            {
                Debug.Log(crashNotes, ExceptionHeader(exception));
            }

            Debug.Log(config.CrashMask, ExceptionToString(exception, includeInner));
        }

        private static string ExceptionHeader(Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\nException Logged: ");
            sb.Append(exception.GetType().FullName);
            if (exception.Message != null)
            {
                sb.Append("\nException Message: ");
                sb.Append(exception.Message);
            }
            if (exception.Source != null)
            {
                sb.Append("\nException Source: ");
                sb.Append(exception.Source);
            }
            return sb.ToString();
        }

        private static string ExceptionToString(Exception exception, bool includeInner)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\nException Type: ");
            sb.Append(exception.GetType().FullName);
            if (exception.Message != null)
            {
                sb.Append("\nException Message: ");
                sb.Append(exception.Message);
            }
            if (exception.Source != null)
            {
                sb.Append("\nException Source: ");
                sb.Append(exception.Source);
            }
            if (exception.HelpLink != null)
            {
                sb.Append("\nException Help Link: ");
                sb.Append(exception.HelpLink);
            }
            sb.Append("\nException HResult: ");
            sb.Append(exception.HResult.ToString());

            if (exception.StackTrace != null)
            {
                sb.Append("\nException Stacktrace: \n");
                sb.Append(exception.StackTrace);
            }

            if (exception.Data.Count != 0)
            {
                sb.Append("\nException Data:");
                foreach (DictionaryEntry dictionaryEntry in exception.Data)
                {
                    sb.Append("\n");
                    sb.Append(dictionaryEntry.Key);
                    sb.Append(":");
                    sb.Append(dictionaryEntry.Value.ToString());
                }
            }

            if (includeInner && exception.InnerException != null)
            {
                sb.Append("\nInner Exception:");
                sb.Append(ExceptionToString(exception.InnerException, true));
            }

            return sb.ToString();

        }




    }
}