using System.Diagnostics;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class DungeonHandler : GamePacketHandler<DungeonHandler>
{
    public override RecvOp OpCode => RecvOp.RoomDungeon;

    private enum Mode : byte
    {
        ResetDungeon = 0x01,
        CreateDungeon = 0x02,
        EnterDungeonButton = 0x03,
        EnterDungeonPortal = 0x0A,
        AddRewards = 0x8,
        GetHelp = 0x10,
        Veteran = 0x11,
        Favorite = 0x19
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.EnterDungeonPortal:
                HandleEnterDungeonPortal(session);
                break;
            case Mode.CreateDungeon:
                HandleCreateDungeon(session, packet);
                break;
            case Mode.EnterDungeonButton:
                HandleEnterDungeonButton(session);
                break;
            case Mode.AddRewards:
                HandleAddRewards(session, packet);
                break;
            case Mode.GetHelp:
                HandleGetHelp(session, packet);
                break;
            case Mode.Veteran:
                HandleVeteran(session, packet);
                break;
            case Mode.Favorite:
                HandleFavorite(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    //if player has both party dungeon session always join the solo session
    //because the solo session would be -1 outside of the solo 
    public static void HandleEnterDungeonPortal(GameSession session)
    {
        //if player dungeon session is -1 they must be in a group dungeon, otherwise
        //they would not be in a dungeon lobby where this function is called from
        int dungeonSessionId = session.Player.DungeonSessionId == -1
            ? (session.Player.Party?.DungeonSessionId) ?? -1
            : session.Player.DungeonSessionId;

        Debug.Assert(dungeonSessionId != -1);

        DungeonSession dungeonSession = GameServer.DungeonManager.GetBySessionId(dungeonSessionId);

        Debug.Assert(dungeonSession != null);

        session.Player.Warp(dungeonSession.DungeonMapIds.First(), instanceId: dungeonSession.DungeonInstanceId, setReturnData: false);

    }

    public static void HandleCreateDungeon(GameSession session, PacketReader packet)
    {
        int dungeonId = packet.ReadInt();
        bool groupEnter = packet.ReadBool();
        Player player = session.Player;

        if (player.DungeonSessionId != -1)
        {
            session.SendNotice("Leave your current dungeon before opening another.");
            return;
        }

        int dungeonLobbyId = DungeonStorage.GetDungeonById(dungeonId).LobbyFieldId;
        MapPlayerSpawn spawn = MapEntityMetadataStorage.GetRandomPlayerSpawn(dungeonLobbyId);

        DungeonSession dungeonSession = GameServer.DungeonManager.CreateDungeonSession(dungeonId, groupEnter ? DungeonType.Group : DungeonType.Solo);

        //TODO: Send packet that greys out enter alone / enter as party when already in a dungeon session (sendRoomDungeon packet/s).
        //the session belongs to the party leader
        if (groupEnter)
        {
            Party party = player.Party;
            if (party.DungeonSessionId != -1)
            {
                session.SendNotice("Need to reset dungeon before entering another instance");
                return;
            }
            foreach (Player member in party.Members)
            {
                if (member.DungeonSessionId != -1)
                {
                    session.SendNotice($"{member.Name} is still in a Dungeon Instance.");
                    return;
                }
            }
            party.DungeonSessionId = dungeonSession.SessionId;
            party.BroadcastPacketParty(PartyPacket.PartyHelp(dungeonId));

            //This packet sets the banner in the dungeon that displays the dungeonname and the playersize it was created for.
            party.BroadcastPacketParty(DungeonWaitPacket.Show(dungeonId, DungeonStorage.GetDungeonById(dungeonId).MaxUserCount));
            //TODO: Update Party with dungeon Info via party packets (0d,0e and others are involved).
        }
        else // solo join dungeon
        {
            player.DungeonSessionId = dungeonSession.SessionId;
        }
        session.Player.Warp(dungeonLobbyId, instanceId: dungeonSession.DungeonInstanceId);
        //TODO: things after map is created here: spawn doctor npc.

    }

    //party dungeon only button
    public static void HandleEnterDungeonButton(GameSession session)
    {
        Party party = session.Player.Party;
        DungeonSession dungeonSession = GameServer.DungeonManager.GetBySessionId(party.DungeonSessionId);
        if (dungeonSession == null) //Can be removed when enter dungeon button is removed on dungeonsession deletion.
        {
            return;
        }

        if (dungeonSession.IsDungeonReservedField(session.Player.MapId, (int) session.Player.InstanceId))
        {
            session.SendNotice("You are already in the dungeon");
            return;
        }

        session.Player.Warp(dungeonSession.DungeonLobbyId, instanceId: dungeonSession.DungeonInstanceId);
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
        if (session.Player.DungeonHelperAccessTime > session.ClientTick)
        {
            session.Send(PartyPacket.DungeonHelperCooldown(session.Player.DungeonHelperAccessTime - session.ClientTick));
            return;
        }

        Party party = session.Player.Party;
        if (party is null)
        {
            party = new(session.Player);
            GameServer.PartyManager.AddParty(party);

            session.Send(PartyPacket.Create(party, false));
            session.Send(PartyPacket.PartyHelp(dungeonId));
            MapleServer.BroadcastPacketAll(DungeonHelperPacket.BroadcastAssist(party, dungeonId));
            return;
        }

        session.Player.DungeonHelperAccessTime = session.ClientTick + 30000; // 30 second cooldown

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
