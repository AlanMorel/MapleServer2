using Maple2Storage.Enums;
using Maple2Storage.Types;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes;

public class DatabaseItem : DatabaseTable
{
    private readonly JsonSerializerSettings Settings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };

    public DatabaseItem() : base("items") { }

    public long Insert(Item item)
    {
        return QueryFactory.Query(TableName).InsertGetId<long>(new
        {
            name = item.Name,
            item.Level,
            item_slot = (byte) item.ItemSlot,
            gem_slot = (byte) item.GemSlot,
            item.Rarity,
            play_count = item.PlayCount,
            item.Amount,
            bank_inventory_id = item.BankInventoryId == 0 ? null : (int?) item.BankInventoryId,
            mail_id = item.MailId == 0 ? null : (int?) item.MailId,
            repackage_count = item.RemainingRepackageCount,
            item.Charges,
            color = JsonConvert.SerializeObject(item.Color),
            creation_time = item.CreationTime,
            enchant_exp = item.EnchantExp,
            item.Enchants,
            expiry_time = item.ExpiryTime,
            face_decoration_data = JsonConvert.SerializeObject(item.FaceDecorationData),
            gacha_dismantle_id = item.GachaDismantleId,
            hair_data = JsonConvert.SerializeObject(item.HairData),
            hat_data = JsonConvert.SerializeObject(item.HatData),
            home_id = item.HomeId == 0 ? null : (int?) item.HomeId,
            item.Id,
            inventory_id = item.InventoryId == 0 ? null : (int?) item.InventoryId,
            is_equipped = item.IsEquipped,
            is_locked = item.IsLocked,
            is_template = item.IsTemplate,
            owner_character_id = item.OwnerCharacterId == 0 ? null : (int?) item.OwnerCharacterId,
            owner_character_name = item.OwnerCharacterName,
            paired_character_id = item.PairedCharacterId,
            paired_character_name = item.PairedCharacterName,
            pet_skin_badge_id = item.PetSkinBadgeId,
            remaining_glamor_forges = item.RemainingGlamorForges,
            remaining_trades = item.RemainingTrades,
            score = JsonConvert.SerializeObject(item.Score),
            item.Slot,
            stats = JsonConvert.SerializeObject(item.Stats, Settings),
            times_attributes_changed = item.TimesAttributesChanged,
            transfer_flag = item.TransferFlag,
            blackmarket_category = item.BlackMarketCategory,
            transparency_badge_bools = JsonConvert.SerializeObject(item.TransparencyBadgeBools),
            unlock_time = item.UnlockTime,
            category = item.Category,
            ugc_uid = item.Ugc == null ? null : (int?) item.Ugc.Uid
        });
    }

    public Item FindByUid(long uid)
    {
        dynamic result = QueryFactory.Query(TableName).Where("uid", uid).FirstOrDefault();

        if (result is null)
        {
            return null;
        }

        return ReadItem(result);
    }

    public Item FindByUgcUid(long ugcUid)
    {
        dynamic result = QueryFactory.Query(TableName).Where("ugc_uid", ugcUid).FirstOrDefault();

        if (result is null)
        {
            return null;
        }

        return ReadItem(result);
    }

    public List<Item> FindAllByInventoryId(long inventoryId)
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("inventory_id", inventoryId).Get();
        List<Item> items = new();
        foreach (dynamic data in result)
        {
            items.Add((Item) ReadItem(data));
        }

        return items;
    }

    public List<Item> FindAllByBankInventoryId(long bankInventoryId)
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("bank_inventory_id", bankInventoryId).Get();
        List<Item> items = new();
        foreach (dynamic data in result)
        {
            items.Add((Item) ReadItem(data));
        }

        return items;
    }

    public Dictionary<long, Item> FindAllByHomeId(long homeId)
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("home_id", homeId).Get();
        Dictionary<long, Item> items = new();
        foreach (dynamic data in result)
        {
            Item item = (Item) ReadItem(data);
            items.Add(item.Uid, item);
        }

        return items;
    }

    public List<Item> FindAllByMailId(long mailId)
    {
        IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("mail_id", mailId).Get();
        List<Item> items = new();
        foreach (dynamic data in result)
        {
            items.Add((Item) ReadItem(data));
        }

        return items;
    }

    public void Update(Item item)
    {
        QueryFactory.Query(TableName).Where("uid", item.Uid).Update(new
        {
            item.Level,
            item_slot = (byte) item.ItemSlot,
            gem_slot = (byte) item.GemSlot,
            item.Rarity,
            play_count = item.PlayCount,
            item.Amount,
            bank_inventory_id = item.BankInventoryId == 0 ? null : (int?) item.BankInventoryId,
            mail_id = item.MailId == 0 ? null : (int?) item.MailId,
            repackage_count = item.RemainingRepackageCount,
            item.Charges,
            color = JsonConvert.SerializeObject(item.Color),
            creation_time = item.CreationTime,
            enchant_exp = item.EnchantExp,
            item.Enchants,
            expiry_time = item.ExpiryTime,
            face_decoration_data = JsonConvert.SerializeObject(item.FaceDecorationData),
            gacha_dismantle_id = item.GachaDismantleId,
            hair_data = JsonConvert.SerializeObject(item.HairData),
            hat_data = JsonConvert.SerializeObject(item.HatData),
            home_id = item.HomeId == 0 ? null : (int?) item.HomeId,
            item.Id,
            inventory_id = item.InventoryId == 0 ? null : (int?) item.InventoryId,
            is_equipped = item.IsEquipped,
            is_locked = item.IsLocked,
            is_template = item.IsTemplate,
            owner_character_id = item.OwnerCharacterId == 0 ? null : (int?) item.OwnerCharacterId,
            owner_character_name = item.OwnerCharacterName,
            paired_character_id = item.PairedCharacterId,
            paired_character_name = item.PairedCharacterName,
            pet_skin_badge_id = item.PetSkinBadgeId,
            remaining_glamor_forges = item.RemainingGlamorForges,
            remaining_trades = item.RemainingTrades,
            score = JsonConvert.SerializeObject(item.Score),
            item.Slot,
            stats = JsonConvert.SerializeObject(item.Stats, Settings),
            times_attributes_changed = item.TimesAttributesChanged,
            transfer_flag = item.TransferFlag,
            transparency_badge_bools = JsonConvert.SerializeObject(item.TransparencyBadgeBools),
            unlock_time = item.UnlockTime,
            ugc_uid = item.Ugc == null ? null : (int?) item.Ugc.Uid
        });
    }

    public bool Delete(long uid)
    {
        return QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;
    }

    private Item ReadItem(dynamic data)
    {
        return new()
        {
            Uid = data.uid,
            Name = data.name,
            Level = data.level,
            ItemSlot = (ItemSlot) data.item_slot,
            GemSlot = (GemSlot) data.gem_slot,
            Rarity = data.rarity,
            PlayCount = data.play_count,
            Amount = data.amount,
            RemainingRepackageCount = data.repackage_count,
            Charges = data.charges,
            Color = JsonConvert.DeserializeObject<EquipColor>(data.color),
            CreationTime = data.creation_time,
            EnchantExp = data.enchant_exp,
            Enchants = data.enchants,
            ExpiryTime = data.expiry_time,
            FaceDecorationData = JsonConvert.DeserializeObject<byte[]>(data.face_decoration_data),
            GachaDismantleId = data.gacha_dismantle_id,
            HairData = JsonConvert.DeserializeObject<HairData>(data.hair_data),
            HatData = JsonConvert.DeserializeObject<HatData>(data.hat_data),
            Id = data.id,
            IsEquipped = data.is_equipped,
            IsLocked = data.is_locked,
            IsTemplate = data.is_template,
            OwnerCharacterId = data.owner_character_id ?? 0,
            OwnerCharacterName = data.owner_character_name ?? "",
            PairedCharacterId = data.paired_character_id,
            PairedCharacterName = data.paired_character_name,
            PetSkinBadgeId = data.pet_skin_badge_id,
            RemainingGlamorForges = data.remaining_glamor_forges,
            RemainingTrades = data.remaining_trades,
            Score = JsonConvert.DeserializeObject<MusicScore>(data.score),
            Slot = data.slot,
            Stats = JsonConvert.DeserializeObject<ItemStats>(data.stats, Settings),
            TimesAttributesChanged = data.times_attributes_changed,
            TransferFlag = (ItemTransferFlag) data.transfer_flag,
            TransparencyBadgeBools = JsonConvert.DeserializeObject<byte[]>(data.transparency_badge_bools),
            UnlockTime = data.unlock_time,
            InventoryId = data.inventory_id ?? 0,
            BankInventoryId = data.bank_inventory_id ?? 0,
            BlackMarketCategory = data.blackmarket_category,
            Category = data.category,
            MailId = data.mail_id ?? 0,
            HomeId = data.home_id ?? 0,
            Ugc = data.ugc_uid == null ? null : DatabaseManager.Ugc.FindByUid(data.ugc_uid)
        };
    }
}
