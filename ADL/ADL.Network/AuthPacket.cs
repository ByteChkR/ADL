using System;
using System.Collections.Generic;
using System.IO;

namespace ADL.Network
{
    public struct AuthPacket
    {
        public int ID;
        public byte[] programAssembly;
        public const int PACKET_SIZE = sizeof(int) + ASSEMBLY_SIZE;
        public const int ASSEMBLY_SIZE = sizeof(short) * 4;


        public byte[] Serialize()
        {
            var ret = new List<byte>();
            ret.AddRange(BitConverter.GetBytes(ID));
            ret.AddRange(programAssembly);
            return ret.ToArray();
        }

        public static bool Deserialize(Stream s, out AuthPacket packet, int length)
        {
            packet = new AuthPacket();
            if (length < PACKET_SIZE) return false;
            var buf = new byte[sizeof(int)];
            s.Read(buf, 0, buf.Length);
            packet.ID = BitConverter.ToInt32(buf, 0);
            packet.programAssembly = new byte[ASSEMBLY_SIZE];
            s.Read(packet.programAssembly, 0, ASSEMBLY_SIZE);
            return true;
        }

        public static AuthPacket Create(int id, Version asm)
        {
            var ver = asm.ToString();
            var buf = new List<byte>();
            var nr = "";
            for (var i = 0; i < ver.Length; i++)
                if (ver[i] == '.')
                {
                    buf.AddRange(BitConverter.GetBytes(short.Parse(nr)));
                    nr = "";
                }
                else
                {
                    nr += ver[i];
                }

            buf.AddRange(BitConverter.GetBytes(short.Parse(nr)));
            return new AuthPacket {ID = id, programAssembly = buf.ToArray()};
        }
    }
}