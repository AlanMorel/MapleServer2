using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class SendCubesPacket
{
    private enum SendCubesMode : byte
    {
        LoadCubes = 0x0,
        AvailablePlots = 0x01,
        LoadPlots = 0x02,
        Expiration = 0x03
    }

    public static PacketWriter LoadCubes(List<Cube> cubes)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CUBES);
        pWriter.Write(SendCubesMode.LoadCubes);
        pWriter.WriteByte();
        pWriter.WriteInt(cubes.Count);
        foreach (Cube cube in cubes)
        {
            pWriter.Write(cube.CoordF.ToByte());
            pWriter.WriteByte();
            pWriter.WriteLong(cube.Item.Uid);
            pWriter.WriteInt(cube.Item.Id);
            pWriter.WriteLong(cube.Uid);
            pWriter.WriteLong();
            pWriter.WriteBool(cube.Item.Ugc is not null);
            if (cube.Item.Ugc is not null)
            {
                pWriter.WriteUgcTemplate(cube.Item.Ugc);
            }
            pWriter.WriteInt(cube.PlotNumber);
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteFloat(cube.Rotation.Z);
            pWriter.WriteInt();
            pWriter.WriteByte();
        }
        return pWriter;
    }

    public static PacketWriter LoadAvailablePlots(List<Home> homes, List<byte> plotNumbers)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CUBES);
        pWriter.Write(SendCubesMode.AvailablePlots);
        pWriter.WriteInt(plotNumbers.Count);
        foreach (int plotId in plotNumbers)
        {
            pWriter.WriteInt(plotId);
            pWriter.WriteBool(homes.Any(x => x.PlotNumber == plotId)); // is occupied
        }

        return pWriter;
    }

    public static PacketWriter LoadPlots(List<Home> homes, int mapId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CUBES);
        pWriter.Write(SendCubesMode.LoadPlots);
        pWriter.WriteInt(homes.Count);
        foreach (Home home in homes)
        {
            pWriter.WriteInt(mapId == (int) Map.PrivateResidence ? 1 : home.PlotNumber);
            pWriter.WriteInt(home.ApartmentNumber);
            pWriter.WriteUnicodeString(home.Name);
            pWriter.WriteLong(home.AccountId);
        }

        return pWriter;
    }

    public static PacketWriter Expiration(List<Home> homes)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CUBES);
        pWriter.Write(SendCubesMode.Expiration);
        pWriter.WriteInt(homes.Count);
        foreach (Home home in homes)
        {
            pWriter.WriteInt(home.PlotNumber);
            pWriter.WriteInt(home.ApartmentNumber);
            pWriter.WriteByte(1); // always 1?
            pWriter.WriteLong(home.Expiration);
        }

        return pWriter;
    }
}
