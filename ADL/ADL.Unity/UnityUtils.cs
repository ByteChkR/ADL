namespace ADL.Unity
{
    /// <summary>
    /// Static functions.
    /// </summary>
    public static class UnityUtils
    {


        public static readonly string VersionURL = "https://raw.githubusercontent.com/ByteChkR/ADL/master/docs/versioning/ADL.Unityversion.txt";

        /// <summary>
        /// Creates a Unity Text Writer and uses some custom code to insert it into the ADL.Debug system without an underlying stream
        /// </summary>
        /// <param name="param">Log Stream Parameter</param>
        /// <param name="WarningMask">Warning Mask(What is a warning)</param>
        /// <param name="ErrorMask">Error Mask (What is an error)</param>
        public static void CreateUnityConsole(LogStreamParams param, int WarningMask = 0, int ErrorMask = 0)
        {
            UnityTextWriter utw = new UnityTextWriter(WarningMask, ErrorMask);
            Debug.AddOutputStream(ToUnityConsoleLogStream(utw, param));
        }

        /// <summary>
        /// Creates A Log Stream based on a UnityTextWriter.
        /// </summary>
        /// <param name="utw">TextWriter</param>
        /// <param name="param">Parameters to create the Log Stream.</param>
        /// <returns></returns>
        public static LogStream ToUnityConsoleLogStream(UnityTextWriter utw, LogStreamParams param)
        {

            LogStream ls = new LogStream(utw)
            {
                Mask = param.Mask,
                MatchType = param.MatchType,
                PrependTimeStamp = param.SetTimeStamp
            };
            return ls;
        }
    }
}
