using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets {
    public static class FishingPacket {
        public static Packet Start(long rodItemUid) {
            return PacketWriter.Of(SendOp.FISHING)
                .WriteByte(0x00)
                .WriteLong(rodItemUid);
        }

        public static Packet Stop() {
            return PacketWriter.Of(SendOp.FISHING)
                .WriteByte(0x01);
        }

        public static Packet Unknown3(int fishId) {
            return PacketWriter.Of(SendOp.FISHING)
                .WriteByte(0x03)
                .WriteInt(fishId)
                .WriteInt(1)
                .WriteShort(1)
                .WriteShort(1);
        }

        public static Packet LoadLog() {
            return PacketWriter.Of(SendOp.FISHING)
                .WriteByte(0x07)
                .WriteInt(0); // Count
            // Entry(Int:FishId, Int:TotalCaught, Int:TotalPrized, Int:LargestSize)
        }

        public static Packet CatchFish(int fishId, int fishSize, bool success) {
            var pWriter = PacketWriter.Of(SendOp.FISHING)
                .Write(0x08)
                .WriteInt(fishId)
                .WriteInt(fishSize)
                .WriteBool(success)
                .WriteByte();

            if (success) {
                pWriter.WriteInt(fishId)
                    .WriteInt(1) // Total Caught
                    .WriteInt(1) // Total PrizedFish
                    .WriteInt(1000); // Largest Fish
            }

            return pWriter;
        }

        // Used for catching those trash coral/clam/wheel things
        public static Packet CatchItem(params int[] itemIds) {
            var pWriter = PacketWriter.Of(SendOp.FISHING)
                .Write(0x05)
                .WriteInt(itemIds.Length); // Count

            foreach (int itemId in itemIds) {
                pWriter.WriteInt(itemId)
                    .WriteShort(1); // Amount?
            }

            return pWriter;
        }
    }
}