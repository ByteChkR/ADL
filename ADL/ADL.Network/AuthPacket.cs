using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ADL.Network
{
    public struct AuthPacket
    {

        public int ID;
        public byte[] programHash;
        public const int PACKET_SIZE = sizeof(int) + 128;


        public byte[] Serialize()
        {
            List<byte> ret = new List<byte>();
            ret.AddRange(BitConverter.GetBytes(ID));
            ret.AddRange(programHash);
            return ret.ToArray();
        }

        public static bool Deserialize(Stream s, out AuthPacket packet, int length)
        {
            packet = new AuthPacket();
            if (length < PACKET_SIZE) return false;
            byte[] buf = new byte[sizeof(int)];
            s.Read(buf, 0, buf.Length);
            packet.ID = BitConverter.ToInt32(buf, 0);
            packet.programHash = new byte[128];
            s.Read(packet.programHash, 0, 128);
            return true;
        }


    }
}
