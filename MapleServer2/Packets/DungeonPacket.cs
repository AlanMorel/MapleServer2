using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{

    public static class DungeonPacket
    {
        private enum DungeonPacketMode : byte
        {
            DungeonInfo = 0x6,
            UpdateDungeonInfo = 0x7,
        }

        public static PacketWriter DungeonInfo(int dungeonId, byte weeklyClearCountMax, byte addRewards, int clearCount, short lifetimeRecord, byte toggle)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ROOM_DUNGEON);
            pWriter.Write(DungeonPacketMode.DungeonInfo);
            pWriter.WriteInt(dungeonId);
            pWriter.WriteLong(0); //timestamp
            pWriter.WriteByte(weeklyClearCountMax);
            pWriter.WriteByte();
            pWriter.WriteLong(0); //timestamp
            pWriter.WriteByte(addRewards);
            pWriter.WriteByte();
            pWriter.WriteLong(0); //timestamp
            pWriter.WriteInt(clearCount);
            pWriter.WriteShort(lifetimeRecord);
            pWriter.WriteLong(0); //timestamp
            pWriter.WriteShort(lifetimeRecord);
            pWriter.WriteByte(toggle); // tbd
            return pWriter;
        }

        public static PacketWriter UpdateDungeonInfo(byte mode, int dungeonId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.ROOM_DUNGEON);
            pWriter.Write(DungeonPacketMode.UpdateDungeonInfo);
            pWriter.WriteByte(mode); //05 = favorite, 04 = become veteran
            pWriter.WriteInt(dungeonId);
            return pWriter;
        }
    }
}
