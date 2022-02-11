using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ResponseLoadUgcMapPacket
{
    public static PacketWriter LoadUgcMap(Home home = null, bool inDecorPlanner = false)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.LOAD_UGC_MAP);
        pWriter.WriteLong();
        pWriter.WriteBool(home is not null);

        if (home is null)
        {
            return pWriter;
        }

        pWriter.WriteLong(home.AccountId);
        pWriter.WriteUnicodeString(home.Name);
        pWriter.WriteUnicodeString(home.Description);
        pWriter.WriteByte();
        pWriter.WriteInt(home.ArchitectScoreCurrent);
        pWriter.WriteInt(home.ArchitectScoreTotal);
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteBool(inDecorPlanner);
        pWriter.WriteByte(home.Size);
        pWriter.WriteByte(home.Height);
        pWriter.WriteByte(home.Background);
        pWriter.WriteByte(home.Lighting);
        pWriter.WriteByte(home.Camera);
        pWriter.WriteByte(Home.PERMISSION_COUNT);
        for (int i = 0; i < Home.PERMISSION_COUNT; i++)
        {
            if (home.Permissions.ContainsKey((HomePermission) i))
            {
                pWriter.WriteBool(true);
                pWriter.WriteByte(home.Permissions[(HomePermission) i]);
            }
            else
            {
                pWriter.WriteBool(false);
            }
        }
        pWriter.WriteByte((byte) home.Layouts.Count);
        foreach (HomeLayout layout in home.Layouts)
        {
            pWriter.WriteInt(layout.Id);
            pWriter.WriteUnicodeString(layout.Name);
            pWriter.WriteLong(layout.Timestamp);
        }
        pWriter.WriteByte();

        return pWriter;
    }
}
