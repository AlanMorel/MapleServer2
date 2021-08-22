using Maple2Storage.Types;
using MapleServer2.Enums;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseItem
    {
        private readonly JsonSerializerSettings Settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
        private readonly string TableName = "Items";

        public long Insert(Item item)
        {
            return DatabaseManager.QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                item.Level,
                ItemSlot = (byte) item.ItemSlot,
                item.Rarity,
                item.PlayCount,
                item.Amount,
                BankInventoryId = item.BankInventoryId == 0 ? null : (int?) item.BankInventoryId,
                item.CanRepackage,
                item.Charges,
                Color = JsonConvert.SerializeObject(item.Color),
                item.CreationTime,
                item.EnchantExp,
                item.Enchants,
                item.ExpiryTime,
                FaceDecorationData = JsonConvert.SerializeObject(item.FaceDecorationData),
                item.GachaDismantleId,
                HairData = JsonConvert.SerializeObject(item.HairData),
                HatData = JsonConvert.SerializeObject(item.HatData),
                HomeId = item.HomeId == 0 ? null : (int?) item.HomeId,
                item.Id,
                InventoryId = item.InventoryId == 0 ? null : (int?) item.InventoryId,
                item.IsEquipped,
                item.IsLocked,
                OwnerCharacterId = item.OwnerCharacterId == 0 ? null : (int?) item.OwnerCharacterId,
                item.OwnerCharacterName,
                item.PairedCharacterId,
                item.PairedCharacterName,
                item.PetSkinBadgeId,
                item.RemainingGlamorForges,
                Score = JsonConvert.SerializeObject(item.Score),
                item.Slot,
                Stats = JsonConvert.SerializeObject(item.Stats, Settings),
                item.TimesAttributesChanged,
                item.TransferFlag,
                TransparencyBadgeBools = JsonConvert.SerializeObject(item.TransparencyBadgeBools),
                item.UnlockTime
            });
        }

        public Item FindByUid(long uid) => ReadItem(DatabaseManager.QueryFactory.Query(TableName).Where("Uid", uid).FirstOrDefault());

        public List<Item> FindAllByInventoryId(long inventoryId)
        {
            IEnumerable<dynamic> result = DatabaseManager.QueryFactory.Query(TableName).Where("InventoryId", inventoryId).Get();
            List<Item> items = new List<Item>();
            foreach (dynamic data in result)
            {
                items.Add((Item) ReadItem(data));
            }
            return items;
        }

        public List<Item> FindAllByBankInventoryId(long bankInventoryId)
        {
            IEnumerable<dynamic> result = DatabaseManager.QueryFactory.Query(TableName).Where("BankInventoryId", bankInventoryId).Get();
            List<Item> items = new List<Item>();
            foreach (dynamic data in result)
            {
                items.Add((Item) ReadItem(data));
            }
            return items;
        }

        public Dictionary<long, Item> FindAllByHomeId(long homeId)
        {
            IEnumerable<dynamic> result = DatabaseManager.QueryFactory.Query(TableName).Where("HomeId", homeId).Get();
            Dictionary<long, Item> items = new Dictionary<long, Item>();
            foreach (dynamic data in result)
            {
                Item item = (Item) ReadItem(data);
                items.Add(item.Uid, item);
            }
            return items;
        }

        public void Update(Item item)
        {
            DatabaseManager.QueryFactory.Query(TableName).Where("Uid", item.Uid).Update(new
            {
                item.Level,
                ItemSlot = (byte) item.ItemSlot,
                item.Rarity,
                item.PlayCount,
                item.Amount,
                BankInventoryId = item.BankInventoryId == 0 ? null : (int?) item.BankInventoryId,
                item.CanRepackage,
                item.Charges,
                Color = JsonConvert.SerializeObject(item.Color),
                item.CreationTime,
                item.EnchantExp,
                item.Enchants,
                item.ExpiryTime,
                FaceDecorationData = JsonConvert.SerializeObject(item.FaceDecorationData),
                item.GachaDismantleId,
                HairData = JsonConvert.SerializeObject(item.HairData),
                HatData = JsonConvert.SerializeObject(item.HatData),
                HomeId = item.HomeId == 0 ? null : (int?) item.HomeId,
                item.Id,
                InventoryId = item.InventoryId == 0 ? null : (int?) item.InventoryId,
                item.IsEquipped,
                item.IsLocked,
                OwnerCharacterId = item.OwnerCharacterId == 0 ? null : (int?) item.OwnerCharacterId,
                item.OwnerCharacterName,
                item.PairedCharacterId,
                item.PairedCharacterName,
                item.PetSkinBadgeId,
                item.RemainingGlamorForges,
                Score = JsonConvert.SerializeObject(item.Score),
                item.Slot,
                Stats = JsonConvert.SerializeObject(item.Stats, Settings),
                item.TimesAttributesChanged,
                item.TransferFlag,
                TransparencyBadgeBools = JsonConvert.SerializeObject(item.TransparencyBadgeBools),
                item.UnlockTime
            });
        }

        public bool Delete(long uid) => DatabaseManager.QueryFactory.Query(TableName).Where("Uid", uid).Delete() == 1;

        private Item ReadItem(dynamic data)
        {
            return new Item()
            {
                Uid = data.Uid,
                Level = data.Level,
                ItemSlot = (ItemSlot) data.ItemSlot,
                Rarity = data.Rarity,
                PlayCount = data.PlayCount,
                Amount = data.Amount,
                CanRepackage = data.CanRepackage,
                Charges = data.Charges,
                Color = JsonConvert.DeserializeObject<EquipColor>(data.Color),
                CreationTime = data.CreationTime,
                EnchantExp = data.EnchantExp,
                Enchants = data.Enchants,
                ExpiryTime = data.ExpiryTime,
                FaceDecorationData = JsonConvert.DeserializeObject<byte[]>(data.FaceDecorationData),
                GachaDismantleId = data.GachaDismantleId,
                HairData = JsonConvert.DeserializeObject<HairData>(data.HairData),
                HatData = JsonConvert.DeserializeObject<HatData>(data.HatData),
                Id = data.Id,
                IsEquipped = data.IsEquipped,
                IsLocked = data.IsLocked,
                OwnerCharacterId = data.OwnerCharacterId ?? 0,
                OwnerCharacterName = data.OwnerCharacterName ?? "",
                PairedCharacterId = data.PairedCharacterId,
                PairedCharacterName = data.PairedCharacterName,
                PetSkinBadgeId = data.PetSkinBadgeId,
                RemainingGlamorForges = data.RemainingGlamorForges,
                Score = JsonConvert.DeserializeObject<MusicScore>(data.Score),
                Slot = data.Slot,
                Stats = JsonConvert.DeserializeObject<ItemStats>(data.Stats, Settings),
                TimesAttributesChanged = data.TimesAttributesChanged,
                TransferFlag = (TransferFlag) data.TransferFlag,
                TransparencyBadgeBools = JsonConvert.DeserializeObject<byte[]>(data.TransparencyBadgeBools),
                UnlockTime = data.UnlockTime,
                InventoryId = data.InventoryId ?? 0,
                BankInventoryId = data.BankInventoryId ?? 0,
                HomeId = data.HomeId ?? 0,
            };
        }
    }
}
