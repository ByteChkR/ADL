using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
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

            msg = "Checking For Updates with Current " + PackageName + " Version[" + currentVer.ToString() + "]...\n";
            try
            {
                onlineVer = new Version(webCli.DownloadString(url));

                int updatesPending = onlineVer.CompareTo(currentVer);
                if (updatesPending == 0)
                {
                    msg += PackageName + " Version Check OK! Newest version installed. ";

                }
                else if (updatesPending < 0)
                {
                    msg += "Version Check OK!. Current " + PackageName + " Version is higher than official release. ";

                }
                else
                {
                    msg += "Update Available!. Current " + PackageName + " Version: " + currentVer.ToString() + "Online " + PackageName + " Version: " + onlineVer.ToString() + "";

                }

            }
            catch (Exception)
            {
                msg += "Could not connect to " + url + " Try again later or disable UpdateChecking flags in Package: " + PackageName + " to prevent checking for updates.";

            }
            return msg;

        }
    }
}
