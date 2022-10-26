using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class PrestigePacket
{
    private enum Mode : byte
    {
        SetLevels = 0x00,
        Exp = 0x01,
        LevelUp = 0x02,
        Reward = 0x04,
        UpdateMissions = 0x06,
        WeeklyMissions = 0x07
    }

    public static PacketWriter SetLevels(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Prestige);
        pWriter.Write(Mode.SetLevels);
        pWriter.WriteLong(player.Levels.PrestigeExp);
        pWriter.WriteInt(player.Levels.PrestigeLevel);
        pWriter.WriteLong(player.Levels.PrestigeExp);

        List<int> rankRewardsClaimed = player.Account.PrestigeRewardsClaimed;
        pWriter.WriteInt(rankRewardsClaimed.Count);
        foreach (int rank in rankRewardsClaimed)
        {
            pWriter.WriteInt(rank);
        }

        return pWriter;
    }

    public static PacketWriter ExpUp(long prestigeExp, long amount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Prestige);

        pWriter.Write(Mode.Exp);
        pWriter.WriteLong(prestigeExp);
        pWriter.WriteLong(amount);

        return pWriter;
    }

    public static PacketWriter LevelUp(int playerObjectId, int level)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Prestige);

        pWriter.Write(Mode.LevelUp);
        pWriter.WriteInt(playerObjectId);
        pWriter.WriteInt(level);

        return pWriter;
    }

    public static PacketWriter Reward(int rank)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Prestige);

        pWriter.Write(Mode.Reward);
        pWriter.WriteByte(0x01); // Unknown maybe boolean for whether to accept?
        pWriter.WriteInt(1); // Amount of rewards to accept (multiple ranks)
        pWriter.WriteInt(rank);

        return pWriter;
    }

    public static PacketWriter UpdateMissions(List<PrestigeMission> missions)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Prestige);
        pWriter.Write(Mode.UpdateMissions);
        pWriter.WriteInt(missions.Count);

        foreach (PrestigeMission mission in missions)
        {
            pWriter.WriteLong(mission.Id);
            pWriter.WriteLong(mission.LevelCount);
            pWriter.WriteBool(mission.Claimed);
        }

        return pWriter;
    }

    public static PacketWriter WeeklyMissions(List<PrestigeMission> missions)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Prestige);
        pWriter.Write(Mode.WeeklyMissions);
        pWriter.WriteInt(missions.Count);

        foreach (PrestigeMission mission in missions)
        {
            pWriter.WriteLong(mission.Id);
            pWriter.WriteLong(mission.LevelCount);
            pWriter.WriteBool(mission.Claimed);
        }

        return pWriter;
    }
}
