using Microsoft.Extensions.Logging;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Packets;
using MapleServer2.Types;
using MapleServer2.Tools;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestItemUseHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_ITEM_USE;

        public RequestItemUseHandler(ILogger<SkillBookTreeHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();

            if (!session.Player.Inventory.Items.ContainsKey(itemUid))
            {
                return;
            }
            Item box = session.Player.Inventory.Items[itemUid];
            Item item = new Item(30001001)
            {
                Amount = 1,
                Uid = GuidGenerator.Long()
            };
            if (box.Amount <= 1)
            {
                session.Player.Inventory.Remove(itemUid, out Item removed);
                session.Send(ItemInventoryPacket.Remove(itemUid));
                InventoryController.Add(session, item);
            }
            else
            {
                box.Amount -= 1;
                session.Send(ItemInventoryPacket.Update(itemUid, box.Amount));
                InventoryController.Add(session, item);
            }

            // Need to handle opening boxes, probably serialize the item xml

            /* Single box workflow
            REQUEST_ITEM_USE
            [B0 A7 0B A9 DA 19 1D 24] [00 00]
            [Box Item Uid] [Short]

            ITEM_INVENTORY
            [02] [B0 A7 0B A9 DA 19 1D 24] [3A 00 00 00]
            [Mode Update] [Box Item Uid] [New Total Amount]

            ITEM_INVENTORY
            [02] [4C 5F 91 C8 37 19 2F 24] [12 00 00 00]
            [Mode Update] [Loot Item Uid] [New Total Amount]

            ITEM_INVENTORY
            [07] [4C 5F 91 C8 37 19 2F 24] [04 00 00 00] [00 00]
            [Mode MarkItemNew] [Loot Item Uid] [Amount That Is New] [Empty String]
            */
        }
    }
}