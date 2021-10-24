﻿using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class BlackMarketPacket
    {
        private enum BlackMarketPacketMode : byte
        {
            Error = 0x0,
            Open = 0x1,
            CreateListing = 0x2,
            CancelListing = 0x3,
            SearchResults = 0x4,
            Purchase = 0x5,
            PrepareListing = 0x8,
        }

        public static Packet Error(int errorCode, int itemId = 0, int itemLevel = 0)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BLACK_MARKET);
            pWriter.WriteEnum(BlackMarketPacketMode.Error);
            pWriter.WriteByte(0);
            pWriter.WriteInt(errorCode);
            pWriter.WriteLong();
            pWriter.WriteInt(itemId);
            pWriter.WriteInt(itemLevel);
            return pWriter;
        }

        public static Packet Open(List<BlackMarketListing> listings)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BLACK_MARKET);
            pWriter.WriteEnum(BlackMarketPacketMode.Open);
            pWriter.WriteInt(listings.Count);
            foreach (BlackMarketListing listing in listings)
            {
                WriteListing(pWriter, listing);
            }
            return pWriter;
        }

        public static Packet CreateListing(BlackMarketListing listing)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BLACK_MARKET);
            pWriter.WriteEnum(BlackMarketPacketMode.CreateListing);
            WriteListing(pWriter, listing);
            return pWriter;
        }

        public static Packet CancelListing(BlackMarketListing listing, bool isSold)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BLACK_MARKET);
            pWriter.WriteEnum(BlackMarketPacketMode.CancelListing);
            pWriter.WriteLong(listing.Id);
            pWriter.WriteBool(isSold);
            return pWriter;
        }

        public static Packet SearchResults(List<BlackMarketListing> listings)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BLACK_MARKET);
            pWriter.WriteEnum(BlackMarketPacketMode.SearchResults);
            pWriter.WriteInt(listings.Count);
            foreach (BlackMarketListing listing in listings)
            {
                WriteListing(pWriter, listing);
            }
            return pWriter;
        }

        public static Packet Purchase(long listingId, int amount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BLACK_MARKET);
            pWriter.WriteEnum(BlackMarketPacketMode.Purchase);
            pWriter.WriteLong(listingId);
            pWriter.WriteInt(amount);
            return pWriter;
        }

        public static Packet PrepareListing(int itemId, int rarity, int npcShopPrice)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BLACK_MARKET);
            pWriter.WriteEnum(BlackMarketPacketMode.PrepareListing);
            pWriter.WriteInt(itemId);
            pWriter.WriteInt(rarity);
            pWriter.WriteLong(npcShopPrice);
            return pWriter;
        }

        private static void WriteListing(PacketWriter pWriter, BlackMarketListing listing)
        {
            pWriter.WriteLong(listing.Id);
            pWriter.WriteLong(listing.ListedTimestamp);
            pWriter.WriteLong(listing.ListedTimestamp);
            pWriter.WriteLong(listing.ExpiryTimestamp);
            pWriter.WriteInt(listing.Item.Amount);
            pWriter.WriteInt();
            pWriter.WriteLong(listing.Price);
            pWriter.WriteByte(); // discontinued bool
            pWriter.WriteLong(listing.Item.Uid);
            pWriter.WriteInt(listing.Item.Id);
            pWriter.WriteByte((byte) listing.Item.Rarity);
            pWriter.WriteLong(listing.OwnerAccountId);
            pWriter.WriteItem(listing.Item);
        }
    }
}
