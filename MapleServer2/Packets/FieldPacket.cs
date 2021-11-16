using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Extensions;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class FieldPacket
{
    private enum PortalType : byte
    {
        AddPortal = 0x00,
        RemovePortal = 0x01,
        UpdatePortal = 0x02
    }

    public static PacketWriter RequestEnter(Player player)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.REQUEST_FIELD_ENTER);
        pWriter.WriteByte(0x00);
        pWriter.WriteInt(player.MapId);
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.Write(player.Session.FieldPlayer.Coord);
        pWriter.Write(player.Session.FieldPlayer.Rotation);
        pWriter.WriteInt(); // Whatever is here seems to be repeated by client in FIELD_ENTER response.

        return pWriter;
    }

    public static PacketWriter AddPlayer(IFieldActor<Player> fieldPlayer)
    {
        Player player = fieldPlayer.Value;
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_ADD_USER);
        pWriter.WriteInt(fieldPlayer.ObjectId);
        CharacterListPacket.WriteCharacter(player, pWriter);

        // Skills
        pWriter.Write(player.JobCode);
        pWriter.WriteByte(1);
        pWriter.Write(player.Job);
        JobPacket.WriteSkills(pWriter, player);

        // Coords
        pWriter.Write(fieldPlayer.Coord);
        pWriter.Write(fieldPlayer.Rotation);
        pWriter.WriteByte();

        // Stats
        StatPacket.WriteFieldStats(pWriter, fieldPlayer.Stats);

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
            pWriter.WriteUnicodeString("");
            pWriter.WriteUnicodeString("");
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteUnicodeString("");
            pWriter.WriteLong();
            pWriter.WriteUnicodeString("");
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

        JobPacket.WritePassiveSkills(pWriter, fieldPlayer);

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
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_REMOVE_USER);
        pWriter.WriteInt(player.ObjectId);

        return pWriter;
    }

    public static PacketWriter AddItem(IFieldObject<Item> item, int userObjectId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_ADD_ITEM);
        pWriter.Write(item.ObjectId); // object id
        pWriter.Write(item.Value.Id);
        pWriter.Write(item.Value.Amount);

        bool flag = true;
        pWriter.WriteBool(flag);
        if (flag)
        {
            pWriter.WriteLong();
        }

        pWriter.Write(item.Coord); // drop location
        pWriter.WriteInt(userObjectId);
        pWriter.WriteInt();
        pWriter.WriteByte(2);
        pWriter.WriteInt(item.Value.Rarity);
        pWriter.WriteShort(1005);
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteItem(item.Value);

        return pWriter;
    }

    public static PacketWriter AddItem(IFieldObject<Item> item, IFieldObject<NpcMetadata> sourceMob, IFieldObject<Player> targetPlayer)
    {
        // Works for meso

        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_ADD_ITEM);
        pWriter.WriteInt(item.ObjectId);
        pWriter.WriteInt(item.Value.Id);
        pWriter.WriteInt(item.Value.Amount);

        pWriter.WriteByte(1); // Unknown (GMS2) (character lock flag?)
        pWriter.WriteLong(targetPlayer.Value.CharacterId); // Lock drop to character

        pWriter.Write(item.Coord);
        pWriter.WriteInt(sourceMob.ObjectId);
        pWriter.WriteInt(); // Unknown (GMS2)
        pWriter.WriteByte();
        pWriter.WriteInt(item.Value.Rarity);
        pWriter.WriteInt(21);

        if (item.Value.Id >= 90000004 && item.Value.Id <= 90000011)
        {
            // Extra for special items
            pWriter.WriteInt(1); // 0 = SP/EP, 1 = quest item?
            pWriter.WriteInt(0);
            pWriter.WriteInt(-1);
            pWriter.WriteInt(targetPlayer.ObjectId); // Unknown
            for (int i = 0; i < 14; i++)
            {
                pWriter.WriteInt();
            }
            pWriter.WriteInt(-1);
            for (int i = 0; i < 24; i++)
            {
                pWriter.WriteInt();
            }
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteInt(1);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteInt(6);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteInt(1);
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteShort();
        }
        //pWriter.Write(sourceMob.Coord);
        //pWriter.WriteItem(item.Value);

        return pWriter;
    }

    public static PacketWriter PickupItem(int objectId, int userObjectId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_PICKUP_ITEM);
        pWriter.WriteByte(0x01);
        pWriter.WriteInt(objectId);
        pWriter.WriteInt(userObjectId);

        return pWriter;
    }

    public static PacketWriter PickupItem(int objectId, Item item, int userObjectId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_PICKUP_ITEM);
        pWriter.WriteByte(0x01);
        pWriter.WriteInt(objectId);
        pWriter.WriteInt(userObjectId);
        pWriter.WriteLong(item.Amount); // Amount (GUI)

        return pWriter;
    }

    public static PacketWriter RemoveItem(int objectId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_REMOVE_ITEM);
        pWriter.WriteInt(objectId);

        return pWriter;
    }

    public static PacketWriter AddNpc(IFieldObject<NpcMetadata> npc)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_ADD_NPC);
        pWriter.WriteInt(npc.ObjectId);
        pWriter.WriteInt(npc.Value.Id);
        pWriter.Write(npc.Coord);
        pWriter.Write(npc.Rotation);
        // If NPC is not valid, the packet seems to stop here

        StatPacket.DefaultStatsNpc(pWriter);

        pWriter.WriteByte();
        short count = 0;
        pWriter.WriteShort(count); // branch
        for (int i = 0; i < count; i++)
        {
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteLong();
        }

        pWriter.WriteLong(); // uid
        pWriter.WriteByte();
        pWriter.WriteInt(1); // NPC level
        pWriter.WriteInt();
        pWriter.WriteByte();

        return pWriter;
    }

    public static PacketWriter AddBoss(IFieldActor<NpcMetadata> mob)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_ADD_NPC);

        pWriter.WriteInt(mob.ObjectId);
        pWriter.WriteInt(mob.Value.Id);
        pWriter.Write(mob.Coord);
        pWriter.Write(mob.Rotation);
        pWriter.WriteString(mob.Value.Model); // StrA - kfm model string
        // If NPC is not valid, the packet seems to stop here

        StatPacket.DefaultStatsMob(pWriter, mob);

        pWriter.WriteByte();
        pWriter.WriteLong();
        pWriter.WriteLong();
        pWriter.WriteInt();
        pWriter.WriteByte();
        int count = 0;
        pWriter.WriteInt(count); // branch
        for (int i = 0; i < count; i++)
        {
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteLong();
        }
        pWriter.WriteLong();
        pWriter.WriteByte();
        pWriter.WriteInt(1);
        pWriter.WriteInt();
        pWriter.WriteByte();

        return pWriter;
    }

    public static PacketWriter AddMob(IFieldActor<NpcMetadata> mob)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_ADD_NPC);

        pWriter.WriteInt(mob.ObjectId);
        pWriter.WriteInt(mob.Value.Id);
        pWriter.Write(mob.Coord);
        pWriter.Write(mob.Rotation);
        // If NPC is not valid, the packet seems to stop here

        StatPacket.DefaultStatsMob(pWriter, mob);

        pWriter.WriteLong();
        pWriter.WriteInt();
        pWriter.WriteInt(0x0E); // NPC level
        pWriter.WriteInt();
        pWriter.WriteByte();

        return pWriter;
    }

    public static PacketWriter RemoveMob(IFieldActor<NpcMetadata> mob)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_REMOVE_NPC);
        pWriter.WriteInt(mob.ObjectId);
        return pWriter;
    }

    public static PacketWriter AddPortal(IFieldObject<Portal> fieldPortal)
    {
        Portal portal = fieldPortal.Value;
        CoordF coord = fieldPortal.Coord;
        coord.Z -= 75; // Looks like every portal coord is offset by 75

        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_PORTAL);
        pWriter.Write(PortalType.AddPortal);
        pWriter.WriteInt(portal.Id);
        pWriter.WriteBool(portal.IsVisible);
        pWriter.WriteBool(portal.IsEnabled);
        pWriter.Write(coord);
        pWriter.Write(portal.Rotation);
        pWriter.Write(CoordF.From(150, 150, 150)); // not sure (200,200,250) was used a lot
        pWriter.WriteUnicodeString("");
        pWriter.WriteInt(portal.TargetMapId);
        pWriter.WriteInt(fieldPortal.ObjectId);
        pWriter.WriteInt((int) portal.UGCPortalMethod);
        pWriter.WriteBool(portal.IsMinimapVisible);
        pWriter.WriteLong(portal.TargetHomeAccountId);
        pWriter.Write(portal.PortalType);
        pWriter.WriteInt(portal.Duration);
        pWriter.WriteShort();
        pWriter.WriteInt();
        pWriter.WriteBool(portal.IsPassEnabled);
        pWriter.WriteUnicodeString("");
        pWriter.WriteUnicodeString("");
        pWriter.WriteUnicodeString("");

        return pWriter;
    }

    public static PacketWriter RemovePortal(Portal portal)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_PORTAL);
        pWriter.Write(PortalType.RemovePortal);
        pWriter.WriteInt(portal.Id);

        return pWriter;
    }

    public static PacketWriter UpdatePortal(IFieldObject<Portal> portal)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_PORTAL);
        pWriter.Write(PortalType.UpdatePortal);
        pWriter.WriteInt(portal.Value.Id);
        pWriter.WriteBool(portal.Value.IsVisible);
        pWriter.WriteBool(portal.Value.IsEnabled);
        pWriter.WriteBool(portal.Value.IsMinimapVisible);
        pWriter.WriteBool(false);
        pWriter.WriteBool(false);
        return pWriter;
    }
}
