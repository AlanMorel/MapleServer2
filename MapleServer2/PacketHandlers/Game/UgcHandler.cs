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

    public UgcHandler() : base() { }

    private enum UgcMode : byte
    {
        CreateUGCItem = 0x01,
        AddUGCItem = 0x03,
        ProfilePicture = 0x0B
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        UgcMode function = (UgcMode) packet.ReadByte();
        switch (function)
        {
            case UgcMode.CreateUGCItem:
                HandleCreateUGCItem(session, packet);
                break;
            case UgcMode.AddUGCItem:
                HandleAddUGCItem(session, packet);
                break;
            case UgcMode.ProfilePicture:
                HandleProfilePicture(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(function);
                break;
        }
    }

    private static void HandleCreateUGCItem(GameSession session, PacketReader packet)
    {
        packet.ReadLong();
        packet.ReadByte();
        packet.ReadByte();
        packet.ReadByte();
        packet.ReadInt();
        long accountId = packet.ReadLong();
        long characterId = packet.ReadLong();
        packet.ReadLong();
        packet.ReadInt();
        packet.ReadShort();
        packet.ReadShort();
        long unk = packet.ReadLong();
        int itemId = packet.ReadInt();
        int amount = packet.ReadInt();
        string itemName = packet.ReadUnicodeString();
        packet.ReadByte();
        long cost = packet.ReadLong();

        UGCDesignMetadata metadata = UGCDesignMetadataStorage.GetItem(itemId);
        if (metadata is null)
        {
            return;
        }

        if (metadata.CurrencyType == CurrencyType.Meso && !session.Player.Wallet.Meso.Modify(-cost))
        {
            session.SendNotice("You don't have enough mesos.");
            return;
        }

        if (metadata.CurrencyType == CurrencyType.Meret && !session.Player.Account.Meret.Modify(-cost))
        {
            session.SendNotice("You don't have enough merets.");
            return;
        }

        Item item = new(itemId, 1)
        {
            Rarity = metadata.Rarity,
            UGC = new(itemName, characterId, session.Player.Name, accountId, metadata.SalePrice),
        };
        DatabaseManager.Items.Update(item);

        session.Send(UgcPacket.CreateUGC(item.UGC is not null, item.UGC));
    }

    private static void HandleAddUGCItem(GameSession session, PacketReader packet)
    {
        packet.ReadByte();
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

        Item item = DatabaseManager.Items.FindByUGCUid(ugcUid);
        if (item is null)
        {
            return;
        }
        item.SetMetadataValues();

        session.Player.Inventory.AddItem(session, item, true);
        session.Send(UgcPacket.UpdateUGCItem(session.FieldPlayer, item));
        session.Send(UgcPacket.SetItemUrl(item.UGC));
    }

    private static void HandleProfilePicture(GameSession session, PacketReader packet)
    {
        string path = packet.ReadUnicodeString();
        session.Player.ProfileUrl = path;
        DatabaseManager.Characters.UpdateProfileUrl(session.Player.CharacterId, path);

        session.FieldManager.BroadcastPacket(UgcPacket.SetProfilePictureURL(session.Player.FieldPlayer.ObjectId, session.Player.CharacterId, path));
    }
}
