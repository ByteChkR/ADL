using System;
using System.Collections;
using System.Text;

namespace ADL.Crash
{
    public static class CrashHandler
    {
        private static bool initialized = false;
        private static BitMask CrashMask;
        public static void Initialize(BitMask crashMask)
        {
            CrashMask = crashMask;
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

            Debug.Log(CrashMask, ExceptionToString(exception, includeInner));
        }

        private static string ExceptionHeader(Exception exception)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\nException Logged: ");
            sb.Append(exception.GetType().FullName);
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