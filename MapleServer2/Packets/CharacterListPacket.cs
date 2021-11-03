using Maple2Storage.Enums;
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
            NameChange = 0x07,
        }
        public static PacketWriter AddEntries(List<Player> players)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.Write(ListMode.AddEntries);
            pWriter.WriteByte((byte) players.Count);
            foreach (Player player in players)
            {
                pWriter.WriteCharacterEntry(player);
            }

            return pWriter;
        }

        // Sent after creating a character to append to list
        public static PacketWriter AppendEntry(Player player)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.Write(ListMode.AppendEntry);
            WriteCharacterEntry(pWriter, player);

            return pWriter;
        }

        public static PacketWriter DeleteCharacter(long playerId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.Write(ListMode.DeleteCharacter);
            pWriter.WriteInt(); // unk
            pWriter.WriteLong(playerId);

            return pWriter;
        }

        public static PacketWriter DeletePending(long playerId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.Write(ListMode.DeletePending);
            pWriter.WriteLong(playerId);
            pWriter.WriteInt(); // unk
            pWriter.WriteLong(); // delete timestamp

            return pWriter;
        }

        public static PacketWriter DeleteCancel(long playerId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.Write(ListMode.DeleteCancel);
            pWriter.WriteLong(playerId);
            pWriter.WriteInt(); // unk

            return pWriter;
        }

        public static PacketWriter SetMax(int unlocked, int total = 11)
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

            WriteEquipsAndCosmetics(pWriter, player);

            WriteBadges(pWriter, player);

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
            pWriter.Write(player.Job);
            pWriter.Write(player.JobCode);
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
            pWriter.WriteLong(player.CreationTime + Environment.TickCount64);
            foreach (int trophyCount in player.TrophyCount)
            {
                pWriter.WriteInt(trophyCount);
            }
            pWriter.WriteLong(player.GuildId);
            pWriter.WriteUnicodeString(player.Guild?.Name ?? "");
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
            pWriter.WriteByte();
            pWriter.WriteInt();
            foreach (MasteryExp mastery in player.Levels.MasteryExp)
            {
                pWriter.WriteInt((int) mastery.CurrentExp);
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
            pWriter.WriteInt(item.Rarity);
            pWriter.WriteItem(item);
        }

        public static void WriteEquipsAndCosmetics(PacketWriter pWriter, Player player)
        {
            pWriter.WriteByte((byte) (player.Inventory.Equips.Count + player.Inventory.Cosmetics.Count));
            foreach ((ItemSlot slot, Item equip) in player.Inventory.Equips)
            {
                WriteEquip(slot, equip, pWriter);
            }
            foreach ((ItemSlot slot, Item equip) in player.Inventory.Cosmetics)
            {
                WriteEquip(slot, equip, pWriter);
            }
        }

        public static void WriteBadges(PacketWriter pWriter, Player player)
        {
            pWriter.WriteByte((byte) player.Inventory.Badges.Where(x => x != null).Count());
            for (int i = 0; i < player.Inventory.Badges.Length; i++)
            {
                Item badge = player.Inventory.Badges[i];
                if (player.Inventory.Badges[i] != null)
                {
                    pWriter.WriteByte((byte) i);
                    pWriter.WriteInt(badge.Id);
                    pWriter.WriteLong(badge.Uid);
                    pWriter.WriteInt(badge.Rarity);
                    pWriter.WriteItem(badge);
                }
            }
        }

        public static PacketWriter StartList()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.Write(ListMode.StartList);

            return pWriter;
        }

        // This only needs to be sent if char count > 0
        public static PacketWriter EndList()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.Write(ListMode.EndList);
            pWriter.WriteBool(false);

            return pWriter;
        }

        public static PacketWriter NameChanged(long characterId, string characterName)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.CHARACTER_LIST);
            pWriter.Write(ListMode.NameChange);
            pWriter.WriteInt(1);
            pWriter.WriteLong(characterId);
            pWriter.WriteUnicodeString(characterName);
            return pWriter;
        }
    }
}
