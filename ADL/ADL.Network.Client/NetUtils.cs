using System;
using System.IO;
using System.Net.Sockets;
using ADL.Network.Streams;
using ADL.Network.Shared;
using ADL.Configs;
using ADL;

namespace ADL.Network.Client
{
    /// <summary>
    ///     Provides wrapper functions to easyliy create a NetWorkStream or directly a NetLogStream
    /// </summary>
    public static class NetUtils
    {
        /// <summary>
        ///     Wrapper function that creates a NetworkStream that is already authenticated with the server.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="id"></param>
        /// <param name="asmVersion"></param>
        /// <returns></returns>
        private static NetworkStream GetNetworkStream(string ip, int port, int id, Version asmVersion)
        {
            TcpClient tcpC;
            try
            {
                tcpC = new TcpClient(ip, port);

                Debug.Log(-1, "Connecting to Network Listener");
                if (!tcpC.Connected) return null;
                Debug.Log(-1, "Connected.");
            }
            catch (Exception)
            {
                Debug.Log(Debug.AdlWarningMask, "Could not connect to server.");
                return null;
            }


            //Authentication
            Stream str = tcpC.GetStream();
            var ap = AuthPacket.Create(id, asmVersion);

            var l = ap.Serialize();
            str.Write(l, 0, l.Length);
            //Authentication End

            return tcpC.GetStream();
        }

        /// <summary>
        ///     Wrapper to create a network log stream.
        /// </summary>
        /// <param name="id">Program ID</param>
        /// <param name="asmVersion">Assembly Version</param>
        /// <param name="ip">IP Address to connect to</param>
        /// <param name="port">Port of the service</param>
        /// <param name="mask">Mask</param>
        /// <param name="matchType">Match Type</param>
        /// <param name="setTimestamp">Timestamp</param>
        /// <returns></returns>
        public static NetLogStream CreateNetworkStream(int id, Version asmVersion, string ip, int port, int mask,
            MatchType matchType, bool setTimestamp = false)
        {
            var str = GetNetworkStream(ip, port, id, asmVersion);

            if (str == null) return null;

            var ls = new NetLogStream(
                str,
                mask,
                matchType,
                setTimestamp
            )
            { OverrideChannelTag = false };



            return ls;
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
        public static NetLogStream CreateNetworkStream(NetworkConfig nc, int id, Version assemblyVersion)
        {
            return CreateNetworkStream(nc, id, assemblyVersion, -1, MatchType.MatchAll, true);
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
        public static NetLogStream CreateNetworkStream(NetworkConfig nc, int id, Version assemblyVersion, int mask,
            MatchType mt, bool setTimestamp)
        {
            if (nc.CheckForUpdates)
            {
               Debug.Log(Debug.UpdateMask, UpdateDataObject.CheckUpdate(typeof(NetUtils)));
            }

            return CreateNetworkStream(id, assemblyVersion, nc.Ip, nc.Port, mask, mt, setTimestamp);
        }

    }
}