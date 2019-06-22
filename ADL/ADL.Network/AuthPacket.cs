using System;
using System.Collections.Generic;
using System.IO;

namespace ADL.Network
{
    public struct AuthPacket
    {
        /// <summary>
        /// The Program ID
        /// </summary>
        public int ID;
        /// <summary>
        /// The Program Assembly Version
        /// </summary>
        public byte[] programAssembly;
        /// <summary>
        /// Convenience Field that has the size of a AuthPacket when serialized.
        /// </summary>
        public const int PACKET_SIZE = sizeof(int) + ASSEMBLY_SIZE;
        /// <summary>
        /// Convenience Field that has the size of the Assembly version when serialized.
        /// </summary>
        public const int ASSEMBLY_SIZE = sizeof(short) * 4;


        /// <summary>
        /// Serializes the Auth Packet into a byte buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] Serialize()
        {
            var ret = new List<byte>();
            ret.AddRange(BitConverter.GetBytes(ID));
            ret.AddRange(programAssembly);
            return ret.ToArray();
        }

        /// <summary>
        /// Deserializes an Auth packet from the stream.
        /// Returns true if sucessful
        /// </summary>
        /// <param name="s">The Stream</param>
        /// <param name="packet">The Packet</param>
        /// <param name="length">The length that we can read.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a Auth Packet based on a ID and an Assembly Version
        /// </summary>
        /// <param name="id"></param>
        /// <param name="asm"></param>
        /// <returns></returns>
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