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

public class UgcHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.UGC;

    private enum UgcMode : byte
    {
        Upload = 0x01,
        ConfirmationPacket = 0x03,
        ProfilePicture = 0x0B
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
            default:
                IPacketHandler<GameSession>.LogUnknownMode(function);
                break;
        }
    }

    private static void HandleUpload(GameSession session, PacketReader packet)
    {
        packet.ReadLong();
        UgcType type = (UgcType) packet.ReadByte();
        packet.ReadByte();
        packet.ReadByte();
        packet.ReadInt();
        long accountId = packet.ReadLong();
        long characterId = packet.ReadLong();
        packet.ReadLong();
        packet.ReadInt();
        packet.ReadShort();
        packet.ReadShort();

        Ugc newUgc = null;
        switch (type)
        {
            case UgcType.Furniture or UgcType.Item:
                newUgc = HandleCreateUgcItem();
                break;
            case UgcType.GuildEmblem:
                newUgc = new(session.Player.Guild.Name, characterId, session.Player.Name, accountId, 0, UgcType.GuildEmblem);
                break;
            default:
                Logger.Warning("Unknown UGC type {0}", type);
                break;
        }

        if (newUgc is null)
        {
            return;
        }

        session.Send(UgcPacket.CreateUgc(newUgc));

        Ugc HandleCreateUgcItem()
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
                        session.SendNotice("You don't have enough mesos.");
                        return null;
                    case CurrencyType.Meret when !session.Player.Account.Meret.Modify(-cost):
                        session.SendNotice("You don't have enough merets.");
                        return null;
                }
            }

            Item item = new(itemId, amount)
            {
                Rarity = metadata.Rarity,
                Ugc = new(itemName, characterId, session.Player.Name, accountId, metadata.SalePrice, type),
                IsTemplate = true
            };
            DatabaseManager.Items.Update(item);
            return item.Ugc;
        }
    }

    private static void HandleConfirmationPacket(GameSession session, PacketReader packet)
    {
        UgcType type = (UgcType) packet.ReadByte();
        packet.ReadByte();
        packet.ReadByte();
        packet.ReadInt();
        long accountId = packet.ReadLong();
        long characterId = packet.ReadLong();
        packet.ReadInt();
        long ugcUid = packet.ReadLong();
        string ugcGuid = packet.ReadUnicodeString();

        if (accountId != session.Player.Account.Id || characterId != session.Player.CharacterId || ugcUid == 0)
        {
            return;
        }

        Ugc ugc = null;
        switch (type)
        {
            case UgcType.Furniture or UgcType.Item:
                ugc = HandleItemPacket();
                break;
            case UgcType.GuildEmblem:
                ugc = HandleGuildPacket();
                break;
        }

        if (ugc is null)
        {
            return;
        }

        session.Send(UgcPacket.SetUgcUrl(ugc));

        Ugc HandleGuildPacket()
        {
            Ugc ugc2 = DatabaseManager.Ugc.FindByUid(ugcUid);
            Guild guild = GameServer.GuildManager.GetGuildById(session.Player.Guild.Id);
            if (ugc2 is null || guild is null || ugc2.Guid != Guid.Parse(ugcGuid))
            {
                return null;
            }

            guild.Emblem = ugc2.Url;
            DatabaseManager.Guilds.UpdateEmblem(guild.Id, ugc2.Url);

            guild.BroadcastPacketGuild(GuildPacket.ChangeEmblemUrl(ugc2.Url));
            guild.BroadcastPacketGuild(GuildPacket.GuildNoticeEmblemChange(session.Player.Name, ugc2.Url));
            return ugc2;
        }

        Ugc HandleItemPacket()
        {
            Item item = DatabaseManager.Items.FindByUgcUid(ugcUid);
            if (item is null)
            {
                return null;
            }

            item.SetMetadataValues();

            session.Player.Inventory.AddItem(session, item, true);
            switch (item.Ugc.Type)
            {
                case UgcType.Furniture:
                    session.Send(UgcPacket.UpdateUgcFurnishing(session.Player.FieldPlayer, item));
                    break;
                case UgcType.Item:
                    session.Send(UgcPacket.UpdateUgcItem(session.Player.FieldPlayer, item));
                    break;
            }

            return item.Ugc;
        }
    }

    private static void HandleProfilePicture(GameSession session, PacketReader packet)
    {
        string path = packet.ReadUnicodeString();
        session.Player.ProfileUrl = path;
        DatabaseManager.Characters.UpdateProfileUrl(session.Player.CharacterId, path);

        session.FieldManager.BroadcastPacket(UgcPacket.SetProfilePictureUrl(session.Player.FieldPlayer.ObjectId, session.Player.CharacterId, path));
    }
}
