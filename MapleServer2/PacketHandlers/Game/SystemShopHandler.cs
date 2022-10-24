using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class SystemShopHandler : GamePacketHandler<SystemShopHandler>
{
    public override RecvOp OpCode => RecvOp.SystemShop;

    private enum Mode : byte
    {
        Arena = 0x03,
        Fishing = 0x04,
        ViaItem = 0x0A
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.ViaItem:
                HandleViaItem(session, packet);
                break;
            case Mode.Fishing:
                HandleFishingShop(session, packet);
                break;
            case Mode.Arena:
                HandleMapleArenaShop(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleViaItem(GameSession session, PacketReader packet)
    {
        bool openShop = packet.ReadBool();

        if (!openShop)
        {

            return;
        }

        int coinId = packet.ReadInt();

        ItemMetadata item = ItemMetadataStorage.GetMetadata(coinId);
        if (item is null)
        {
            return;
        }

        ShopHelper.OpenShop(session, item.Shop.ShopId, 0);
        session.Send(SystemShopPacket.Open());
    }

    private static void HandleFishingShop(GameSession session, PacketReader packet)
    {
        bool openShop = packet.ReadBool();

        if (!openShop)
        {
            return;
        }

        ShopHelper.OpenShop(session, 161, 11001609);
        session.Send(SystemShopPacket.Open());
    }

    private static void HandleMapleArenaShop(GameSession session, PacketReader packet)
    {
        bool openShop = packet.ReadBool();

        if (!openShop)
        {
            return;
        }

        ShopHelper.OpenShop(session, 168, 11001562);
        session.Send(SystemShopPacket.Open());
    }
}
