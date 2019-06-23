using System;
using System.IO;
using System.Text.RegularExpressions;
using ADL.Configs;
using ADL.Network.Streams;
using ADL.Streams;

namespace ADL.Network.Streams
{
    public class NetLogStream : LogStream
    {
        public NetLogStream(Stream s, int id, int mask, MatchType mt, bool hasTimestamp) :
            base(s, mask, mt, hasTimestamp)
        {

        }

        public static NetLogStream CreateNetLogStream(NetworkConfig nc, int id, Version assemblyversion)
        {
            return CreateNetLogStream(nc, id, assemblyversion, -1, MatchType.MATCH_ALL, true);
        }

        public static NetLogStream CreateNetLogStream(NetworkConfig nc, int id, Version assemblyVersion, int mask, MatchType mt, bool setTimestamp)
        {
            return CreateNetLogStream(id, assemblyVersion, nc.IP, nc.Port, mask, mt, setTimestamp);
        }

        public static NetLogStream CreateNetLogStream(int id, Version assemblyVersion, string hostname, int port, int mask, MatchType mt, bool hasTimestamp)
        { 
            NetLogStream ls = NetUtils.CreateNetworkTextStream(id, assemblyVersion, hostname, port, mask, mt, hasTimestamp);
            return ls;
        }


        
    }
}