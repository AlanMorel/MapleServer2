using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class UserBattlePacket
    {
        public static Packet UserBattle(IFieldObject<Player> player, bool flag)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.USER_BATTLE);
            pWriter.WriteInt(player.ObjectId);
            if (flag)
            {
                pWriter.WriteBool(flag);
            }
            else
            {
                pWriter.WriteBool(flag);
            }
            return pWriter;
        }
    }
}
