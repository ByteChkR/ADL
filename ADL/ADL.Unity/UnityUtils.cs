using ADL.Streams;

namespace ADL.Unity
{
    /// <summary>
    ///     Static functions.
    /// </summary>
    public static class UnityUtils
    {
        /// <summary>
        ///     Creates a Unity Text Writer and uses some custom code to insert it into the ADL.Debug system without an underlying
        ///     stream
        /// </summary>
        /// <param name="param">Log Stream Parameter</param>
        /// <param name="warningMask">Warning Mask(What is a warning)</param>
        /// <param name="errorMask">Error Mask (What is an error)</param>
        public static void CreateUnityConsole(LogStreamParams param, int warningMask = 0, int errorMask = 0)
        {
            var utw = new UnityTextWriter(warningMask, errorMask);
            Debug.AddOutputStream(ToUnityConsoleLogStream(utw, param));
        }

        /// <summary>
        ///     Creates A Log Stream based on a UnityTextWriter.
        /// </summary>
        /// <param name="utw">TextWriter</param>
        /// <param name="param">Parameters to create the Log Stream.</param>
        /// <returns></returns>
        public static LogStream ToUnityConsoleLogStream(UnityTextWriter utw, LogStreamParams param)
        {
            var ls = new LogStream(utw, param.Mask, param.MatchType, param.SetTimeStamp);
            return ls;
        }
    }
}