using System.Collections.Generic;
using System.Linq;
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
            pWriter.WriteInt(item.GachaDismantleId);

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


            if (item.IsCustomScore)
            {
                pWriter.WriteMusicScore(item);
            }

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
                switch (item.GemSlot)
                {
                    case GemSlot.PET:
                        pWriter.WriteInt(item.PetSkinBadgeId);
                        break;
                    case GemSlot.TRANS:
                        pWriter.Write(item.TransparencyBadgeBools);
                        break;
                }
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
            pWriter.Write(item.Color);
            // Positioning Data
            switch (item.ItemSlot)
            {
                case ItemSlot.CP:
                    pWriter.Write(item.HatData);
                    break;
                case ItemSlot.HR:
                    pWriter.Write(item.HairData.BackLength);
                    pWriter.Write(item.HairData.BackPositionCoord);
                    pWriter.Write(item.HairData.BackPositionRotation);
                    pWriter.Write(item.HairData.FrontLength);
                    pWriter.Write(item.HairData.FrontPositionCoord);
                    pWriter.Write(item.HairData.FrontPositionRotation);
                    break;
                case ItemSlot.FD:
                    pWriter.Write(item.FaceDecorationData);
                    break;
            }

            return pWriter;
        }

        // 9 Blocks of stats, still missing some stats
        private static PacketWriter WriteStats(this PacketWriter pWriter, ItemStats stats)
        {
            pWriter.WriteByte(); // Not part of appearance sub!
            List<ItemStat> basicStats = stats.BasicStats.Where(x => x.GetType() == typeof(NormalStat)).ToList();
            pWriter.WriteShort((short) basicStats.Count);
            foreach (NormalStat stat in basicStats)
            {
                pWriter.Write(stat);
            }
            List<ItemStat> specialBasicStats = stats.BasicStats.Where(x => x.GetType() == typeof(SpecialStat)).ToList();
            pWriter.WriteShort((short) specialBasicStats.Count);
            foreach (SpecialStat stat in specialBasicStats)
            {
                pWriter.Write(stat);
            }
            pWriter.WriteInt();

            // Another basic stats block
            pWriter.WriteShort();
            pWriter.WriteShort();
            pWriter.WriteInt();

            List<ItemStat> bonusStats = stats.BonusStats.Where(x => x.GetType() == typeof(NormalStat)).ToList();
            pWriter.WriteShort((short) bonusStats.Count);
            foreach (NormalStat stat in bonusStats)
            {
                pWriter.Write(stat);
            }
            List<ItemStat> specialBonusStats = stats.BonusStats.Where(x => x.GetType() == typeof(SpecialStat)).ToList();
            pWriter.WriteShort((short) specialBonusStats.Count);
            foreach (SpecialStat stat in specialBonusStats)
            {
                pWriter.Write(stat);
            }
            pWriter.WriteInt();

            // Ignore other stats
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
            List<NormalStat> generalStatDiff = new List<NormalStat>();
            pWriter.WriteByte((byte) generalStatDiff.Count);
            foreach (NormalStat stat in generalStatDiff)
            {
                pWriter.Write(stat);
            }

            pWriter.WriteInt(); // ???

            List<NormalStat> statDiff = new List<NormalStat>();
            pWriter.WriteInt(statDiff.Count);
            foreach (NormalStat stat in statDiff)
            {
                pWriter.Write(stat);
            }

            List<SpecialStat> bonusStatDiff = new List<SpecialStat>();
            pWriter.WriteInt(bonusStatDiff.Count);
            foreach (SpecialStat stat in bonusStatDiff)
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

        private static PacketWriter WriteMusicScore(this PacketWriter pWriter, Item item)
        {
            pWriter.WriteInt(item.Score.Length);
            pWriter.WriteInt(item.Score.Type);
            pWriter.WriteUnicodeString(item.Score.Title);
            pWriter.WriteUnicodeString(item.Score.Composer);
            pWriter.WriteInt(4); // seems like it's always 4. 1 in KMS2. 
            pWriter.WriteLong(item.Score.ComposerCharacterId);
            pWriter.WriteBool(item.Score.Locked);
            pWriter.WriteLong();
            pWriter.WriteLong();
            return pWriter;
        }
    }
}
