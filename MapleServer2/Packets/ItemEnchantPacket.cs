using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Enums;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class ItemEnchantPacket
{
    private enum ItemEnchantPacketMode : byte
    {
        BeginEnchant = 0x05,
        UpdateExp = 0x06,
        UpdateCharges = 0x07,
        EnchantSuccess = 0x0A,
        EnchantFail = 0x0B,
        Notice = 0x0C,
    }

    public static PacketWriter BeginEnchant(EnchantType type, Item item, ItemEnchant enchantInfo, Dictionary<StatAttribute, ItemStat> stats)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemEnchant);
        pWriter.Write(ItemEnchantPacketMode.BeginEnchant);
        pWriter.Write(type);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteByte((byte) enchantInfo.Ingredients.Count);
        foreach (EnchantIngredient ingredient in enchantInfo.Ingredients)
        {
            pWriter.WriteInt();
            pWriter.Write(ingredient.Tag);
            pWriter.WriteInt(ingredient.Amount);
        }

        pWriter.WriteShort();
        pWriter.WriteInt(enchantInfo.Stats.Count);
        foreach (ItemStat stat in stats.Values)
        {
            pWriter.Write(stat.ItemAttribute);
            pWriter.WriteInt(stat.Flat);
            pWriter.WriteFloat(stat.Rate);
        }

        if (type is EnchantType.Ophelia)
        {
            WriteEnchantRates(pWriter, enchantInfo.Rates);
            pWriter.WriteLong();
            pWriter.WriteLong();
            pWriter.WriteByte(1);
        }

        // Item copies required
        if (type is EnchantType.Ophelia or EnchantType.Peachy)
        {
            pWriter.WriteInt(enchantInfo.CatalystAmountRequired > 0 ? item.Id : 0);
            pWriter.WriteShort((short) (enchantInfo.CatalystAmountRequired > 0 ? item.Rarity : 0));
            pWriter.WriteInt(enchantInfo.CatalystAmountRequired > 0 ? item.Amount : 0);
        }
        return pWriter;
    }

    public static PacketWriter UpdateExp(Item item)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemEnchant);
        pWriter.Write(ItemEnchantPacketMode.UpdateExp);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteInt(item.EnchantExp);
        return pWriter;
    }

    public static PacketWriter UpdateCharges(ItemEnchant itemEnchant)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemEnchant);
        pWriter.Write(ItemEnchantPacketMode.UpdateCharges);
        pWriter.WriteInt(itemEnchant.Rates.ChargesAdded);
        pWriter.WriteInt(itemEnchant.CatalystItemUids.Count);
        pWriter.WriteInt(itemEnchant.CatalystItemUids.Count);
        foreach (long uid in itemEnchant.CatalystItemUids)
        {
            pWriter.WriteLong(uid);
        }
        WriteEnchantRates(pWriter, itemEnchant.Rates);
        return pWriter;
    }

    public static PacketWriter EnchantSuccess(Item item, List<ItemStat> enchants)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemEnchant);
        pWriter.Write(ItemEnchantPacketMode.EnchantSuccess);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteItem(item);

        pWriter.WriteInt(enchants.Count);
        foreach (ItemStat enchant in enchants)
        {
            pWriter.Write(enchant.ItemAttribute);
            pWriter.WriteInt(enchant.Flat);
            pWriter.WriteFloat(enchant.Rate);
        }
        return pWriter;
    }

    public static PacketWriter EnchantFail(Item item, ItemEnchant enchantInfo)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemEnchant);
        pWriter.Write(ItemEnchantPacketMode.EnchantFail);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteItem(item);
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteLong();
        pWriter.WriteInt(enchantInfo.PityCharges);
        return pWriter;
    }

    public static PacketWriter Notice(short noticeId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.ItemEnchant);
        pWriter.Write(ItemEnchantPacketMode.Notice);
        pWriter.WriteShort(noticeId);
        return pWriter;
    }

    private static void WriteEnchantRates(PacketWriter pWriter, EnchantRates rate)
    {
        pWriter.WriteFloat(rate.BaseSuccessRate);
        pWriter.WriteFloat();
        pWriter.WriteFloat();
        pWriter.WriteFloat(rate.CatalystTotalRate());
        pWriter.WriteFloat(rate.ChargeTotalRate());
    }
}
