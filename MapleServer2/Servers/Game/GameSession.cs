﻿using System;
using System.Collections.Generic;
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

        // TODO: Come up with a better solution
        // Using this for now to store arbitrary state objects by key.
        public readonly Dictionary<string, object> StateStorage;

        public int ServerTick { get; private set; }
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
            StateStorage = new Dictionary<string, object>();

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
            FieldManager = FieldManagerFactory.GetManager(player.MapId);
            FieldPlayer = FieldManager.RequestFieldObject(player);
            GameServer.Storage.AddPlayer(player);
        }

        public void EnterField(int newMapId)
        {
            // If moving maps, need to get the FieldManager for new map
            if (newMapId != FieldManager.MapId)
            {
                FieldManager.RemovePlayer(this, FieldPlayer); // Leave previous field
                FieldManagerFactory.Release(FieldManager.MapId);

                // Initialize for new Map
                FieldManager = FieldManagerFactory.GetManager(newMapId);
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
