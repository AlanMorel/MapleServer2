using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class EnchantScrollPacket
{
    private enum EnchantScrollMode : byte
    {
        OpenWindow = 0x0,
        AddItem = 0x1,
        UseScroll = 0x3,
    }

    public static PacketWriter OpenWindow(long itemUid, EnchantScrollMetadata metadata, float successRate)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.EnchantScroll);
        pWriter.Write(EnchantScrollMode.OpenWindow);
        pWriter.WriteLong(itemUid);
        pWriter.Write(metadata.ScrollType);
        pWriter.WriteBool(false); // untradeable reminder
        pWriter.WriteInt(metadata.EnchantLevels.First());
        pWriter.WriteInt((int) (successRate * 10000));
        pWriter.WriteShort(metadata.MinLevel);
        pWriter.WriteShort(metadata.MaxLevel);
        pWriter.WriteInt(metadata.ItemTypes.Count);
        foreach (ItemType type in metadata.ItemTypes)
        {
            pWriter.Write(type);
        }
        pWriter.WriteInt(metadata.Rarities.Count);
        foreach (int rarity in metadata.Rarities)
        {
            pWriter.WriteInt(rarity);
        }

        if (metadata.ScrollType == EnchantScrollType.RandomEnchant)
        {
            pWriter.WriteInt(metadata.EnchantLevels.First());
            pWriter.WriteInt(metadata.EnchantLevels.Last());
        }
        return pWriter;
    }

    public static PacketWriter AddItem(long itemUid, Dictionary<StatAttribute, ItemStat> enchantStats)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.EnchantScroll);
        pWriter.Write(EnchantScrollMode.AddItem);
        pWriter.WriteLong(itemUid);
        pWriter.WriteShort(1);
        List<BasicStat> stats = enchantStats.Values.OfType<BasicStat>().ToList();
        pWriter.WriteInt(stats.Count);
        foreach (BasicStat stat in stats)
        {
            pWriter.Write(stat.ItemAttribute);
            pWriter.WriteFloat(stat.Rate);
            pWriter.WriteInt(stat.Flat);
        }
        return pWriter;
    }

    public static PacketWriter UseScroll(short errorId, Item item = null)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.EnchantScroll);
        pWriter.Write(EnchantScrollMode.UseScroll);
        pWriter.WriteShort(errorId);
        if (item is not null)
        {
            pWriter.WriteLong(item.Uid);
            pWriter.WriteItem(item);
        }
        return pWriter;
    }
}
