using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Autofac;
using NLog;
using ThreadState = System.Threading.ThreadState;

namespace MapleServer2.Network;

public abstract class Server<T> where T : Session
{
    private TcpListener Listener;
    private Thread ServerThread;

    private readonly CancellationTokenSource Source;
    private readonly ManualResetEvent ClientConnected;
    private readonly PacketRouter<T> Router;
    private readonly IComponentContext Context;

    protected static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public Server(PacketRouter<T> router, IComponentContext context)
    {
        Trace.Assert(context != null);

        Source = new();
        ClientConnected = new(false);
        Router = router;
        Context = context;
    }

    public void Start(ushort port)
    {
        Listener = new(IPAddress.Any, port);
        Listener.Start();

        ServerThread = new(() =>
        {
            while (!Source.IsCancellationRequested)
            {
                ClientConnected.Reset();
                Logger.Info($"Thread from {GetType().Name} has started on Port:{port}");
                Listener.BeginAcceptTcpClient(AcceptTcpClient, null);
                ClientConnected.WaitOne();
            }
        })
        {
            Name = $"{GetType().Name}Thread"
        };
        ServerThread.Start();
    }

    public void Stop()
    {
        switch (ServerThread.ThreadState)
        {
            case ThreadState.Unstarted:
                Logger.Info($"{GetType().Name} has not been started.");
                break;
            case ThreadState.Stopped:
                Logger.Info($"{GetType().Name} has already been stopped.");
                break;
            default:
                Source.Cancel();
                ClientConnected.Set();
                ServerThread.Join();
                Logger.Info($"{GetType().Name} was stopped.");
                break;
        }
    }

    public abstract void AddSession(T session);

    public abstract void RemoveSession(T session);

    private void AcceptTcpClient(IAsyncResult result)
    {
        T session = Context.Resolve<T>();
        TcpClient client = Listener.EndAcceptTcpClient(result);
        session.Init(client);
        session.OnPacket += Router.OnPacket;

        AddSession(session);

        ClientConnected.Set();
    }
}
