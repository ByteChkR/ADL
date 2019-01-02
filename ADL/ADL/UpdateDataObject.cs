using System;
using System.Collections.Generic;
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

            msg = "Checking For Updates.\nCurrent " + PackageName + " Version(" + currentVer.ToString() + ")...\n";
            try
            {
                msg += "Downloading Version from Github Pages...\n";
                onlineVer = new Version(webCli.DownloadString(url));

                int updatesPending = onlineVer.CompareTo(currentVer);
                if (updatesPending == 0)
                {
                    msg += PackageName + " Version Check OK!\nNewest version installed.\n";

                }
                else if (updatesPending < 0)
                {
                    msg += "Version Check OK!.\n Current " + PackageName + " Version is higher than official release.\n ";

                }
                else
                {
                    msg += "Update Available!.\n Current " + PackageName + " Version: (" + currentVer.ToString() + ")\n Online " + PackageName + " Version: (" + onlineVer.ToString() + ").\n";

                }

            }
            catch (Exception)
            {
                msg += "Could not connect to " + url + ".\n Try again later or disable UpdateChecking flags in Package: " + PackageName + "\n to prevent checking for updates.";

            }
            return msg;

        }
    }
}
