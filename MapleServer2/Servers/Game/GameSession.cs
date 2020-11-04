using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using MaplePacketLib2.Tools;
using Maple2Storage.Enums;
using MapleServer2.Network;
using MapleServer2.Packets;
using MapleServer2.Tools;
using MapleServer2.Types;
using MapleServer2.Types.FieldObjects;
using Microsoft.Extensions.Logging;
using Maple2.Data.Types;

namespace MapleServer2.Servers.Game {
    public class GameSession : Session {
        protected override SessionType Type => SessionType.Game;

        // TODO: Come up with a better solution
        // Using this for now to store arbitrary state objects by key.
        public readonly Dictionary<string, object> StateStorage;
        public readonly Inventory Inventory;

        public int ServerTick { get; private set; }
        public int ClientTick;

        public IFieldObject<Player> FieldPlayer { get; private set; }
        public Player Player => FieldPlayer.Value;

        public FieldManager FieldManager { get; private set; }

        private readonly ManagerFactory<FieldManager> fieldManagerFactory;

        // TODO: Replace this with a scheduler.
        private readonly CancellationTokenSource cancellationToken;

        public GameSession(ManagerFactory<FieldManager> fieldManagerFactory, ILogger<GameSession> logger) : base(logger) {
            this.fieldManagerFactory = fieldManagerFactory;
            this.cancellationToken = new CancellationTokenSource();
            this.StateStorage = new Dictionary<string, object>();
            this.Inventory = new Inventory(48);

            // Continuously sends field updates to client
            new Thread(() => {
                while (!cancellationToken.IsCancellationRequested) {
                    if (FieldManager != null) {
                        foreach (Packet update in FieldManager.GetUpdates()) {
                            this.Send(update);
                        }
                    }

                    Thread.Sleep(1000);
                }
            }).Start();
        }

        public new void Dispose() {
            FieldManager.RemovePlayer(this, FieldPlayer);
            cancellationToken.Cancel();
            // Should we Join the thread to wait for it to complete?
            base.Dispose();
        }

        public void SendNotice(string message) {
            Send(ChatPacket.Send(Player, message, ChatType.NoticeAlert));
        }

        // Called first time when starting a new session
        public void InitPlayer(Player player) {
            Debug.Assert(FieldPlayer == null, "Not allowed to reinitialize player.");
            FieldManager = fieldManagerFactory.GetManager(player.MapId);
            this.FieldPlayer = FieldManager.RequestFieldObject(player);
        }

        public void EnterField(int newMapId) {
            // If moving maps, need to get the FieldManager for new map
            if (newMapId != FieldManager.MapId) {
                FieldManager.RemovePlayer(this, FieldPlayer); // Leave previous field
                fieldManagerFactory.Release(FieldManager.MapId);

                // Initialize for new Map
                FieldManager = fieldManagerFactory.GetManager(newMapId);
                this.FieldPlayer = FieldManager.RequestFieldObject(Player);
            }

            FieldManager.AddPlayer(this, FieldPlayer); // Add player
        }

        public void SyncTicks() {
            ServerTick = Environment.TickCount;
            Send(RequestPacket.TickSync(ServerTick));
        }
    }
}