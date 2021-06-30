using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;
using MapleServer2.Data.Static;
using Maple2Storage.Types.Metadata;
namespace MapleServer2.PacketHandlers.Game
{
    public class DungeonHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.ROOM_DUNGEON;

        public DungeonHandler(ILogger<DungeonHandler> logger) : base(logger) { }

        private enum DungeonMode : byte
        {
            ResetDungeon = 0x01,
            EnterDungeonRoom = 0x02,
            ConfirmEnterDungeonRoom = 0x03,
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
                case DungeonMode.ConfirmEnterDungeonRoom:
                    HandleConfirmEnterDungeonRoom(session, packet);
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
            //if existing dungeonlobby, return
            int dungeonId = packet.ReadInt();
            bool groupEnter = packet.ReadBool();
            Player player = session.Player;

            int dungeonLobbyId = DungeonStorage.GetDungeonByDungeonId(dungeonId).LobbyFieldId;
            MapPlayerSpawn spawn = MapEntityStorage.GetRandomPlayerSpawn(dungeonLobbyId);

            DungeonSession dungeonSession = GameServer.DungeonManager.CreateDungeonSession(dungeonId);

            //partyleader should not have the option to enteralone or enter with party, but have an if check until that is the case.
            //if solo enter and dungeonsession != -1 complain that you gotta leave first before making a new dungeon
            //set dungeonsesion to-1 leaving field, except when coords are dungeonsession.dungeonid.lobbyfieldid if solo
            //for party dont reset until 1 is in lobby or all of party are offline, dungeonsession.checkresetcondition() ?
            if (groupEnter)
            {

                Party party = GameServer.PartyManager.GetPartyById(session.Player.PartyId);
                foreach (Player member in party.Members)
                {
                    if (member.DungeonSessionId != -1)
                    {
                        session.SendNotice($"{member.Name} is still in a Dungeon Instance.");
                        return;
                    }
                }
                if (party.DungeonSessionId != -1)
                {
                    session.SendNotice("Need to reset dungeon before entering another instance");
                    return;
                }
                //group takes dungeonsession of leader, because this packet is coming from leader.
                party.DungeonSessionId = dungeonSession.SessionId;
                party.BroadcastParty(session => session.SendNotice($"You are now member of dungeonsession {dungeonSession.SessionId}"));
                //    party.BroadcastParty(session => session.Send(PartyPacket.UpdatePlayer(session.Player)));
                //  party.BroadcastPacketParty(PartyPacket.UpdatePlayer(session.Player));
                //       party.BroadcastPacketParty(PartyPacket.PartyHelp(dungeonId, 1));
                //        party.BroadcastPacketParty(DungeonWaitPacket.Show(dungeonId, DungeonStorage.GetDungeonByDungeonId(dungeonId).MaxUserCount));

                //if party dungeonsession -1 and remove dungeonsession from list , get new session, loop over every partymember and assign the same session as first person, on party join assign dungeon session
                //or give dungeon session to party instead of player here, still set player to -1 in that case., dungesonesseionget parameter for party
                //dungeonmanager check for existing session

                //add warp to map, warp to coord

                // session.Send(DungeonWaitPacket.Show(dungeonId, DungeonStorage.GetDungeonByDungeonId(dungeonId).MaxUserCount));
            }
            //DungeonSession dungeonSession = GameServer.DungeonManager.GetDungeonSession(player, dungeonId);

            if (!groupEnter)
            {
                player.DungeonSessionId = dungeonSession.SessionId;
            }
            session.Player.Warp(spawn.Coord.ToFloat(), spawn.Rotation.ToFloat(), dungeonLobbyId, dungeonSession.DungeonInstanceId);

        }
        public static void HandleConfirmEnterDungeonRoom(GameSession session, PacketReader packet)
        {
            //send session.player to dungeonlobby
            System.Console.WriteLine($"Send to dungeon lobby {session.Player.PartyId}");
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
