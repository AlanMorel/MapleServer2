using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Network;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Common;

public class ResponseKeyHandler : CommonPacketHandler<ResponseKeyHandler>
{
    public override RecvOp OpCode => RecvOp.ResponseKey;

    public override void Handle(GameSession session, PacketReader packet)
    {
        long accountId = packet.ReadLong();
        if (accountId is 0)
        {
            Logger.Error("Account ID was 0. Login has failed or connection was made directly to game server.");
            session.Send(LoginResultPacket.SendLoginMode(LoginMode.SessionVerificationFailed));
            return;
        }

        AuthData authData = DatabaseManager.AuthData.GetByAccountId(accountId);
        if (authData is null)
        {
            Logger.Error("AuthData with account ID {accountId} was not found in database.", accountId);
            session.Send(LoginResultPacket.SendLoginMode(LoginMode.SystemErrorDB));
            return;
        }

        Player dbPlayer = DatabaseManager.Characters.FindPlayerById(authData.OnlineCharacterId, session);

        // Backwards seeking because we read accountId here
        packet.Skip(-8);
        HandleCommon(session, packet);

        session.InitPlayer(dbPlayer);

        Player player = session.Player;

        player.BuddyList = GameServer.BuddyManager.GetBuddies(player.CharacterId);
        player.Mailbox = GameServer.MailManager.GetMails(player.CharacterId);

        GameServer.PlayerManager.AddPlayer(player);
        GameServer.BuddyManager.SetFriendSessions(player);

        // Only send buddy login notification if player is not changing channels
        if (!player.IsMigrating)
        {
            player.UpdateBuddies();
        }

        if (player.GuildId != 0)
        {
            Guild guild = GameServer.GuildManager.GetGuildById(player.GuildId);
            player.Guild = guild;
            GuildMember guildMember = guild.Members.First(x => x.Id == player.CharacterId);
            guildMember.Player = player;
            player.GuildMember = guildMember;
            session.Send(GuildPacket.UpdateGuild(guild));
            guild.BroadcastPacketGuild(GuildPacket.UpdatePlayer(player));
            if (!player.IsMigrating)
            {
                guild.BroadcastPacketGuild(GuildPacket.MemberLoggedIn(player), session);
            }
        }

        // Get Clubs
        foreach (ClubMember member in player.ClubMembers)
        {
            Club club = GameServer.ClubManager.GetClubById(member.ClubId);
            club.Members.First(x => x.Player.CharacterId == player.CharacterId).Player = player;
            club.BroadcastPacketClub(ClubPacket.UpdateClub(club));
            if (!player.IsMigrating)
            {
                club.BroadcastPacketClub(ClubPacket.LoginNotice(player, club), session);
            }

            player.Clubs.Add(club);
            member.Player = player;
        }

        // Get Group Chats
        player.GroupChats = GameServer.GroupChatManager.GetGroupChatsByMember(player.CharacterId);
        foreach (GroupChat groupChat in player.GroupChats)
        {
            session.Send(GroupChatPacket.Update(groupChat));
            if (!player.IsMigrating)
            {
                groupChat.BroadcastPacketGroupChat(GroupChatPacket.LoginNotice(groupChat, player));
            }
        }

        session.Send(MushkingRoyaleSystemPacket.LoadStats(player.Account.MushkingRoyaleStats));
        session.Send(MushkingRoyaleSystemPacket.LoadMedals(player.Account));

        player.GetUnreadMailCount();
        session.Send(BuddyPacket.Initialize());
        session.Send(BuddyPacket.LoadList(player.BuddyList));
        session.Send(BuddyPacket.EndList(player.BuddyList.Count));

        // Meret market
        session.Player.GetMeretMarketPersonalListings();
        session.Player.GetMeretMarketSales();
        // UserConditionEvent
        //session.Send("BF 00 00 00 00 00 00".ToByteArray());
        // PCBangBonus
        //session.Send("A7 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00".ToByteArray());

        session.Send(TimeSyncPacket.SetInitial1());
        session.Send(TimeSyncPacket.SetInitial2());

        session.Send(StatPacket.SetStats(session.Player.FieldPlayer));
        // session.Send(StatPacket.SetStats(session.Player.FieldPlayer)); // Second packet is meant to send the stats initialized, for now we'll just send the first one

        session.Player.ClientTickSyncLoop();
        session.Send(DynamicChannelPacket.DynamicChannel(short.Parse(ConstantsMetadataStorage.GetConstant("ChannelCount"))));

        session.Send(ServerEnterPacket.Enter(session));
        session.Send(UGCPacket.Unknown22());
        session.Send(CashPacket.Unknown09());

        // SendContentShutdown f(0x01, 0x04)
        session.Send(PvpPacket.Mode0C());
        session.Send(SyncNumberPacket.Send());
        session.Send(SyncValuePacket.SetSyncValue(120000)); // unknown what this value means

        session.Send(PrestigePacket.SetLevels(player));
        session.Send(PrestigePacket.WeeklyMissions(player.PrestigeMissions));

        // Load inventory tabs
        foreach (InventoryTab tab in Enum.GetValues(typeof(InventoryTab)))
        {
            player.Inventory.LoadInventoryTab(session, tab);
        }

        if (player.Account.HomeId != 0)
        {
            Home home = GameServer.HomeManager.GetHomeById(player.Account.HomeId);
            player.Account.Home = home;
            session.Send(WarehouseInventoryPacket.StartList());
            int counter = 0;
            foreach (KeyValuePair<long, Item> kvp in home.WarehouseInventory)
            {
                session.Send(WarehouseInventoryPacket.Load(kvp.Value, ++counter));
            }

            session.Send(WarehouseInventoryPacket.EndList());

            session.Send(FurnishingInventoryPacket.StartList());
            foreach (Cube cube in home.FurnishingInventory.Values.Where(x => x.Item != null))
            {
                session.Send(FurnishingInventoryPacket.Load(cube));
            }

            session.Send(FurnishingInventoryPacket.EndList());
        }

        session.Send(QuestPacket.StartList());
        session.Send(QuestPacket.Packet1F());
        session.Send(QuestPacket.Packet20());

        IEnumerable<List<QuestStatus>> packetCount = player.QuestData.Values.ToList().SplitList(200); // Split the quest list in 200 quests per packet
        foreach (List<QuestStatus> item in packetCount)
        {
            session.Send(QuestPacket.SendQuests(item));
        }

        session.Send(QuestPacket.EndList());

        session.Send(TrophyPacket.WriteTableStart());
        List<Trophy> trophyList = new(player.TrophyData.Values);
        IEnumerable<List<Trophy>> trophyListPackets = trophyList.SplitList(60);

        foreach (List<Trophy> trophy in trophyListPackets)
        {
            session.Send(TrophyPacket.WriteTableContent(trophy));
        }

        // SendUserMaid
        session.Send(UserEnvPacket.SetTitles(player));
        session.Send(UserEnvPacket.Send04());
        session.Send(UserEnvPacket.Send05());
        session.Send(UserEnvPacket.UpdateLifeSkills(player.GatheringCount));
        session.Send(UserEnvPacket.Send09());
        session.Send(UserEnvPacket.Send10());
        session.Send(UserEnvPacket.Send12());

        session.Send(MeretMarketPacket.ModeC9());

        session.Send(FishingPacket.LoadAlbum(player));

        session.Send(PvpPacket.Mode16());
        session.Send(PvpPacket.Mode17());

        session.Send(ResponsePetPacket.Mode07());
        session.Send(CharacterAbilityPacket.Send());

        // LegionBattle (0xF6)

        // If the character is not a new character, this is what we would send
        session.Send(KeyTablePacket.SendFullOptions(player.GameOptions));

        if (player.MapId == (int) Map.UnknownLocation) // tutorial map
        {
            session.Send(KeyTablePacket.AskKeyboardOrMouse());
        }

        GameEventHelper.LoadEvents(session.Player);
        List<GameEvent> gameEvents = DatabaseManager.Events.FindAll();
        IEnumerable<List<GameEvent>> gameEventPackets = gameEvents.SplitList(5);
        foreach (List<GameEvent> gameEvent in gameEventPackets)
        {
            session.Send(GameEventPacket.Load(gameEvent));
        }

        //  SendGuideRecord f(0x03), RoomDungeon x1A

        session.Send(DungeonListPacket.DungeonList());

        // InGameRank F0

        session.Send(ResponsePetPacket.Mode0F());

        session.Send(RequestFieldEnterPacket.RequestEnter(player.FieldPlayer));

        Party party = GameServer.PartyManager.GetPartyByMember(player.CharacterId);
        if (party != null)
        {
            player.Party = party;
            if (!player.IsMigrating)
            {
                party.BroadcastPacketParty(PartyPacket.LoginNotice(player), session);
            }

            session.Send(PartyPacket.Create(party, false));
            party.BroadcastPacketParty(PartyPacket.UpdatePlayer(player));
            party.BroadcastPacketParty(PartyPacket.UpdateDungeonInfo(player));
        }

        player.IsMigrating = false;

        session.Send(UGCPacket.Unknown21());
        session.Send(HomeCommandPacket.LoadHome(player));
        session.Send(WorldPacket.Send());

        // player.TimeSyncLoop();
        // session.Send(TimeSyncPacket.SetSessionServerTick(0));

        session.Send(ChatStickerPacket.LoadChatSticker(player));

        session.Send(BypassKeyPacket.Send());

        //session.Send(0xF0, 0x00, 0x1F, 0x78, 0x00, 0x00, 0x00, 0x3C, 0x00, 0x00, 0x00);
    }

    public override void Handle(LoginSession session, PacketReader packet)
    {
        session.AccountId = packet.ReadLong();

        // Backwards seeking because we read accountId here
        packet.Skip(-8);
        HandleCommon(session, packet);
    }

    protected override void HandleCommon(Session session, PacketReader packet)
    {
        long accountId = packet.ReadLong();
        int tokenA = packet.ReadInt();
        int tokenB = packet.ReadInt();

        Logger.Information("LOGIN USER: {accountId}", accountId);
        AuthData authData = DatabaseManager.AuthData.GetByAccountId(accountId);
        if (authData == null)
        {
            throw new ArgumentException("Attempted connection to game with unauthorized account");
        }

        if (tokenA != authData.TokenA || tokenB != authData.TokenB)
        {
            throw new ArgumentException("Attempted login with invalid tokens...");
        }

        session.Send(RequestSystemInfo.Send());
        session.Send(MoveResultPacket.SendStatus(status: 0));
    }
}
