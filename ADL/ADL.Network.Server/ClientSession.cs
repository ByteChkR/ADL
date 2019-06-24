using System;
using System.IO;
using System.Net.Sockets;
using ADL.Network.Shared;
using ADL.Streams;

namespace ADL.Network.Server
{
    /// <summary>
    ///     Client session that the server maintains and gets the data from.
    /// </summary>
    public class ClientSession
    {
        /// <summary>
        ///     Static int to keep track who receives what logs.
        /// </summary>
        private static int _instanceCount = 1;

        /// <summary>
        ///     Client that is used to communicate
        /// </summary>
        private readonly TcpClient _client;

        /// <summary>
        ///     The Stream that is writing the Logs to disk
        /// </summary>
        private Stream _fileStream;

        /// <summary>
        ///     Reference to the LogStream to be able to remove it from ADL again.
        /// </summary>
        private LogTextStream _lts;

        /// <summary>
        ///     The Program ID
        /// </summary>
        public int Id;

        /// <summary>
        ///     Assembly version of the Program
        /// </summary>
        public string Version;

        private NetworkListener parent;

        /// <summary>
        /// </summary>
        /// <param name="client"></param>
        public ClientSession(TcpClient client, NetworkListener parent)
        {
            InstanceId = _instanceCount;
            this.parent = parent;
            _instanceCount++;
            Authenticated = false;
            _client = client;

        }

        /// <summary>
        ///     Instance ID that gets used as a mask to write the logs into the right file.
        /// </summary>
        public int InstanceId { get; }

        /// <summary>
        ///     Wrapper that wraps around the TcpClient.Connected Property
        /// </summary>
        public bool Connected => _client.Connected;

        /// <summary>
        ///     A Flag that gets set when the client authenticates himself
        /// </summary>
        public bool Authenticated { get; }

        /// <summary>
        ///     Returns the path of the log file for this Client Session
        /// </summary>
        /// <returns></returns>
        private string GetLogPath()
        {
            const string path = "logs/";
            var id = parent.Config.Id2NameMap.Length >= Id
                ? parent.Config.Id2NameMap[Id - 1]
                : "ID" + (Id - 1);
            return path + id + "_" + Version + "_" + DateTime.UtcNow.ToString(parent.Config.TimeFormatString) +
                   ".log";
        }

        /// <summary>
        ///     Initialization
        ///     Gets the File Stream and the Logstream ready.
        /// </summary>
        public void Initialize()
        {
            var str = GetLogPath();
            _fileStream = File.Open(str, FileMode.Create);
            _lts = new LogTextStream(_fileStream, InstanceId) {OverrideChannelTag = true};
            Debug.AddOutputStream(_lts);
        }

        /// <summary>
        ///     Starts the Authentication routine
        /// </summary>
        /// <returns></returns>
        public bool Authenticate()
        {
            Stream s = _client.GetStream();

            while (_client.Available < AuthPacket.PacketSize)
            {
            }

            return AuthPacket.Deserialize(s, out var packet, _client.Available) && Auth(packet.Id, packet.ProgramAssembly);
        }


        /// <summary>
        ///     Internal function that converts the Hash to a valid assembly version, stores the ID
        ///     and checks if the client is authorized to connect to this server.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="hash"></param>
        /// <returns></returns>
        private bool Auth(int id, byte[] hash)
        {
            Id = id;
            Version = "";
            for (var i = 0; i < 4; i++)
            {
                var idx = i * sizeof(short);
                Version += BitConverter.ToInt16(hash, idx);
                if (i != 3) Version += '.';
            }

            return true; //Really strict security settings ;)
        }


        /// <summary>
        ///     Closes this session TcpClient and File Stream
        /// </summary>
        public void CloseSession()
        {
            _client.Close();
            _fileStream.Close();
            Debug.RemoveOutputStream(_lts);
        }


        /// <summary>
        ///     Hacky way of checking if the client has dropped the connection
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        ///     Returns the Log Package that was sent by the client.
        ///     Empty Packet when disconnected or empty.
        /// </summary>
        /// <param name="disconnect"></param>
        /// <returns></returns>
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