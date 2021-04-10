using System.Collections.Generic;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class FishingPacket
    {
        private enum FishingPacketMode : byte
        {
            PrepareFishing = 0x0,
            Stop = 0x1,
            Notice = 0x2,
            IncreaseMastery = 0x3,
            LoadFishTiles = 0x4,
            CatchItem = 0x5,
            LoadAlbum = 0x7,
            CatchFish = 0x8,
            Start = 0x9,
        }

        public static Packet PrepareFishing(long rodItemUid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.WriteEnum(FishingPacketMode.PrepareFishing);
            pWriter.WriteLong(rodItemUid);
            return pWriter;
        }

        public static Packet Stop()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.WriteEnum(FishingPacketMode.Stop);
            return pWriter;
        }

        public static Packet Notice(short notice)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.WriteEnum(FishingPacketMode.Notice);
            pWriter.WriteShort(notice);
            return pWriter;
        }

        public static Packet IncreaseMastery(MasteryType type, int fishId, int exp)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.WriteEnum(FishingPacketMode.IncreaseMastery);
            pWriter.WriteInt(fishId);
            pWriter.WriteInt(exp);
            pWriter.WriteInt((int) type);
            return pWriter;
        }

        public static Packet LoadFishTiles(List<MapBlock> tiles, int reduceTime)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.WriteEnum(FishingPacketMode.LoadFishTiles);
            pWriter.WriteByte();
            pWriter.WriteInt(tiles.Count);
            foreach (MapBlock tile in tiles)
            {
                pWriter.Write(tile.Coord.ToByte());
                pWriter.WriteByte();
                pWriter.WriteInt(10000001);
                pWriter.WriteInt(25);
                pWriter.WriteInt(15000 - reduceTime); // fishing time
                pWriter.WriteShort(1);
            }
            return pWriter;
        }

        public static Packet CatchItem(List<Item> items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.WriteEnum(FishingPacketMode.CatchItem);
            pWriter.WriteInt(items.Count);

            foreach (Item item in items)
            {
                pWriter.WriteInt(item.Id);
                pWriter.WriteShort((short) item.Amount);
            }

            return pWriter;
        }

        public static Packet LoadAlbum(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.WriteEnum(FishingPacketMode.LoadAlbum);
            pWriter.WriteInt(player.FishAlbum.Count);
            foreach (KeyValuePair<int, Fishing> fish in player.FishAlbum)
            {
                pWriter.WriteInt(fish.Value.FishId);
                pWriter.WriteInt(fish.Value.TotalPrizeFish);
                pWriter.WriteInt(fish.Value.LargestFish);
            }
            return pWriter;
        }

        public static Packet CatchFish(Player player, FishMetadata fish, int fishSize, bool success)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.WriteEnum(FishingPacketMode.CatchFish);
            pWriter.WriteInt(fish.Id);
            pWriter.WriteInt(fishSize);
            pWriter.WriteBool(success);
            pWriter.WriteByte();

            if (success)
            {
                pWriter.WriteInt(fish.Id);
                pWriter.WriteInt(player.FishAlbum[fish.Id].TotalCaught);
                pWriter.WriteInt(player.FishAlbum[fish.Id].TotalPrizeFish);
                pWriter.WriteInt(player.FishAlbum[fish.Id].LargestFish);
            }

            return pWriter;
        }

        public static Packet Start(int fishingTick, bool minigame)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.WriteEnum(FishingPacketMode.Start);
            pWriter.WriteBool(minigame);
            pWriter.WriteInt(fishingTick);
            return pWriter;
        }
    }
}
