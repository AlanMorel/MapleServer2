using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class RequestTutorialItemHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.RequestTutorialItem;

    public override void Handle(GameSession session, PacketReader packet)
    {
        List<TutorialItemMetadata> metadata = JobMetadataStorage.GetTutorialItems(session.Player.Job);

        foreach (TutorialItemMetadata tutorialItem in metadata)
        {
            int tutorialItemsCount = session.Player.Inventory.GetAllById(tutorialItem.ItemId).Count;
            tutorialItemsCount += session.Player.Inventory.Cosmetics.Count(x => x.Value.Id == tutorialItem.ItemId);
            tutorialItemsCount += session.Player.Inventory.Equips.Count(x => x.Value.Id == tutorialItem.ItemId);

            if (tutorialItemsCount >= tutorialItem.Amount)
            {
                continue;
            }

            int amountRemaining = tutorialItem.Amount - tutorialItemsCount;

            Item item = new(tutorialItem.ItemId)
            {
                Rarity = tutorialItem.Rarity,
                Amount = amountRemaining
            };
            session.Player.Inventory.AddItem(session, item, true);
        }
    }
}
