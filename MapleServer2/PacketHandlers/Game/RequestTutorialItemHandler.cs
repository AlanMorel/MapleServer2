using System.Collections.Generic;
using System.Linq;
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

            foreach(TutorialItemMetadata tutorialItem in metadata)
            {
                if (session.Player.Inventory.Items.Any(x => x.Value.Id == tutorialItem.ItemId) || session.Player.Inventory.Equips.Any(x => x.Value.Id == tutorialItem.ItemId))
                {
                    continue;
                }

                Item item = new Item(tutorialItem.ItemId)
                {
                    Rarity = tutorialItem.Rarity,
                    Amount = tutorialItem.Amount,
                };
                InventoryController.Add(session, item, true);
            }
        }
    }
}
