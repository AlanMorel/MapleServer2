using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class TrophyPacket
{
    private enum Mode : byte
    {
        TableStart = 0x0,
        TableContent = 0x1,
        Update = 0x2,
        Favorite = 0x04
    }

    private enum GradeStatus : byte
    {
        InProgress = 0x2,
        Finished = 0x3
    }

    public static PacketWriter WriteTableStart()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trophy);
        pWriter.Write(Mode.TableStart);

        return pWriter;
    }

    // packet from WriteTableStart() must be sent immediately before sending these packets
    public static PacketWriter WriteTableContent(List<Trophy> trophies)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trophy);
        pWriter.Write(Mode.TableContent);
        pWriter.WriteInt(trophies.Count);

        foreach (Trophy trophy in trophies)
        {
            pWriter.WriteInt(trophy.Id);
            pWriter.WriteInt(1); // unknown 'SS' ?
            WriteIndividualTrophy(pWriter, trophy);
        }

        return pWriter;
    }

    public static PacketWriter WriteUpdate(Trophy trophy)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trophy);
        pWriter.Write(Mode.Update);
        pWriter.WriteInt(trophy.Id);
        WriteIndividualTrophy(pWriter, trophy);

        return pWriter;
    }

    public static PacketWriter ToggleFavorite(Trophy trophy, bool favorited)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trophy);
        pWriter.Write(Mode.Favorite);
        pWriter.WriteInt(trophy.Id);
        pWriter.WriteBool(favorited);

        return pWriter;
    }

    private static void WriteIndividualTrophy(PacketWriter pWriter, Trophy trophy)
    {
        int timestampsCount = trophy.Timestamps.Count;

        pWriter.Write(trophy.IsDone ? GradeStatus.Finished : GradeStatus.InProgress);
        pWriter.WriteInt(trophy.IsDone ? 1 : 0);
        pWriter.WriteInt(trophy.NextGrade);
        pWriter.WriteInt(trophy.LastReward);
        pWriter.WriteBool(trophy.Favorited);
        pWriter.WriteLong(trophy.Counter);
        pWriter.WriteInt(timestampsCount);
        for (int i = 0; i < timestampsCount; i++)
        {
            pWriter.WriteInt(i + 1);
            pWriter.WriteLong(trophy.Timestamps[i]);
        }
    }
}
