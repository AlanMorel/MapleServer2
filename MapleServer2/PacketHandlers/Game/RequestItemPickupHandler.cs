using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestItemPickupHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_ITEM_PICKUP;

        public RequestItemPickupHandler() : base() { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            int objectId = packet.ReadInt();

            bool foundItem = session.FieldManager.State.TryGetItem(objectId, out IFieldObject<Item> fieldItem);
            if (foundItem)
            {
                switch (fieldItem.Value.Id)
                {
                    case 90000004:
                    case 90000011:
                    case 90000015:
                    case 90000016:
                    case 90000020:
                        session.Player.Account.Meret.Modify(fieldItem.Value.Amount);
                        break;
                    case 90000008:
                        session.Player.Levels.GainExp(fieldItem.Value.Amount);
                        break;
                    case 90000009:
                        session.Player.RecoverSp(fieldItem.Value.Amount);
                        break;
                    case 90000010:
                        session.Player.RecoverStamina(fieldItem.Value.Amount);
                        break;
                    default:
                        // TODO: This will be bugged when you have a full inventory, check inventory before looting
                        fieldItem.Value.Slot = -1; // add to first empty slot
                        InventoryController.Add(session, fieldItem.Value, true);
                        break;
                }

                if (session.FieldManager.RemoveItem(objectId, out Item item))
                {
                    session.FieldManager.BroadcastPacket(FieldPacket.PickupItem(objectId, item, session.FieldPlayer.ObjectId));
                    session.FieldManager.BroadcastPacket(FieldPacket.RemoveItem(objectId));
                }

            }

            int countExtra = packet.ReadByte();
            for (int i = 0; i < countExtra; i++)
            {
            }
        }
    }
}
