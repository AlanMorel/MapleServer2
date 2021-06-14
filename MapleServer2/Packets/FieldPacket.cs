using System;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Extensions;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class FieldPacket
    {
        public static Packet RequestEnter(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.REQUEST_FIELD_ENTER);
            pWriter.WriteByte(0x00);
            pWriter.WriteInt(player.Value.MapId);
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.Write(player.Coord);
            pWriter.Write(player.Value.Rotation);
            pWriter.WriteInt(); // Whatever is here seems to be repeated by client in FIELD_ENTER response.

            return pWriter;
        }

        public static Packet AddPlayer(IFieldObject<Player> fieldPlayer)
        {
            Player player = fieldPlayer.Value;
            PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_ADD_USER);
            pWriter.WriteInt(fieldPlayer.ObjectId);
            CharacterListPacket.WriteCharacter(player, pWriter);

            // Skills
            pWriter.WriteEnum(player.JobCode);
            pWriter.WriteByte(1);
            pWriter.WriteEnum(player.Job);
            JobPacket.WriteSkills(pWriter, player);

            // Coords
            pWriter.Write(fieldPlayer.Coord);
            pWriter.Write(player.Rotation);
            pWriter.WriteByte();

            // Stats
            StatPacket.WriteFieldStats(pWriter, player.Stats);

            pWriter.WriteByte(); // battle stance bool
            if (player.Guide != null)
            {
                pWriter.WriteByte(player.Guide.Value.Type);
            }
            else
            {
                pWriter.WriteByte();
            }
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
            pWriter.Write<SkinColor>(player.SkinColor);
            pWriter.WriteUnicodeString(player.ProfileUrl); // Profile URL

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
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds()); // some timestamp
            pWriter.WriteInt();
            pWriter.WriteInt();

            // This seems to be character appearance encoded as a blob
            pWriter.WriteBool(true);
            if (true)
            {
                PacketWriter appearanceBuffer = new PacketWriter();
                appearanceBuffer.WriteByte((byte) (player.Inventory.Equips.Count + player.Inventory.Cosmetics.Count)); // num equips
                foreach ((ItemSlot slot, Item equip) in player.Inventory.Equips)
                {
                    CharacterListPacket.WriteEquip(slot, equip, appearanceBuffer);
                }
                foreach ((ItemSlot slot, Item equip) in player.Inventory.Cosmetics)
                {
                    CharacterListPacket.WriteEquip(slot, equip, appearanceBuffer);
                }

                appearanceBuffer.WriteByte(1);
                appearanceBuffer.WriteLong();
                appearanceBuffer.WriteLong();
                appearanceBuffer.WriteByte();

                pWriter.WriteDeflated(appearanceBuffer.Buffer, 0, appearanceBuffer.Length);
                pWriter.WriteByte(); // Separator?
                pWriter.WriteDeflated(new byte[1], 0, 1); // Unknown
                pWriter.WriteByte(); // Separator?
                pWriter.WriteDeflated(new byte[1], 0, 1); // Badge appearances

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
            }
            else
            {
                //pWriter.WriteInt(); commented out to remove warning
            }

            return pWriter;
        }

        public static Packet RemovePlayer(IFieldObject<Player> player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_REMOVE_USER);
            pWriter.WriteInt(player.ObjectId);

            return pWriter;
        }

        public static Packet AddItem(IFieldObject<Item> item, int userObjectId)
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

        public static Packet AddItem(IFieldObject<Item> item, IFieldObject<Mob> sourceMob, IFieldObject<Player> targetPlayer)
        {
            // Works for meso

            PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_ADD_ITEM);
            pWriter.WriteInt(item.ObjectId);
            pWriter.WriteInt(item.Value.Id);
            pWriter.WriteInt(item.Value.Amount);

            pWriter.WriteByte(1);               // Unknown (GMS2) (character lock flag?)
            pWriter.WriteLong(targetPlayer.Value.CharacterId);  // Lock drop to character

            pWriter.Write(item.Coord);
            pWriter.WriteInt(sourceMob.ObjectId);
            pWriter.WriteInt();                 // Unknown (GMS2)
            pWriter.WriteByte();
            pWriter.WriteInt(item.Value.Rarity);
            pWriter.WriteInt(21);

            if (item.Value.Id >= 90000004 && item.Value.Id <= 90000011)
            {
                // Extra for special items
                pWriter.WriteInt(1);                        // 0 = SP/EP, 1 = quest item?
                pWriter.WriteInt(0);
                pWriter.WriteInt(-1);
                pWriter.WriteInt(targetPlayer.ObjectId);    // Unknown
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

        public static Packet PickupItem(int objectId, int userObjectId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_PICKUP_ITEM);
            pWriter.WriteByte(0x01);
            pWriter.WriteInt(objectId);
            pWriter.WriteInt(userObjectId);

            return pWriter;
        }

        public static Packet PickupItem(int objectId, Item item, int userObjectId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_PICKUP_ITEM);
            pWriter.WriteByte(0x01);
            pWriter.WriteInt(objectId);
            pWriter.WriteInt(userObjectId);
            pWriter.WriteLong(item.Amount);  // Amount (GUI)

            return pWriter;
        }

        public static Packet RemoveItem(int objectId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_REMOVE_ITEM);
            pWriter.WriteInt(objectId);

            return pWriter;
        }

        public static Packet AddNpc(IFieldObject<Npc> npc)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_ADD_NPC);
            pWriter.WriteInt(npc.ObjectId);
            pWriter.WriteInt(npc.Value.Id);
            pWriter.Write(npc.Coord);
            pWriter.Write(CoordF.From(0, 0, 0)); // Rotation
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

        public static Packet AddBoss(IFieldObject<Mob> mob)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_ADD_NPC);

            pWriter.WriteInt(mob.ObjectId);
            pWriter.WriteInt(mob.Value.Id);
            pWriter.Write(mob.Coord);
            pWriter.Write(CoordF.From(0, 0, 0)); // Rotation
            pWriter.WriteMapleString(mob.Value.Model); // StrA - kfm model string
            // If NPC is not valid, the packet seems to stop here

            StatPacket.DefaultStatsMob(pWriter, mob);

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

        public static Packet AddMob(IFieldObject<Mob> mob)
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

        public static Packet RemoveMob(IFieldObject<Mob> mob)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_REMOVE_NPC);
            pWriter.WriteInt(mob.ObjectId);
            return pWriter;
        }

        public static Packet AddPortal(IFieldObject<Portal> portal)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.FIELD_PORTAL);
            pWriter.WriteByte(0x00);
            pWriter.WriteInt(portal.Value.Id);
            pWriter.WriteBool(portal.Value.IsVisible);
            pWriter.WriteBool(portal.Value.IsEnabled);
            pWriter.Write(portal.Coord);
            pWriter.Write(portal.Value.Rotation);
            pWriter.Write<CoordF>(default); // not sure (200,200,250) was used a lot
            pWriter.WriteUnicodeString("");
            pWriter.WriteInt(portal.Value.TargetMapId);
            pWriter.WriteInt(portal.ObjectId);
            pWriter.WriteInt();
            pWriter.WriteBool(portal.Value.IsMinimapVisible);
            pWriter.WriteLong();
            pWriter.WriteByte();
            pWriter.WriteInt(portal.Value.Duration);
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteBool(portal.Value.IsPassEnabled);
            pWriter.WriteUnicodeString("");
            pWriter.WriteUnicodeString("");
            pWriter.WriteUnicodeString("");

            return pWriter;
        }
    }
}
