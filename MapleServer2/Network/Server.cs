using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Autofac;
using MapleServer2.Extensions;
using Microsoft.Extensions.Logging;
using ThreadState = System.Threading.ThreadState;

namespace MapleServer2.Network {
    public abstract class Server<T> where T : Session {
        private TcpListener listener;
        private Thread serverThread;

        private readonly CancellationTokenSource source;
        private readonly ManualResetEvent clientConnected;
        private readonly PacketRouter<T> router;
        private readonly List<T> sessions;
        private readonly ILogger logger;
        private readonly IComponentContext context;

        public Server(PacketRouter<T> router, ILogger<Server<T>> logger, IComponentContext context) {
            Trace.Assert(context != null);

            this.router = router;
            this.logger = logger;
            this.context = context;

            source = new CancellationTokenSource();
            clientConnected = new ManualResetEvent(false);
            sessions = new List<T>();
        }

        public void Start(ushort port) {
            listener = new TcpListener(IPAddress.Any, port);
            listener.Start();

            serverThread = new Thread(() => {
                while (!source.IsCancellationRequested) {
                    clientConnected.Reset();
                    logger.Info($"{GetType().Name} started on Port:{port}");
                    listener.BeginAcceptTcpClient(AcceptTcpClient, null);
                    clientConnected.WaitOne();
                }
            }) {Name = $"{GetType().Name}Thread"};
            serverThread.Start();
        }

        public void Stop() {
            switch (serverThread.ThreadState) {
                case ThreadState.Unstarted:
                    logger.Info($"{GetType().Name} has not been started.");
                    break;
                case ThreadState.Stopped:
                    logger.Info($"{GetType().Name} has already been stopped.");
                    break;
                default:
                    source.Cancel();
                    clientConnected.Set();
                    serverThread.Join();
                    logger.Info($"{GetType().Name} was stopped.");
                    break;
            }
        }

        public IEnumerable<T> GetSessions() {
            sessions.RemoveAll(session => !session.Connected());
            return sessions;
        }

        private void AcceptTcpClient(IAsyncResult result) {
            T session = context.Resolve<T>();
            TcpClient client = listener.EndAcceptTcpClient(result);
            session.Init(client);
            session.OnPacket += router.OnPacket;

            sessions.Add(session);
            logger.Info($"Client connected: {session}");
            session.Start();

            clientConnected.Set();
        }
    }
}