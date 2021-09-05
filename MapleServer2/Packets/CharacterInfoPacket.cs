using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public class CharacterInfoPacket
    {
        public static Packet WriteCharacterInfo(long characterId, Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_INFO);
            pWriter.WriteLong(characterId);
            pWriter.WriteBool(player != null);
            if (player == null)
            {
                return pWriter;
            }
            pWriter.WriteLong(); // unknown
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

            PacketWriter characterBuffer = new PacketWriter();
            characterBuffer.WriteLong(player.AccountId);
            characterBuffer.WriteLong(player.CharacterId);
            characterBuffer.WriteUnicodeString(player.Name);
            characterBuffer.WriteShort(player.Levels.Level);
            characterBuffer.WriteEnum(player.Job);
            characterBuffer.WriteEnum(player.JobCode);
            characterBuffer.WriteInt();
            characterBuffer.WriteInt(player.Levels.PrestigeLevel);
            characterBuffer.WriteByte();
            WriteStats(characterBuffer, player);

            // Unknown data
            characterBuffer.WriteZero(1300);

            characterBuffer.WriteUnicodeString(player.ProfileUrl);
            characterBuffer.WriteUnicodeString(player.Motto);

            if (player.Guild == null)
            {
                characterBuffer.WriteUnicodeString(string.Empty);
                characterBuffer.WriteUnicodeString(string.Empty);
            }
            else
            {
                characterBuffer.WriteUnicodeString(player.Guild.Name);
                characterBuffer.WriteUnicodeString(player.Guild.Ranks[player.GuildMember.Rank].Name);
            }

            characterBuffer.WriteUnicodeString(player.Account.Home?.Name ?? string.Empty);
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
            pWriter.Write(characterBuffer.ToArray());

            PacketWriter appearanceBuffer = new PacketWriter();
            CharacterListPacket.WriteEquipsAndCosmetics(appearanceBuffer, player);

            appearanceBuffer.WriteByte(1);
            appearanceBuffer.WriteLong();
            appearanceBuffer.WriteLong();
            appearanceBuffer.WriteByte();

            pWriter.WriteInt(appearanceBuffer.Length);
            pWriter.Write(appearanceBuffer.ToArray());

            PacketWriter badgeBuffer = new PacketWriter();
            CharacterListPacket.WriteBadges(badgeBuffer, player);

            pWriter.WriteInt(badgeBuffer.Length);
            pWriter.Write(badgeBuffer.ToArray());

            return pWriter;
        }

        private static void WriteStats(PacketWriter pWritter, Player player)
        {
            foreach (PlayerStat item in player.Stats.Data.Values)
            {
                pWritter.WriteLong(item.Max);
            }

            foreach (PlayerStat item in player.Stats.Data.Values)
            {
                pWritter.WriteLong(item.Min);
            }

            foreach (PlayerStat item in player.Stats.Data.Values)
            {
                pWritter.WriteLong(item.Current);
            }

            foreach (PlayerStat item in player.Stats.Data.Values)
            {
                pWritter.WriteLong();
            }
        }
    }
}
