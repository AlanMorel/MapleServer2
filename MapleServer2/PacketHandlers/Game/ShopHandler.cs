using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
namespace MapleServer2.PacketHandlers.Game;

public class ShopHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.SHOP;

    public ShopHandler() : base() { }

    private enum ShopMode : byte
    {
        Buy = 0x4,
        Sell = 0x5,
        Close = 0x6,
        OpenViaItem = 0x0A
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        ShopMode mode = (ShopMode) packet.ReadByte();

        switch (mode)
        {
            case ShopMode.Close:
                HandleClose(session);
                break;
            case ShopMode.Buy:
                HandleBuy(session, packet);
                break;
            case ShopMode.Sell:
                HandleSell(session, packet);
                break;
            case ShopMode.OpenViaItem:
                HandleOpenViaItem(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    public static void HandleOpen(GameSession session, IFieldObject<Npc> npcFieldObject)
    {
        NpcMetadata metadata = NpcMetadataStorage.GetNpcMetadata(npcFieldObject.Value.Id);

        Shop shop = DatabaseManager.Shops.FindById(metadata.ShopId);
        if (shop == null)
        {
            Logger.Warn($"Unknown shop ID: {metadata.ShopId}");
            return;
        }

        session.Send(ShopPacket.Open(shop));
        foreach (ShopItem shopItem in shop.Items)
        {
            session.Send(ShopPacket.LoadProducts(shopItem));
        }
        session.Send(ShopPacket.Reload());
        session.Send(NpcTalkPacket.Respond(npcFieldObject, NpcType.Default, DialogType.None, 0));
        session.Player.ShopId = shop.Id;
    }

    private static void HandleClose(GameSession session)
    {
        session.Send(ShopPacket.Close());
        session.Player.ShopId = 0;
    }

    private static void HandleSell(GameSession session, PacketReader packet)
    {
        // sell to shop
        long itemUid = packet.ReadLong();
        int quantity = packet.ReadInt();

        if (!session.Player.Inventory.Items.TryGetValue(itemUid, out Item item))
        {
            return;
        }

        int price = ItemMetadataStorage.GetCustomSellPrice(item.Id);
        session.Player.Wallet.Meso.Modify(price * quantity);

        session.Player.Inventory.ConsumeItem(session, item.Uid, quantity);

        session.Send(ShopPacket.Sell(item, quantity));
    }

    private static void HandleBuy(GameSession session, PacketReader packet)
    {
        int itemUid = packet.ReadInt();
        int quantity = packet.ReadInt();

        ShopItem shopItem = DatabaseManager.ShopItems.FindByUid(itemUid);

        switch (shopItem.TokenType)
        {
            case ShopCurrencyType.Meso:
                session.Player.Wallet.Meso.Modify(-(shopItem.Price * quantity));
                break;
            case ShopCurrencyType.ValorToken:
                session.Player.Wallet.ValorToken.Modify(-(shopItem.Price * quantity));
                break;
            case ShopCurrencyType.Treva:
                session.Player.Wallet.Treva.Modify(-(shopItem.Price * quantity));
                break;
            case ShopCurrencyType.Rue:
                session.Player.Wallet.Rue.Modify(-(shopItem.Price * quantity));
                break;
            case ShopCurrencyType.HaviFruit:
                session.Player.Wallet.HaviFruit.Modify(-(shopItem.Price * quantity));
                break;
            case ShopCurrencyType.Meret:
            case ShopCurrencyType.GameMeret:
            case ShopCurrencyType.EventMeret:
                session.Player.Account.RemoveMerets(shopItem.Price * quantity);
                break;
            case ShopCurrencyType.Item:
                Item itemCost = session.Player.Inventory.Items.FirstOrDefault(x => x.Value.Id == shopItem.RequiredItemId).Value;
                if (itemCost.Amount < shopItem.Price)
                {
                    return;
                }
                session.Player.Inventory.ConsumeItem(session, itemCost.Uid, shopItem.Price);
                break;
            default:
                session.SendNotice($"Unknown currency: {shopItem.TokenType}");
                return;
        }

        // add item to inventory
        Item item = new(shopItem.ItemId)
        {
            Amount = quantity * shopItem.Quantity,
            Rarity = shopItem.ItemRank
        };
        session.Player.Inventory.AddItem(session, item, true);

        // complete purchase
        session.Send(ShopPacket.Buy(shopItem.ItemId, quantity, shopItem.Price, shopItem.TokenType));
    }

    private static void HandleOpenViaItem(GameSession session, PacketReader packet)
    {
        byte unk = packet.ReadByte();
        int itemId = packet.ReadInt();

        List<Item> playerInventory = new(session.Player.Inventory.Items.Values);

        Item item = playerInventory.FirstOrDefault(x => x.Id == itemId);
        if (item == null)
        {
            return;
        }

        Shop shop = DatabaseManager.Shops.FindById(item.ShopID);
        if (shop == null)
        {
            Logger.Warn($"Unknown shop ID: {item.ShopID}");
            return;
        }

        session.Send(ShopPacket.Open(shop));
        foreach (ShopItem shopItem in shop.Items)
        {
            session.Send(ShopPacket.LoadProducts(shopItem));
        }
        session.Send(ShopPacket.Reload());
    }
}
