using System;
using System.Diagnostics;
using System.Threading;
using MaplePacketLib2.Tools;
using MapleServer2.Enums;
using MapleServer2.Network;
using MapleServer2.Packets;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.Servers.Game
{
    public class GameSession : Session
    {
        protected override SessionType Type => SessionType.Game;

        public int ServerTick;
        public int ClientTick;

        public IFieldObject<Player> FieldPlayer { get; private set; }
        public Player Player => FieldPlayer.Value;

        public FieldManager FieldManager { get; private set; }

        private readonly ManagerFactory<FieldManager> FieldManagerFactory;

        // TODO: Replace this with a scheduler.
        private readonly CancellationTokenSource CancellationToken;

        public GameSession(ManagerFactory<FieldManager> fieldManagerFactory, ILogger<GameSession> logger) : base(logger)
        {
            FieldManagerFactory = fieldManagerFactory;
            CancellationToken = new CancellationTokenSource();

            // Continuously sends field updates to client
            new Thread(() =>
            {
                while (!CancellationToken.IsCancellationRequested)
                {
                    if (FieldManager != null)
                    {
                        foreach (Packet update in FieldManager.GetUpdates())
                        {
                            Send(update);
                        }
                    }
                    Thread.Sleep(1000);
                }
            }).Start();
        }

        public void SendNotice(string message)
        {
            Send(ChatPacket.Send(Player, message, ChatType.NoticeAlert));
        }

        // Called first time when starting a new session
        public void InitPlayer(Player player)
        {
            Debug.Assert(FieldPlayer == null, "Not allowed to reinitialize player.");
            FieldManager = FieldManagerFactory.GetManager(player.MapId, instanceId: 0);
            FieldPlayer = FieldManager.RequestFieldObject(player);
            GameServer.Storage.AddPlayer(player);
        }

        public void EnterField(Player player)
        {
            // If moving maps, need to get the FieldManager for new map
            if (player.MapId != FieldManager.MapId)
            {
                FieldManager.RemovePlayer(this, FieldPlayer); // Leave previous field
                FieldManagerFactory.Release(FieldManager.MapId, player.InstanceId);

                // Initialize for new Map
                FieldManager = FieldManagerFactory.GetManager(player.MapId, player.InstanceId);
                FieldPlayer = FieldManager.RequestFieldObject(Player);
            }

            FieldManager.AddPlayer(this, FieldPlayer); // Add player
        }

        public void SyncTicks()
        {
            ServerTick = Environment.TickCount;
            Send(RequestPacket.TickSync(ServerTick));
        }

        public override void EndSession()
        {
            FieldManager.RemovePlayer(this, FieldPlayer);
            GameServer.Storage.RemovePlayer(FieldPlayer.Value);
            CancellationToken.Cancel();
            // Should we Join the thread to wait for it to complete?
        }
    }
}
