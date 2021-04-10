using System;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class ShopPacket
    {
        private enum ShopMode : byte
        {
            Open = 0x0,
            LoadProducts = 0x1,
            Buy = 0x4,
            Sell = 0x5,
            Reload = 0x6
        }

        public static Packet Open(ShopMetadata shop)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteEnum(ShopMode.Open);
            pWriter.WriteInt(shop.TemplateId);
            pWriter.WriteInt(shop.Id);
            pWriter.WriteLong(shop.NextRestock);
            pWriter.WriteInt();
            pWriter.WriteShort(15);
            pWriter.WriteInt(shop.Category);
            pWriter.WriteBool(false);
            pWriter.WriteBool(shop.RestrictSales);
            pWriter.WriteBool(shop.CanRestock);
            pWriter.WriteBool(false);
            pWriter.WriteEnum(shop.ShopType);
            pWriter.WriteBool(shop.AllowBuyback);
            pWriter.WriteBool(false);
            pWriter.WriteBool(false);
            pWriter.WriteBool(false);
            pWriter.WriteMapleString(shop.Name);

            return pWriter;
        }

        public static Packet Reload()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteEnum(ShopMode.Reload);
            pWriter.WriteByte();
            pWriter.WriteByte();

            return pWriter;
        }

        public static Packet Close()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteShort();

            return pWriter;
        }

        public static Packet Buy(int itemId, int quantity, int price, ShopCurrencyType shopCurrencyType)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteEnum(ShopMode.Buy);
            pWriter.WriteInt(itemId);
            pWriter.WriteInt(quantity);
            pWriter.WriteInt(price * quantity);
            pWriter.WriteEnum(shopCurrencyType);
            pWriter.WriteByte();

            return pWriter;
        }

        public static Packet Sell(int itemId, int quantity)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteEnum(ShopMode.Sell);
            pWriter.WriteInt(quantity);
            pWriter.WriteShort();
            pWriter.WriteInt(itemId);
            pWriter.WriteByte(1);
            pWriter.WriteByte(1);
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteItem(new Item(itemId)
            {
                Amount = quantity,
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Owner = null
            });

            return pWriter;
        }

        public static Packet LoadProducts(ShopItem product)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.SHOP);
            pWriter.WriteEnum(ShopMode.LoadProducts);
            pWriter.WriteByte(1);
            pWriter.WriteInt(product.UniqueId);
            pWriter.WriteInt(product.ItemId);
            pWriter.WriteEnum(product.TokenType);
            pWriter.WriteInt(product.RequiredItemId);
            pWriter.WriteInt();
            pWriter.WriteInt(product.Price);
            pWriter.WriteInt(product.SalePrice);
            pWriter.WriteByte(product.ItemRank);
            pWriter.WriteUInt(0xEFDA5D2D);
            pWriter.WriteInt(product.StockCount);
            pWriter.WriteInt(product.StockPurchased);
            pWriter.WriteInt(product.GuildTrophy);
            pWriter.WriteMapleString(product.Category);
            pWriter.WriteInt(product.RequiredAchievementId);
            pWriter.WriteInt(product.RequiredAchievementGrade);
            pWriter.WriteByte(product.RequiredChampionshipGrade);
            pWriter.WriteShort(product.RequiredChampionshipJoinCount);
            pWriter.WriteByte(product.RequiredGuildMerchantType);
            pWriter.WriteShort(product.RequiredGuildMerchantLevel);
            pWriter.WriteBool(false);
            pWriter.WriteShort(product.Quantity);
            pWriter.WriteByte(1);
            pWriter.WriteEnum(product.Flag);
            pWriter.WriteMapleString(product.TemplateName);
            pWriter.WriteShort(product.RequiredQuestAlliance);
            pWriter.WriteInt(product.RequiredFameGrade);
            pWriter.WriteBool(product.AutoPreviewEquip);
            pWriter.WriteByte();
            pWriter.WriteItem(new Item(product.ItemId)
            {
                Amount = product.Quantity,
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Owner = null
            });


            return pWriter;
        }
    }
}
