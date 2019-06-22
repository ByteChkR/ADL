using System;
using System.IO;
using System.Net.Sockets;
using ADL.Streams;

namespace ADL.Network
{
    public static class NetUtils
    {
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