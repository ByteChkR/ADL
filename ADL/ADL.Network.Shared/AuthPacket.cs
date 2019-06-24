using System;
using System.Collections.Generic;
using System.IO;

namespace ADL.Network.Shared
{
    /// <summary>
    ///     Authentication Packet containing
    ///     Program ID
    ///     Assembly Version of the Program
    /// </summary>
    public struct AuthPacket
    {
        /// <summary>
        ///     The Program ID
        /// </summary>
        public int Id;

        /// <summary>
        ///     The Program Assembly Version
        /// </summary>
        public byte[] ProgramAssembly;

        /// <summary>
        ///     Convenience Field that has the size of a AuthPacket when serialized.
        /// </summary>
        public const int PacketSize = sizeof(int) + AssemblySize;

        /// <summary>
        ///     Convenience Field that has the size of the Assembly version when serialized.
        /// </summary>
        public const int AssemblySize = sizeof(short) * 4;


        /// <summary>
        ///     Serializes the Auth Packet into a byte buffer.
        /// </summary>
        /// <returns></returns>
        public byte[] Serialize()
        {
            var ret = new List<byte>();
            ret.AddRange(BitConverter.GetBytes(Id));
            ret.AddRange(ProgramAssembly);
            return ret.ToArray();
        }

        /// <summary>
        ///     Deserializes an Auth packet from the stream.
        ///     Returns true if sucessful
        /// </summary>
        /// <param name="s">The Stream</param>
        /// <param name="packet">The Packet</param>
        /// <param name="length">The length that we can read.</param>
        /// <returns></returns>
        public static bool Deserialize(Stream s, out AuthPacket packet, int length)
        {
            packet = new AuthPacket();
            if (length < PacketSize) return false;
            var buf = new byte[sizeof(int)];
            s.Read(buf, 0, buf.Length);
            packet.Id = BitConverter.ToInt32(buf, 0);
            packet.ProgramAssembly = new byte[AssemblySize];
            s.Read(packet.ProgramAssembly, 0, AssemblySize);
            return true;
        }

        /// <summary>
        ///     Creates a Auth Packet based on a ID and an Assembly Version
        /// </summary>
        /// <param name="id"></param>
        /// <param name="asm"></param>
        /// <returns></returns>
        public static AuthPacket Create(int id, Version asm)
        {
            var ver = asm.ToString();
            var buf = new List<byte>();
            var nr = "";
            foreach (var t in ver)
                if (t == '.')
                {
                    buf.AddRange(BitConverter.GetBytes(short.Parse(nr)));
                    nr = "";
                }
                else
                {
                    nr += t;
                }

            buf.AddRange(BitConverter.GetBytes(short.Parse(nr)));
            return new AuthPacket {Id = id, ProgramAssembly = buf.ToArray()};
        }
    }
}