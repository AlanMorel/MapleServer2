using System.Collections.Generic;
using Maple2.Data.Types;
using Maple2.Data.Types.Items;
using Maple2Storage.Enums;

namespace Maple2.Data.Storage {
    public partial class UserStorage {
        public partial class Request {
            #region ReadOperations
            public Item GetItem(long itemId) {
                return itemOperations.GetItem(itemId);
            }

            public IList<Item> GetItems(long ownerId) {
                return itemOperations.GetItems(ownerId);
            }

            public InventoryState LoadInventoryState(InventoryType type, long characterId) {
                long ownerId = InventoryState.GetOwnerId(characterId, type);
                ICollection<Item> items = itemOperations.GetItems(ownerId);

                return new InventoryState(type, items);
            }
            #endregion

            #region WriteOperations
            public long CreateItem(long ownerId, Item item) {
                return itemOperations.CreateItem(ownerId, item);
            }

            public void StageSyncItems(long ownerId, IEnumerable<Item> items) {
                itemOperations.StageSyncItems(ownerId, items);
            }

            public void StageInventoryState(long characterId, InventoryState state) {
                long ownerId = InventoryState.GetOwnerId(characterId, state.Type);
                itemOperations.StageSyncItems(ownerId, state);
            }
            #endregion
        }
    }
}