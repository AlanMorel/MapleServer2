using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Autofac;
using NLog;
using ThreadState = System.Threading.ThreadState;

namespace MapleServer2.Network
{
    public abstract class Server<T> where T : Session
    {
        private TcpListener Listener;
        private Thread ServerThread;

        private readonly CancellationTokenSource Source;
        private readonly ManualResetEvent ClientConnected;
        private readonly PacketRouter<T> Router;
        private readonly List<T> Sessions;
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly IComponentContext Context;

        public Server(PacketRouter<T> router, IComponentContext context)
        {
            Trace.Assert(context != null);

            Router = router;
            Context = context;

            Source = new CancellationTokenSource();
            ClientConnected = new ManualResetEvent(false);
            Sessions = new List<T>();
        }

        public void Start(ushort port)
        {
            Listener = new TcpListener(IPAddress.Any, port);
            Listener.Start();

            ServerThread = new Thread(() =>
            {
                while (!Source.IsCancellationRequested)
                {
                    ClientConnected.Reset();
                    Logger.Info("{GetType().Name} started on Port:{port}", GetType().Name, port);
                    Listener.BeginAcceptTcpClient(AcceptTcpClient, null);
                    ClientConnected.WaitOne();
                }
            })
            { Name = $"{GetType().Name}Thread" };
            ServerThread.Start();
        }

        public void Stop()
        {
            switch (ServerThread.ThreadState)
            {
                case ThreadState.Unstarted:
                    Logger.Info("{GetType().Name} has not been started.", GetType().Name);
                    break;
                case ThreadState.Stopped:
                    Logger.Info("{GetType().Name} has already been stopped.", GetType().Name);
                    break;
                default:
                    Source.Cancel();
                    ClientConnected.Set();
                    ServerThread.Join();
                    Logger.Info("{GetType().Name} was stopped.", GetType().Name);
                    break;
            }
        }

        public IEnumerable<T> GetSessions()
        {
            Sessions.RemoveAll(session => !session.Connected());
            return Sessions;
        }

        private void AcceptTcpClient(IAsyncResult result)
        {
            T session = Context.Resolve<T>();
            TcpClient client = Listener.EndAcceptTcpClient(result);
            session.Init(client);
            session.OnPacket += Router.OnPacket;

            Sessions.Add(session);
            Logger.Info("Client connected: {session}", session);
            session.Start();

            ClientConnected.Set();
        }
    }
}
