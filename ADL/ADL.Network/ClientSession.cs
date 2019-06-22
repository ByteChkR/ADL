using System;
using System.IO;
using System.Net.Sockets;
using ADL.Streams;

namespace ADL.Network
{
    public class ClientSession
    {
        private static int instanceCount = 1;
        private readonly TcpClient _client;
        private Stream _fileStream;
        private LogTextStream _lts;
        public int ID;
        public string Version;

        public ClientSession(TcpClient client)
        {
            instanceID = instanceCount;
            instanceCount++;
            Authenticated = false;
            _client = client;
        }

        public int instanceID { get; }

        public bool Connected => _client.Connected;

        public bool Authenticated { get; }

        private string GetLogPath()
        {
            var Path = "logs/";
            var id = NetworkListener.Config.ID2NameMap.Length >= ID
                ? NetworkListener.Config.ID2NameMap[ID - 1]
                : "ID" + (ID - 1);
            return Path + id + "_" + Version + "_" + DateTime.UtcNow.ToString(NetworkListener.Config.TimeFormatString) +
                   ".log";
        }

        public void Initialize()
        {
            var str = GetLogPath();
            _fileStream = File.Open(str, FileMode.Create);
            _lts = new LogTextStream(_fileStream, instanceID);
            _lts.OverrideChannelTag = true;
            Debug.AddOutputStream(_lts);
        }

        public bool Authenticate()
        {
            Stream s = _client.GetStream();

            while (_client.Available < AuthPacket.PACKET_SIZE)
            {
            }

            if (AuthPacket.Deserialize(s, out var packet, _client.Available))
                return Auth(packet.ID, packet.programAssembly);
            return false;
        }


        private bool Auth(int id, byte[] hash)
        {
            ID = id;
            Version = "";
            for (var i = 0; i < 4; i++)
            {
                var idx = i * sizeof(short);
                Version += BitConverter.ToInt16(hash, idx);
                if (i != 3) Version += '.';
            }

            return true;
        }


        public void CloseSession()
        {
            _client.Close();
            _fileStream.Close();
            Debug.RemoveOutputStream(_lts);
        }


        private bool IsConnectionUp()
        {
            var testBuffer = new byte[1];
            try
            {
                _client.GetStream().Write(testBuffer, 0, 1);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public LogPackage GetPackage(out bool disconnect)
        {
            var availableRead = _client.Available;
            disconnect = false;
            if (availableRead == 0)
                disconnect = !IsConnectionUp();

            if (disconnect || availableRead == 0) return new LogPackage(new byte[0]);


            var lp = LogPackage.ReadBlock(_client.GetStream(), availableRead);


            return lp;
        }
    }
}