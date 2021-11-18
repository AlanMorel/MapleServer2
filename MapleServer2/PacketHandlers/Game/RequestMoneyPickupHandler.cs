using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class RequestMoneyPickupHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.REQUEST_MONEY_PICKUP;

    public RequestMoneyPickupHandler() : base() { }

    public override void Handle(GameSession session, PacketReader packet)
    {
        int objectCount = packet.ReadByte();

        for (int i = 0; i < objectCount; i++)
        {
            int objectId = packet.ReadInt();

            bool foundItem = session.FieldManager.State.TryGetItem(objectId, out IFieldObject<Item> fieldItem);
            if (!foundItem || fieldItem.Value.Id is < 90000001 or > 90000003)
            {
                continue;
            }

            if (!session.FieldManager.RemoveItem(objectId, out Item item))
            {
                continue;
            }

            session.Player.Wallet.Meso.Modify(fieldItem.Value.Amount);
            session.FieldManager.BroadcastPacket(FieldItemPacket.PickupItem(objectId, item, session.Player.FieldPlayer.ObjectId));
            session.FieldManager.BroadcastPacket(FieldItemPacket.RemoveItem(objectId));
        }
    }
}
