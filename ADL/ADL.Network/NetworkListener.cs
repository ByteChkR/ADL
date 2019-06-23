using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using ADL.Configs;
using ADL.Streams;

namespace ADL.Network
{
    /// <summary>
    ///     Network Listener is acting as the server that receives logs from a client
    /// </summary>
    public class NetworkListener
    {
        /// <summary>
        ///     Client/Server Config
        /// </summary>
        public static NetworkConfig Config;

        /// <summary>
        ///     List of all active and connected clients
        /// </summary>
        private readonly List<ClientSession> _clients = new List<ClientSession>();

        /// <summary>
        ///     Flag to optionally turn multithreading off
        ///     Will execute the Main loop on the current thread.
        /// </summary>
        private readonly bool _multiThread;

        /// <summary>
        ///     Queue of all accepted clients that are waiting to be served.
        /// </summary>
        private readonly GenPipeStream<ClientSession> _pendingClients = new GenPipeStream<ClientSession>();

        /// <summary>
        ///     Main Loop Slowdown
        /// </summary>
        private readonly int _refreshMillis;

        /// <summary>
        ///     Thread lock object
        /// </summary>
        private readonly object _stopListenLock = new object();

        /// <summary>
        ///     Thread lock object
        /// </summary>
        private readonly object _stopLock = new object();

        /// <summary>
        ///     Thread of the TCP Listener
        /// </summary>
        private Thread _listenerThread;

        /// <summary>
        ///     Stream that will save all the incoming logs from one client into a file.
        /// </summary>
        private LogTextStream _lts;

        /// <summary>
        ///     Flag that indicates if the Network listener should search the Github pages for the version
        /// </summary>
        private readonly bool _noUpdateCheck;

        /// <summary>
        ///     Thread of the main server loop
        /// </summary>
        private Thread _serverThread;

        /// <summary>
        ///     Flag to stop the main loop
        /// </summary>
        private bool _stop = true;

        /// <summary>
        ///     Flag to stop the TCPListener Loop
        /// </summary>
        private bool _stopListen = true;

        /// <summary>
        ///     Creates a Network Listener that can be started with .Start
        /// </summary>
        /// <param name="refreshMillis">Main Loop Delay</param>
        /// <param name="config">Path to NetworkConfig file</param>
        /// <param name="multiThread">flag to enable multithreading</param>
        /// <param name="noUpdateCheck">flag to disable checking for updates</param>
        public NetworkListener(int refreshMillis, string config = "", bool multiThread = true,
            bool noUpdateCheck = false)
        {
            _noUpdateCheck = noUpdateCheck;
            Config = NetworkConfig.Load(config);
            _multiThread = multiThread;
            _refreshMillis = refreshMillis;
        }


        /// <summary>
        ///     Starts the Server threads
        /// </summary>
        public void Start()
        {
            _lts = new LogTextStream(Console.OpenStandardOutput(), 0);
            Debug.AddOutputStream(_lts);


            if (!_noUpdateCheck)
            {
                var msg = UpdateDataObject.CheckUpdate(Assembly.GetExecutingAssembly().GetName().Name,
                    Assembly.GetExecutingAssembly().GetName().Version);

                Debug.Log(0, msg);
            }

            Debug.Log(0, "Starting Network Listener...");
            lock (_stopListenLock)
            {
                if (_stopListen)
                {
                    _stopListen = false;
                    _listenerThread = new Thread(ListenerThread) {IsBackground = true};
                    _listenerThread.Start();
                }
            }


            Debug.Log(0, "Starting Server...");
            lock (_stopLock)
            {
                if (!_stop) return;
                _stop = false;
                if (_multiThread)
                {
                    _serverThread = new Thread(Run) {IsBackground = true};
                    _serverThread.Start();
                }
                else
                {
                    Run();
                }
            }
        }

        /// <summary>
        ///     Stopping server threads.
        /// </summary>
        public void Stop()
        {
            lock (_stopLock)
            {
                if (!_stop) _stop = true;
            }

            lock (_stopListenLock)
            {
                if (!_stopListen) _stopListen = true;
            }

            Debug.RemoveOutputStream(_lts);
        }

        /// <summary>
        ///     Listener thread
        ///     Handles Auth/Init of Client sessions
        /// </summary>
        private void ListenerThread()
        {
            var tcpL = new TcpListener(IPAddress.Any, Config.Port);

            tcpL.Start();

            while (true)
            {
                lock (_stopListenLock)
                {
                    if (_stopListen) break;
                }

                var cs = new ClientSession(tcpL.AcceptTcpClient());
                if (cs.Authenticate())
                {
                    cs.Initialize();


                    ClientSession[] cla = {cs};
                    _pendingClients.WriteGen(cla, 0, 1);
                }
                else
                {
                    cs.CloseSession();
                }
            }

            tcpL.Stop();
        }

        /// <summary>
        ///     Main Server loop
        ///     Handles saving/logging the incoming messages
        /// </summary>
        private void Run()
        {
            while (true)
            {
                Thread.Sleep(_refreshMillis);
                lock (_stopLock)
                {
                    if (_stop)
                    {
                        for (var i = _clients.Count - 1; i >= 0; i--) _clients[i].CloseSession();
                        _clients.Clear();
                        break;
                    }
                }

                var pendingClientsCount = (int) _pendingClients.Length;
                if (pendingClientsCount > 0)
                {
                    Debug.Log(0, "Accepting " + pendingClientsCount + " Clients");
                    var clients = new ClientSession[pendingClientsCount];
                    _pendingClients.ReadGen(clients, 0, pendingClientsCount);

                    _clients.AddRange(clients);
                    Debug.Log(0, "Total Clients:  " + _clients.Count + " Clients");
                }

                var removeList = new List<ClientSession>();

                for (var i = _clients.Count - 1; i >= 0; i--) //Serve each client
                {
                    var clientSession = _clients[i];

                    if (!clientSession.Connected)
                    {
                        removeList.Add(clientSession); //Remove when disconnected.
                        clientSession.CloseSession();
                    }
                    else
                    {
                        var lp = clientSession.GetPackage(out var dc);

                        if (!dc)
                        {
                            if (lp.Logs.Count <= 0) continue;
                            for (var j = 0; j < lp.Logs.Count; j++)
                                Debug.Log(clientSession.InstanceId, lp.Logs[j].Message.Trim());
                        }
                        else
                        {
                            removeList.Add(clientSession); //Remove when disconnected.
                            clientSession.CloseSession();
                        }
                    }
                }

                for (var i = 0; i < removeList.Count; i++)
                {
                    Debug.Log(0, "Removing Client :" + i);
                    _clients.Remove(removeList[i]);
                }
            }


            for (var i = 0; i < _clients.Count; i++)
            {
                Debug.Log(0, "Removing Client :" + i);
                _clients[i].CloseSession();
            }

            _clients.Clear();
        }
    }
}