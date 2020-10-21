using System.Collections.Generic;
using System.Linq;
using Maple2.Data.Converter;
using Maple2.Data.Types;
using Maple2.Data.Types.Items;
using Maple2.Data.Utils;
using Maple2.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Maple2.Data.Storage {
    public class MarketStorage {
        private readonly DbContextOptions options;
        private readonly IModelConverter<Item, Maple2.Sql.Model.Item> itemConverter;
        private readonly IModelConverter<BlackMarketListing, Maple2.Sql.Model.BlackMarketListing> blackMarketListingConverter;
        private readonly IModelConverter<MesoMarketListing, Maple2.Sql.Model.MesoMarketListing> mesoMarketListingConverter;
        private readonly ILogger logger;

        public MarketStorage(DbContextOptions options,
                IModelConverter<Item, Maple2.Sql.Model.Item> itemConverter,
                IModelConverter<BlackMarketListing, Maple2.Sql.Model.BlackMarketListing> blackMarketListingConverter,
                IModelConverter<MesoMarketListing, Maple2.Sql.Model.MesoMarketListing> mesoMarketListingConverter,
                ILogger<MarketStorage> logger) {
            this.options = options;
            this.itemConverter = itemConverter;
            this.blackMarketListingConverter = blackMarketListingConverter;
            this.mesoMarketListingConverter = mesoMarketListingConverter;
            this.logger = logger;
        }

        public Request Context() {
            return new Request(this, new MarketContext(options), logger);
        }

        public class Request : DatabaseRequest<MarketContext> {
            private readonly MarketStorage storage;
            private readonly ItemStorageOperations<MarketContext> itemOperations;

            public Request(MarketStorage storage, MarketContext context, ILogger logger) : base(context, logger) {
                this.storage = storage;
                itemOperations = new ItemStorageOperations<MarketContext>(context, storage.itemConverter, logger);
            }

            /* Read */
            public BlackMarketListing GetBlackMarketListing(long listingId) {
                return context.BlackMarketListing.AsQueryable()
                    .Where(dbListing => dbListing.Id == listingId)
                    .Include(dbListing => dbListing.Item)
                    .Select(storage.blackMarketListingConverter.FromModel)
                    .SingleOrDefault();
            }

            public ICollection<BlackMarketListing> GetBlackMarketListings(long characterId) {
                return context.BlackMarketListing.AsQueryable()
                    .Where(dbListing => dbListing.CharacterId == characterId)
                    .Include(dbListing => dbListing.Item)
                    .Select(storage.blackMarketListingConverter.FromModel)
                    .ToList();
            }

            public ICollection<BlackMarketListing> GetAllBlackMarketListings() {
                return context.BlackMarketListing
                    .Include(dbListing => dbListing.Item)
                    .OrderBy(dbListing => dbListing.Price)
                    .Select(storage.blackMarketListingConverter.FromModel)
                    .ToList();
            }

            public MesoMarketListing GetMesoMarketListing(long listingId) {
                return context.MesoMarketListing.AsQueryable()
                    .Where(dbListing => dbListing.Id == listingId)
                    .AsEnumerable()
                    .Select(storage.mesoMarketListingConverter.FromModel)
                    .SingleOrDefault();
            }

            public ICollection<MesoMarketListing> GetMesoMarketListings(long accountId) {
                return context.MesoMarketListing.AsQueryable()
                    .Where(dbListing => dbListing.AccountId == accountId)
                    .AsEnumerable()
                    .Select(storage.mesoMarketListingConverter.FromModel)
                    .ToList();
            }

            public ICollection<MesoMarketListing> SearchMesoMarketListings(long accountId, long min, long max) {
                return context.MesoMarketListing.AsQueryable()
                    .Where(listing => listing.AccountId != accountId)
                    .Where(listing => listing.Mesos >= min)
                    .Where(listing => listing.Mesos <= max)
                    .OrderBy(dbListing => dbListing.Price)
                    .Select(storage.mesoMarketListingConverter.FromModel)
                    .ToList();
            }

            /* Write */
            public long CreateBlackMarketListing(BlackMarketListing listing) {
                var dbListing = new Maple2.Sql.Model.BlackMarketListing();
                // Perform lookup for item if it exists, so we UPDATE instead of ADD
                dbListing.Item = context.Item.Find(listing.Item.Id) ?? new Maple2.Sql.Model.Item();
                dbListing.Item.OwnerId = 100; // 100 = BlackMarket

                storage.blackMarketListingConverter.ToModel(listing, dbListing);
                context.BlackMarketListing.Add(dbListing);
                return context.TrySaveChanges() ? dbListing.Id : -1;
            }

            public bool DeleteBlackMarketListing(long listingId) {
                Maple2.Sql.Model.BlackMarketListing listing = context.BlackMarketListing.Find(listingId);
                if (listing == null) {
                    return false;
                }

                context.Remove(listing);
                return context.TrySaveChanges();
            }

            public void UpdateItemAmount(Item item) {
                Maple2.Sql.Model.Item dbItem = context.Item.Find(item.Id);
                storage.itemConverter.ToModel(item, dbItem);
                context.SaveChanges();
            }

            public long CreateMesoMarketListing(MesoMarketListing listing) {
                Maple2.Sql.Model.MesoMarketListing dbListing = storage.mesoMarketListingConverter.ToModel(listing);
                context.MesoMarketListing.Add(dbListing);
                return context.TrySaveChanges() ? dbListing.Id : -1;
            }

            public bool DeleteMesoMarketListing(long listingId) {
                Maple2.Sql.Model.MesoMarketListing listing = context.MesoMarketListing.Find(listingId);
                if (listing == null) {
                    return false;
                }

                context.Remove(listing);
                return context.TrySaveChanges();
            }
        }
    }
}