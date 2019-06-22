using System;
using System.IO;
using System.Net.Sockets;
using System.Reflection;
using ADL.Streams;

namespace ADL.Network
{
    public static class NetUtils
    {
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
        public static LogStream CreateNetworkTextStream(int id, Version asmVersion, string ip, int port, int mask,
            MatchType matchType, bool setTimestamp = false)
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

            var ls = new LogStream(
                str,
                mask,
                matchType,
                setTimestamp
            );


            ls.OverrideChannelTag = true;

            return ls;
        }
    }
}