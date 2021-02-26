using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    class InteractObjectHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.INTERACT_OBJECT;

        public InteractObjectHandler(ILogger<InteractObjectHandler> logger) : base(logger) { }

        private enum InteractObjectMode : byte
        {
            Use = 0x0C,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            InteractObjectMode mode = (InteractObjectMode) packet.ReadByte();

            switch (mode)
            {
                case InteractObjectMode.Use:
                    HandleUse(session, packet);
                    break;
            }
        }

        private static void HandleUse(GameSession session, PacketReader packet)
        {
            string uuid = packet.ReadMapleString();
            MapInteractActor actor = MapEntityStorage.GetInteractActors(session.Player.MapId).FirstOrDefault(x => x.Uuid == uuid);
            if (actor == null)
            {
                return;
            }
            if (actor.Type == InteractActorType.Binoculars)
            {
                List<QuestStatus> questList = session.Player.QuestList;
                foreach (QuestStatus item in questList.Where(x => x.Basic.QuestID >= 72000000 && x.Condition != null))
                {
                    QuestCondition condition = item.Condition.FirstOrDefault(x => x.Code != "" && int.Parse(x.Code) == actor.Id);
                    if (condition == null)
                    {
                        continue;
                    }

                    item.Completed = true;
                    item.CompleteTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    session.Send(QuestPacket.CompleteQuest(item.Basic.QuestID));
                    break;
                }
            }

            session.Send(InteractActorPacket.UseObject(actor));
            session.Send(InteractActorPacket.Extra(actor));
        }
    }
}
