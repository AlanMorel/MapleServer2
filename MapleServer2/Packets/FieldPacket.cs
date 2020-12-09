using System;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Extensions;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets {
    public static class FieldPacket {
        public static Packet RequestEnter(IFieldObject<Player> player) {
            return PacketWriter.Of(SendOp.REQUEST_FIELD_ENTER)
                .WriteByte(0x00)
                .WriteInt(player.Value.MapId)
                .WriteByte()
                .WriteByte()
                .WriteInt()
                .WriteInt()
                .Write<CoordF>(player.Coord)
                .Write<CoordF>(player.Value.UnknownCoord)
                .WriteInt(); // Whatever is here seems to be repeated by client in FIELD_ENTER response.
        }

        public static Packet AddPlayer(IFieldObject<Player> fieldPlayer) {
            Player player = fieldPlayer.Value;
            var pWriter = PacketWriter.Of(SendOp.FIELD_ADD_USER);
            pWriter.WriteInt(fieldPlayer.ObjectId);
            CharacterListPacket.WriteCharacter(player, pWriter);

            // Skills
            pWriter.WriteInt(player.JobId);
            pWriter.WriteByte(1);
            pWriter.WriteInt(player.JobGroupId);
            JobPacket.WriteSkills(pWriter, player);

            // Coords
            pWriter.Write<CoordF>(fieldPlayer.Coord);
            pWriter.Write<CoordF>(player.UnknownCoord);
            pWriter.WriteByte();

            pWriter.WriteTotalStats(ref player.Stats);
            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteLong();
            pWriter.WriteLong();

            // ???
            bool flagA = false;
            pWriter.WriteBool(flagA);
            if (flagA) {
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
            if (player.Mount != null) {
                pWriter.WriteMount(player.Mount);

                // Unknown
                byte countA = 0;
                pWriter.WriteByte(countA);
                for (int i = 0; i < countA; i++) {
                    pWriter.WriteInt()
                        .WriteByte();
                }
            }
            pWriter.WriteInt();
            pWriter.WriteLong(DateTimeOffset.UtcNow.ToUnixTimeSeconds()); // some timestamp
            pWriter.WriteInt();
            pWriter.WriteInt();

            // This seems to be character appearance encoded as a blob
            pWriter.WriteBool(true);
            if (true) {
                var appearanceBuffer = new PacketWriter();
                appearanceBuffer.WriteByte((byte)player.Equips.Count); // num equips
                foreach ((ItemSlot slot, Item equip) in player.Equips) {
                    CharacterListPacket.WriteEquip(slot, equip, appearanceBuffer);
                }

                appearanceBuffer.WriteByte(1)
                    .WriteLong()
                    .WriteLong()
                    .WriteByte();

                pWriter.WriteDeflated(appearanceBuffer.Buffer, 0, appearanceBuffer.Length);
                pWriter.WriteByte(); // Separator?
                pWriter.WriteDeflated(new byte[1], 0, 1); // Unknown
                pWriter.WriteByte(); // Separator?
                pWriter.WriteDeflated(new byte[1], 0, 1); // Badge appearances

                WritePassiveSkills(pWriter);

                pWriter.WriteInt();
                pWriter.WriteInt();
                pWriter.WriteByte();
                pWriter.WriteInt();
                pWriter.WriteByte();
                pWriter.WriteByte();
                pWriter.WriteInt(); // TitleId
                pWriter.WriteShort();
                pWriter.WriteByte();
                pWriter.WriteInt();
                pWriter.WriteByte();
                pWriter.WriteLong(); // Another timestamp
                pWriter.WriteInt(int.MaxValue);
                pWriter.WriteByte();
                pWriter.WriteInt();
                pWriter.WriteInt();
                pWriter.WriteShort();
            } else {
                //pWriter.WriteInt(); commented out to remove warning
            }

            return pWriter;
        }

        public static Packet RemovePlayer(IFieldObject<Player> player) {
            return PacketWriter.Of(SendOp.FIELD_REMOVE_USER)
                .WriteInt(player.ObjectId);
        }

        private static void WriteTotalStats(this PacketWriter pWriter, ref PlayerStats stats) {
            pWriter.WriteByte(0x23);
            for (int i = 0; i < 3; i++) {
                pWriter.WriteLong(stats.Hp[i])
                    .WriteInt(stats.AtkSpd[i])
                    .WriteInt(stats.MoveSpd[i])
                    .WriteInt(stats.MountSpeed[i])
                    .WriteInt(stats.JumpHeight[i]);
            }

            /* Alternative Stat Struct
            pWriter.WriteByte(); // Count
            for (int i = 0; i < count; i++) {
                pWriter.WriteByte(); // Type
                if (type == 4) pWriter.WriteLong();
                else pWriter.WriteInt();
            }
            */
        }

        private static void WritePassiveSkills(PacketWriter pWriter) {
            pWriter.Write(
                "01 00 3E FF 5A 00 A4 63 12 02 3E FF 5A 00 D0 71 85 28 D0 71 85 28 0F 38 A0 00 01 00 01 00 00 00 01 00 00 00 00 00 00 00 00"
                    .ToByteArray());
            /*short count = 0;
            pWriter.WriteShort(count);
            for (int i = 0; i < count; i++) {
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
            }*/
        }

        public static Packet AddItem(IFieldObject<Item> item, int userObjectId) {
            var pWriter = PacketWriter.Of(SendOp.FIELD_ADD_ITEM)
                .Write(item.ObjectId) // object id
                .Write(item.Value.Id)
                .Write(item.Value.Amount);

            bool flag = true;
            pWriter.WriteBool(flag);
            if (flag) {
                pWriter.WriteLong();
            }

            return pWriter.Write<CoordF>(item.Coord) // drop location
                .WriteInt(userObjectId)
                .WriteInt()
                .WriteByte(2)
                .WriteInt(item.Value.Rarity)
                .WriteShort(1005)
                .WriteByte()
                .WriteByte()
                .WriteItem(item.Value);
        }

        public static Packet PickupItem(int objectId, int userObjectId) {
            return PacketWriter.Of(SendOp.FIELD_PICKUP_ITEM)
                .WriteByte(0x01)
                .WriteInt(objectId)
                .WriteInt(userObjectId);
        }

        public static Packet RemoveItem(int objectId) {
            return PacketWriter.Of(SendOp.FIELD_REMOVE_ITEM)
                .WriteInt(objectId);
        }

        public static Packet AddNpc(IFieldObject<Npc> npc) {
            var pWriter = PacketWriter.Of(SendOp.FIELD_ADD_NPC)
                .WriteInt(npc.ObjectId)
                .WriteInt(npc.Value.Id)
                .Write<CoordF>(npc.Coord)
                .Write<CoordF>(CoordF.From(0,0,0)); // Unknown
            // If NPC is not valid, the packet seems to stop here

            // NPC Stat
            byte flag = 0x23;
            pWriter.WriteByte(flag);
            if (flag == 1) {
                byte value = 0;
                pWriter.WriteByte(value); // value
                if (value == 4) {
                    pWriter.WriteLong()
                        .WriteLong()
                        .WriteLong();
                } else {
                    pWriter.WriteInt()
                        .WriteInt()
                        .WriteInt();
                }
            } else {
                pWriter.WriteLong(29)
                    .WriteInt()
                    .WriteLong(29)
                    .WriteInt()
                    .WriteLong(29)
                    .WriteInt();
            }
            // NPC Stat

            pWriter.WriteByte();

            short count = 0;
            pWriter.WriteShort(count); // branch
            for (int i = 0; i < count; i++) {
                pWriter.WriteInt()
                    .WriteInt()
                    .WriteInt()
                    .WriteInt()
                    .WriteInt()
                    .WriteInt()
                    .WriteShort()
                    .WriteInt()
                    .WriteByte()
                    .WriteLong();
            }

            pWriter.WriteLong() // uid
                .WriteByte()
                .WriteInt(1)
                .WriteInt()
                .WriteByte();

            return pWriter;
        }

        public static Packet AddPortal(IFieldObject<Portal> portal) {
            return PacketWriter.Of(SendOp.FIELD_PORTAL)
                .WriteByte(0x00)
                .WriteInt(portal.Value.Id)
                .WriteBool(portal.Value.IsVisible)
                .WriteBool(portal.Value.IsEnabled)
                .Write<CoordF>(portal.Coord)
                .Write<CoordF>(portal.Value.Rotation)
                .Write<CoordF>(default) // not sure (200,200,250) was used a lot
                .WriteUnicodeString("")
                .WriteInt(portal.Value.TargetMapId)
                .WriteInt(portal.ObjectId)
                .WriteInt()
                .WriteBool(portal.Value.IsMinimapVisible)
                .WriteLong()
                .WriteByte()
                .WriteInt()
                .WriteShort()
                .WriteInt()
                .WriteBool(false)
                .WriteUnicodeString("")
                .WriteUnicodeString("")
                .WriteUnicodeString("");
        }
    }
}