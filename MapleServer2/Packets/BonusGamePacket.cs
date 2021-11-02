using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets
{
    public static class BonusGamePacket
    {
        private enum BonusGameMode : byte
        {
            OpenWheel = 0x01,
            SpinWheel = 0x02,
        }

        public static PacketWriter OpenWheel(List<Tuple<int, byte, int>> items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BONUS_GAME);
            pWriter.Write(BonusGameMode.OpenWheel);
            pWriter.WriteByte();
            pWriter.WriteInt(items.Count);
            foreach (Tuple<int, byte, int> item in items)
            {
                pWriter.WriteInt(item.Item1);
                pWriter.WriteByte(item.Item2);
                pWriter.WriteInt(item.Item3);
            }
            pWriter.WriteInt();

            return pWriter;
        }

        public static PacketWriter SpinWheel(int index, Tuple<int, byte, int> item)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BONUS_GAME);
            pWriter.Write(BonusGameMode.SpinWheel);
            pWriter.WriteInt(1); // spins? | loop count?
            pWriter.WriteInt(index);
            pWriter.WriteInt(item.Item1);
            pWriter.WriteInt(2); // amount?
            pWriter.WriteShort(1); // unknown

            return pWriter;
        }
    }
}
