using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class RequestItemPickupHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.REQUEST_ITEM_PICKUP;

    public override void Handle(GameSession session, PacketReader packet)
    {
        int objectId = packet.ReadInt();

        if (!session.FieldManager.State.TryGetItem(objectId, out IFieldObject<Item> fieldItem))
        {
            return;
        }

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
                session.Player.FieldPlayer.RecoverSp(fieldItem.Value.Amount);
                break;
            case 90000010:
                session.Player.FieldPlayer.RecoverStamina(fieldItem.Value.Amount);
                break;
            default:
                if (!session.Player.Inventory.CanHold(fieldItem.Value))
                {
                    // No need to send "Inventory full" message since it's client sided.
                    return;
                }

                fieldItem.Value.Slot = -1; // add to first empty slot
                session.Player.Inventory.AddItem(session, fieldItem.Value, true);
                break;
        }

        if (session.FieldManager.RemoveItem(objectId, out Item item))
        {
            session.FieldManager.BroadcastPacket(FieldItemPacket.PickupItem(objectId, item, session.Player.FieldPlayer.ObjectId));
            session.FieldManager.BroadcastPacket(FieldItemPacket.RemoveItem(objectId));
        }

        int countExtra = packet.ReadByte();
        for (int i = 0; i < countExtra; i++)
        {
        }
    }
}
