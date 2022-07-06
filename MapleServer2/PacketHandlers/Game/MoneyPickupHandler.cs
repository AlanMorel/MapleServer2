using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class MoneyPickupHandler : GamePacketHandler<MoneyPickupHandler>
{
    public override RecvOp OpCode => RecvOp.RequestMoneyPickup;

    public override void Handle(GameSession session, PacketReader packet)
    {
        int objectCount = packet.ReadByte();

        for (int i = 0; i < objectCount; i++)
        {
            int objectId = packet.ReadInt();

            if (!session.FieldManager.PickupItem(objectId, session.Player.FieldPlayer.ObjectId, out IFieldObject<Item> fieldItem)
                || fieldItem.Value.Id is < 90000001 or > 90000003)
            {
                continue;
            }

            session.Player.Wallet.Meso.Modify(fieldItem.Value.Amount);
        }
    }
}
