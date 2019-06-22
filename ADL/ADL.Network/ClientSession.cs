using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using ADL.Streams;

namespace ADL.Network
{
    public class ClientSession
    {
        TcpClient _client;
        public bool Connected
        {
            get
            {
                
                return _client.Connected;
            }
        }
        public bool Authenticated
        {
            get;
            private set;
        }


        public ClientSession(TcpClient client)
        {
            Authenticated = false;
            _client = client;
        }

        public bool Authenticate()
        {
            System.IO.Stream s = _client.GetStream();

            while (_client.Available < AuthPacket.PACKET_SIZE) { }

            if (AuthPacket.Deserialize(s, out AuthPacket packet, (int)_client.Available))
            {
                return Auth(packet.ID, packet.programHash);
            }
            return false;
        }


        bool Auth(int id, byte[] hash)
        {
            Console.WriteLine("ID: " + id + " Hash: " + Debug.TextEncoding.GetString(hash));
            return true;
        }


        public void CloseSession()
        {
            _client.Close();
        }


        public LogPackage GetPackage(out bool disconnect)
        {
            int availableRead = _client.Available;
            
            LogPackage lp;
            disconnect = !LogPackage.ReadBlock(_client.GetStream(), out lp);


            return lp;

        }
    }
}
