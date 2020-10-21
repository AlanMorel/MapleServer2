using Maple2.Data.Types;
using Maple2.Data.Utils;

namespace Maple2.Data.Converter {
    public class MesoMarketListingConverter : IModelConverter<MesoMarketListing, Maple2.Sql.Model.MesoMarketListing> {
        public Maple2.Sql.Model.MesoMarketListing ToModel(MesoMarketListing value, Maple2.Sql.Model.MesoMarketListing listing) {
            if (value == null) return null;

            listing ??= new Maple2.Sql.Model.MesoMarketListing();
            listing.Id = value.Id;
            listing.AccountId = value.AccountId;
            listing.ExpiryTime = value.ExpiryTime.FromEpochSeconds();
            listing.Price = value.Price;
            listing.Mesos = value.Mesos;

            return listing;
        }

        public MesoMarketListing FromModel(Maple2.Sql.Model.MesoMarketListing value) {
            if (value == null) return null;

            var listing = new MesoMarketListing();
            listing.Id = value.Id;
            listing.AccountId = value.AccountId;
            listing.CreationTime = value.CreationTime.ToEpochSeconds();
            listing.ExpiryTime = value.ExpiryTime.ToEpochSeconds();
            listing.Price = value.Price;
            listing.Mesos = value.Mesos;

            return listing;
        }
    }
}