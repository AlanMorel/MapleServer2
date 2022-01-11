using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets;

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
        Start = 0x9
    }

    public static PacketWriter PrepareFishing(long rodItemUid)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
        pWriter.Write(FishingPacketMode.PrepareFishing);
        pWriter.WriteLong(rodItemUid);
        return pWriter;
    }

    public static PacketWriter Stop()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
        pWriter.Write(FishingPacketMode.Stop);
        return pWriter;
    }

    public static PacketWriter Notice(short notice)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
        pWriter.Write(FishingPacketMode.Notice);
        pWriter.WriteShort(notice);
        return pWriter;
    }

    public static PacketWriter IncreaseMastery(MasteryType type, int fishId, int exp)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
        pWriter.Write(FishingPacketMode.IncreaseMastery);
        pWriter.WriteInt(fishId);
        pWriter.WriteInt(exp);
        pWriter.WriteInt((int) type);
        return pWriter;
    }

    public static PacketWriter LoadFishTiles(List<MapBlock> tiles, int reduceTime)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
        pWriter.Write(FishingPacketMode.LoadFishTiles);
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

    public static PacketWriter CatchItem(List<Item> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
        pWriter.Write(FishingPacketMode.CatchItem);
        pWriter.WriteInt(items.Count);

        foreach (Item item in items)
        {
            pWriter.WriteInt(item.Id);
            pWriter.WriteShort((short) item.Amount);
        }

        return pWriter;
    }

    public static PacketWriter LoadAlbum(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
        pWriter.Write(FishingPacketMode.LoadAlbum);
        pWriter.WriteInt(player.FishAlbum.Count);
        foreach ((int _, Fishing fishing) in player.FishAlbum)
        {
            pWriter.WriteInt(fishing.FishId);
            pWriter.WriteInt(fishing.TotalPrizeFish);
            pWriter.WriteInt(fishing.LargestFish);
        }
        return pWriter;
    }

    public static PacketWriter CatchFish(Player player, FishMetadata fish, int fishSize, bool success)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
        pWriter.Write(FishingPacketMode.CatchFish);
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

    public static PacketWriter Start(int fishingTick, bool minigame)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
        pWriter.Write(FishingPacketMode.Start);
        pWriter.WriteBool(minigame);
        pWriter.WriteInt(fishingTick);
        return pWriter;
    }
}
