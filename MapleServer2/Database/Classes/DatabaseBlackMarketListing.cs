using MapleServer2.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseBlackMarketListing : DatabaseTable
    {
        public DatabaseBlackMarketListing() : base("black_market_listings") { }

        public long Insert(BlackMarketListing listing)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                listing_timestamp = listing.ListedTimestamp,
                expiry_timestamp = listing.ExpiryTimestamp,
                price = listing.Price,
                deposit = listing.Deposit,
                listed_quantity = listing.ListedQuantity,
                item_uid = listing.Item.Uid,
                owner_account_id = listing.OwnerAccountId,
                owner_character_id = listing.OwnerCharacterId
            });
        }

        public List<BlackMarketListing> FindAll()
        {
            IEnumerable<dynamic> result = QueryFactory.Query(TableName).Get();
            List<BlackMarketListing> listings = new List<BlackMarketListing>();

            foreach (dynamic data in result)
            {
                listings.Add(ReadListing(data));
            }
            return listings;
        }

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("id", id).Delete() == 1;

        private static BlackMarketListing ReadListing(dynamic data)
        {
            return new BlackMarketListing()
            {
                Id = data.id,
                ListedTimestamp = data.listing_timestamp,
                ExpiryTimestamp = data.expiry_timestamp,
                Price = data.price,
                Deposit = data.deposit,
                ListedQuantity = data.listed_quantity,
                Item = DatabaseManager.Items.FindByUid(data.item_uid),
                OwnerAccountId = data.owner_account_id,
                OwnerCharacterId = data.owner_character_id
            };
        }
    }
}
