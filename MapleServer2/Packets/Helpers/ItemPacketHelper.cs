using System.Collections.Generic;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Types;

namespace MapleServer2.Packets.Helpers {
    public static class ItemPacketHelper {
        public static PacketWriter WriteItem(this PacketWriter pWriter, Item item) {
            pWriter.WriteInt(item.Amount)
                .WriteInt()
                .WriteInt(-1)
                .WriteLong(item.CreationTime)
                .WriteLong(item.ExpiryTime)
                .WriteLong()
                .WriteInt(item.TimesAttributesChanged)
                .WriteInt()
                .WriteBool(item.IsLocked)
                .WriteLong(item.UnlockTime)
                .WriteShort(item.RemainingGlamorForges)
                .WriteByte()
                .WriteInt()
                .WriteAppearance(item)
                .WriteStats(item.Stats)
                .WriteInt(item.Enchants)
                .WriteInt(item.EnchantExp)
                .WriteBool(true) // Enchant based peachy charges, otherwise always require 10 charges
                .WriteLong()
                .WriteInt()
                .WriteInt()
                .WriteBool(item.CanRepackage)
                .WriteInt(item.Charges)
                .WriteStatDiff(item.Stats, item.Stats);

            if (item.IsTemplate) {
                pWriter.WriteTemplate();
            }

            if (item.InventoryType == InventoryTab.Pets) {
                pWriter.WritePet();
            }

            pWriter.WriteInt((int)item.TransferFlag)
                .WriteByte()
                .WriteInt()
                .WriteInt()
                .WriteByte()
                .WriteByte(1);

            // CharBound means untradable, unsellable, bound to char (ignores TransferFlag)
            bool isCharBound = (item.Owner != null);
            pWriter.WriteBool(isCharBound);
            if (isCharBound) {
                pWriter.WriteLong(item.Owner.CharacterId);
                pWriter.WriteUnicodeString(item.Owner.Name);
            }

            pWriter.WriteByte();

            pWriter.WriteSockets(item.Stats);

            pWriter.WriteLong(item.PairedCharacterId);
            if (item.PairedCharacterId != 0) {
                pWriter.WriteUnicodeString(item.PairedCharacterName);
            }

            // Bound to character
            return pWriter.WriteLong()
                .WriteUnicodeString("");
        }

        private static PacketWriter WriteAppearance(this PacketWriter pWriter, Item item) {
            pWriter.Write<EquipColor>(item.Color);
            pWriter.WriteInt(item.AppearanceFlag);
            // Positioning Data
            switch (item.ItemSlot) {
                case ItemSlot.CP:
                    for (int i = 0; i < 13; i++) {
                        pWriter.Write<float>(0);
                    }
                    break;
                case ItemSlot.HR:
                    pWriter.Write<float>(0.3f);
                    pWriter.WriteZero(24);
                    pWriter.Write<float>(0.3f);
                    pWriter.WriteZero(24);
                    break;
                case ItemSlot.FD:
                    for (int i = 0; i < 4; i++) {
                        pWriter.Write<float>(0);
                    }
                    break;
            }

            return pWriter.WriteByte();
        }

        // 9 Blocks of stats, Only handling Basic and Bonus attributes for now
        private static PacketWriter WriteStats(this PacketWriter pWriter, ItemStats stats) {
            List<ItemStat> basicAttributes = stats.BasicAttributes;
            pWriter.WriteShort((short)basicAttributes.Count);
            foreach (ItemStat stat in basicAttributes) {
                pWriter.Write<ItemStat>(stat);
            }
            pWriter.WriteShort().WriteInt(); // SpecialAttributes

            // Another basic attributes block
            pWriter.WriteShort().WriteShort().WriteInt();

            List<ItemStat> bonusAttributes = stats.BonusAttributes;
            pWriter.WriteShort((short)bonusAttributes.Count);
            foreach (ItemStat stat in bonusAttributes) {
                pWriter.Write<ItemStat>(stat);
            }
            pWriter.WriteShort().WriteInt(); // SpecialAttributes


            // Ignore other attributes
            pWriter.WriteShort().WriteShort().WriteInt();
            pWriter.WriteShort().WriteShort().WriteInt();
            pWriter.WriteShort().WriteShort().WriteInt();
            pWriter.WriteShort().WriteShort().WriteInt();
            pWriter.WriteShort().WriteShort().WriteInt();
            pWriter.WriteShort().WriteShort().WriteInt();

            return pWriter;
        }

        private static PacketWriter WriteStatDiff(this PacketWriter pWriter, ItemStats old, ItemStats @new) {
            // TODO: Find stat diffs (low priority)
            List<ItemStat> generalStatDiff = new List<ItemStat>();
            pWriter.WriteByte((byte)generalStatDiff.Count);
            foreach (ItemStat stat in generalStatDiff) {
                pWriter.Write<ItemStat>(stat);
            }

            pWriter.WriteInt(); // ???

            List<ItemStat> statDiff = new List<ItemStat>();
            pWriter.WriteInt(statDiff.Count);
            foreach (ItemStat stat in statDiff) {
                pWriter.Write<ItemStat>(stat);
            }

            List<SpecialItemStat> bonusStatDiff = new List<SpecialItemStat>();
            pWriter.WriteInt(bonusStatDiff.Count);
            foreach (SpecialItemStat stat in bonusStatDiff) {
                pWriter.Write<SpecialItemStat>(stat);
            }

            return pWriter;
        }

        // Writes UGC template data
        private static PacketWriter WriteTemplate(this PacketWriter pWriter) {
            return pWriter.WriteUgc()
                .WriteLong()
                .WriteInt()
                .WriteInt()
                .WriteInt()
                .WriteLong()
                .WriteInt()
                .WriteLong()
                .WriteLong()
                .WriteUnicodeString("");
        }

        private static PacketWriter WritePet(this PacketWriter pWriter) {
            return pWriter.WriteUnicodeString("") // Name
                .WriteLong() // Exp
                .WriteInt()
                .WriteInt(1) // Level
                .WriteByte();
        }

        private static PacketWriter WriteSockets(this PacketWriter pWriter, ItemStats stats) {
            pWriter.WriteByte(stats.TotalSockets);
            for (int i = 0; i < stats.TotalSockets; i++) {
                if (i >= stats.Gemstones.Count) {
                    pWriter.WriteBool(false); // Locked
                    continue;
                }

                pWriter.WriteBool(true); // Unlocked
                Gemstone gem = stats.Gemstones[i];
                pWriter.WriteInt(gem.Id);
                pWriter.WriteBool(gem.OwnerId != 0);
                if (gem.OwnerId != 0) {
                    pWriter.WriteLong(gem.OwnerId)
                        .WriteUnicodeString(gem.OwnerName);
                }

                pWriter.WriteBool(gem.Unknown != 0);
                if (gem.Unknown != 0) {
                    pWriter.WriteByte()
                        .WriteLong(gem.Unknown);
                }
            }

            return pWriter;
        }
    }
}