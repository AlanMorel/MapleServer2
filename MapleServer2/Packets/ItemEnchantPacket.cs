using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ItemEnchantPacket
{
    // Sent when putting item into enchant window
    public static PacketWriter BeginEnchant(byte type, Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemEnchant);
        pWriter.WriteByte(0x05);
        pWriter.WriteShort(type);
        pWriter.WriteLong(item.Uid);

        // TODO: Make this dynamic
        Tuple<int, int>[] requiredItems =
        {
            new(100, 1000), // Crystal Fragment
            new(101, 2000), // Onyx
            new(102, 3000) // Chaos Onyx
        };
        pWriter.WriteByte((byte) requiredItems.Length);
        foreach ((int item1, int item2) in requiredItems)
        {
            pWriter.WriteInt();
            pWriter.WriteInt(item1);
            pWriter.WriteInt(item2);
        }

        pWriter.WriteShort();

        // Enchant stat multipliers
        int count = 0;
        pWriter.WriteInt(count);
        for (int i = 0; i < count; i++)
        {
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.Write(0f);
        }

        if (type == 1)
        {
            pWriter.Write(90f); // SuccessRate
            pWriter.Write(0f);
            pWriter.Write(0f);
            pWriter.Write(0f);
            pWriter.Write(0f);
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteByte(1);
        }

        // Item copies required
        if (type is 1 or 2)
        {
            pWriter.WriteInt(); // ItemId
            pWriter.WriteShort(); // Rarity
            pWriter.WriteInt(); // Amount
        }

        return pWriter;
    }

    public static PacketWriter UpdateCharges(Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemEnchant);
        pWriter.WriteByte(0x06);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteInt(item.EnchantExp);

        return pWriter;
    }

    public static PacketWriter EnchantResult(Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemEnchant);
        pWriter.WriteByte(0x0A);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteItem(item);

        // These are the stat bonus from enchanting
        int count = 0;
        pWriter.WriteInt(count);
        for (int i = 0; i < count; i++)
        {
            pWriter.WriteShort();
            pWriter.WriteInt();
            pWriter.Write(0f);
        }

        return pWriter;
    }
}
