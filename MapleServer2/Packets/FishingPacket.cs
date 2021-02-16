using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class FishingPacket
    {
        public static Packet Start(long rodItemUid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.WriteByte(0x00);
            pWriter.WriteLong(rodItemUid);

            return pWriter;
        }

        public static Packet Stop()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.WriteByte(0x01);

            return pWriter;
        }

        public static Packet Unknown3(int fishId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.WriteByte(0x03);
            pWriter.WriteInt(fishId);
            pWriter.WriteInt(1);
            pWriter.WriteShort(1);
            pWriter.WriteShort(1);

            return pWriter;
        }

        public static Packet LoadLog()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.WriteByte(0x07);
            pWriter.WriteInt(0); // Count
            // Entry(Int:FishId, Int:TotalCaught, Int:TotalPrized, Int:LargestSize)

            return pWriter;
        }

        public static Packet CatchFish(int fishId, int fishSize, bool success)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.Write(0x08);
            pWriter.WriteInt(fishId);
            pWriter.WriteInt(fishSize);
            pWriter.WriteBool(success);
            pWriter.WriteByte();

            if (success)
            {
                pWriter.WriteInt(fishId);
                pWriter.WriteInt(1); // Total Caught
                pWriter.WriteInt(1); // Total PrizedFish
                pWriter.WriteInt(1000); // Largest Fish
            }

            return pWriter;
        }

        // Used for catching those trash coral/clam/wheel things
        public static Packet CatchItem(params int[] itemIds)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FISHING);
            pWriter.Write(0x05);
            pWriter.WriteInt(itemIds.Length); // Count

            foreach (int itemId in itemIds)
            {
                pWriter.WriteInt(itemId);
                pWriter.WriteShort(1); // Amount?
            }

            return pWriter;
        }
    }
}
