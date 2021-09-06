using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestTutorialItemHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_TUTORIAL_ITEM;

        public RequestTutorialItemHandler(ILogger<RequestTutorialItemHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            List<TutorialItemMetadata> metadata = JobMetadataStorage.GetTutorialItems((int) session.Player.Job);

            foreach (TutorialItemMetadata tutorialItem in metadata)
            {
                int tutorialItemsCount = session.Player.Inventory.Items.Where(x => x.Value.Id == tutorialItem.ItemId).Count();
                tutorialItemsCount += session.Player.Inventory.Cosmetics.Where(x => x.Value.Id == tutorialItem.ItemId).Count();
                tutorialItemsCount += session.Player.Inventory.Equips.Where(x => x.Value.Id == tutorialItem.ItemId).Count();

                if (tutorialItemsCount >= tutorialItem.Amount)
                {
                    continue;
                }

                int amountRemaining = tutorialItem.Amount - tutorialItemsCount;

                Item item = new Item(tutorialItem.ItemId)
                {
                    Rarity = tutorialItem.Rarity,
                    Amount = amountRemaining,
                };
                InventoryController.Add(session, item, true);
            }
        }
    }
}
