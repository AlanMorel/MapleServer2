using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public class ChangeAttributesScrollPacket
    {
        private enum ChangeAttributesScrollMode : byte
        {
            Open = 0,
            Preview = 2,
            Add = 3,
        }

        public static Packet Open(long uid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHANGE_ATTRIBUTES_SCROLL);
            pWriter.WriteEnum(ChangeAttributesScrollMode.Open);
            pWriter.WriteLong(uid);

            return pWriter;
        }

        public static Packet PreviewNewItem(Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHANGE_ATTRIBUTES_SCROLL);
            pWriter.WriteEnum(ChangeAttributesScrollMode.Preview);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteItem(item);

            return pWriter;
        }

        public static Packet AddNewItem(Item item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHANGE_ATTRIBUTES_SCROLL);
            pWriter.WriteEnum(ChangeAttributesScrollMode.Add);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteItem(item);

            return pWriter;
        }
    }
}
