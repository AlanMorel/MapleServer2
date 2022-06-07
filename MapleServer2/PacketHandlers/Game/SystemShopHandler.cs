using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.PacketHandlers.Game;

public class SystemShopHandler : GamePacketHandler<SystemShopHandler>
{
    public override RecvOp OpCode => RecvOp.SystemShop;

    private enum ShopMode : byte
    {
        Arena = 0x03,
        Fishing = 0x04,
        ViaItem = 0x0A
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        ShopMode mode = (ShopMode) packet.ReadByte();

        switch (mode)
        {
            case ShopMode.ViaItem:
                HandleViaItem(session, packet);
                break;
            case ShopMode.Fishing:
                HandleFishingShop(session, packet);
                break;
            case ShopMode.Arena:
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

        ShopHelper.OpenSystemShop(session, item.ShopID, 0);
    }

    private static void HandleFishingShop(GameSession session, PacketReader packet)
    {
        bool openShop = packet.ReadBool();

        if (!openShop)
        {
            return;
        }

        ShopHelper.OpenSystemShop(session, 161, 11001609);
    }

    private static void HandleMapleArenaShop(GameSession session, PacketReader packet)
    {
        bool openShop = packet.ReadBool();

        if (!openShop)
        {
            return;
        }

        ShopHelper.OpenSystemShop(session, 168, 11001562);
    }


}
