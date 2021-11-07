using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public class HomeActionPacket
{
    private enum HomeActionMode : byte
    {
        PortalCube = 0x06
    }

    public static PacketWriter SendCubePortalSettings(Cube cube, List<Cube> otherPortals)
    {
        CubePortalSettings portalSettings = cube.PortalSettings;

        PacketWriter pWriter = PacketWriter.Of(SendOp.RESPONSE_HOME_ACTION);

        pWriter.Write(HomeActionMode.PortalCube);
        pWriter.WriteByte();
        pWriter.Write(cube.CoordF.ToByte());
        pWriter.WriteByte();
        pWriter.WriteUnicodeString(portalSettings.PortalName);
        pWriter.Write(portalSettings.Method);
        pWriter.Write(portalSettings.Destination);
        pWriter.WriteUnicodeString(portalSettings.DestinationTarget ?? "");
        pWriter.WriteInt(otherPortals.Count);
        foreach (Cube otherPortal in otherPortals)
        {
            pWriter.WriteUnicodeString(otherPortal.PortalSettings.PortalName);
        }

        return pWriter;
    }
}
