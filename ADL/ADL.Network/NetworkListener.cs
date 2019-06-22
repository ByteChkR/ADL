using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using ADL.Configs;
using ADL.Streams;

namespace ADL.Network
{
    public class NetworkListener
    {
        public static NetworkConfig Config;
        private readonly int _refreshMillis;
        private readonly object _stopListenLock = new object();


        private readonly object _stopLock = new object();
        private readonly List<ClientSession> _clients = new List<ClientSession>();
        private Thread _listenerThread;
        private LogTextStream _lts;
        private readonly bool _multiThread;
        private readonly GenPipeStream<ClientSession> _pendingClients = new GenPipeStream<ClientSession>();
        private Thread _serverThread;
        private bool _stop = true;
        private bool _stopListen = true;

        public NetworkListener(int refreshMillis, string config = "", bool multiThread = true)
        {
            Config = NetworkConfig.Load(config);
            _multiThread = multiThread;
            _refreshMillis = refreshMillis;
        }


        public void Start()
        {
            _lts = new LogTextStream(Console.OpenStandardOutput(), 0);
            Debug.AddOutputStream(_lts);

            Debug.Log(0, "Starting Network Listener...");
            lock (_stopListenLock)
            {
                if (_stopListen)
                {
                    _stopListen = false;
                    _listenerThread = new Thread(ListenerThread);
                    _listenerThread.Start();
                }
            }


            Debug.Log(0, "Starting Server...");
            lock (_stopLock)
            {
                if (_stop)
                {
                    _stop = false;
                    if (_multiThread)
                    {
                        _serverThread = new Thread(Run);
                        _serverThread.Start();
                    }
                    else
                    {
                        Run();
                    }
                }
            }
        }

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
                            if (lp.Logs.Count > 0)
                                //Debug.Log(0, "Logs: " + lp.Logs.Count);
                                for (var j = 0; j < lp.Logs.Count; j++)
                                    Debug.Log(clientSession.instanceID, lp.Logs[j].Message.Trim());
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