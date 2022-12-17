using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class CharacterInfoPacket
{
    public static PacketWriter WriteCharacterInfo(long characterId, Player? player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.CharInfo);
        pWriter.WriteLong(characterId);
        pWriter.WriteBool(player is not null);
        if (player is null)
        {
            return pWriter;
        }
        pWriter.WriteLong(); // unknown
        pWriter.WriteLong(player.CharacterId);
        pWriter.WriteLong(TimeInfo.Now());

        PacketWriter characterBuffer = new();
        characterBuffer.WriteLong(player.AccountId);
        characterBuffer.WriteLong(player.CharacterId);
        characterBuffer.WriteUnicodeString(player.Name);
        characterBuffer.WriteShort(player.Levels.Level);
        characterBuffer.Write(player.JobCode);
        characterBuffer.Write(player.SubJobCode);
        characterBuffer.WriteInt((int) player.Gender);
        characterBuffer.WriteInt(player.Account.Prestige.Level);
        characterBuffer.WriteByte();
        WriteStats(characterBuffer, player);

        // Unknown data
        characterBuffer.WriteZero(1300);

        characterBuffer.WriteUnicodeString(player.ProfileUrl);
        characterBuffer.WriteUnicodeString(player.Motto);
        if (player.GuildMember is not null && player.Guild is not null)
        {
            characterBuffer.WriteUnicodeString(player.Guild.Name);
            characterBuffer.WriteUnicodeString(player.Guild.Ranks[player.GuildMember.Rank].Name);
        }
        else
        {
            characterBuffer.WriteUnicodeString();
            characterBuffer.WriteUnicodeString();
        }

        characterBuffer.WriteUnicodeString(player.Account.Home?.Name);
        characterBuffer.WriteZero(12);
        characterBuffer.WriteInt(player.TitleId);
        characterBuffer.WriteInt(player.Titles.Count);
        foreach (int titleId in player.Titles)
        {
            characterBuffer.WriteInt(titleId);
        }
        characterBuffer.WriteInt(player.TrophyCount.Sum());
        characterBuffer.WriteInt(); // gear score
        characterBuffer.WriteLong(); // timestamp
        characterBuffer.WriteLong();
        characterBuffer.Write(player.SkinColor);
        characterBuffer.WriteZero(14);

        pWriter.WriteInt(characterBuffer.Length);
        pWriter.WriteBytes(characterBuffer.ToArray());

        PacketWriter appearanceBuffer = new();
        CharacterListPacket.WriteEquipsAndCosmetics(appearanceBuffer, player);

        appearanceBuffer.WriteByte(1);
        appearanceBuffer.WriteLong();
        appearanceBuffer.WriteLong();
        appearanceBuffer.WriteByte();

        pWriter.WriteInt(appearanceBuffer.Length);
        pWriter.WriteBytes(appearanceBuffer.ToArray());

        PacketWriter badgeBuffer = new();
        CharacterListPacket.WriteBadges(badgeBuffer, player);

        pWriter.WriteInt(badgeBuffer.Length);
        pWriter.WriteBytes(badgeBuffer.ToArray());

        return pWriter;
    }

    private static void WriteStats(PacketWriter pWriter, Player player)
    {
        foreach (Stat item in player.Stats.Data.Values)
        {
            pWriter.WriteLong(item.Bonus);
        }

        foreach (Stat item in player.Stats.Data.Values)
        {
            pWriter.WriteLong(item.Base);
        }

        foreach (Stat item in player.Stats.Data.Values)
        {
            pWriter.WriteLong(item.Total);
        }

        foreach (Stat unused in player.Stats.Data.Values)
        {
            pWriter.WriteLong();
        }
    }
}
