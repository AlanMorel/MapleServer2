using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class BeautyHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.BEAUTY;

        public BeautyHandler(ILogger<BeautyHandler> logger) : base(logger) { }

        private enum BeautyMode : byte
        {
            LoadShop = 0x0,
            NewBeauty = 0x3,
            ModifyExistingBeauty = 0x5,
            ModifySkin = 0x6,
            RandomHair = 0x7,
            Teleport = 0xA,
            ChooseRandomHair = 0xC,
            SaveHair = 0x10,
            DeleteSavedHair = 0x12,
            ChangeToSavedHair = 0x15,
            DyeItem = 0x16
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            BeautyMode mode = (BeautyMode) packet.ReadByte();

            switch (mode)
            {
                case BeautyMode.LoadShop:
                    HandleLoadShop(session, packet);
                    break;
                case BeautyMode.NewBeauty:
                    HandleNewBeauty(session, packet);
                    break;
                case BeautyMode.ModifyExistingBeauty:
                    HandleModifyExistingBeauty(session, packet);
                    break;
                case BeautyMode.ModifySkin:
                    HandleModifySkin(session, packet);
                    break;
                case BeautyMode.RandomHair:
                    HandleRandomHair(session, packet);
                    break;
                case BeautyMode.ChooseRandomHair:
                    HandleChooseRandomHair(session, packet);
                    break;
                case BeautyMode.SaveHair:
                    HandleSaveHair(session, packet);
                    break;
                case BeautyMode.Teleport:
                    HandleTeleport(session, packet);
                    break;
                case BeautyMode.DeleteSavedHair:
                    HandleDeleteSavedHair(session, packet);
                    break;
                case BeautyMode.ChangeToSavedHair:
                    HandleChangeToSavedHair(session, packet);
                    break;
                case BeautyMode.DyeItem:
                    HandleDyeItem(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleLoadShop(GameSession session, PacketReader packet)
        {
            int npcId = packet.ReadInt();
            BeautyCategory category = (BeautyCategory) packet.ReadByte();

            NpcMetadata beautyNpc = NpcMetadataStorage.GetNpc(npcId);
            if (beautyNpc == null)
            {
                return;
            }

            BeautyMetadata beautyShop = BeautyMetadataStorage.GetShopById(beautyNpc.ShopId);
            if (beautyShop == null)
            {
                return;
            }

            if (beautyShop.BeautyCategory == BeautyCategory.Dye)
            {
                if (beautyShop.BeautyType == BeautyShopType.Dye)
                {
                    session.Send(BeautyPacket.LoadDyeShop(beautyShop));
                    return;
                }
                session.Send(BeautyPacket.LoadBeautyShop(beautyShop));
                return;
            }

            if (beautyShop.BeautyCategory == BeautyCategory.Save)
            {
                session.Send(BeautyPacket.LoadSaveShop(beautyShop));
                session.Send(BeautyPacket.InitializeSaves());
                session.Send(BeautyPacket.LoadSaveWindow());
                session.Send(BeautyPacket.LoadSavedHairCount((short) session.Player.HairInventory.SavedHair.Count));
                if (session.Player.HairInventory.SavedHair.Count != 0)
                {
                    session.Player.HairInventory.SavedHair = session.Player.HairInventory.SavedHair.OrderBy(o => o.CreationTime).ToList();
                    session.Send(BeautyPacket.LoadSavedHairs(session.Player.HairInventory.SavedHair));
                }

                return;
            }

            List<BeautyItem> beautyItems = BeautyMetadataStorage.GetGenderItems(beautyShop.ShopId, session.Player.Gender);

            session.Send(BeautyPacket.LoadBeautyShop(beautyShop, beautyItems));
        }

        private static void HandleNewBeauty(GameSession session, PacketReader packet)
        {
            byte unk = packet.ReadByte();
            bool useVoucher = packet.ReadBool();
            int beautyItemId = packet.ReadInt();
            EquipColor equipColor = packet.Read<EquipColor>();

            Item beautyItem = new Item(beautyItemId)
            {
                Color = equipColor,
                IsTemplate = false,
                IsEquipped = true,
                Owner = session.Player
            };
            BeautyMetadata beautyShop = BeautyMetadataStorage.GetCosmeticShopByItemId(beautyItem.Id);

            if (useVoucher)
            {
                if (!PayWithVoucher(session, beautyShop))
                {
                    return;
                }
            }
            else
            {
                if (!PayWithShopItemTokenCost(session, beautyItemId))
                {
                    return;
                }
            }

            ModifyBeauty(session, packet, beautyItem);
        }

        private static void HandleModifyExistingBeauty(GameSession session, PacketReader packet)
        {
            byte unk = packet.ReadByte();
            bool useVoucher = packet.ReadBool();
            long beautyItemUid = packet.ReadLong();
            EquipColor equipColor = packet.Read<EquipColor>();

            Item beautyItem = session.Player.GetEquippedItem(beautyItemUid);

            if (beautyItem.ItemSlot == ItemSlot.CP)
            {
                HatData hatData = packet.Read<HatData>();
                beautyItem.HatData = hatData;
                session.FieldManager.BroadcastPacket(ItemExtraDataPacket.Update(session.FieldPlayer, beautyItem));
                return;
            }

            BeautyMetadata beautyShop = BeautyMetadataStorage.GetCosmeticShopByItemId(beautyItem.Id);

            if (!HandleShopPay(session, beautyShop, useVoucher))
            {
                return;
            }

            beautyItem.Color = equipColor;
            ModifyBeauty(session, packet, beautyItem);
        }

        private static void HandleModifySkin(GameSession session, PacketReader packet)
        {
            byte unk = packet.ReadByte();
            SkinColor skinColor = packet.Read<SkinColor>();
            bool useVoucher = packet.ReadBool();

            BeautyMetadata beautyShop = BeautyMetadataStorage.GetShopById(501);

            if (!HandleShopPay(session, beautyShop, useVoucher))
            {
                return;
            }

            session.Player.SkinColor = skinColor;
            session.FieldManager.BroadcastPacket(SkinColorPacket.Update(session.FieldPlayer, skinColor));
        }
        private static void HandleRandomHair(GameSession session, PacketReader packet)
        {
            int shopId = packet.ReadInt();
            bool useVoucher = packet.ReadBool();

            BeautyMetadata beautyShop = BeautyMetadataStorage.GetShopById(shopId);
            List<BeautyItem> beautyItems = BeautyMetadataStorage.GetGenderItems(beautyShop.ShopId, session.Player.Gender);

            if (!HandleShopPay(session, beautyShop, useVoucher))
            {
                return;
            }

            // Grab random hair
            Random random = new Random();
            int indexHair = random.Next(beautyItems.Count);
            BeautyItem chosenHair = beautyItems[indexHair];

            //Grab a preset hair and length of hair
            ItemMetadata beautyItemData = ItemMetadataStorage.GetMetadata(chosenHair.ItemId);
            int indexPreset = random.Next(beautyItemData.HairPresets.Count);
            HairPresets chosenPreset = beautyItemData.HairPresets[indexPreset];

            //Grab random front hair length
            double chosenFrontLength = random.NextDouble() *
                (beautyItemData.HairPresets[indexPreset].MaxScale - beautyItemData.HairPresets[indexPreset].MinScale) + beautyItemData.HairPresets[indexPreset].MinScale;

            //Grab random back hair length
            double chosenBackLength = random.NextDouble() *
                (beautyItemData.HairPresets[indexPreset].MaxScale - beautyItemData.HairPresets[indexPreset].MinScale) + beautyItemData.HairPresets[indexPreset].MinScale;

            // Grab random preset color
            ColorPaletteMetadata palette = ColorPaletteMetadataStorage.GetMetadata(2); // pick from palette 2. Seems like it's the correct palette for basic hair colors

            int indexColor = random.Next(palette.DefaultColors.Count);
            MixedColor color = palette.DefaultColors[indexColor];

            Item newHair = new Item(chosenHair.ItemId)
            {
                Color = EquipColor.Argb(color, indexColor, palette.PaletteId),
                HairData = new HairData((float) chosenBackLength, (float) chosenFrontLength, chosenPreset.BackPositionCoord, chosenPreset.BackPositionRotation, chosenPreset.FrontPositionCoord, chosenPreset.FrontPositionRotation),
                IsTemplate = false,
                IsEquipped = true,
                Owner = session.Player
            };
            Dictionary<ItemSlot, Item> cosmetics = session.Player.Inventory.Cosmetics;

            //Remove old hair
            if (cosmetics.Remove(ItemSlot.HR, out Item previousHair))
            {
                previousHair.Slot = -1;
                session.Player.HairInventory.RandomHair = previousHair; // store the previous hair
                DatabaseManager.Delete(previousHair);
                session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, previousHair));
            }

            cosmetics[ItemSlot.HR] = newHair;

            session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.FieldPlayer, newHair, ItemSlot.HR));
            session.Send(BeautyPacket.RandomHairOption(previousHair, newHair));
        }

        private static void HandleChooseRandomHair(GameSession session, PacketReader packet)
        {
            byte selection = packet.ReadByte();

            if (selection == 0) // player chose previous hair
            {
                Dictionary<ItemSlot, Item> cosmetics = session.Player.Inventory.Cosmetics;
                //Remove current hair
                if (cosmetics.Remove(ItemSlot.HR, out Item newHair))
                {
                    newHair.Slot = -1;
                    DatabaseManager.Delete(newHair);
                    session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, newHair));
                }

                cosmetics[ItemSlot.HR] = session.Player.HairInventory.RandomHair; // apply the previous hair

                session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.FieldPlayer, session.Player.HairInventory.RandomHair, ItemSlot.HR));

                Item voucher = new Item(20300246) { }; // Chic Salon Voucher
                InventoryController.Add(session, voucher, true);

                session.Send(BeautyPacket.ChooseRandomHair(voucher.Id));
            }
            else // player chose new hair
            {
                session.Send(BeautyPacket.ChooseRandomHair());
            }

            session.Player.HairInventory.RandomHair = null; // remove random hair option from hair inventory
        }

        private static void HandleSaveHair(GameSession session, PacketReader packet)
        {
            long hairUid = packet.ReadLong();

            Item hair = session.Player.Inventory.Cosmetics.FirstOrDefault(x => x.Value.Uid == hairUid).Value;
            if (hair == null || hair.ItemSlot != ItemSlot.HR)
            {
                return;
            }

            if (session.Player.HairInventory.SavedHair.Count > 30) // 30 is the max slots
            {
                return;
            }

            Item hairCopy = new Item(hair.Id)
            {
                HairData = hair.HairData,
                Color = hair.Color,
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + Environment.TickCount
            };

            session.Player.HairInventory.SavedHair.Add(hairCopy);

            session.Send(BeautyPacket.SaveHair(hair, hairCopy));
        }

        private static void HandleTeleport(GameSession session, PacketReader packet)
        {
            byte teleportId = packet.ReadByte();

            Map mapId;
            switch (teleportId)
            {
                case 1:
                    mapId = Map.RosettaBeautySalon;
                    break;
                case 3:
                    mapId = Map.TriaPlasticSurgery;
                    break;
                case 5:
                    mapId = Map.DouglasDyeWorkshop;
                    break;
                default:
                    Console.WriteLine($"teleportId: {teleportId} not found");
                    return;
            }

            MoveFieldHandler.HandleInstanceMove(session, (int) mapId);
        }

        private static void HandleDeleteSavedHair(GameSession session, PacketReader packet)
        {
            long hairUid = packet.ReadLong();

            Item hair = session.Player.HairInventory.SavedHair.FirstOrDefault(x => x.Uid == hairUid);
            if (hair == null)
            {
                return;
            }

            session.Send(BeautyPacket.DeleteSavedHair(hair.Uid));
            session.Player.HairInventory.SavedHair.Remove(hair);
        }

        private static void HandleChangeToSavedHair(GameSession session, PacketReader packet)
        {
            long hairUid = packet.ReadLong();

            Item hair = session.Player.HairInventory.SavedHair.FirstOrDefault(x => x.Uid == hairUid);
            if (hair == null)
            {
                return;
            }

            BeautyMetadata beautyShop = BeautyMetadataStorage.GetShopById(510);

            if (!PayWithShopTokenCost(session, beautyShop))
            {
                return;
            }

            Dictionary<ItemSlot, Item> cosmetics = session.Player.Inventory.Cosmetics;
            if (cosmetics.Remove(hair.ItemSlot, out Item removeItem))
            {
                removeItem.Slot = -1;
                session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, removeItem));
            }

            cosmetics[removeItem.ItemSlot] = hair;

            session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.FieldPlayer, hair, hair.ItemSlot));
            session.Send(BeautyPacket.ChangetoSavedHair());
        }

        private static void HandleDyeItem(GameSession session, PacketReader packet)
        {
            BeautyMetadata beautyShop = BeautyMetadataStorage.GetShopById(506);

            byte itemCount = packet.ReadByte();

            short[] quantity = new short[itemCount];
            bool[] useVoucher = new bool[itemCount];
            byte[] unk1 = new byte[itemCount];
            long[] unk2 = new long[itemCount];
            int[] unk3 = new int[itemCount];
            long[] itemUid = new long[itemCount];
            int[] itemId = new int[itemCount];
            EquipColor[] equipColor = new EquipColor[itemCount];
            HatData[] hatData = new HatData[itemCount];

            for (int i = 0; i < itemCount; i++)
            {
                quantity[i] = packet.ReadShort(); // should always be one
                useVoucher[i] = packet.ReadBool();
                unk1[i] = packet.ReadByte(); // just 0
                unk2[i] = packet.ReadLong(); // just 0
                unk3[i] = packet.ReadInt(); // also 0
                itemUid[i] = packet.ReadLong();
                itemId[i] = packet.ReadInt();
                equipColor[i] = packet.Read<EquipColor>();
                Item item = session.Player.GetEquippedItem(itemUid[i]);
                if (item == null)
                {
                    return;
                }

                if (item.ItemSlot == ItemSlot.CP)
                {
                    hatData[i] = packet.Read<HatData>();
                }

                if (!HandleShopPay(session, beautyShop, useVoucher[i]))
                {
                    return;
                }

                item.Color = equipColor[i];
                session.FieldManager.BroadcastPacket(ItemExtraDataPacket.Update(session.FieldPlayer, item));
            }
        }

        private static void ModifyBeauty(GameSession session, PacketReader packet, Item beautyItem)
        {
            ItemSlot itemSlot = ItemMetadataStorage.GetSlot(beautyItem.Id);
            Dictionary<ItemSlot, Item> cosmetics = session.Player.Inventory.Cosmetics;

            // remove current item
            if (cosmetics.Remove(itemSlot, out Item removeItem))
            {
                removeItem.Slot = -1;
                DatabaseManager.Delete(removeItem);
                session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.FieldPlayer, removeItem));
            }
            // equip new item

            switch (itemSlot)
            {
                case ItemSlot.HR:
                    float backLength = BitConverter.ToSingle(packet.Read(4), 0);
                    CoordF backPositionCoord = packet.Read<CoordF>();
                    CoordF backPositionRotation = packet.Read<CoordF>();
                    float frontLength = BitConverter.ToSingle(packet.Read(4), 0);
                    CoordF frontPositionCoord = packet.Read<CoordF>();
                    CoordF frontPositionRotation = packet.Read<CoordF>();

                    beautyItem.HairData = new HairData(backLength, frontLength, backPositionCoord, backPositionRotation, frontPositionCoord, frontPositionRotation);

                    cosmetics[itemSlot] = beautyItem;

                    session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.FieldPlayer, beautyItem, itemSlot));
                    break;
                case ItemSlot.FA:

                    cosmetics[itemSlot] = beautyItem;

                    session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.FieldPlayer, beautyItem, itemSlot));
                    break;
                case ItemSlot.FD:
                    byte[] faceDecorationPosition = packet.Read(16);

                    beautyItem.FaceDecorationData = faceDecorationPosition;

                    cosmetics[itemSlot] = beautyItem;

                    session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.FieldPlayer, beautyItem, itemSlot));
                    break;
            }
        }

        private static bool HandleShopPay(GameSession session, BeautyMetadata shop, bool useVoucher)
        {
            if (useVoucher)
            {
                if (!PayWithVoucher(session, shop))
                {
                    return false;
                }
            }
            else
            {
                if (!PayWithShopTokenCost(session, shop))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool PayWithVoucher(GameSession session, BeautyMetadata shop)
        {
            string voucherTag = ""; // using an Item's tag to search for any applicable voucher
            switch (shop.BeautyType)
            {
                case BeautyShopType.Hair:
                    if (shop.BeautyCategory == BeautyCategory.Special)
                    {
                        voucherTag = "beauty_hair_special";
                        break;
                    }
                    voucherTag = "beauty_hair";
                    break;
                case BeautyShopType.Face:
                    voucherTag = "beauty_face";
                    break;
                case BeautyShopType.Makeup:
                    voucherTag = "beauty_makeup";
                    break;
                case BeautyShopType.Skin:
                    voucherTag = "beauty_skin";
                    break;
                case BeautyShopType.Dye:
                    voucherTag = "beauty_itemcolor";
                    break;
                default:
                    session.Send(NoticePacket.Notice("Unknown Beauty Shop", NoticeType.FastText));
                    return false;
            }

            Item voucher = session.Player.Inventory.Items.FirstOrDefault(x => x.Value.Tag == voucherTag).Value;
            if (voucher == null)
            {
                session.Send(NoticePacket.Notice(SystemNotice.ItemNotFound, NoticeType.FastText));
                return false;
            }

            session.Send(BeautyPacket.UseVoucher(voucher.Id, 1));
            InventoryController.Consume(session, voucher.Uid, 1);
            return true;
        }

        private static bool PayWithShopTokenCost(GameSession session, BeautyMetadata beautyShop)
        {
            int cost = beautyShop.TokenCost;
            if (beautyShop.SpecialCost != 0)
            {
                cost = beautyShop.SpecialCost;
            }

            if (!Pay(session, beautyShop.TokenType, cost, beautyShop.RequiredItemId))
            {
                return false;
            }
            return true;
        }

        private static bool PayWithShopItemTokenCost(GameSession session, int beautyItemId)
        {
            BeautyMetadata beautyShop = BeautyMetadataStorage.GetCosmeticShopByItemId(beautyItemId);
            BeautyItem item = beautyShop.Items.FirstOrDefault(x => x.ItemId == beautyItemId);

            if (!Pay(session, item.TokenType, item.TokenCost, item.RequiredItemId))
            {
                return false;
            }
            return true;
        }

        private static bool Pay(GameSession session, ShopCurrencyType type, int tokenCost, int requiredItemId)
        {
            switch (type)
            {
                case ShopCurrencyType.Meso:
                    if (!session.Player.Wallet.Meso.Modify(-tokenCost))
                    {
                        return false;
                    }
                    break;
                case ShopCurrencyType.ValorToken:
                    if (!session.Player.Wallet.ValorToken.Modify(-tokenCost))
                    {
                        return false;
                    }
                    break;
                case ShopCurrencyType.Treva:
                    if (!session.Player.Wallet.Treva.Modify(-tokenCost))
                    {
                        return false;
                    }
                    break;
                case ShopCurrencyType.Rue:
                    if (!session.Player.Wallet.Rue.Modify(-tokenCost))
                    {
                        return false;
                    }
                    break;
                case ShopCurrencyType.HaviFruit:
                    if (!session.Player.Wallet.HaviFruit.Modify(-tokenCost))
                    {
                        return false;
                    }
                    break;
                case ShopCurrencyType.Meret:
                case ShopCurrencyType.GameMeret:
                case ShopCurrencyType.EventMeret:
                    if (!session.Player.Wallet.RemoveMerets(tokenCost))
                    {
                        return false;
                    }
                    break;
                case ShopCurrencyType.Item:
                    Item itemCost = session.Player.Inventory.Items.FirstOrDefault(x => x.Value.Id == requiredItemId).Value;
                    if (itemCost == null)
                    {
                        return false;
                    }
                    if (itemCost.Amount < tokenCost)
                    {
                        return false;
                    }
                    InventoryController.Consume(session, itemCost.Uid, tokenCost);
                    break;
                default:
                    session.SendNotice($"Unknown currency: {type}");
                    return false;
            }
            return true;
        }
    }
}
