using Maple2Storage.Types;
using MapleServer2.Enums;
using MapleServer2.Types;
using Newtonsoft.Json;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseItem : DatabaseTable
    {
        private readonly JsonSerializerSettings Settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

        public DatabaseItem() : base("items") { }

        public long Insert(Item item)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                item.Level,
                itemslot = (byte) item.ItemSlot,
                item.Rarity,
                item.PlayCount,
                item.Amount,
                bankinventoryid = item.BankInventoryId == 0 ? null : (int?) item.BankInventoryId,
                item.RepackageCount,
                item.Charges,
                color = JsonConvert.SerializeObject(item.Color),
                item.CreationTime,
                item.EnchantExp,
                item.Enchants,
                item.ExpiryTime,
                facedecorationdata = JsonConvert.SerializeObject(item.FaceDecorationData),
                item.GachaDismantleId,
                hairdata = JsonConvert.SerializeObject(item.HairData),
                hatdata = JsonConvert.SerializeObject(item.HatData),
                homeid = item.HomeId == 0 ? null : (int?) item.HomeId,
                item.Id,
                inventoryid = item.InventoryId == 0 ? null : (int?) item.InventoryId,
                item.IsEquipped,
                item.IsLocked,
                ownercharacterid = item.OwnerCharacterId == 0 ? null : (int?) item.OwnerCharacterId,
                item.OwnerCharacterName,
                item.PairedCharacterId,
                item.PairedCharacterName,
                item.PetSkinBadgeId,
                item.RemainingGlamorForges,
                item.RemainingTrades,
                score = JsonConvert.SerializeObject(item.Score),
                item.Slot,
                stats = JsonConvert.SerializeObject(item.Stats, Settings),
                item.TimesAttributesChanged,
                item.TransferFlag,
                transparencybadgebools = JsonConvert.SerializeObject(item.TransparencyBadgeBools),
                item.UnlockTime
            });
        }

        public Item FindByUid(long uid) => ReadItem(QueryFactory.Query(TableName).Where("uid", uid).FirstOrDefault());

        public List<Item> FindAllByInventoryId(long inventoryId)
        {
            IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("inventoryid", inventoryId).Get();
            List<Item> items = new List<Item>();
            foreach (dynamic data in result)
            {
                items.Add((Item) ReadItem(data));
            }
            return items;
        }

        public List<Item> FindAllByBankInventoryId(long bankInventoryId)
        {
            IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("bankinventoryid", bankInventoryId).Get();
            List<Item> items = new List<Item>();
            foreach (dynamic data in result)
            {
                items.Add((Item) ReadItem(data));
            }
            return items;
        }

        public Dictionary<long, Item> FindAllByHomeId(long homeId)
        {
            IEnumerable<dynamic> result = QueryFactory.Query(TableName).Where("homeid", homeId).Get();
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
            QueryFactory.Query(TableName).Where("uid", item.Uid).Update(new
            {
                item.Level,
                itemslot = (byte) item.ItemSlot,
                item.Rarity,
                item.PlayCount,
                item.Amount,
                bankinventoryid = item.BankInventoryId == 0 ? null : (int?) item.BankInventoryId,
                item.RepackageCount,
                item.Charges,
                color = JsonConvert.SerializeObject(item.Color),
                item.CreationTime,
                item.EnchantExp,
                item.Enchants,
                item.ExpiryTime,
                facedecorationdata = JsonConvert.SerializeObject(item.FaceDecorationData),
                item.GachaDismantleId,
                hairdata = JsonConvert.SerializeObject(item.HairData),
                hatdata = JsonConvert.SerializeObject(item.HatData),
                homeid = item.HomeId == 0 ? null : (int?) item.HomeId,
                item.Id,
                inventoryid = item.InventoryId == 0 ? null : (int?) item.InventoryId,
                item.IsEquipped,
                item.IsLocked,
                ownercharacterid = item.OwnerCharacterId == 0 ? null : (int?) item.OwnerCharacterId,
                item.OwnerCharacterName,
                item.PairedCharacterId,
                item.PairedCharacterName,
                item.PetSkinBadgeId,
                item.RemainingGlamorForges,
                item.RemainingTrades,
                score = JsonConvert.SerializeObject(item.Score),
                item.Slot,
                stats = JsonConvert.SerializeObject(item.Stats, Settings),
                item.TimesAttributesChanged,
                item.TransferFlag,
                transparencybadgebools = JsonConvert.SerializeObject(item.TransparencyBadgeBools),
                item.UnlockTime
            });
        }

        public bool Delete(long uid) => QueryFactory.Query(TableName).Where("uid", uid).Delete() == 1;

        private Item ReadItem(dynamic data)
        {
            return new Item()
            {
                Uid = data.uid,
                Level = data.level,
                ItemSlot = (ItemSlot) data.itemslot,
                Rarity = data.rarity,
                PlayCount = data.playcount,
                Amount = data.amount,
                RepackageCount = data.repackagecount,
                Charges = data.charges,
                Color = JsonConvert.DeserializeObject<EquipColor>(data.color),
                CreationTime = data.creationtime,
                EnchantExp = data.enchantexp,
                Enchants = data.enchants,
                ExpiryTime = data.expirytime,
                FaceDecorationData = JsonConvert.DeserializeObject<byte[]>(data.facedecorationdata),
                GachaDismantleId = data.gachadismantleid,
                HairData = JsonConvert.DeserializeObject<HairData>(data.hairdata),
                HatData = JsonConvert.DeserializeObject<HatData>(data.hatdata),
                Id = data.id,
                IsEquipped = data.isequipped,
                IsLocked = data.islocked,
                OwnerCharacterId = data.ownercharacterid ?? 0,
                OwnerCharacterName = data.ownercharactername ?? "",
                PairedCharacterId = data.pairedcharacterid,
                PairedCharacterName = data.pairedcharactername,
                PetSkinBadgeId = data.petskinbadgeid,
                RemainingGlamorForges = data.remainingglamorforges,
                RemainingTrades = data.remainingtrades,
                Score = JsonConvert.DeserializeObject<MusicScore>(data.score),
                Slot = data.slot,
                Stats = JsonConvert.DeserializeObject<ItemStats>(data.stats, Settings),
                TimesAttributesChanged = data.timesattributeschanged,
                TransferFlag = (TransferFlag) data.transferflag,
                TransparencyBadgeBools = JsonConvert.DeserializeObject<byte[]>(data.transparencybadgebools),
                UnlockTime = data.unlocktime,
                InventoryId = data.inventoryid ?? 0,
                BankInventoryId = data.bankinventoryid ?? 0,
                HomeId = data.homeid ?? 0,
            };
        }
    }
}
