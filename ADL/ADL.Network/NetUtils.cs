using System;
using System.IO;
using System.Net.Sockets;
using ADL.Network.Streams;

namespace ADL.Network
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
        public static NetworkStream GetNetworkStream(string ip, int port, int id, Version asmVersion)
        {
            var tcpC = new TcpClient(ip, port);

            Debug.Log(-1, "Connecting to Network Listener");
            if (!tcpC.Connected) return null;
            Debug.Log(-1, "Connected.");

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
        public static NetLogStream CreateNetworkTextStream(int id, Version asmVersion, string ip, int port, int mask,
            MatchType matchType, bool setTimestamp = false)
        {
            var str = GetNetworkStream(ip, port, id, asmVersion);

            var ls = new NetLogStream(
                str,
                mask,
                matchType,
                setTimestamp
            ) {OverrideChannelTag = true};



            return ls;
        }
    }
}