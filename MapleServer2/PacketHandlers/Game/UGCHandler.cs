using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class UgcHandler : GamePacketHandler<UgcHandler>
{
    public override RecvOp OpCode => RecvOp.UGC;

    private enum UgcMode : byte
    {
        Upload = 0x01,
        ConfirmationPacket = 0x03,
        ProfilePicture = 0x0B,
        LoadBanner = 0x12,
        UpdateBanner = 0x13
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        UgcMode function = (UgcMode) packet.ReadByte();
        switch (function)
        {
            case UgcMode.Upload:
                HandleUpload(session, packet);
                break;
            case UgcMode.ConfirmationPacket:
                HandleConfirmationPacket(session, packet);
                break;
            case UgcMode.ProfilePicture:
                HandleProfilePicture(session, packet);
                break;
            case UgcMode.LoadBanner:
                HandleLoadBanner(session, packet);
                break;
            case UgcMode.UpdateBanner:
                HandleUpdateBanner(session, packet);
                break;
            default:
                LogUnknownMode(function);
                break;
        }
    }

    private static void HandleUpload(GameSession session, PacketReader packet)
    {
        packet.ReadLong();
        UGCType type = (UGCType) packet.ReadByte();
        packet.ReadByte();
        packet.ReadByte();
        packet.ReadInt();
        long accountId = packet.ReadLong();
        long characterId = packet.ReadLong();
        packet.ReadLong();
        packet.ReadInt();
        packet.ReadShort();
        packet.ReadShort();

        UGC newUGC = null;
        switch (type)
        {
            case UGCType.Furniture or UGCType.Item or UGCType.Mount:
                newUGC = HandleCreateUGCItem(packet, session, characterId, accountId, type);
                break;
            case UGCType.GuildEmblem:
                newUGC = new($"{session.Player.Guild.Name} Emblem", characterId, session.Player.Name, accountId, 0, type);
                break;
            case UGCType.GuildBanner:
                packet.ReadLong(); // guild id
                int bannerId = packet.ReadInt();
                newUGC = new($"{session.Player.Guild.Name} Banner", characterId, session.Player.Name, accountId, 0, type, bannerId);
                break;
            case UGCType.Banner:
                newUGC = HandleCreateUGCBanner(session, packet);
                break;
            default:
                Logger.Warning("Unknown UGC type {0}", type);
                break;
        }

        if (newUGC is null)
        {
            return;
        }

        session.Send(UGCPacket.CreateUGC(newUGC));
    }

    private static UGC HandleCreateUGCItem(PacketReader packet, GameSession session, long characterId, long accountId, UGCType type)
    {
        long unk = packet.ReadLong(); // some kind of UID
        int itemId = packet.ReadInt();
        int amount = packet.ReadInt();
        string itemName = packet.ReadUnicodeString();
        packet.ReadByte();
        long cost = packet.ReadLong();
        bool useVoucher = packet.ReadBool();

        UgcDesignMetadata metadata = UgcDesignMetadataStorage.GetItem(itemId);
        if (metadata is null)
        {
            return null;
        }

        if (useVoucher)
        {
            Item voucher = session.Player.Inventory.GetAllByTag("FreeDesignCoupon").FirstOrDefault();
            if (voucher is null)
            {
                return null;
            }

            session.Player.Inventory.ConsumeItem(session, voucher.Uid, 1);
        }
        else
        {
            switch (metadata.CurrencyType)
            {
                case CurrencyType.Meso when !session.Player.Wallet.Meso.Modify(-cost):
                    return null;
                case CurrencyType.Meret when !session.Player.Account.Meret.Modify(-cost):
                    return null;
            }
        }

        Item item = new(itemId, amount)
        {
            Rarity = metadata.Rarity,
        };
        
        item.Ugc.UpdateItem(itemName, characterId, session.Player.Name, accountId, metadata.SalePrice, type);
        DatabaseManager.Items.Update(item);
        return item.Ugc;
    }

    private static UGC HandleCreateUGCBanner(GameSession session, PacketReader packet)
    {
        Player player = session.Player;

        long bannerId = packet.ReadLong();
        UGCBanner banner = GameServer.UGCBannerManager.GetBanner(bannerId);
        if (banner is null)
        {
            Logger.Warning("Banner {0} not found.", bannerId);
            return null;
        }

        // get metadata for prices
        AdBannerMetadata metadata = AdBannerMetadataStorage.GetMetadata(bannerId);
        if (metadata is null)
        {
            Logger.Warning("Banner {0} metadata not found.", bannerId);
        }


        byte count = packet.ReadByte();

        UGC newUgc = new($"AD Banner {bannerId}", player.CharacterId, player.Name, player.Account.Id, 0, UGCType.Banner);

        for (int i = 0; i < count; i++)
        {
            long id = packet.ReadLong();
            packet.ReadInt(); // 1
            packet.ReadLong(); // banner id
            packet.ReadInt(); // date as YYYYMMDD
            int hour = packet.ReadInt();
            packet.ReadLong();

            if (!player.Account.Meret.Modify(-metadata.Prices[hour]))
            {
                return null;
            }

            BannerSlot bannerSlot = banner.Slots.FirstOrDefault(x => x.Id == id);
            if (bannerSlot is null)
            {
                Logger.Warning("Invalid banner slot with id {id} and hour {hour}.", id, hour);
                return null;
            }

            bannerSlot.UGC = newUgc;
            DatabaseManager.BannerSlot.UpdateUGCUid(bannerSlot.Id, newUgc.Uid);
        }

        return newUgc;
    }

    private static void HandleConfirmationPacket(GameSession session, PacketReader packet)
    {
        UGCType type = (UGCType) packet.ReadByte();
        packet.ReadByte();
        packet.ReadByte();
        packet.ReadInt();
        long accountId = packet.ReadLong();
        long characterId = packet.ReadLong();
        packet.ReadInt();
        long ugcUid = packet.ReadLong();
        string ugcGuid = packet.ReadUnicodeString();

        Player player = session.Player;
        if (accountId != player.Account.Id || characterId != player.CharacterId || ugcUid == 0)
        {
            return;
        }

        UGC ugc = DatabaseManager.UGC.FindByUid(ugcUid);
        if (ugc is null || ugc.Guid != Guid.Parse(ugcGuid))
        {
            return;
        }

        session.Send(UGCPacket.SetUGCUrl(ugc));

        switch (type)
        {
            case UGCType.Furniture or UGCType.Item or UGCType.Mount:
                Item item = DatabaseManager.Items.FindByUgcUid(ugcUid);
                if (item is null)
                {
                    return;
                }

                item.SetMetadataValues();

                player.Inventory.AddItem(session, item, true);
                switch (item.Ugc.Type)
                {
                    case UGCType.Furniture:
                        session.Send(UGCPacket.UpdateUGCFurnishing(player.FieldPlayer, item));
                        break;
                    case UGCType.Item:
                        session.Send(UGCPacket.UpdateUGCItem(player.FieldPlayer, item));
                        break;
                    case UGCType.Mount:
                        session.Send(UGCPacket.UpdateUGCMount(player.FieldPlayer, item));
                        break;
                }

                break;
            case UGCType.GuildEmblem:
                {
                    Guild guild = GameServer.GuildManager.GetGuildById(player.Guild.Id);

                    guild.Emblem = ugc.Url;
                    DatabaseManager.Guilds.UpdateEmblem(guild.Id, ugc.Url);

                    guild.BroadcastPacketGuild(GuildPacket.ChangeEmblemUrl(ugc.Url));
                    guild.BroadcastPacketGuild(GuildPacket.GuildNoticeEmblemChange(player.Name, ugc.Url));
                    break;
                }
            case UGCType.GuildBanner:
                {
                    Guild guild = GameServer.GuildManager.GetGuildById(player.Guild.Id);

                    UGC oldUGCBanner = guild.Banners.FirstOrDefault(x => x.GuildPosterId == ugc.GuildPosterId);
                    if (oldUGCBanner is not null)
                    {
                        guild.Banners.Remove(oldUGCBanner);
                        DatabaseManager.UGC.Delete(oldUGCBanner.Uid);
                    }

                    guild.Banners.Add(ugc);
                    DatabaseManager.Guilds.UpdateBanners(guild.Id, guild.Banners);

                    guild.BroadcastPacketGuild(GuildPacket.UpdateBannerUrl(player, ugc));
                    break;
                }
            case UGCType.Banner:
                UGCBanner ugcBanner = GameServer.UGCBannerManager.UpdateBannerSlots(ugc);
                if (ugcBanner is null)
                {
                    return;
                }

                session.Send(UGCPacket.UpdateUGCBanner(ugcBanner));
                // TrophyManager.OnReserveBanner();
                break;
        }
    }

    private static void HandleProfilePicture(GameSession session, PacketReader packet)
    {
        string path = packet.ReadUnicodeString();
        session.Player.ProfileUrl = path;
        DatabaseManager.Characters.UpdateProfileUrl(session.Player.CharacterId, path);

        session.FieldManager.BroadcastPacket(UGCPacket.SetProfilePictureUrl(session.Player.FieldPlayer.ObjectId, session.Player.CharacterId, path));
    }

    private static void HandleLoadBanner(GameSession session, PacketReader packet)
    {
        int mapId = packet.ReadInt();

        session.Send(UGCPacket.LoadUGCBanner(GameServer.UGCBannerManager.GetBanners(mapId)));
    }

    private static void HandleUpdateBanner(GameSession session, PacketReader packet)
    {
        long bannerId = packet.ReadLong();
        int hourCount = packet.ReadInt();

        if (hourCount is < 0 or > 24)
        {
            return;
        }

        UGCBanner ugcBanner = GameServer.UGCBannerManager.GetBanner(bannerId);
        if (ugcBanner is null)
        {
            Logger.Error("Banner not found: " + bannerId);
            return;
        }

        List<BannerSlot> newSlots = new();
        for (int i = 0; i < hourCount; i++)
        {
            packet.ReadLong();
            packet.ReadInt(); // 1
            long bannerId2 = packet.ReadLong(); // bannerId
            int date = packet.ReadInt(); // date as YYYYMMDD
            int hour = packet.ReadInt();
            packet.ReadLong();

            if (ugcBanner.Slots.Any(x => x.Hour == hour && x.Date == date))
            {
                Logger.Warning("Banner slot already exists for hour {hour}.", hour);
                continue;
            }

            BannerSlot bannerSlot = new(bannerId2, date, hour);
            ugcBanner.Slots.Add(bannerSlot);
            newSlots.Add(bannerSlot);
        }

        GameServer.UGCBannerManager.UpdateBanner(ugcBanner);
        session.Send(UGCPacket.ReserveBannerSlots(ugcBanner.Id, newSlots));
    }
}
