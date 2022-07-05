using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class BeautyHandler : GamePacketHandler<BeautyHandler>
{
    public override RecvOp OpCode => RecvOp.Beauty;

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
        DyeItem = 0x16,
        BeautyVoucher = 0x17
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
            case BeautyMode.BeautyVoucher:
                HandleBeautyVoucher(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleLoadShop(GameSession session, PacketReader packet)
    {
        int npcId = packet.ReadInt();
        BeautyCategory category = (BeautyCategory) packet.ReadByte();

        NpcMetadata beautyNpc = NpcMetadataStorage.GetNpcMetadata(npcId);
        if (beautyNpc == null)
        {
            return;
        }

        BeautyShop beautyShop = DatabaseManager.BeautyShops.FindById(beautyNpc.ShopId);
        if (beautyShop == null)
        {
            return;
        }

        session.Player.ShopId = beautyNpc.ShopId;

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

        List<BeautyShopItem> beautyItems = DatabaseManager.BeautyShopItems.FindAllByShopIdAndGender(beautyShop.Id, session.Player.Gender);

        session.Send(BeautyPacket.LoadBeautyShop(beautyShop, beautyItems));
    }

    private static void HandleNewBeauty(GameSession session, PacketReader packet)
    {
        byte unk = packet.ReadByte();
        bool useVoucher = packet.ReadBool();
        int beautyItemId = packet.ReadInt();
        EquipColor equipColor = packet.Read<EquipColor>();

        Item beautyItem = new(beautyItemId)
        {
            Color = equipColor,
            IsEquipped = true,
            OwnerCharacterId = session.Player.CharacterId,
            OwnerCharacterName = session.Player.Name
        };
        BeautyShop beautyShop = DatabaseManager.BeautyShops.FindById(session.Player.ShopId);
        if (beautyShop is null)
        {
            return;
        }

        if (useVoucher)
        {
            if (!PayWithVoucher(session, beautyShop))
            {
                return;
            }
        }
        else
        {
            if (!PayWithShopItemTokenCost(session, beautyItemId, beautyShop))
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

        Item beautyItem = session.Player.Inventory.GetEquippedItem(beautyItemUid);

        if (beautyItem.ItemSlot is ItemSlot.CP) // This should only work with the mirror shop
        {
            HatData hatData = packet.Read<HatData>();
            beautyItem.HatData = hatData;
            session.FieldManager.BroadcastPacket(ItemExtraDataPacket.Update(session.Player.FieldPlayer, beautyItem));
            return;
        }

        BeautyShop beautyShop = DatabaseManager.BeautyShops.FindById(session.Player.ShopId);
        if (beautyShop is null)
        {
            return;
        }

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

        BeautyShop beautyShop = DatabaseManager.BeautyShops.FindById(session.Player.ShopId);

        if (!HandleShopPay(session, beautyShop, useVoucher))
        {
            return;
        }

        session.Player.SkinColor = skinColor;
        session.FieldManager.BroadcastPacket(SkinColorPacket.Update(session.Player.FieldPlayer, skinColor));
    }

    private static void HandleRandomHair(GameSession session, PacketReader packet)
    {
        int shopId = packet.ReadInt();
        bool useVoucher = packet.ReadBool();

        BeautyShop beautyShop = DatabaseManager.BeautyShops.FindById(shopId);
        List<BeautyShopItem> beautyItems = DatabaseManager.BeautyShopItems.FindAllByShopIdAndGender(beautyShop.Id, session.Player.Gender);

        if (!HandleShopPay(session, beautyShop, useVoucher))
        {
            return;
        }

        // Grab random hair
        Random random = Random.Shared;
        int indexHair = random.Next(beautyItems.Count);
        BeautyShopItem chosenHair = beautyItems[indexHair];

        BeautyHelper.ChangeHair(session, chosenHair.ItemId, out Item previousHair, out Item newHair);

        session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.Player.FieldPlayer, newHair, ItemSlot.HR));
        session.Send(BeautyPacket.RandomHairOption(previousHair, newHair));
    }

    private static void HandleChooseRandomHair(GameSession session, PacketReader packet)
    {
        byte selection = packet.ReadByte();

        if (selection == 0) // player chose previous hair
        {
            Player player = session.Player;
            Dictionary<ItemSlot, Item> cosmetics = player.Inventory.Cosmetics;
            //Remove current hair
            if (cosmetics.Remove(ItemSlot.HR, out Item newHair))
            {
                newHair.Slot = -1;
                DatabaseManager.Items.Delete(newHair.Uid);
                session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.Player.FieldPlayer, newHair));
            }

            cosmetics[ItemSlot.HR] = player.HairInventory.RandomHair; // apply the previous hair

            session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.Player.FieldPlayer, player.HairInventory.RandomHair, ItemSlot.HR));

            int voucherId = DatabaseManager.BeautyShops.GetSpecialVoucher();
            Item voucher = new(voucherId);
            player.Inventory.AddItem(session, voucher, true);

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

        Item hairCopy = new(hair.Id)
        {
            HairData = hair.HairData,
            Color = hair.Color,
            CreationTime = TimeInfo.Now() + Environment.TickCount
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
                Logger.Warning("teleportId: {teleportId} not found", teleportId);
                return;
        }

        if ((int) mapId == session.Player.MapId)
        {
            session.Send(NoticePacket.Notice(SystemNotice.BeautyGotoMapInvalidSamefield, NoticeType.Chat | NoticeType.FastText));
            return;
        }

        session.Player.Warp(mapId, instanceId: session.Player.CharacterId);
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

        BeautyShop beautyShop = DatabaseManager.BeautyShops.FindById(session.Player.ShopId);

        if (!PayWithShopTokenCost(session, beautyShop))
        {
            return;
        }

        Dictionary<ItemSlot, Item> cosmetics = session.Player.Inventory.Cosmetics;
        if (cosmetics.Remove(hair.ItemSlot, out Item removeItem))
        {
            removeItem.Slot = -1;
            session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.Player.FieldPlayer, removeItem));
        }

        cosmetics[removeItem.ItemSlot] = hair;

        session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.Player.FieldPlayer, hair, hair.ItemSlot));
        session.Send(BeautyPacket.ChangetoSavedHair());
    }

    private static void HandleDyeItem(GameSession session, PacketReader packet)
    {
        BeautyShop beautyShop = DatabaseManager.BeautyShops.FindById(session.Player.ShopId);

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
            Item item = session.Player.Inventory.GetEquippedItem(itemUid[i]);
            if (item == null)
            {
                return;
            }

            if (!HandleShopPay(session, beautyShop, useVoucher[i]))
            {
                return;
            }

            if (item.ItemSlot == ItemSlot.CP)
            {
                hatData[i] = packet.Read<HatData>();
                item.HatData = hatData[i];
            }

            item.Color = equipColor[i];
            session.FieldManager.BroadcastPacket(ItemExtraDataPacket.Update(session.Player.FieldPlayer, item));
        }
    }

    private static void HandleBeautyVoucher(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();

        Player player = session.Player;
        Item voucher = player.Inventory.GetByUid(itemUid);
        if (voucher == null || voucher.Function.Name != "ItemChangeBeauty")
        {
            return;
        }

        BeautyShop beautyShop = DatabaseManager.BeautyShops.FindById(voucher.Function.Id);
        if (beautyShop == null)
        {
            return;
        }

        List<BeautyShopItem> beautyItems = DatabaseManager.BeautyShopItems.FindAllByShopIdAndGender(beautyShop.Id, player.Gender);

        player.ShopId = beautyShop.Id;
        session.Send(BeautyPacket.LoadBeautyShop(beautyShop, beautyItems));
        player.Inventory.ConsumeItem(session, voucher.Uid, 1);
    }

    private static void ModifyBeauty(GameSession session, PacketReader packet, Item beautyItem)
    {
        List<ItemSlot> itemSlots = ItemMetadataStorage.GetItemSlots(beautyItem.Id);
        if (itemSlots.Count > 1)
        {
            // beauty items shouldn't have more than one slot
            return;
        }
        ItemSlot itemSlot = itemSlots.First();

        Dictionary<ItemSlot, Item> cosmetics = session.Player.Inventory.Cosmetics;

        if (cosmetics.TryGetValue(itemSlot, out Item removeItem))
        {
            // Only remove if it isn't the same item
            if (removeItem.Uid != beautyItem.Uid)
            {
                cosmetics.Remove(itemSlot);
                removeItem.Slot = -1;
                DatabaseManager.Items.Delete(removeItem.Uid);
                session.FieldManager.BroadcastPacket(EquipmentPacket.UnequipItem(session.Player.FieldPlayer, removeItem));
            }
        }

        // equip & update new item
        switch (itemSlot)
        {
            case ItemSlot.HR:
                float backLength = BitConverter.ToSingle(packet.ReadBytes(4), 0);
                CoordF backPositionCoord = packet.Read<CoordF>();
                CoordF backPositionRotation = packet.Read<CoordF>();
                float frontLength = BitConverter.ToSingle(packet.ReadBytes(4), 0);
                CoordF frontPositionCoord = packet.Read<CoordF>();
                CoordF frontPositionRotation = packet.Read<CoordF>();

                beautyItem.HairData = new(backLength, frontLength, backPositionCoord, backPositionRotation, frontPositionCoord, frontPositionRotation);

                cosmetics[itemSlot] = beautyItem;

                session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.Player.FieldPlayer, beautyItem, itemSlot));
                break;
            case ItemSlot.FA:
                cosmetics[itemSlot] = beautyItem;

                session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.Player.FieldPlayer, beautyItem, itemSlot));
                break;
            case ItemSlot.FD:
                byte[] faceDecorationPosition = packet.ReadBytes(16);

                beautyItem.FaceDecorationData = faceDecorationPosition;

                cosmetics[itemSlot] = beautyItem;

                session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(session.Player.FieldPlayer, beautyItem, itemSlot));
                break;
        }
    }

    private static bool HandleShopPay(GameSession session, BeautyShop shop, bool useVoucher)
    {
        return useVoucher ? PayWithVoucher(session, shop) : PayWithShopTokenCost(session, shop);
    }

    private static bool PayWithVoucher(GameSession session, BeautyShop shop)
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
                Logger.Warning($"Unknown Beauty Shop: {shop.BeautyType}");
                session.Send(NoticePacket.Notice("Unknown Beauty Shop", NoticeType.FastText));
                return false;
        }

        Item voucher = session.Player.Inventory.GetAllByTag(voucherTag).FirstOrDefault();
        if (voucher == null)
        {
            session.Send(NoticePacket.Notice(SystemNotice.ErrorItemNotFound, NoticeType.FastText));
            return false;
        }

        session.Send(BeautyPacket.UseVoucher(voucher.Id, 1));
        session.Player.Inventory.ConsumeItem(session, voucher.Uid, 1);
        return true;
    }

    private static bool PayWithShopTokenCost(GameSession session, BeautyShop beautyShop)
    {
        int cost = beautyShop.CurrencyCost;
        if (beautyShop.SpecialCost != 0)
        {
            cost = beautyShop.SpecialCost;
        }

        return Pay(session, beautyShop.CurrencyType, cost, beautyShop.RequiredItemId);
    }

    private static bool PayWithShopItemTokenCost(GameSession session, int beautyItemId, BeautyShop beautyShop)
    {
        BeautyShopItem item = beautyShop.Items.FirstOrDefault(x => x.ItemId == beautyItemId);

        return item != null && Pay(session, item.CurrencyType, item.CurrencyCost, item.RequiredItemId);
    }

    private static bool Pay(GameSession session, ShopCurrencyType type, int tokenCost, int requiredItemId)
    {
        switch (type)
        {
            case ShopCurrencyType.Meso:
                return session.Player.Wallet.Meso.Modify(-tokenCost);
            case ShopCurrencyType.ValorToken:
                return session.Player.Wallet.ValorToken.Modify(-tokenCost);
            case ShopCurrencyType.Treva:
                return session.Player.Wallet.Treva.Modify(-tokenCost);
            case ShopCurrencyType.Rue:
                return session.Player.Wallet.Rue.Modify(-tokenCost);
            case ShopCurrencyType.HaviFruit:
                return session.Player.Wallet.HaviFruit.Modify(-tokenCost);
            case ShopCurrencyType.Meret:
            case ShopCurrencyType.GameMeret:
            case ShopCurrencyType.EventMeret:
                return session.Player.Account.RemoveMerets(tokenCost);
            case ShopCurrencyType.Item:
                Item itemCost = session.Player.Inventory.GetById(requiredItemId);
                if (itemCost == null)
                {
                    return false;
                }

                if (itemCost.Amount < tokenCost)
                {
                    return false;
                }

                session.Player.Inventory.ConsumeItem(session, itemCost.Uid, tokenCost);
                return true;
            default:
                session.SendNotice($"Unknown currency: {type}");
                return false;
        }
    }
}
