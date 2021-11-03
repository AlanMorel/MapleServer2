using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class UserBattlePacket
    {
        public static PacketWriter UserBattle(IFieldObject<Player> player, bool flag)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_BATTLE);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteBool(flag);
            return pWriter;
        }
    }
}
