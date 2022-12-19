using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class MesoMarketPacket
{
    private enum Mode : byte
    {
        Error = 0x0,
        LoadMarket = 0x1,
        AccountStats = 0x2,
        MyListings = 0x4,
        CreateListing = 0x5,
        CancelListing = 0x6,
        LoadListings = 0x7,
        Purchase = 0x8
    }

    public static PacketWriter Error(int errorCode)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MesoMarket);
        pWriter.Write(Mode.Error);
        pWriter.WriteInt(errorCode);
        return pWriter;
    }

    public static PacketWriter LoadMarket()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MesoMarket);
        pWriter.Write(Mode.LoadMarket);
        pWriter.WriteFloat(float.Parse(ConstantsMetadataStorage.GetConstant("MesoMarketTax") ?? "0"));
        pWriter.WriteFloat(float.Parse(ConstantsMetadataStorage.GetConstant("MesoMarketPriceRange") ?? "0"));
        pWriter.WriteLong(long.Parse(ConstantsMetadataStorage.GetConstant("MesoMarketAveragePrice") ?? "0")); // TODO: Calculate average price
        pWriter.WriteInt(int.Parse(ConstantsMetadataStorage.GetConstant("MesoMarketTotaListingsLimit") ?? "0"));
        pWriter.WriteInt(int.Parse(ConstantsMetadataStorage.GetConstant("MesoMarketDailyListingsLimit") ?? "0"));
        pWriter.WriteInt(int.Parse(ConstantsMetadataStorage.GetConstant("MesoMarketMonthlyPurchaseLimit") ?? "0"));
        pWriter.WriteInt(int.Parse(ConstantsMetadataStorage.GetConstant("MesoMarketListingDayDuration") ?? "0"));
        pWriter.WriteInt(int.Parse(ConstantsMetadataStorage.GetConstant("MesoMarketListingsDisplayCount") ?? "0"));
        pWriter.WriteInt(100); // unk
        pWriter.WriteInt(1000); // unk
        return pWriter;
    }

    public static PacketWriter AccountStats(int todaysListingCount, int monthlyPurchaseCount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MesoMarket);
        pWriter.Write(Mode.AccountStats);
        pWriter.WriteInt(todaysListingCount);
        pWriter.WriteInt(monthlyPurchaseCount);
        return pWriter;
    }

    public static PacketWriter MyListings(List<MesoMarketListing> listings)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MesoMarket);
        pWriter.Write(Mode.MyListings);
        pWriter.WriteInt(listings.Count);
        foreach (MesoMarketListing listing in listings)
        {
            pWriter.WriteLong(listing.Id);
            WriteListing(pWriter, listing);
        }
        return pWriter;
    }

    public static PacketWriter CreateListing(MesoMarketListing listing)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MesoMarket);
        pWriter.Write(Mode.CreateListing);
        WriteListing(pWriter, listing);
        pWriter.WriteInt();
        return pWriter;
    }

    public static PacketWriter CancelListing(long listingId, int errorCode = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MesoMarket);
        pWriter.Write(Mode.CancelListing);
        pWriter.WriteInt(errorCode);
        pWriter.WriteLong(listingId);
        return pWriter;
    }

    public static PacketWriter LoadListings(List<MesoMarketListing> listings)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MesoMarket);
        pWriter.Write(Mode.LoadListings);
        pWriter.WriteInt(listings.Count);
        foreach (MesoMarketListing listing in listings)
        {
            WriteListing(pWriter, listing);
        }
        return pWriter;
    }

    public static PacketWriter Purchase(long listingId, int errorCode = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MesoMarket);
        pWriter.Write(Mode.Purchase);
        pWriter.WriteInt(errorCode);
        pWriter.WriteLong(listingId);
        pWriter.WriteInt(1);
        return pWriter;
    }

    private static void WriteListing(PacketWriter pWriter, MesoMarketListing listing)
    {
        pWriter.WriteLong(listing.Id);
        pWriter.WriteLong(listing.Mesos);
        pWriter.WriteLong(listing.Price);
        pWriter.WriteLong(listing.ListedTimestamp);
        pWriter.WriteLong(listing.ExpiryTimestamp);
        pWriter.WriteByte();
    }
}
