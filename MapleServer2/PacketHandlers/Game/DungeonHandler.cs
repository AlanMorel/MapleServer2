﻿using System.Diagnostics;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
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
            case Mode.ResetDungeon:
                HandleResetDungeon(session);
                break;
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

    private static void HandleResetDungeon(GameSession session)
    {
        Party? party = session.Player.Party;
        Debug.Assert(party != null, "party was null - should never happen");
        DungeonSession? partyDungeonSession = GameServer.DungeonManager.GetBySessionId(party.DungeonSessionId);
        Debug.Assert(partyDungeonSession != null, "partyDungeonSession was null - should never happen");

        foreach (Player player in party.Members)
        {
            if (partyDungeonSession.IsDungeonReservedField(player.MapId, (int) player.InstanceId))
            {
                session.SendNotice($"{player.Name} is still in the dungeon");
                return;
            }
        }

        foreach (int mapId in partyDungeonSession.DungeonMapIds)
        {

            FieldManagerFactory.ReleaseManagerById(mapId, partyDungeonSession.DungeonInstanceId);
        }

        FieldManagerFactory.ReleaseManagerById(partyDungeonSession.DungeonLobbyId, partyDungeonSession.DungeonInstanceId);
        partyDungeonSession.IsReset = true;
        session.SendNotice("Dungeon has been reset");
    }

    //if player has both party dungeon session always join the solo session
    //because the solo session would be -1 outside of the solo 
    private static void HandleEnterDungeonPortal(GameSession session)
    {
        Player player = session.Player;

        //if player dungeon session is -1, aka they are in a player dungeon session, they must be in a group dungeon, otherwise
        //they would not be in a dungeon lobby where this function is called from
        int dungeonSessionId = player.DungeonSessionId;

        if (player.DungeonSessionId == -1)
        {
            dungeonSessionId = player.Party?.DungeonSessionId ?? -1;
        }

        Debug.Assert(dungeonSessionId != -1, "Should never happen");
        DungeonSession? dungeonSession = GameServer.DungeonManager.GetBySessionId(dungeonSessionId);
        if (dungeonSession is null)
        {
            return;
        }

        session.Player.Warp(dungeonSession.DungeonMapIds.First(), instanceId: dungeonSession.DungeonInstanceId, setReturnData: false);
    }

    private static void HandleCreateDungeon(GameSession session, PacketReader packet)
    {
        int dungeonId = packet.ReadInt();
        bool groupEnter = packet.ReadBool();
        Player player = session.Player;

        //is player in solo dungeon?
        if (player.HasDungeonSession())
        {
            session.SendNotice("Leave your current dungeon before opening another.");
            return;
        }

        DungeonType dungeonType = groupEnter ? DungeonType.Group : DungeonType.Solo;

        DungeonMetadata? dungeonById = DungeonStorage.GetDungeonById(dungeonId);
        if (dungeonById is null)
        {
            return;
        }

        int dungeonLobbyFieldId = dungeonById.LobbyFieldId;
        MapPlayerSpawn? spawn = MapEntityMetadataStorage.GetPlayerSpawns(dungeonLobbyFieldId)?.FirstOrDefault(); //TODO: spawn at correct coords

        if (dungeonType == DungeonType.Solo)
        {
            DungeonSession dungeonSession = GameServer.DungeonManager.CreateDungeonSession(dungeonId, dungeonType);
            session.Player.Warp(dungeonLobbyFieldId, instanceId: dungeonSession.DungeonInstanceId);
            player.DungeonSessionId = dungeonSession.SessionId;
        }

        //TODO: Send packet that greys out enter alone / enter as party when already in a dungeon session (sendRoomDungeon packet/s).
        //the session belongs to the party leader
        if (dungeonType == DungeonType.Group)
        {
            Party? party = player.Party;
            // the button to create a group dungeon only appears when in party
            Debug.Assert(party != null, "No party when entering group dungeon");

            if (party.IsAnyMemberInSoloDungeon())
            {
                return;
            }

            if (party.DungeonSessionId != -1) //there is an existing dungeon session
            {
                DungeonSession? partyDungeonSession = GameServer.DungeonManager.GetBySessionId(party.DungeonSessionId);
                Debug.Assert(partyDungeonSession != null,
                    "There should always be a dungeon session if there is a dungeonSessionId != -1");

                //TODO: resetting a dungeon resets IsReset to true, so resetting -> enter dungeon will update the dungeon session with a new dungeon
                //TODO: When resetting an instance removes the enter dungeon button, this behavior will not be possible
                //TODO: until this is done, the current behavior seems sensible: it creates a new dungeon as intended by the player
                if (partyDungeonSession.IsReset == false)
                {
                    session.SendNotice("Need to reset dungeon before entering another instance");
                    return;
                }

                if (partyDungeonSession.IsPartyMemberInDungeonField(party))
                {
                    return;
                }

                partyDungeonSession.UpdateDungeonSession(dungeonId);
                session.SendNotice("Dungeon session updated");
                party.BroadcastPacketParty(PartyPacket.PartyHelp(dungeonId));
                //set the banner in the dungeon that displays the dungeonname and the playersize it was created for.
                party.BroadcastPacketParty(DungeonWaitPacket.Show(dungeonId, dungeonById.MaxUserCount));
                session.Player.Warp(dungeonLobbyFieldId, instanceId: partyDungeonSession.DungeonInstanceId);
                return;
            }

            //create new group dungeon session
            DungeonSession dungeonSession = GameServer.DungeonManager.CreateDungeonSession(dungeonId, dungeonType);
            party.DungeonSessionId = dungeonSession.SessionId;
            session.SendNotice("New Group Dungeon Created");
            party.BroadcastPacketParty(PartyPacket.PartyHelp(dungeonId));
            //set the banner in the dungeon that displays the dungeonname and the playersize it was created for.
            party.BroadcastPacketParty(DungeonWaitPacket.Show(dungeonId, dungeonById.MaxUserCount));
            session.Player.Warp(dungeonLobbyFieldId, instanceId: dungeonSession.DungeonInstanceId);
            //TODO: Update Party with dungeon Info via party packets (0d,0e and others are involved).
        }
    }

    //party dungeon only button
    private static void HandleEnterDungeonButton(GameSession session)
    {
        Party? party = session.Player.Party;
        if (party is null)
        {
            return;
        }

        DungeonSession? dungeonSession = GameServer.DungeonManager.GetBySessionId(party.DungeonSessionId);
        if (dungeonSession is null)
        {
            return;
        }

        if (dungeonSession.IsCompleted)
        {
            session.SendNotice("The dungeon is expired");
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

        Party? party = session.Player.Party;
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
