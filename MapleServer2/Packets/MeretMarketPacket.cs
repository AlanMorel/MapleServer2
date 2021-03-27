using System.Collections.Generic;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{

    public static class MeretMarketPacket
    {
        private enum MeretMarketMode : byte
        {
            Premium = 0x1B,
            Purchase = 0x1E,
            Initialize = 016,
            Home = 0x65,
            LoadCart = 0x6B,
        }

        public static Packet Initialize()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
            pWriter.WriteEnum(MeretMarketMode.Initialize);
            pWriter.WriteByte(0);
            return pWriter;
        }

        public static Packet Premium(List<MeretMarketMetadata> items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
            pWriter.WriteEnum(MeretMarketMode.Premium);
            pWriter.WriteInt(items.Count);
            pWriter.WriteInt(items.Count);
            pWriter.WriteInt(1);
            foreach (MeretMarketMetadata item in items)
            {
                pWriter.WriteByte(1);
                pWriter.WriteByte();
                pWriter.WriteInt(item.MarketItemId);
                pWriter.WriteLong();
                WriteMeretMarketItem(pWriter, item);
                pWriter.WriteByte(0);
                pWriter.WriteByte(0);
                pWriter.WriteByte(0);
                pWriter.WriteInt();
                pWriter.WriteByte((byte) item.AdditionalQuantities.Count);
                foreach (MeretMarketMetadata additionalItem in item.AdditionalQuantities)
                {
                    pWriter.WriteByte(1);
                    WriteMeretMarketItem(pWriter, additionalItem);
                    pWriter.WriteByte(0);
                    pWriter.WriteByte(0);
                    pWriter.WriteByte(0);
                }

            }
            return pWriter;
        }

        public static Packet Purchase(MeretMarketMetadata item, int itemIndex, int totalQuantity)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
            pWriter.WriteEnum(MeretMarketMode.Purchase);
            pWriter.WriteByte((byte) totalQuantity);
            pWriter.WriteInt(item.MarketItemId);
            pWriter.WriteLong();
            pWriter.WriteInt(1);
            pWriter.WriteInt();
            pWriter.WriteLong();
            pWriter.WriteInt(itemIndex);
            pWriter.WriteInt(totalQuantity);
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteUnicodeString("");
            pWriter.WriteUnicodeString("");
            pWriter.WriteLong(item.SalePrice);
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet Promos(List<MeretMarketMetadata> items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
            pWriter.WriteEnum(MeretMarketMode.Home);
            pWriter.WriteByte(0);
            pWriter.WriteByte(10);
            pWriter.WriteByte(1);
            pWriter.WriteByte(14);
            pWriter.WriteByte(0);
            pWriter.WriteByte(0);

            foreach (MeretMarketMetadata item in items)
            {
                pWriter.WriteByte(1);
                pWriter.WriteByte(0);
                pWriter.WriteInt(item.MarketItemId);
                pWriter.WriteLong(0);
                WriteMeretMarketItem(pWriter, item);

                pWriter.WriteByte(1);
                pWriter.WriteMapleString(item.PromoImageName);
                pWriter.WriteLong(item.PromoBannerBeginTime);
                pWriter.WriteLong(item.PromoBannerEndTime);
                pWriter.WriteByte(0);
                pWriter.WriteBool(item.ShowSaleTime);
                pWriter.WriteInt(0);
                pWriter.WriteByte((byte) item.AdditionalQuantities.Count);
                foreach (MeretMarketMetadata additionalItem in item.AdditionalQuantities)
                {
                    pWriter.WriteByte(1);
                    WriteMeretMarketItem(pWriter, additionalItem);
                    pWriter.WriteByte();
                    pWriter.WriteByte();
                    pWriter.WriteByte();
                }

            }

            pWriter.WriteByte(0);
            pWriter.WriteByte(0);
            pWriter.WriteByte(0);
            pWriter.WriteByte(0);
            pWriter.WriteByte(0);
            pWriter.WriteByte(0);
            pWriter.WriteByte(0);
            pWriter.WriteByte(0);
            pWriter.WriteByte(0);
            pWriter.WriteByte(0);
            pWriter.WriteByte(0);
            return pWriter;
        }

        public static Packet LoadCart()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
            pWriter.WriteEnum(MeretMarketMode.LoadCart);
            pWriter.WriteInt(1);
            pWriter.WriteInt();
            pWriter.WriteInt(3);
            pWriter.WriteInt(10);
            return pWriter;
        }

        public static void WriteMeretMarketItem(PacketWriter pWriter, MeretMarketMetadata item)
        {
            pWriter.WriteInt(item.MarketItemId);
            pWriter.WriteByte(2);
            pWriter.WriteUnicodeString(item.ItemName);
            pWriter.WriteByte(1);
            pWriter.WriteInt(item.ParentMarketItemId);
            pWriter.WriteInt(254);
            pWriter.WriteInt(); // promo bool
            pWriter.WriteByte(2);
            pWriter.WriteEnum(item.Flag);
            pWriter.WriteEnum(item.TokenType);
            pWriter.WriteLong(item.Price);
            pWriter.WriteLong(item.SalePrice);
            pWriter.WriteByte(1);
            pWriter.WriteLong(item.SellBeginTime);
            pWriter.WriteLong(item.SellEndTime);
            pWriter.WriteEnum(item.JobRequirement);
            pWriter.WriteInt(3);
            pWriter.WriteBool(item.RestockUnavailable);
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteShort(item.MinLevelRequirement);
            pWriter.WriteShort(item.MaxLevelRequirement);
            pWriter.WriteEnum(item.JobRequirement);
            pWriter.WriteInt(item.ItemId);
            pWriter.WriteByte(item.Rarity);
            pWriter.WriteInt(item.Quantity);
            pWriter.WriteInt(item.Duration);
            pWriter.WriteInt(item.BonusQuantity);
            pWriter.WriteInt(40300);
            pWriter.WriteInt(0);
            pWriter.WriteByte(0);
            pWriter.WriteEnum(item.PromoFlag);
            pWriter.WriteMapleString(item.PromoName);
            pWriter.WriteMapleString("");
            pWriter.WriteByte();
            pWriter.WriteByte(0);
            pWriter.WriteInt(0);
            pWriter.WriteByte(0);
            pWriter.WriteInt(item.RequiredAchievementId);
            pWriter.WriteInt(item.RequiredAchievementGrade);
            pWriter.WriteInt(0);
            pWriter.WriteBool(item.PCCafe);
            pWriter.WriteByte(0);
            pWriter.WriteInt(0);

        }
    }
}
