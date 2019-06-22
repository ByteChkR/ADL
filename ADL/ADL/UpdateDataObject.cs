using System;
using System.Net;

namespace ADL
{
    public static class UpdateDataObject
    {
        /// <summary>
        /// The Raw link to the location where i have my version files.
        /// </summary>
        private static readonly string _rawLink =
            "https://raw.githubusercontent.com/ByteChkR/ADL/master/docs/versioning/{0}version.txt";

        /// <summary>
        /// Creates the right link from the raw link and the package name
        /// </summary>
        /// <param name="package">the package name</param>
        /// <returns>returns a valid link to the version file.</returns>
        public static string GenerateLink(string package)
        {
            return string.Format(_rawLink, package);
        }

        /// <summary>
        /// Checks For Updates for the specified package.
        /// </summary>
        /// <param name="PackageName">Package name to search for</param>
        /// <param name="currentVer">The current assembly version of the package</param>
        /// <returns></returns>
        public static string CheckUpdate(string PackageName, Version currentVer)
        {
            var url = GenerateLink(PackageName);
            string msg;
            Version onlineVer;
            var webCli = new WebClient();

            msg = "Checking For Updates." + Utils.NEW_LINE + "Current " + PackageName + " Version(" +
                  currentVer.ToString() + ")..." + Utils.NEW_LINE;
            try
            {
                msg += "Downloading Version from Github Pages..." + Utils.NEW_LINE;
                onlineVer = new Version(webCli.DownloadString(url));

                var updatesPending = onlineVer.CompareTo(currentVer);
                if (updatesPending == 0)
                    msg += PackageName + "Version Check OK!" + Utils.NEW_LINE + "Newest version installed." +
                           Utils.NEW_LINE;
                else if (updatesPending < 0)
                    msg += "Version Check OK!." + Utils.NEW_LINE + "Current " + PackageName +
                           " Version is higher than official release." + Utils.NEW_LINE;
                else
                    msg += "Update Available!." + Utils.NEW_LINE + "Current " + PackageName + " Version: (" +
                           currentVer.ToString() + ")" + Utils.NEW_LINE + "Online " + PackageName + " Version: (" +
                           onlineVer.ToString() + ")." + Utils.NEW_LINE;
            }
            catch (Exception)
            {
                msg += "Could not connect to " + url + "." + Utils.NEW_LINE +
                       "Try again later or disable UpdateChecking flags in Package: " + PackageName + "" +
                       Utils.NEW_LINE + "to prevent checking for updates.";
            }

            return msg;
        }
    }
}