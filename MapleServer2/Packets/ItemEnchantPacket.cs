using System;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets {
    public static class ItemEnchantPacket {
        // Sent when putting item into enchant window
        public static Packet BeginEnchant(byte type, Item item) {
            var pWriter = PacketWriter.Of(SendOp.ITEM_ENCHANT)
                .WriteByte(0x05)
                .WriteShort(type)
                .WriteLong(item.Uid);

            // TODO: Make this dynamic
            Tuple<int, int>[] requiredItems = {
                new Tuple<int, int>(100,1000), // Crystal Fragment
                new Tuple<int, int>(101,2000), // Onyx
                new Tuple<int, int>(102,3000) // Chaos Onyx
            };
            pWriter.WriteByte((byte)requiredItems.Length);
            foreach (Tuple<int, int> requiredItem in requiredItems) {
                pWriter.WriteInt()
                    .WriteInt(requiredItem.Item1)
                    .WriteInt(requiredItem.Item2);
            }

            pWriter.WriteShort();

            // Enchant stat multipliers
            int count = 0;
            pWriter.WriteInt(count);
            for (int i = 0; i < count; i++) {
                pWriter.WriteShort()
                    .WriteInt()
                    .Write<float>(0f);
            }

            if (type == 1) {
                pWriter.Write<float>(90f) // SuccessRate
                    .Write<float>(0f)
                    .Write<float>(0f)
                    .Write<float>(0f)
                    .Write<float>(0f)
                    .WriteLong()
                    .WriteLong()
                    .WriteByte(1);
            }

            // Item copies required
            if (type == 1 || type == 2) {
                pWriter.WriteInt() // ItemId
                    .WriteShort() // Rarity
                    .WriteInt(); // Amount
            }

            return pWriter;
        }

        public static Packet UpdateCharges(Item item) {
            return PacketWriter.Of(SendOp.ITEM_ENCHANT)
                .WriteByte(0x06)
                .WriteLong(item.Uid)
                .WriteInt(item.EnchantExp);
        }

        public static Packet EnchantResult(Item item) {
            var pWriter = PacketWriter.Of(SendOp.ITEM_ENCHANT)
                .WriteByte(0x0A)
                .WriteLong(item.Uid)
                .WriteItem(item);

            // These are the stat bonus from enchanting
            int count = 0;
            pWriter.WriteInt(count);
            for (int i = 0; i < count; i++) {
                pWriter.WriteShort()
                    .WriteInt()
                    .Write<float>(0f);
            }

            return pWriter;
        }
    }
}