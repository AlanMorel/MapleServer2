using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database.Types;

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
            pWriter.WriteByte();
            return pWriter;
        }

        public static Packet Premium(List<MeretMarketItem> items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
            pWriter.WriteEnum(MeretMarketMode.Premium);
            pWriter.WriteInt(items.Count);
            pWriter.WriteInt(items.Count);
            pWriter.WriteInt(1);
            foreach (MeretMarketItem item in items)
            {
                pWriter.WriteByte(1);
                pWriter.WriteByte();
                pWriter.WriteInt(item.MarketId);
                pWriter.WriteLong();
                WriteMeretMarketItem(pWriter, item);
                pWriter.WriteByte();
                pWriter.WriteByte();
                pWriter.WriteByte();
                pWriter.WriteInt();
                pWriter.WriteByte((byte) item.AdditionalQuantities.Count);
                foreach (MeretMarketItem additionalItem in item.AdditionalQuantities)
                {
                    pWriter.WriteByte(1);
                    WriteMeretMarketItem(pWriter, additionalItem);
                    pWriter.WriteByte();
                    pWriter.WriteByte();
                    pWriter.WriteByte();
                }
            }
            return pWriter;
        }

        public static Packet Purchase(MeretMarketItem item, int itemIndex, int totalQuantity)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
            pWriter.WriteEnum(MeretMarketMode.Purchase);
            pWriter.WriteByte((byte) totalQuantity);
            pWriter.WriteInt(item.MarketId);
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

        public static Packet Promos(List<MeretMarketItem> items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.MERET_MARKET);
            pWriter.WriteEnum(MeretMarketMode.Home);
            pWriter.WriteByte();
            pWriter.WriteByte(10);
            pWriter.WriteByte(1);
            pWriter.WriteByte(14);
            pWriter.WriteByte();
            pWriter.WriteByte();

            foreach (MeretMarketItem item in items)
            {
                pWriter.WriteByte(1);
                pWriter.WriteByte();
                pWriter.WriteInt(item.MarketId);
                pWriter.WriteLong();
                WriteMeretMarketItem(pWriter, item);

                pWriter.WriteByte(1);
                pWriter.WriteMapleString(item.PromoName);
                pWriter.WriteLong(item.PromoBannerBeginTime);
                pWriter.WriteLong(item.PromoBannerEndTime);
                pWriter.WriteByte();
                pWriter.WriteBool(item.ShowSaleTime);
                pWriter.WriteInt();
                pWriter.WriteByte((byte) item.AdditionalQuantities.Count);
                foreach (MeretMarketItem additionalItem in item.AdditionalQuantities)
                {
                    pWriter.WriteByte(1);
                    WriteMeretMarketItem(pWriter, additionalItem);
                    pWriter.WriteByte();
                    pWriter.WriteByte();
                    pWriter.WriteByte();
                }
            }

            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteByte();
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

        public static void WriteMeretMarketItem(PacketWriter pWriter, MeretMarketItem item)
        {
            pWriter.WriteInt(item.MarketId);
            pWriter.WriteByte(2);
            pWriter.WriteUnicodeString(item.ItemName);
            pWriter.WriteByte(1);
            pWriter.WriteInt(item.ParentMarketId);
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
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteEnum(item.PromoFlag);
            string bannerName = "";
            if (item.Banner != null)
            {
                bannerName = item.Banner.Name;
            }
            pWriter.WriteMapleString(bannerName);
            pWriter.WriteMapleString("");
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteInt(item.RequiredAchievementId);
            pWriter.WriteInt(item.RequiredAchievementGrade);
            pWriter.WriteInt();
            pWriter.WriteBool(item.PCCafe);
            pWriter.WriteByte();
            pWriter.WriteInt();

        }
    }
}
