using System;
using System.Net;

namespace ADL
{
    public static class UpdateDataObject
    {
        /// <summary>
        ///     The Raw link to the location where i have my version files.
        /// </summary>
        private const string RawLink = "https://raw.githubusercontent.com/ByteChkR/ADL/master/docs/versioning/{0}version.txt";

        /// <summary>
        ///     Creates the right link from the raw link and the package name
        /// </summary>
        /// <param name="package">the package name</param>
        /// <returns>returns a valid link to the version file.</returns>
        public static string GenerateLink(string package)
        {
            return string.Format(RawLink, package);
        }

        /// <summary>
        ///     Checks For Updates for the specified package.
        /// </summary>
        /// <param name="packageName">Package name to search for</param>
        /// <param name="currentVer">The current assembly version of the package</param>
        /// <returns></returns>
        public static string CheckUpdate(Type type)
        {
            string packageName=type.Assembly.GetName().Name;
            Version currentVer=type.Assembly.GetName().Version;


            var url = GenerateLink(packageName);
            var webCli = new WebClient();

            var msg = "Checking For Updates." + Utils.NewLine + "Current " + packageName + " Version(" +
                         currentVer + ")..." + Utils.NewLine;
            try
            {
                msg += "Downloading Version from Github Pages..." + Utils.NewLine;
                var onlineVer = new Version(webCli.DownloadString(url));

                var updatesPending = onlineVer.CompareTo(currentVer);
                if (updatesPending == 0)
                    msg += packageName + "Version Check OK!" + Utils.NewLine + "Newest version installed." +
                           Utils.NewLine;
                else if (updatesPending < 0)
                    msg += "Version Check OK!." + Utils.NewLine + "Current " + packageName +
                           " Version is higher than official release." + Utils.NewLine;
                else
                    msg += "Update Available!." + Utils.NewLine + "Current " + packageName + " Version: (" +
                           currentVer + ")" + Utils.NewLine + "Online " + packageName + " Version: (" +
                           onlineVer + ")." + Utils.NewLine;
            }
            catch (Exception)
            {
                msg += "Could not connect to " + url + "." + Utils.NewLine +
                       "Try again later or disable UpdateChecking flags in Package: " + packageName + "" +
                       Utils.NewLine + "to prevent checking for updates.";
            }

            return msg;
        }
    }
}