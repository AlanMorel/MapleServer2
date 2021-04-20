using System.Collections.Generic;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class BeautyPacket
    {
        private enum BeautyPacketMode : byte
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
            ChangeToSavedHair = 0x15,
        }

        public static Packet LoadBeautyShop(BeautyMetadata beautyShop)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BEAUTY);
            pWriter.WriteEnum(BeautyPacketMode.LoadBeautyShop);
            WriteBeautyShop(pWriter, beautyShop);
            pWriter.WriteByte(0);
            pWriter.WriteByte(0);
            pWriter.WriteShort(0);
            return pWriter;
        }

        public static Packet LoadBeautyShop(BeautyMetadata beautyShop, List<BeautyItem> items)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BEAUTY);
            pWriter.WriteEnum(BeautyPacketMode.LoadBeautyShop);
            WriteBeautyShop(pWriter, beautyShop);
            pWriter.WriteByte(30);
            pWriter.WriteByte(6);
            pWriter.WriteShort((short) items.Count);
            foreach (BeautyItem item in items)
            {
                pWriter.WriteInt(item.ItemId);
                pWriter.WriteEnum(item.Flag);
                pWriter.WriteShort(item.RequiredLevel);
                pWriter.WriteInt(item.RequiredAchievementId);
                pWriter.WriteByte(item.RequiredAchievementGrade);
                pWriter.WriteEnum(item.TokenType);
                pWriter.WriteInt(item.RequiredItemId);
                pWriter.WriteInt(item.TokenCost);
                pWriter.WriteMapleString("");
            }
            return pWriter;
        }

        public static Packet LoadDyeShop(BeautyMetadata beautyShop)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BEAUTY);
            pWriter.WriteEnum(BeautyPacketMode.LoadDyeShop);
            WriteBeautyShop(pWriter, beautyShop);
            return pWriter;
        }

        public static Packet LoadSaveShop(BeautyMetadata beautyShop)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BEAUTY);
            pWriter.WriteEnum(BeautyPacketMode.LoadSaveShop);
            WriteBeautyShop(pWriter, beautyShop);
            pWriter.WriteByte(30);
            pWriter.WriteByte(6);
            pWriter.WriteShort(0);
            return pWriter;
        }

        public static Packet UseVoucher(int voucherId, int quantity)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BEAUTY);
            pWriter.WriteEnum(BeautyPacketMode.UseVoucher);
            pWriter.WriteInt(voucherId);
            pWriter.WriteInt(quantity);
            return pWriter;
        }

        public static Packet RandomHairOption(Item previousHair, Item newHair)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BEAUTY);
            pWriter.WriteEnum(BeautyPacketMode.RandomHairOption);
            pWriter.WriteInt(previousHair.Id);
            pWriter.WriteInt(newHair.Id);
            return pWriter;
        }

        public static Packet ChooseRandomHair(int voucherId = 0)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BEAUTY);
            pWriter.WriteEnum(BeautyPacketMode.ChooseRandomHair);
            pWriter.WriteInt(voucherId);
            pWriter.WriteByte();
            return pWriter;
        }

        public static Packet InitializeSaves()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BEAUTY);
            pWriter.WriteEnum(BeautyPacketMode.InitializeSaves);
            return pWriter;
        }

        public static Packet LoadSavedHairCount(short hairCount)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BEAUTY);
            pWriter.WriteEnum(BeautyPacketMode.LoadSavedHairCount);
            pWriter.WriteShort(hairCount);
            return pWriter;
        }

        public static Packet LoadSavedHairs(List<Item> savedHairs)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BEAUTY);
            pWriter.WriteEnum(BeautyPacketMode.LoadSavedHairs);
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

        public static Packet SaveHair(Item playerHair, Item hairCopy)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BEAUTY);
            pWriter.WriteEnum(BeautyPacketMode.SaveHair);
            pWriter.WriteLong(playerHair.Uid);
            pWriter.WriteLong(hairCopy.Uid);
            pWriter.WriteByte();
            pWriter.WriteLong(hairCopy.CreationTime);
            return pWriter;
        }

        public static Packet DeleteSavedHair(long itemUid)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BEAUTY);
            pWriter.WriteEnum(BeautyPacketMode.DeleteSavedHair);
            pWriter.WriteLong(itemUid);
            return pWriter;
        }

        public static Packet LoadSaveWindow()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BEAUTY);
            pWriter.WriteEnum(BeautyPacketMode.LoadSaveWindow);
            pWriter.WriteByte(0);
            pWriter.WriteShort(0);
            return pWriter;
        }

        public static Packet ChangetoSavedHair()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.BEAUTY);
            pWriter.WriteEnum(BeautyPacketMode.ChangeToSavedHair);
            return pWriter;
        }

        public static void WriteBeautyShop(PacketWriter pWriter, BeautyMetadata beautyShop)
        {
            pWriter.WriteEnum(beautyShop.BeautyCategory);
            pWriter.WriteInt(beautyShop.ShopId);
            pWriter.WriteEnum(beautyShop.BeautyType);
            pWriter.WriteInt(beautyShop.VoucherId);
            pWriter.WriteByte(0);
            pWriter.WriteInt(0);
            pWriter.WriteInt(beautyShop.UniqueId);
            pWriter.WriteByte(0);
            pWriter.WriteByte(4);
            pWriter.WriteInt(0);
            pWriter.WriteInt(beautyShop.SpecialCost);
            pWriter.WriteMapleString("");
            pWriter.WriteEnum(beautyShop.TokenType);
            pWriter.WriteInt(beautyShop.RequiredItemId);
            pWriter.WriteInt(beautyShop.TokenCost);
            pWriter.WriteMapleString("");
        }
    }
}
