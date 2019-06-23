using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using ADL.Streams;
using ADL.Network;
using ADL.Network.Streams;

namespace ADL.Network
{
    public static class NetUtils
    {

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
        /// Wrapper to create a network log stream.
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

            Stream str = GetNetworkStream(ip, port, id, asmVersion);

            NetLogStream ls;
            ls = new NetLogStream(
                str,
                id,
                mask,
                matchType,
                setTimestamp
            );


            ls.OverrideChannelTag = true;

            return ls;
        }
    }
}