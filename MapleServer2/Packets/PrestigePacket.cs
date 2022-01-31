using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class PrestigePacket
{
    private enum PrestigePacketMode : byte
    {
        SetLevels = 0x00,
        Exp = 0x01,
        LevelUp = 0x02,
        Reward = 0x04,
        WeeklyMissions = 0x07
    }

    public static PacketWriter SetLevels(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PRESTIGE);
        pWriter.Write(PrestigePacketMode.SetLevels);
        pWriter.WriteLong(player.Levels.PrestigeExp); // PrestigeExp
        pWriter.WriteInt(player.Levels.PrestigeLevel); // PrestigeLevel
        pWriter.WriteLong(player.Levels.PrestigeExp); // Same Prestige Exp??

        List<int> rankRewardsClaimed = player.PrestigeRewardsClaimed;
        pWriter.WriteInt(rankRewardsClaimed.Count);
        foreach (int rank in rankRewardsClaimed)
        {
            pWriter.WriteInt(rank);
        }

        return pWriter;
    }

    public static PacketWriter ExpUp(long prestigeExp, long amount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PRESTIGE);

        pWriter.Write(PrestigePacketMode.Exp);
        pWriter.WriteLong(prestigeExp);
        pWriter.WriteLong(amount);

        return pWriter;
    }

    public static PacketWriter LevelUp(int playerObjectId, int level)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PRESTIGE);

        pWriter.Write(PrestigePacketMode.LevelUp);
        pWriter.WriteInt(playerObjectId);
        pWriter.WriteInt(level);

        return pWriter;
    }

    public static PacketWriter Reward(int rank)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PRESTIGE);

        pWriter.Write(PrestigePacketMode.Reward);
        pWriter.WriteByte(0x01); // Unknown maybe boolean for whether to accept?
        pWriter.WriteInt(1); // Amount of rewards to accept (multiple ranks)
        pWriter.WriteInt(rank);

        return pWriter;
    }

    public static PacketWriter WeeklyMissions()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PRESTIGE);
        pWriter.Write(PrestigePacketMode.WeeklyMissions);
        pWriter.WriteInt(3); // Amount of missions
        for (int i = 1; i <= 3; i++)
        {
            pWriter.WriteLong(i); // id?
            pWriter.WriteLong();
            pWriter.WriteBool(false); // completed
        }

        return pWriter;
    }
}
