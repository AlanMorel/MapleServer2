using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Extensions;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class FieldPlayerPacket
{
    public static PacketWriter AddPlayer(IFieldActor<Player> fieldPlayer)
    {
        Player player = fieldPlayer.Value;
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldAddPlayer);
        pWriter.WriteInt(fieldPlayer.ObjectId);
        pWriter.WriteCharacter(player);

        // Skills
        pWriter.WriteJobInfo(player);

        // Coords
        pWriter.Write(fieldPlayer.Coord);
        pWriter.Write(fieldPlayer.Rotation);
        pWriter.WriteByte();

        // Stats
        pWriter.WriteFieldStats(fieldPlayer.Stats);

        pWriter.WriteBool(fieldPlayer.OnCooldown);
        pWriter.WriteBool(player.Guide?.Value.Type == 0);
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteLong();

        // ???
        bool flagA = false;
        pWriter.WriteBool(flagA);
        if (flagA)
        {
            pWriter.WriteLong();
            pWriter.WriteUnicodeString();
            pWriter.WriteUnicodeString();
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteUnicodeString();
            pWriter.WriteLong();
            pWriter.WriteUnicodeString();
            pWriter.WriteByte();
        }

        pWriter.WriteInt(1);
        pWriter.Write(player.SkinColor);
        pWriter.WriteUnicodeString(player.ProfileUrl);

        pWriter.WriteBool(player.Mount != null);
        if (player.Mount != null)
        {
            pWriter.WriteMount(player.Mount);

            // Unknown
            byte countA = 0;
            pWriter.WriteByte(countA);
            for (int i = 0; i < countA; i++)
            {
                pWriter.WriteInt();
                pWriter.WriteByte();
            }
        }

        pWriter.WriteInt();
        pWriter.WriteLong(TimeInfo.Now()); // some timestamp
        pWriter.WriteInt();
        pWriter.WriteInt();

        bool appearance = true;
        pWriter.WriteBool(appearance);
        if (appearance)
        {
            PacketWriter appearanceBuffer = new();
            CharacterListPacket.WriteEquipsAndCosmetics(appearanceBuffer, player);

            appearanceBuffer.WriteByte(1);
            appearanceBuffer.WriteLong();
            appearanceBuffer.WriteLong();
            appearanceBuffer.WriteByte();

            pWriter.WriteDeflated(appearanceBuffer.Buffer, 0, appearanceBuffer.Length);
        }
        else
        {
            pWriter.WriteDeflated(new byte[1], 0, 1); // Empty buffer
        }

        bool unusuedBuffer = false;
        pWriter.WriteBool(unusuedBuffer);
        if (unusuedBuffer)
        {
            // kms2 outfits? Unused buffer for gms2
        }
        else
        {
            pWriter.WriteDeflated(new byte[1], 0, 1); // Empty buffer
        }

        List<Item> badges = player.Inventory.Badges.Where(x => x != null).ToList();
        pWriter.WriteBool(badges.Count > 0);
        if (badges.Count > 0)
        {
            PacketWriter badgesBuffer = new();
            CharacterListPacket.WriteBadges(badgesBuffer, player);
            pWriter.WriteDeflated(badgesBuffer.Buffer, 0, badgesBuffer.Length);
        }
        else
        {
            pWriter.WriteDeflated(new byte[1], 0, 1); // Empty buffer
        }

        pWriter.WritePassiveSkills(fieldPlayer);

        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteInt(player.TitleId);
        pWriter.WriteShort(player.InsigniaId);
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteByte();
        pWriter.WriteLong(); // Another timestamp
        pWriter.WriteInt(int.MaxValue);
        pWriter.WriteByte();
        pWriter.WriteInt(); // MushkingRoyale taileffect kill count
        pWriter.WriteInt();
        pWriter.WriteShort();

        return pWriter;
    }

    public static PacketWriter RemovePlayer(IFieldObject<Player> player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FieldRemovePlayer);
        pWriter.WriteInt(player.ObjectId);

        return pWriter;
    }
}
