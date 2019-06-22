using System;
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
        private readonly int _refreshMillis;
        private Thread _listenerThread = null;
        public NetworkListener(int refreshMillis)
        {
            _refreshMillis = refreshMillis;
        }

        public void Start()
        {
            Console.WriteLine("Starting Network Listener...");
            lock (_stopLock)
            {
                if (_stop)
                {
                    _stop = false;
                    _listenerThread = new Thread(Run);
                    _listenerThread.Start();
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
        }

        void Run()
        {
            TcpListener tcpL = new TcpListener(IPAddress.Any, 1337);
            tcpL.Start();

            TcpClient tcpC = tcpL.AcceptTcpClient();
            LogTextStream ls = new LogTextStream(Console.OpenStandardOutput(), -1, MatchType.MATCH_ALL, true);
            Debug.AddOutputStream(ls);

            int readB = 0;
            while (true)
            {
                Thread.Sleep(_refreshMillis);
                int availableRead = tcpC.Available;
                lock (_stopLock)
                    if (_stop)
                        break;
                if (!tcpC.Connected || availableRead - readB <= 0) continue;
                LogPackage lp = LogPackage.ReadBlock(tcpC.GetStream(), availableRead - readB);
                readB += availableRead;
                foreach (Log l in lp.Logs)
                {
                    Debug.Log(l.Mask, l.Message.Trim());
                }
            }
        }

    }
}
