using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using ADL.Streams;

namespace ADL.Network
{
    public class NetworkListener
    {
        private readonly object _stopLock = new object();
        private bool _stop = true;
        private readonly object _stopListenLock = new object();
        private bool _stopListen = true;
        private readonly int _refreshMillis;
        private Thread _listenerThread = null;
        private Thread _serverThread = null;
        private bool _multiThread = false;
        private List<ClientSession> _clients = new List<ClientSession>();
        private ADL.Streams.GenPipeStream<ClientSession> _pendingClients = new GenPipeStream<ClientSession>();
        public NetworkListener(int refreshMillis, bool multiThread = true)
        {
            _multiThread = multiThread;
            _refreshMillis = refreshMillis;
        }

        public void Start()
        {



            Console.WriteLine("Starting Network Listener...");
            lock (_stopListenLock)
            {
                if (_stopListen)
                {
                    _stopListen = false;
                    _listenerThread = new Thread(ListenerThread);
                    _listenerThread.Start();
                }
            }


            Console.WriteLine("Starting Server...");
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
                    else Run();
                }
            }
        }

        public void Stop()
        {
            lock (_stopLock)
            {
                if (!_stop)
                {
                    _stop = true;
                }
            }
            lock (_stopListenLock)
            {
                if (!_stopListen) _stopListen = true;
            }
        }


        void ListenerThread()
        {
            TcpListener tcpL = new TcpListener(IPAddress.Any, 1337);

            tcpL.Start();

            while (true)
            {
                lock (_stopListenLock) if (_stopListen) break;
                TcpClient cl = tcpL.AcceptTcpClient();

                if (cl == null) continue;
                ClientSession cs = new ClientSession(cl);
                if (cs.Authenticate())
                {
                    ClientSession[] cla = new ClientSession[] { cs };
                    _pendingClients.WriteGen(cla, 0, 1);
                }
                else cs.CloseSession();


            }
        }


        void Run()
        {

            LogTextStream ls = new LogTextStream(Console.OpenStandardOutput(), -1, MatchType.MATCH_ALL, true);
            Debug.AddOutputStream(ls);

            while (true)
            {
                Thread.Sleep(_refreshMillis);
                lock (_stopLock)
                    if (_stop)
                    {
                        for (int i = _clients.Count - 1; i >= 0; i--)
                        {
                            _clients[i].CloseSession();
                        }
                        _clients.Clear();
                        break;
                    }

                int pendingClientsCount = (int)_pendingClients.Length;
                if (pendingClientsCount > 0)
                {

                    Console.WriteLine("Accepting " + pendingClientsCount + " Clients");
                    ClientSession[] clients = new ClientSession[pendingClientsCount];
                    _pendingClients.ReadGen(clients, 0, pendingClientsCount);

                    _clients.AddRange(clients);
                }

                List<ClientSession> removeList = new List<ClientSession>();

                for (int i = _clients.Count - 1; i >= 0; i--) //Serve each client
                {
                    ClientSession clientSession = _clients[i];

                    if (!clientSession.Connected)
                    {
                        removeList.Add(clientSession); //Remove when disconnected.
                        clientSession.CloseSession();
                    }
                    else
                    {

                        LogPackage lp = clientSession.GetPackage(out bool dc);

                        if (!dc)
                            foreach (Log log in lp.Logs)
                            {
                                Debug.Log(log.Mask, log.Message.Trim());
                            }
                        else
                        {
                            removeList.Add(clientSession); //Remove when disconnected.
                            clientSession.CloseSession();
                        }


                    }
                }

                for (int i = 0; i < removeList.Count; i++)
                {
                    Console.WriteLine("Removing Client :" + i);
                    _clients.Remove(removeList[i]);
                }
            }
        }

    }
}
