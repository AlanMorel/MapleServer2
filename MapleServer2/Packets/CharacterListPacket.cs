using System.Collections.Generic;
using Maple2Storage.Types;
using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using Maple2.Data.Types;
using Maple2.Data.Types.Items;

namespace MapleServer2.Packets {
    public static class CharacterListPacket {

        // TODO: Load real data
        public static Packet AddEntries(List<Character> players) {
            var pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST)
                .WriteByte(0x00)
                .WriteByte((byte)players.Count); // CharCount
            foreach (Character player in players) {
                pWriter.WriteCharacterEntry(player);
            }

            return pWriter;
        }

        // Sent after creating a character to append to list
        public static Packet AppendEntry(Character player) {
            var pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST)
                .WriteByte(0x01);
            WriteCharacterEntry(pWriter, player);

            return pWriter;
        }

        public static Packet SetMax(int unlocked, int total) {
            return PacketWriter.Of(SendOp.CHAR_MAX_COUNT)
                .WriteInt(unlocked)
                .WriteInt(total);
        }

        private static void WriteCharacterEntry(this PacketWriter pWriter, Character player) {
            WriteCharacter(player, pWriter);

            pWriter.WriteUnicodeString(player.DisplayPicture);
            pWriter.WriteLong();

            pWriter.WriteByte((byte)player.Equip.Count); // num equips
            foreach ((EquipSlot slot, Item equip) in player.Equip) {
                WriteEquip(slot, equip, pWriter);
            }

            byte badgeCount = 0;
            pWriter.WriteByte(badgeCount);
            for (int i = 0; i < badgeCount; i++) {
                pWriter.WriteByte();

                pWriter.WriteInt(); // BRANCH HERE if Badge
                // Badge data here is causing a LOT of potential branching...
                pWriter.WriteLong();
                pWriter.WriteInt();
            }

            var boolValue = false;
            pWriter.WriteBool(boolValue);
            if (boolValue) {
                pWriter.WriteLong();
                pWriter.WriteLong();
                var otherBoolValue = true;
                pWriter.WriteBool(otherBoolValue);
                if (otherBoolValue) {
                    pWriter.WriteInt();
                    pWriter.WriteLong();
                    pWriter.WriteUnicodeString("abc");
                    pWriter.WriteInt();
                }
            }
        }

        public static void WriteCharacter(Character player, PacketWriter pWriter) {
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteLong(player.Id);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteByte(player.Gender);
            pWriter.WriteByte(1);

            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteInt(player.MapId);
            pWriter.WriteInt(player.MapId); // Sometimes 0
            pWriter.WriteInt();
            pWriter.WriteShort(player.Level);
            pWriter.WriteShort();
            pWriter.Write<JobType>(player.jobType);
            pWriter.Write<JobCode>(player.jobCode);
            pWriter.WriteInt(); // CurHp?
            pWriter.WriteInt(); // MaxHp?
            pWriter.WriteShort();
            pWriter.WriteLong();
            pWriter.WriteLong(); // Some timestamp
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.Write<CoordF>(new CoordF()); // NOT char Coord/UnknownCoord
            pWriter.WriteInt();
            pWriter.Write<SkinColor>(player.SkinColor);
            pWriter.WriteLong(player.CreationTime);
            foreach (int trophyCount in player.Trophy) {
                pWriter.WriteInt(trophyCount);
            }

            pWriter.WriteLong(); // some uid
            pWriter.WriteUnicodeString(player.GuildName);
            pWriter.WriteUnicodeString(player.Motto);

            pWriter.WriteUnicodeString(player.DisplayPicture);

            byte clubCount = 0;
            pWriter.WriteByte(clubCount); // # Clubs
            for (int i = 0; i < clubCount; i++) {
                bool clubBool = true;
                pWriter.WriteBool(clubBool);
                if (clubBool) {
                    pWriter.WriteLong();
                    pWriter.WriteUnicodeString("club name");
                }
            }
            pWriter.WriteByte();
            for (int i = 0; i < 12; i++) {
                pWriter.WriteInt(); // ???
            }


            // Some function call on CCharacterList property
            pWriter.WriteUnicodeString("");
            pWriter.WriteLong(player.UnknownId); // THIS MUST BE CORRECT... BYPASS KEY...
            pWriter.WriteLong(2000);
            pWriter.WriteLong(3000);
            // End

            int countA = 0;
            pWriter.WriteInt(countA);
            for (int i = 0; i < countA; i++) {
                pWriter.WriteLong();
            }

            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteLong(); // Timestamp
            pWriter.WriteInt(player.PrestigeLevel);
            pWriter.WriteLong(); // Timestamp

            int countB = 0;
            pWriter.WriteInt(countB);
            for (int i = 0; i < countB; i++) {
                pWriter.WriteLong();
            }

            int countC = 0;
            pWriter.WriteInt(countC);
            for (int i = 0; i < countC; i++) {
                pWriter.WriteLong();
            }

            pWriter.WriteShort();
            pWriter.WriteLong();
        }

        // Note, the client actually uses item id to determine type
        public static void WriteEquip(EquipSlot slot, Item item, PacketWriter pWriter) {
            pWriter.WriteInt(item.Id)
                .WriteLong(item.Id)
                .WriteUnicodeString(slot.ToString())
                .WriteInt(1)
                .WriteItem(item);
        }

        public static void WriteBadge(PacketWriter pWriter) {
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteItem(new Item(70100000));
        }

        public static Packet StartList() {
            return PacketWriter.Of(SendOp.CHARACTER_LIST)
                .WriteByte(0x03);
        }

        // This only needs to be sent if char count > 0
        public static Packet EndList() {
            return PacketWriter.Of(SendOp.CHARACTER_LIST)
                .WriteByte(0x04)
                .WriteBool(false);
        }
    }
}