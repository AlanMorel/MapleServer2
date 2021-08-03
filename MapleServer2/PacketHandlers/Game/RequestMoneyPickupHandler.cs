﻿using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestMoneyPickupHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_MONEY_PICKUP;

        public RequestMoneyPickupHandler(ILogger<GamePacketHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            int objectCount = packet.ReadByte();

            for (int i = 0; i < objectCount; i++)
            {
                int objectId = packet.ReadInt();

                bool foundItem = session.FieldManager.State.TryGetItem(objectId, out IFieldObject<Item> fieldItem);
                if (foundItem && fieldItem.Value.Id >= 90000001 && fieldItem.Value.Id <= 90000003)
                {
                    session.Player.Wallet.Meso.Modify(session, fieldItem.Value.Amount);
                    if (session.FieldManager.RemoveItem(objectId, out Item item))
                    {
                        session.FieldManager.BroadcastPacket(FieldPacket.PickupItem(objectId, item, session.FieldPlayer.ObjectId));
                        session.FieldManager.BroadcastPacket(FieldPacket.RemoveItem(objectId));
                    }
                }
            }
        }
    }
}
