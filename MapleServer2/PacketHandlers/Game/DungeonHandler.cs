using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class DungeonHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.ROOM_DUNGEON;

        public DungeonHandler(ILogger<DungeonHandler> logger) : base(logger) { }

        private enum DungeonMode : byte
        {
            EnterDungeonRoom = 0x02,
            AddRewards = 0x8,
            GetHelp = 0x10,
            Veteran = 0x11,
            Favorite = 0x19,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            DungeonMode mode = (DungeonMode) packet.ReadByte();

            switch (mode)
            {
                case DungeonMode.EnterDungeonRoom:
                    HandleEnterDungeonRoom(session, packet);
                    break;
                case DungeonMode.AddRewards:
                    HandleAddRewards(session, packet);
                    break;
                case DungeonMode.GetHelp:
                    HandleGetHelp(session, packet);
                    break;
                case DungeonMode.Veteran:
                    HandleVeteran(session, packet);
                    break;
                case DungeonMode.Favorite:
                    HandleFavorite(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }
        public static void HandleEnterDungeonRoom(GameSession session, PacketReader packet)
        {
            int dungeonId = packet.ReadInt();
        }

        private static void HandleAddRewards(GameSession session, PacketReader packet)
        {
            int dungeonId = packet.ReadInt();

            session.Send(DungeonPacket.UpdateDungeonInfo(3, dungeonId));
            // session.Send(DungeonPacket.UpdateDungeon(dungeonId, toggle));
        }

        private static void HandleGetHelp(GameSession session, PacketReader packet)
        {
            int dungeonId = packet.ReadInt();

            if (session.Player.PartyId == 0)
            {
                Party newParty = new(session.Player);
                GameServer.PartyManager.AddParty(newParty);

                session.Send(PartyPacket.Create(newParty));
                session.Send(PartyPacket.PartyHelp(dungeonId));
                MapleServer.BroadcastPacketAll(DungeonHelperPacket.BroadcastAssist(newParty, dungeonId));

                return;
            }

            Party party = GameServer.PartyManager.GetPartyById(session.Player.PartyId);

            party.BroadcastPacketParty(PartyPacket.PartyHelp(dungeonId));
            MapleServer.BroadcastPacketAll(DungeonHelperPacket.BroadcastAssist(party, dungeonId));
        }

        private static void HandleVeteran(GameSession session, PacketReader packet)
        {
            int dungeonId = packet.ReadInt();

            session.Send(DungeonPacket.UpdateDungeonInfo(4, dungeonId));
            // session.Send(DungeonPacket.UpdateDungeon(dungeonId, toggle));
        }

        private static void HandleFavorite(GameSession session, PacketReader packet)
        {
            int dungeonId = packet.ReadInt();
            byte toggle = packet.ReadByte();

            session.Send(DungeonPacket.UpdateDungeonInfo(5, dungeonId));
            // session.Send(DungeonPacket.UpdateDungeon(dungeonId, toggle));
        }
    }
}
