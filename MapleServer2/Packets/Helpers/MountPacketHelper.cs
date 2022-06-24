using MaplePacketLib2.Tools;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets.Helpers;

public static class MountPacketHelper
{
    public static void WriteMount(this PacketWriter pWriter, IFieldObject<Mount> mount)
    {
        pWriter.WriteByte((byte) mount.Value.Type);

        // Base class constructor (RideOnAction)
        pWriter.WriteInt(mount.Value.Id);
        pWriter.WriteInt(mount.ObjectId);

        switch (mount.Value.Type)
        {
            case RideType.UseItem:
                pWriter.WriteInt(mount.Value.Id);
                pWriter.WriteLong(mount.Value.Uid);
                pWriter.WriteUGCTemplate(mount.Value.Ugc ?? UGC.Default); // For template mounts
                break;
            case RideType.AdditionalEffect:
                pWriter.WriteInt();
                pWriter.WriteShort();
                break;
        }
    }
}
