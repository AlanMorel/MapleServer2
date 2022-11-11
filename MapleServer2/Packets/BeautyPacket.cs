using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database.Types;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class BeautyPacket
{
    private enum Mode : byte
    {
        LoadBeautyShop = 0x0,
        LoadDyeShop = 0x1,
        LoadSaveShop = 0x2,
        UseVoucher = 0x9,
        RandomHairOption = 0xB,
        ChooseRandomHair = 0xC,
        InitializeSaves = 0xD,
        LoadSavedHairCount = 0xE,
        LoadSavedHairs = 0xF,
        SaveHair = 0x10,
        DeleteSavedHair = 0x12,
        LoadSaveWindow = 0x14,
        ChangeToSavedHair = 0x15
    }

    public static PacketWriter LoadBeautyShop(BeautyShop beautyShop)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Beauty);
        pWriter.Write(Mode.LoadBeautyShop);
        WriteBeautyShop(pWriter, beautyShop);
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteShort();
        return pWriter;
    }

    public static PacketWriter LoadBeautyShop(BeautyShop beautyShop, List<BeautyShopItem> items)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Beauty);
        pWriter.Write(Mode.LoadBeautyShop);
        WriteBeautyShop(pWriter, beautyShop);
        pWriter.WriteByte(30);
        pWriter.WriteByte(6);
        pWriter.WriteShort((short) items.Count);
        foreach (BeautyShopItem item in items)
        {
            pWriter.WriteInt(item.ItemId);
            pWriter.Write(item.Label);
            pWriter.WriteShort(item.RequiredLevel);
            pWriter.WriteInt(item.RequiredAchievementId);
            pWriter.WriteByte(item.RequiredAchievementGrade);
            pWriter.Write(item.CurrencyType);
            pWriter.WriteInt(item.RequiredItemId);
            pWriter.WriteInt(item.CurrencyCost);
            pWriter.WriteString();
        }
        return pWriter;
    }

    public static PacketWriter LoadDyeShop(BeautyShop beautyShop)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Beauty);
        pWriter.Write(Mode.LoadDyeShop);
        WriteBeautyShop(pWriter, beautyShop);
        return pWriter;
    }

    public static PacketWriter LoadSaveShop(BeautyShop beautyShop)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Beauty);
        pWriter.Write(Mode.LoadSaveShop);
        WriteBeautyShop(pWriter, beautyShop);
        pWriter.WriteByte(30);
        pWriter.WriteByte(6);
        pWriter.WriteShort();
        return pWriter;
    }

    public static PacketWriter UseVoucher(int voucherId, int quantity)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Beauty);
        pWriter.Write(Mode.UseVoucher);
        pWriter.WriteInt(voucherId);
        pWriter.WriteInt(quantity);
        return pWriter;
    }

    public static PacketWriter RandomHairOption(Item previousHair, Item newHair)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Beauty);
        pWriter.Write(Mode.RandomHairOption);
        pWriter.WriteInt(previousHair.Id);
        pWriter.WriteInt(newHair.Id);
        return pWriter;
    }

    public static PacketWriter ChooseRandomHair(int voucherId = 0)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Beauty);
        pWriter.Write(Mode.ChooseRandomHair);
        pWriter.WriteInt(voucherId);
        pWriter.WriteByte();
        return pWriter;
    }

    public static PacketWriter InitializeSaves()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Beauty);
        pWriter.Write(Mode.InitializeSaves);
        return pWriter;
    }

    public static PacketWriter LoadSavedHairCount(short hairCount)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Beauty);
        pWriter.Write(Mode.LoadSavedHairCount);
        pWriter.WriteShort(hairCount);
        return pWriter;
    }

    public static PacketWriter LoadSavedHairs(List<Item> savedHairs)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Beauty);
        pWriter.Write(Mode.LoadSavedHairs);
        pWriter.WriteShort((short) savedHairs.Count);
        foreach (Item hair in savedHairs)
        {
            pWriter.WriteInt(hair.Id);
            pWriter.WriteLong(hair.Uid);
            pWriter.WriteInt(savedHairs.IndexOf(hair));
            pWriter.WriteLong(hair.CreationTime);
            pWriter.Write(hair.Color);
            pWriter.WriteInt();
            pWriter.Write(hair.HairData.BackLength);
            pWriter.Write(hair.HairData.BackPositionCoord);
            pWriter.Write(hair.HairData.BackPositionRotation);
            pWriter.Write(hair.HairData.FrontLength);
            pWriter.Write(hair.HairData.FrontPositionCoord);
            pWriter.Write(hair.HairData.FrontPositionRotation);
        }
        return pWriter;
    }

    public static PacketWriter SaveHair(Item playerHair, Item hairCopy)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Beauty);
        pWriter.Write(Mode.SaveHair);
        pWriter.WriteLong(playerHair.Uid);
        pWriter.WriteLong(hairCopy.Uid);
        pWriter.WriteByte();
        pWriter.WriteLong(hairCopy.CreationTime);
        return pWriter;
    }

    public static PacketWriter DeleteSavedHair(long itemUid)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Beauty);
        pWriter.Write(Mode.DeleteSavedHair);
        pWriter.WriteLong(itemUid);
        return pWriter;
    }

    public static PacketWriter LoadSaveWindow()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Beauty);
        pWriter.Write(Mode.LoadSaveWindow);
        pWriter.WriteByte();
        pWriter.WriteShort();
        return pWriter;
    }

    public static PacketWriter ChangetoSavedHair()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Beauty);
        pWriter.Write(Mode.ChangeToSavedHair);
        return pWriter;
    }

    public static void WriteBeautyShop(PacketWriter pWriter, BeautyShop beautyShop)
    {
        pWriter.Write(beautyShop.BeautyCategory);
        pWriter.WriteInt(beautyShop.Id);
        pWriter.Write(beautyShop.BeautyType);
        pWriter.WriteInt(beautyShop.VoucherId);
        pWriter.WriteByte();
        pWriter.WriteInt();
        pWriter.WriteInt(beautyShop.UniqueId);
        pWriter.WriteByte();
        pWriter.Write(beautyShop.CurrencyType);
        pWriter.WriteInt();
        pWriter.WriteInt(beautyShop.SpecialCost);
        pWriter.WriteString();
        pWriter.Write(beautyShop.CurrencyType);
        pWriter.WriteInt(beautyShop.RequiredItemId);
        pWriter.WriteInt(beautyShop.CurrencyCost);
        pWriter.WriteString();
    }
}
