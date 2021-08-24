using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data;
using MapleServer2.Database;
using MapleServer2.Extensions;
using MapleServer2.Network;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Servers.Login;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Common
{
    public class ResponseKeyHandler : CommonPacketHandler
    {
        public override RecvOp OpCode => RecvOp.RESPONSE_KEY;

        public ResponseKeyHandler(ILogger<ResponseKeyHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            long accountId = packet.ReadLong();
            AuthData authData = AuthStorage.GetData(accountId);

            // Backwards seeking because we read accountId here
            packet.Skip(-8);
            HandleCommon(session, packet);

            Player player = DatabaseManager.Characters.FindById(authData.CharacterId);
            if (player == default)
            {
                throw new ArgumentException("Character not found!");
            }

            player.Session = session;
            player.Wallet.Meso.Session = session;
            player.Wallet.ValorToken.Session = session;
            player.Wallet.Treva.Session = session;
            player.Wallet.Rue.Session = session;
            player.Wallet.HaviFruit.Session = session;
            player.Wallet.MesoToken.Session = session;
            player.Wallet.Bank.Session = session;
            player.Account.Meret.Session = session;
            player.Account.GameMeret.Session = session;
            player.Account.EventMeret.Session = session;
            player.Levels.Player = player;

            session.InitPlayer(player);

            //session.Send(0x27, 0x01); // Meret market related...?

            session.Send(LoginPacket.LoginRequired(accountId));

            if (player.GuildId != 0)
            {
                Guild guild = GameServer.GuildManager.GetGuildById(player.GuildId);
                player.Guild = guild;
                player.GuildMember = guild.Members.First(x => x.Id == player.CharacterId);
                session.Send(GuildPacket.UpdateGuild(guild));
                session.Send(GuildPacket.MemberJoin(player));
            }
            session.Send(BuddyPacket.Initialize());
            session.Send(BuddyPacket.LoadList(player));
            session.Send(BuddyPacket.EndList(player.BuddyList.Count));

            // Meret market
            //session.Send("6E 00 0B 00 00 00 00 00 00 00 00 00 00 00 00".ToByteArray());
            //session.Send("6E 00 0C 00 00 00 00".ToByteArray());
            // UserConditionEvent
            //session.Send("BF 00 00 00 00 00 00".ToByteArray());
            // PCBangBonus
            //session.Send("A7 00 03 00 00 00 00 00 00 00 00 00 00 00 00 00".ToByteArray());
            TimeSyncPacket.SetInitial1();
            TimeSyncPacket.SetInitial2();
            TimeSyncPacket.Request();
            session.Send(StatPacket.SetStats(session.FieldPlayer));
            // TODO: Grab Hp/Spirit/Stam from last login
            player.Stats.InitializePools(player.Stats[PlayerStatId.Hp].Max, player.Stats[PlayerStatId.Spirit].Max, player.Stats[PlayerStatId.Stamina].Max);

            session.SyncTicks();
            session.Send(DynamicChannelPacket.DynamicChannel());
            session.Send(ServerEnterPacket.Enter(session));
            // SendUgc f(0x16), SendCash f(0x09), SendContentShutdown f(0x01, 0x04), SendPvp f(0x0C)
            PacketWriter pWriter = PacketWriter.Of(SendOp.SYNC_NUMBER);
            pWriter.WriteByte();
            session.Send(pWriter);
            // 0x112, Prestige f(0x00, 0x07)
            session.Send(PrestigePacket.Prestige(player));

            // Load inventory tabs
            foreach (InventoryTab tab in Enum.GetValues(typeof(InventoryTab)))
            {
                InventoryController.LoadInventoryTab(session, tab);
            }

            if (player.Account.HomeId != 0)
            {
                Home home = GameServer.HomeManager.GetHome(player.Account.HomeId);
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

            IEnumerable<List<QuestStatus>> packetCount = SplitList(player.QuestList, 200); // Split the quest list in 200 quests per packet
            foreach (List<QuestStatus> item in packetCount)
            {
                session.Send(QuestPacket.SendQuests(item));
            }
            session.Send(QuestPacket.EndList());

            session.Send(TrophyPacket.WriteTableStart());
            List<Trophy> trophyList = new List<Trophy>(player.TrophyData.Values);
            IEnumerable<List<Trophy>> trophyListPackets = SplitList(trophyList, 60);

            foreach (List<Trophy> trophy in trophyListPackets)
            {
                session.Send(TrophyPacket.WriteTableContent(trophy));
            }

            // SendQuest, SendAchieve, SendManufacturer, SendUserMaid
            session.Send(UserEnvPacket.SetTitles(player));
            session.Send(UserEnvPacket.Send04());
            session.Send(UserEnvPacket.Send05());
            session.Send(UserEnvPacket.Send08());
            session.Send(UserEnvPacket.Send09());
            session.Send(UserEnvPacket.Send10());
            session.Send(UserEnvPacket.Send12());

            // SendMeretMarket f(0xC9)
            session.Send(FishingPacket.LoadAlbum(player));
            // SendPvp f(0x16,0x17), ResponsePet f(0x07), 0xF6
            // CharacterAbility
            // E1 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00 00

            // Key bindings and skill slots would normally be loaded from a database
            // If the character is not a new character, this is what we would send
            session.Send(KeyTablePacket.SendFullOptions(player.GameOptions));

            if (player.MapId == (int) Map.UnknownLocation) // tutorial map
            {
                session.Send(KeyTablePacket.AskKeyboardOrMouse());
            }

            // SendKeyTable f(0x00), SendGuideRecord f(0x03), GameEvent f(0x00)
            // SendBannerList f(0x19), SendRoomDungeon f(0x05, 0x14, 0x17)
            // FieldEntrance
            session.Send("19 00 00 65 00 00 00 29 7C 7D 01 0C 4D A1 6F 01 0C D3 1A 5F 01 0C EF 03 00 00 01 A2 3C 31 01 0C 3F 0C B7 0D 01 6B 55 5F 01 0C 3A 77 31 01 0C B1 98 BA 01 0C 03 90 5F 01 0C F9 7A 40 01 0C 91 B5 40 01 0C F9 57 31 01 0C 2F C7 BB 0D 01 81 97 7D 01 0C C2 70 5F 01 0C 51 96 40 01 0C B9 38 31 01 0C 41 78 7D 01 0C 65 9D 6F 01 0C 83 51 5F 01 0C 52 73 31 01 0C FF E0 B8 0D 01 11 77 40 01 0C A9 B1 40 01 0C 11 54 31 01 0C DA 6C 5F 01 0C 69 92 40 01 0C D1 34 31 01 0C 7D 99 6F 01 0C 03 13 5F 01 0C 69 6F 31 01 0C 32 88 5F 01 0C 9B 4D 5F 01 0C FF 6F B6 0D 01 29 73 40 01 0C C1 AD 40 01 0C 29 50 31 01 0C 81 8E 40 01 0C E9 30 31 01 0C 09 CE 8C 01 0C 95 95 6F 01 0C 1B 0F 5F 01 0C 4A 84 5F 01 0C B3 49 5F 01 0C 82 6B 31 01 0C 4F 15 BC 0D 01 F9 8C BA 01 00 D9 A9 40 01 0C 41 4C 31 01 0C EF B9 B8 0D 01 99 8A 40 01 0C 21 CA 8C 01 0C 01 AD 6F 01 0C 33 0B 5F 01 0C 99 67 31 01 0C 62 80 5F 01 0C CB 45 5F 01 0C 79 08 9C 01 0C F1 A5 40 01 0C E1 87 7D 01 0C BB 9B 5F 01 0C B1 86 40 01 0C 39 C6 8C 01 0C 7A 7C 5F 01 0C B2 63 31 01 0C 29 85 BA 01 0E 91 04 9C 01 0C 09 A2 40 01 0C 71 44 31 01 0C F9 83 7D 01 0C 1D A9 6F 01 0C D3 97 5F 01 0C C9 82 40 01 0C 51 C2 8C 01 0C 61 BD 40 01 0C C9 5F 31 01 0C 51 9F 7D 01 0C 92 78 5F 01 0C 0F 08 B9 0D 01 A9 00 9C 01 0C 89 40 31 01 0C 11 80 7D 01 0C 35 A5 6F 01 0C BB 1E 5F 01 0C 53 59 5F 01 0C E9 03 00 00 01 22 7B 31 01 0C EB 93 5F 01 0C EA 03 00 00 01 E1 7E 40 01 0C 69 BE 8C 01 0C 79 B9 40 01 0C E1 5B 31 01 0C EB 03 00 00 01 69 9B 7D 01 0C AA 74 5F 01 0C EC 03 00 00 01 ED 03 00 00 01 C1 FC 9B 01 0C EE 03 00 00 01".ToByteArray());
            // 0xF0, ResponsePet P(0F 01)
            // RequestFieldEnter
            //session.Send("16 00 00 41 75 19 03 00 01 8A 42 0F 00 00 00 00 00 00 C0 28 C4 00 40 03 44 00 00 16 44 00 00 00 00 00 00 00 00 55 FF 33 42 E8 49 01 00".ToByteArray());
            session.Send(FieldPacket.RequestEnter(player));

            Party party = GameServer.PartyManager.GetPartyByMember(player.CharacterId);
            if (party != null)
            {
                player.Party = party;
                session.Send(PartyPacket.Create(party, false));
            }

            // SendUgc: 15 01 00 00 00 00 00 00 00 00 00 00 00 4B 00 00 00
            // SendHomeCommand: 00 E1 0F 26 89 7F 98 3C 26 00 00 00 00 00 00 00 00

            // Client is supposed to request SetSessionServerTick packet but it is not.
            // As a temporary fix, we're sending the packet to set it
            session.Send(TimeSyncPacket.SetSessionServerTick(0));
            //session.Send("B9 00 00 E1 0F 26 89 7F 98 3C 26 00 00 00 00 00 00 00 00".ToByteArray());
            session.Send(ServerEnterPacket.Confirm());

            //session.Send(0xF0, 0x00, 0x1F, 0x78, 0x00, 0x00, 0x00, 0x3C, 0x00, 0x00, 0x00);
            //session.Send(0x28, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00);
            //session.Send(ServerEnterPacket.Confirm());
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

            Logger.Info($"LOGIN USER: {accountId}");
            AuthData authData = AuthStorage.GetData(accountId);
            if (authData == null)
            {
                throw new ArgumentException("Attempted connection to game with unauthorized account");
            }
            else if (tokenA != authData.TokenA || tokenB != authData.TokenB)
            {
                throw new ArgumentException("Attempted login with invalid tokens...");
            }

            session.Send((byte) SendOp.MOVE_RESULT, 0x00, 0x00);
        }

        public static IEnumerable<List<T>> SplitList<T>(List<T> locations, int nSize = 30)
        {
            for (int i = 0; i < locations.Count; i += nSize)
            {
                yield return locations.GetRange(i, Math.Min(nSize, locations.Count - i));
            }
        }
    }
}
