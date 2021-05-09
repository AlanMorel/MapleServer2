using System.Collections.Generic;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class CharacterListPacket
    {
        private enum ListMode : byte
        {
            AddEntries = 0x00,
            AppendEntry = 0x01,
            DeleteCharacter = 0x02,
            StartList = 0x03,
            EndList = 0x04,
            DeletePending = 0x05,
            DeleteCancel = 0x06,
        }
        public static Packet AddEntries(List<Player> players)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.WriteEnum(ListMode.AddEntries);
            pWriter.WriteByte((byte) players.Count);
            foreach (Player player in players)
            {
                pWriter.WriteCharacterEntry(player);
            }

            return pWriter;
        }

        // Sent after creating a character to append to list
        public static Packet AppendEntry(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.WriteEnum(ListMode.AppendEntry);
            WriteCharacterEntry(pWriter, player);

            return pWriter;
        }

        public static Packet DeleteCharacter(long playerId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.WriteEnum(ListMode.DeleteCharacter);
            pWriter.WriteInt(); // unk
            pWriter.WriteLong(playerId);

            return pWriter;
        }

        public static Packet DeletePending(long playerId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.WriteEnum(ListMode.DeletePending);
            pWriter.WriteLong(playerId);
            pWriter.WriteInt(); // unk
            pWriter.WriteLong(); // delete timestamp

            return pWriter;
        }

        public static Packet DeleteCancel(long playerId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.WriteEnum(ListMode.DeleteCancel);
            pWriter.WriteLong(playerId);
            pWriter.WriteInt(); // unk

            return pWriter;
        }

        public static Packet SetMax(int unlocked, int total)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHAR_MAX_COUNT);
            pWriter.WriteInt(unlocked);
            pWriter.WriteInt(total);

            return pWriter;
        }

        private static void WriteCharacterEntry(this PacketWriter pWriter, Player player)
        {
            WriteCharacter(player, pWriter);

            pWriter.WriteUnicodeString(player.ProfileUrl);
            pWriter.WriteLong();

            pWriter.WriteByte((byte) (player.Inventory.Equips.Count + player.Inventory.Cosmetics.Count)); // num equips
            foreach ((ItemSlot slot, Item equip) in player.Inventory.Equips)
            {
                WriteEquip(slot, equip, pWriter);
            }
            foreach ((ItemSlot slot, Item equip) in player.Inventory.Cosmetics)
            {
                WriteEquip(slot, equip, pWriter);
            }

            byte badgeCount = 0;
            pWriter.WriteByte(badgeCount);
            for (int i = 0; i < badgeCount; i++)
            {
                pWriter.WriteByte();

                pWriter.WriteInt(); // BRANCH HERE if Badge
                // Badge data here is causing a LOT of potential branching...
                pWriter.WriteLong();
                pWriter.WriteInt();
            }

            bool boolValue = false;
            pWriter.WriteBool(boolValue);
            if (boolValue)
            {
                pWriter.WriteLong();
                pWriter.WriteLong();
                bool otherBoolValue = true;
                pWriter.WriteBool(otherBoolValue);
                if (otherBoolValue)
                {
                    pWriter.WriteInt();
                    pWriter.WriteLong();
                    pWriter.WriteUnicodeString("abc");
                    pWriter.WriteInt();
                }
            }
        }

        public static void WriteCharacter(Player player, PacketWriter pWriter)
        {
            pWriter.WriteLong(player.AccountId);
            pWriter.WriteLong(player.CharacterId);
            pWriter.WriteUnicodeString(player.Name);
            pWriter.WriteByte(player.Gender);
            pWriter.WriteByte(1);

            pWriter.WriteLong(player.AccountId);
            pWriter.WriteInt();
            pWriter.WriteInt(player.MapId);
            pWriter.WriteInt(player.MapId); // Sometimes 0
            pWriter.WriteInt();
            pWriter.WriteShort(player.Levels.Level);
            pWriter.WriteShort();
            pWriter.WriteEnum(player.Job);
            pWriter.WriteEnum(player.JobCode);
            pWriter.WriteInt(player.Stats[PlayerStatId.Hp].Current);
            pWriter.WriteInt(player.Stats[PlayerStatId.Hp].Max);
            pWriter.WriteShort();
            pWriter.WriteLong();
            pWriter.WriteLong(); // Some timestamp
            pWriter.WriteLong();
            pWriter.WriteInt(player.ReturnMapId);
            pWriter.Write(player.ReturnCoord);
            pWriter.WriteInt(); // gearscore
            pWriter.Write(player.SkinColor);
            pWriter.WriteLong(player.CreationTime);
            foreach (int trophyCount in player.TrophyCount)
            {
                pWriter.WriteInt(trophyCount);
            }

            pWriter.WriteLong(player.GuildId);
            pWriter.WriteUnicodeString(player.GuildName);
            pWriter.WriteUnicodeString(player.Motto);

            pWriter.WriteUnicodeString(player.ProfileUrl);

            byte clubCount = 0;
            pWriter.WriteByte(clubCount); // # Clubs
            for (int i = 0; i < clubCount; i++)
            {
                bool clubBool = true;
                pWriter.WriteBool(clubBool);
                if (clubBool)
                {
                    pWriter.WriteLong();
                    pWriter.WriteUnicodeString("club name");
                }
            }
            pWriter.WriteByte(); // groups?
            for (int i = 0; i < 12; i++)
            {
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
            for (int i = 0; i < countA; i++)
            {
                pWriter.WriteLong();
            }

            pWriter.WriteByte();
            pWriter.WriteByte();
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteLong(); // Timestamp
            pWriter.WriteInt(player.Levels.PrestigeLevel);
            pWriter.WriteLong(); // Timestamp

            int countB = 0;
            pWriter.WriteInt(countB);
            for (int i = 0; i < countB; i++)
            {
                pWriter.WriteLong();
            }

            int countC = 0;
            pWriter.WriteInt(countC);
            for (int i = 0; i < countC; i++)
            {
                pWriter.WriteLong();
            }

            pWriter.WriteShort();
            pWriter.WriteLong();
        }

        // Note, the client actually uses item id to determine type
        public static void WriteEquip(ItemSlot slot, Item item, PacketWriter pWriter)
        {
            pWriter.WriteInt(item.Id);
            pWriter.WriteLong(item.Uid);
            pWriter.WriteUnicodeString(slot.ToString());
            pWriter.WriteInt(1);
            pWriter.WriteItem(item);
        }

        public static void WriteBadge(PacketWriter pWriter)
        {
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteItem(new Item(70100000));
        }

        public static Packet StartList()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.WriteEnum(ListMode.StartList);

            return pWriter;
        }

        // This only needs to be sent if char count > 0
        public static Packet EndList()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.WriteEnum(ListMode.EndList);
            pWriter.WriteBool(false);

            return pWriter;
        }
    }
}
