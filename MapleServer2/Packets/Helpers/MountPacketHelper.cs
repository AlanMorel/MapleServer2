using MaplePacketLib2.Tools;
using MapleServer2.Enums;
using MapleServer2.Types;

namespace MapleServer2.Packets.Helpers
{
    public static class MountPacketHelper
    {
        public static PacketWriter WriteMount(this PacketWriter pWriter, IFieldObject<Mount> mount)
        {
            pWriter.WriteByte((byte)mount.Value.Type);

            // Base class constructor (RideOnAction)
            pWriter.WriteInt(mount.Value.Id)
                .WriteInt(mount.ObjectId);

            switch (mount.Value.Type)
            {
                case RideType.UseItem:
                    pWriter.WriteInt(mount.Value.Id)
                        .WriteLong(mount.Value.Uid)
                        .WriteUgc(); // For template mounts
                    break;
                case RideType.AdditionalEffect:
                    pWriter.WriteInt()
                        .WriteShort();
                    break;
            }

            return pWriter;
        }
    }
}