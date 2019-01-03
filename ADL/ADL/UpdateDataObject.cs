using System;
using System.Net;
namespace ADL
{

    public class UpdateDataObject
    {
        public static string rawLink = "https://raw.githubusercontent.com/ByteChkR/ADL/master/docs/versioning/{0}version.txt";
        public static string GenerateLink(string package)
        {
            return string.Format(rawLink, package);
        }

        public static string CheckUpdate(string PackageName, Version currentVer)
        {
            string url = GenerateLink(PackageName);
            string msg;
            Version onlineVer;
            WebClient webCli = new WebClient();

            msg = "Checking For Updates."+ Utils.NEW_LINE + "Current " + PackageName + " Version(" + currentVer.ToString() + ")..."+ Utils.NEW_LINE + "";
            try
            {
                msg += "Downloading Version from Github Pages..."+ Utils.NEW_LINE + "";
                onlineVer = new Version(webCli.DownloadString(url));

                int updatesPending = onlineVer.CompareTo(currentVer);
                if (updatesPending == 0)
                {
                    msg += PackageName + " Version Check OK!"+ Utils.NEW_LINE + "Newest version installed."+ Utils.NEW_LINE + "";

                }
                else if (updatesPending < 0)
                {
                    msg += "Version Check OK!."+ Utils.NEW_LINE + " Current " + PackageName + " Version is higher than official release."+ Utils.NEW_LINE + " ";

                }
                else
                {
                    msg += "Update Available!."+ Utils.NEW_LINE + " Current " + PackageName + " Version: (" + currentVer.ToString() + ")"+ Utils.NEW_LINE + " Online " + PackageName + " Version: (" + onlineVer.ToString() + ")."+ Utils.NEW_LINE + "";

                }

            }
            catch (Exception)
            {
                msg += "Could not connect to " + url + "."+ Utils.NEW_LINE + " Try again later or disable UpdateChecking flags in Package: " + PackageName + ""+ Utils.NEW_LINE + " to prevent checking for updates.";

            }
            return msg;

        }
    }
}
