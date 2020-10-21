using Maple2.Data.Types;
using Maple2.Data.Types.Items;
using Maple2.Data.Utils;

namespace Maple2.Data.Converter {
    public class BlackMarketListingConverter : IModelConverter<BlackMarketListing, Maple2.Sql.Model.BlackMarketListing> {
        private readonly IModelConverter<Item, Maple2.Sql.Model.Item> itemConverter;

        public BlackMarketListingConverter(IModelConverter<Item, Maple2.Sql.Model.Item> itemConverter) {
            this.itemConverter = itemConverter;
        }

        public Maple2.Sql.Model.BlackMarketListing ToModel(BlackMarketListing value,
                Maple2.Sql.Model.BlackMarketListing listing) {
            if (value == null) return null;

            listing ??= new Maple2.Sql.Model.BlackMarketListing();
            listing.Id = value.Id;
            listing.AccountId = value.AccountId;
            listing.CharacterId = value.CharacterId;
            listing.ExpiryTime = value.ExpiryTime.FromEpochSeconds();
            listing.Price = value.Price;
            listing.Item = itemConverter.ToModel(value.Item, listing.Item);

            return listing;
        }

        public BlackMarketListing FromModel(Maple2.Sql.Model.BlackMarketListing value) {
            if (value == null) return null;

            var listing = new BlackMarketListing();
            listing.Id = value.Id;
            listing.AccountId = value.AccountId;
            listing.CharacterId = value.CharacterId;
            listing.CreationTime = value.CreationTime.ToEpochSeconds();
            listing.ExpiryTime = value.ExpiryTime.ToEpochSeconds();
            listing.Price = value.Price;
            listing.Item = itemConverter.FromModel(value.Item);

            return listing;
        }
    }
}