using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Autofac;
using Serilog;
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

    private readonly ILogger Logger = Log.Logger.ForContext<T>();

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

                Logger.Information("Thread from {name} has started on Port:{port}", GetType().Name, port);
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
                Logger.Information("{name} has not been started.", GetType().Name);
                break;
            case ThreadState.Stopped:
                Logger.Information("{name} has already been stopped.", GetType().Name);
                break;
            default:
                Source.Cancel();
                ClientConnected.Set();
                ServerThread.Join();
                Logger.Information("{name} was stopped.", GetType().Name);
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
