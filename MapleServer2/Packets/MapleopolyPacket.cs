using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class MapleopolyPacket
{
    private enum MapleopolyPacketMode : byte
    {
        Open = 0x0,
        Roll = 0x2,
        ProcessTile = 0x4,
        Notice = 0x6
    }

    public static PacketWriter Open(Mapleopoly game, List<MapleopolyTile> tiles, int playerTokenAmount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MAPLEOPOLY);
        pWriter.Write(MapleopolyPacketMode.Open);
        pWriter.WriteInt(game.TotalTileCount);
        pWriter.WriteInt(game.FreeRollAmount);
        pWriter.WriteInt(Mapleopoly.TOKEN_ITEM_ID);
        pWriter.WriteInt(playerTokenAmount);
        pWriter.WriteInt(tiles.Count);

        foreach (MapleopolyTile tile in tiles)
        {
            pWriter.Write(tile.Type);
            pWriter.WriteInt(tile.TileParameter);
            pWriter.WriteInt(tile.ItemId);
            pWriter.WriteByte(tile.ItemRarity);
            pWriter.WriteInt(tile.ItemAmount);
        }
        return pWriter;
    }

    public static PacketWriter Roll(int tileLocation, int roll1, int roll2)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MAPLEOPOLY);
        pWriter.Write(MapleopolyPacketMode.Roll);
        pWriter.WriteByte();
        pWriter.WriteInt(tileLocation);
        pWriter.WriteInt(roll1);
        pWriter.WriteInt(roll2);
        pWriter.WriteInt();
        return pWriter;
    }

    public static PacketWriter ProcessTile(Mapleopoly playerGame, MapleopolyTile tile)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MAPLEOPOLY);
        pWriter.Write(MapleopolyPacketMode.ProcessTile);
        pWriter.Write(tile.Type);
        pWriter.WriteInt(tile.TileParameter);
        pWriter.WriteInt(playerGame.TotalTileCount);
        pWriter.WriteInt(playerGame.FreeRollAmount);
        pWriter.WriteInt(tile.ItemId);
        pWriter.WriteByte(tile.ItemRarity);
        pWriter.WriteInt(tile.ItemAmount);
        return pWriter;
    }

    public static PacketWriter Notice(byte noticeId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.MAPLEOPOLY);
        pWriter.Write(MapleopolyPacketMode.Notice);
        pWriter.WriteByte(noticeId);
        return pWriter;
    }
}
