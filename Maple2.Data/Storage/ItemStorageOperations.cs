using System.Collections.Generic;
using System.Linq;
using Maple2.Data.Converter;
using Maple2.Data.Types.Items;
using Maple2.Data.Utils;
using Maple2.Sql.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Maple2.Data.Storage {
    public class ItemStorageOperations<TContext> where TContext : DbContext, IItemAccessor {
        private readonly TContext context;
        private readonly IModelConverter<Item, Maple2.Sql.Model.Item> itemConverter;
        private readonly ILogger logger;

        public ItemStorageOperations(TContext context, IModelConverter<Item, Maple2.Sql.Model.Item> itemConverter,
                ILogger logger) {
            this.context = context;
            this.itemConverter = itemConverter;
            this.logger = logger;
        }

        #region ReadOperations
        public Item GetItem(long itemId) {
            Item item = itemConverter.FromModel(context.Item.Find(itemId));
            SetSocketGemstones(item);

            return item;
        }

        public IList<Item> GetItems(long ownerId) {
            IList<Item> items = context.Item.AsQueryable()
                .Where(item => item.OwnerId == ownerId)
                .AsEnumerable()
                .Select(itemConverter.FromModel)
                .ToList();

            foreach (Item item in items) {
                SetSocketGemstones(item);
            }

            return items;
        }

        public void SetSocketGemstones(Item item) {
            if (item.Sockets.Unlocked <= 0) {
                return;
            }

            IEnumerable<Item> gems = context.Item.AsQueryable()
                .Where(dbItem => dbItem.OwnerId == item.Id)
                .AsEnumerable()
                .Select(itemConverter.FromModel);
            item.Sockets.Load(gems);
        }
        #endregion

        #region WriteOperations
        public long CreateItem(long ownerId, Item item) {
            Maple2.Sql.Model.Item model = itemConverter.ToModel(item);
            model.OwnerId = ownerId;
            context.Item.Add(model);
            context.SaveChanges();
            return model.Id;
        }

        public bool DeleteItem(long itemUid) {
            Maple2.Sql.Model.Item item = context.Item.Find(itemUid);
            if (item.OwnerId != 0) {
                return false;
            }

            context.Item.Remove(item);
            return context.TrySaveChanges();
        }

        public void StageSocketGemstones(Item item) {
            if (item.Sockets.Unlocked <= 0) {
                return;
            }

            long ownerId = item.Id;
            StageSyncItems(ownerId, item.Sockets);
        }

        // Saves items for ownerId, removing any owned items that are not present.
        public void StageSyncItems(long ownerId, IEnumerable<Item> items) {
            Dictionary<long, Maple2.Sql.Model.Item> dbItems = context.Item.AsQueryable()
                .Where(dbItem => dbItem.OwnerId == ownerId)
                .ToDictionary(dbItem => dbItem.Id, dbItem => dbItem);

            foreach (Item item in items) {
                Maple2.Sql.Model.Item dbItem;
                if (item.Id == 0) { // New items will fall back to creation
                    dbItem = new Maple2.Sql.Model.Item();
                } else if (!dbItems.Remove(item.Id, out dbItem)) {
                    dbItem = context.Item.Find(item.Id);

                    if (dbItem == null) { // Item does not exist
                        logger.LogError($"Failed to save non-existent item: {item.Id} for {ownerId}");
                        continue;
                    }
                }

                StageSocketGemstones(item);
                itemConverter.ToModel(item, dbItem);
                dbItem.OwnerId = ownerId;
            }

            // Remaining items are no longer owned (soft-delete)
            foreach (Maple2.Sql.Model.Item dbItem in dbItems.Values) {
                // Ensure that you still own this item at this point before "deleting"
                if (dbItem.OwnerId == ownerId) {
                    dbItem.OwnerId = 0;
                }
            }
        }
        #endregion
    }
}