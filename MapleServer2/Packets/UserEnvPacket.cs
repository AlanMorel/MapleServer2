using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class UserEnvPacket
{
    private enum UserEnvPacketMode : byte
    {
        AddTitle = 0x0,
        UpdateTitles = 0x1,
        SetTitles = 0x2,
        LifeSkills = 0x8
    }

    public static PacketWriter AddTitle(int titleId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserEnv);
        pWriter.Write(UserEnvPacketMode.AddTitle);
        pWriter.WriteInt(titleId);
        return pWriter;
    }

    public static PacketWriter UpdateTitle(GameSession session, int titleId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserEnv);
        pWriter.Write(UserEnvPacketMode.UpdateTitles);
        pWriter.WriteInt(session.Player.FieldPlayer.ObjectId);
        pWriter.WriteInt(titleId);
        return pWriter;
    }

    // Unlocked Titles
    public static PacketWriter SetTitles(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserEnv);
        pWriter.Write(UserEnvPacketMode.SetTitles);
        pWriter.WriteInt(player.Titles.Count);
        foreach (int titleId in player.Titles)
        {
            pWriter.WriteInt(titleId);
        }

        return pWriter;
    }

    public static PacketWriter UpdateTrophy()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserEnv);
        // seems unchanged before and after gaining trophy
        // TODO: figure out what the bytes mean
        byte[] toSend =
        {
            0x03, 0x03, 0x00, 0x00, 0x00, 0x17, 0xC9, 0xC9, 0x01, 0x01, 0x19, 0xC9, 0xC9, 0x01, 0x01, 0xDA, 0xC9, 0xA2, 0x03, 0x01
        };
        pWriter.WriteBytes(toSend);
        return pWriter;
    }

    public static PacketWriter Send03()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserEnv);
        pWriter.WriteByte(0x03);
        pWriter.WriteInt();
        // Loop: Int + Byte

        return pWriter;
    }

    public static PacketWriter Send04()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserEnv);
        pWriter.WriteByte(0x04);
        pWriter.WriteInt();
        // Loop: Int

        return pWriter;
    }

    public static PacketWriter Send05()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserEnv);
        pWriter.WriteByte(0x05);
        pWriter.WriteInt();

        return pWriter;
    }

    public static PacketWriter UpdateLifeSkills(List<GatheringCount> gatherings)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserEnv);
        pWriter.Write(UserEnvPacketMode.LifeSkills);
        pWriter.WriteInt(gatherings.Count);
        foreach (GatheringCount gathering in gatherings)
        {
            pWriter.WriteInt(gathering.RecipeId);
            pWriter.WriteInt(gathering.CurrentCount);
        }
        pWriter.WriteInt();

        return pWriter;
    }

    public static PacketWriter Send09()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserEnv);
        pWriter.WriteByte(0x09);
        pWriter.WriteInt();
        // Loop: Int + Byte

        return pWriter;
    }

    public static PacketWriter Send10()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserEnv);
        pWriter.WriteByte(0x0A);
        pWriter.WriteInt();
        // Loop: Short + Byte

        return pWriter;
    }

    public static PacketWriter Send11()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserEnv);
        pWriter.WriteByte(0x0B);
        pWriter.WriteShort();
        pWriter.WriteInt();

        return pWriter;
    }

    public static PacketWriter Send12()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserEnv);
        pWriter.WriteByte(0x0C);
        pWriter.WriteInt();
        // Loop: Int + Long

        return pWriter;
    }

    public static PacketWriter Send13()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.UserEnv);
        pWriter.WriteByte(0x0D);
        pWriter.WriteInt();
        pWriter.WriteLong();

        return pWriter;
    }
}
