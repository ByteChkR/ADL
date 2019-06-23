using System;
using System.IO;
using ADL.Configs;
using ADL.Streams;

namespace ADL.Network.Streams
{
    public class NetLogStream : LogStream
    {
        /// <summary>
        ///     Creates a NetworkLogStream that uses TCP Clients as input
        /// </summary>
        /// <param name="s"></param>
        /// <param name="mask"></param>
        /// <param name="mt"></param>
        /// <param name="hasTimestamp"></param>
        public NetLogStream(Stream s, int mask, MatchType mt, bool hasTimestamp) :
            base(s, mask, mt, hasTimestamp)
        {
        }

        /// <summary>
        ///     Wrapper that skips the most unchanged values
        ///     mask: -1
        ///     MatchType: Match_ALL
        ///     SetTimestamp: true
        /// </summary>
        /// <param name="nc"></param>
        /// <param name="id"></param>
        /// <param name="assemblyVersion"></param>
        /// <returns></returns>
        public static NetLogStream CreateNetLogStream(NetworkConfig nc, int id, Version assemblyVersion)
        {
            return CreateNetLogStream(nc, id, assemblyVersion, -1, MatchType.MatchAll, true);
        }

        /// <summary>
        ///     Wrapper that uses the network config to obtain the IP/Port
        ///     mask: -1
        ///     MatchType: Match_ALL
        ///     SetTimestamp: true
        /// </summary>
        /// <param name="nc"></param>
        /// <param name="id"></param>
        /// <param name="assemblyVersion"></param>
        /// <param name="mask"></param>
        /// <param name="mt"></param>
        /// <param name="setTimestamp"></param>
        /// <returns></returns>
        public static NetLogStream CreateNetLogStream(NetworkConfig nc, int id, Version assemblyVersion, int mask,
            MatchType mt, bool setTimestamp)
        {
            return CreateNetLogStream(id, assemblyVersion, nc.Ip, nc.Port, mask, mt, setTimestamp);
        }


        /// <summary>
        ///     Plain Wrapper that creates the NetLogStream.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="assemblyVersion"></param>
        /// <param name="hostname"></param>
        /// <param name="port"></param>
        /// <param name="mask"></param>
        /// <param name="mt"></param>
        /// <param name="hasTimestamp"></param>
        /// <returns></returns>
        public static NetLogStream CreateNetLogStream(int id, Version assemblyVersion, string hostname, int port,
            int mask, MatchType mt, bool hasTimestamp)
        {
            var ls = NetUtils.CreateNetworkTextStream(id, assemblyVersion, hostname, port, mask, mt, hasTimestamp);
            return ls;
        }
    }
}