using System;
using System.Linq;
using System.Net;
namespace ADL
{
    /// <summary>
    /// Helpful functions are stored here.
    /// </summary>
    public static class Utils
    {
        public static readonly string VersionURL = "https://raw.githubusercontent.com/ByteChkR/ADL/master/docs/versioning/ADLversion.txt";
        public static readonly int BYTE_SIZE = 8;

        /// <summary>
        /// Returns the Enum Size for the specified enum
        /// </summary>
        /// <param name="enumType">typeof(enum)</param>
        /// <returns>bitwise length of enum.</returns>
        public static int GetEnumSize(Type enumType)
        {
            int i = Enum.GetValues(enumType).Cast<int>().Max(); //Maximum Value (32 for LoggingTypes)
            return i + i - 1; //Actual Bitwise Maximal value. from 000000(0) to 111111(63)
        }


        /// <summary>
        /// Computes basis by the power of exp
        /// </summary>
        /// <param name="basis">basis</param>
        /// <param name="exp">exponent</param>
        /// <returns></returns>
        public static int IntPow(int basis, int exp)
        {
            if (exp == 0) return 1;
            int ret = basis;
            for (int i = 1; i < exp; i++)
            {
                ret *= basis;
            }
            return ret;
        }

        #region TimeStamp
        /// <summary>
        /// Current Time Stamp based on DateTime.Now
        /// </summary>
        public static string TimeStamp
        {
            get
            {
                return "[" + NumToTimeFormat(DateTime.Now.Hour) + ":" + NumToTimeFormat(DateTime.Now.Minute) + ":" + NumToTimeFormat(DateTime.Now.Second) + "]";
            }
        }

        /// <summary>
        /// Makes 1-9 => 01-09
        /// </summary>
        /// <param name="time">Integer</param>
        /// <returns></returns>
        public static string NumToTimeFormat(int time)
        {
            return (time < 10) ? "0" + time : time.ToString();
        }

        #endregion


        public static void CheckUpdate(out string msg, string PackageName, string URL, Version currentVer)
        {
            Version onlineVer;
            WebClient webCli = new WebClient();

            msg = "Checking For Updates with Current " + PackageName + " Version[" + currentVer.ToString() + "]...\n";
            try
            {
                onlineVer = new Version(webCli.DownloadString(URL));

                int updatesPending = onlineVer.CompareTo(currentVer);
                if (updatesPending == 0)
                {
                    msg += PackageName + " Version Check OK! Newest version installed. ";
                    return;
                }
                else if (updatesPending < 0)
                {
                    msg += "Version Check OK!. Current " + PackageName + " Version is higher than official release. ";
                    return;
                }
                else
                {
                    msg += "Update Available!. Current " + PackageName + " Version: " + currentVer.ToString() + "Online " + PackageName + " Version: " + onlineVer.ToString() + "";
                    return;
                }

            }
            catch (Exception)
            {
                msg += "Could not connect to " + URL + " Try again later or disable UpdateChecking flags in Package: " + PackageName + " to prevent checking for updates.";
                return;
            }

        }

    }
}
