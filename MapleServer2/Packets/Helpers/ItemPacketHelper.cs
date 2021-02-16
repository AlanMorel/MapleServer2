using System.Collections.Generic;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Types;

namespace MapleServer2.Packets.Helpers
{
    public static class ItemPacketHelper
    {
        public static PacketWriter WriteItem(this PacketWriter pWriter, Item item)
        {
            pWriter.WriteInt(item.Amount);
            pWriter.WriteInt();
            pWriter.WriteInt(-1);
            pWriter.WriteLong(item.CreationTime);
            pWriter.WriteLong(item.ExpiryTime);
            pWriter.WriteLong();
            pWriter.WriteInt(item.TimesAttributesChanged);
            pWriter.WriteInt(item.PlayCount);
            pWriter.WriteBool(item.IsLocked);
            pWriter.WriteLong(item.UnlockTime);
            pWriter.WriteShort(item.RemainingGlamorForges);
            pWriter.WriteByte();
            pWriter.WriteInt();

            // Write Appearance 
            pWriter.WriteAppearance(item);

            // Write Stats 0x0582B10
            pWriter.WriteStats(item.Stats);
            pWriter.WriteInt(item.Enchants);
            pWriter.WriteInt(item.EnchantExp);
            pWriter.WriteBool(true); // Enchant based peachy charges, otherwise always require 10 charges
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteBool(item.CanRepackage);
            pWriter.WriteInt(item.Charges);
            pWriter.WriteStatDiff(/*item.Stats, item.Stats*/);

            if (item.IsTemplate)
            {
                // Not implemented, causes issues for non-default character creation outfits
                pWriter.WriteTemplate();
            }

            if (item.InventoryTab == InventoryTab.Pets)
            {
                pWriter.WritePet();
            }

            if (item.GemSlot != 0)
            {
                // Now deviate from WriteItem
                pWriter.WriteBool(true);
                pWriter.WriteByte((byte) item.GemSlot);
                pWriter.WriteUnicodeString(item.Id.ToString());
            }

            // Item Transfer Data 0x058AD00
            pWriter.WriteInt((int) item.TransferFlag);
            pWriter.WriteByte();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteByte();
            pWriter.WriteByte(); // 2nd flag, use to skip charbound

            // CharBound means untradable, unsellable, bound to char (ignores TransferFlag, but not 2nd flag!!)
            bool isCharBound = item.Owner != null;
            pWriter.WriteBool(isCharBound);
            if (isCharBound)
            {
                pWriter.WriteLong(item.Owner.CharacterId);
                pWriter.WriteUnicodeString(item.Owner.Name);
            }

            pWriter.WriteSockets(item.Stats);

            pWriter.WriteLong(item.PairedCharacterId);
            if (item.PairedCharacterId != 0)
            {
                pWriter.WriteUnicodeString(item.PairedCharacterName);
                pWriter.WriteBool(false);
            }

            // Unknwon | BoundCharacter?
            pWriter.WriteLong();
            pWriter.WriteUnicodeString("");

            return pWriter;
        }

        private static PacketWriter WriteAppearance(this PacketWriter pWriter, Item item)
        {
            pWriter.Write<EquipColor>(item.Color);
            pWriter.WriteInt(item.AppearanceFlag);
            // Positioning Data
            switch (item.ItemSlot)
            {
                case ItemSlot.CP:
                    for (int i = 0; i < 13; i++)
                    {
                        pWriter.Write(0);
                    }
                    break;
                case ItemSlot.HR:
                    //pWriter.Write<HairData>(item.HairD);
                    pWriter.Write(item.HairD.BackLength);
                    pWriter.Write(item.HairD.BackPositionArray);
                    pWriter.Write(item.HairD.FrontLength);
                    pWriter.Write(item.HairD.FrontPositionArray);
                    break;
                case ItemSlot.FD:
                    pWriter.Write(item.FaceDecorationD);
                    break;
            }

            return pWriter;
        }

        // 9 Blocks of stats, Only handling Basic and Bonus attributes for now
        private static PacketWriter WriteStats(this PacketWriter pWriter, ItemStats stats)
        {
            pWriter.WriteByte(); // Not part of appearance sub!
            List<ItemStat> basicAttributes = stats.BasicAttributes;
            pWriter.WriteShort((short) basicAttributes.Count);
            foreach (ItemStat stat in basicAttributes)
            {
                pWriter.Write(stat);
            }
            pWriter.WriteShort(); // SpecialAttributes
            pWriter.WriteInt();

            // Another basic attributes block
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteInt();

            List<ItemStat> bonusAttributes = stats.BonusAttributes;
            pWriter.WriteShort((short) bonusAttributes.Count);
            foreach (ItemStat stat in bonusAttributes)
            {
                pWriter.Write(stat);
            }
            pWriter.WriteShort();
            pWriter.WriteInt(); // SpecialAttributes


            // Ignore other attributes
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteInt();

            return pWriter;
        }

        private static PacketWriter WriteStatDiff(this PacketWriter pWriter/*, ItemStats old, ItemStats new*/)
        {
            // TODO: Find stat diffs (low priority)
            List<ItemStat> generalStatDiff = new List<ItemStat>();
            pWriter.WriteByte((byte) generalStatDiff.Count);
            foreach (ItemStat stat in generalStatDiff)
            {
                pWriter.Write(stat);
            }

            pWriter.WriteInt(); // ???

            List<ItemStat> statDiff = new List<ItemStat>();
            pWriter.WriteInt(statDiff.Count);
            foreach (ItemStat stat in statDiff)
            {
                pWriter.Write(stat);
            }

            List<SpecialItemStat> bonusStatDiff = new List<SpecialItemStat>();
            pWriter.WriteInt(bonusStatDiff.Count);
            foreach (SpecialItemStat stat in bonusStatDiff)
            {
                pWriter.Write(stat);
            }

            return pWriter;
        }

        // Writes UGC template data
        private static PacketWriter WriteTemplate(this PacketWriter pWriter)
        {
            pWriter.WriteUgc();
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteInt();
            pWriter.WriteLong();
            pWriter.WriteInt();
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteUnicodeString("");

            return pWriter;
        }

        private static PacketWriter WritePet(this PacketWriter pWriter)
        {
            pWriter.WriteUnicodeString(""); // Name
            pWriter.WriteLong(); // Exp
            pWriter.WriteInt();
            pWriter.WriteInt(1);// Level
            pWriter.WriteByte();

            return pWriter;
        }

        private static PacketWriter WriteSockets(this PacketWriter pWriter, ItemStats stats)
        {
            pWriter.WriteByte();
            pWriter.WriteByte(stats.TotalSockets);
            for (int i = 0; i < stats.TotalSockets; i++)
            {
                if (i >= stats.Gemstones.Count)
                {
                    pWriter.WriteBool(false); // Locked
                    continue;
                }

                pWriter.WriteBool(true); // Unlocked
                Gemstone gem = stats.Gemstones[i];
                pWriter.WriteInt(gem.Id);
                pWriter.WriteBool(gem.OwnerId != 0);
                if (gem.OwnerId != 0)
                {
                    pWriter.WriteLong(gem.OwnerId);
                    pWriter.WriteUnicodeString(gem.OwnerName);
                }

                pWriter.WriteBool(gem.Unknown != 0);
                if (gem.Unknown != 0)
                {
                    pWriter.WriteByte();
                    pWriter.WriteLong(gem.Unknown);
                }
            }

            return pWriter;
        }
    }
}
