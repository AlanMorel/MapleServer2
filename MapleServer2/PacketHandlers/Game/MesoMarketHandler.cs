using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class MesoMarketHandler : GamePacketHandler<MesoMarketHandler>
{
    public override RecvOp OpCode => RecvOp.MesoMarket;

    private enum Mode : byte
    {
        Load = 0x3,
        CreateListing = 0x5,
        CancelListing = 0x6,
        RefreshListings = 0x7,
        Purchase = 0x8
    }

    private enum MesoMarketError
    {
        TryAgain = 0x2,
        TradingUnavailable = 0x3,
        AnotherRequestInProcess = 0x4,
        NotEnoughMesosToList = 0x5,
        NotEnoughMesos = 0x6,
        NotEnoughTokensBuyMorePrompt = 0x7,
        ReachedListingLimit = 0x8,
        ItemNotSold = 0x9,
        ErrorSearchingRange = 0xA,
        CantPurchaseOwnMeso = 0xB,
        InvalidMesoQuantity = 0xF,
        PriceMustBeWithinRange = 0x10,
        ItemSoldOut = 0x11
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.Load:
                HandleLoad(session);
                break;
            case Mode.CreateListing:
                HandleCreateListing(session, packet);
                break;
            case Mode.CancelListing:
                HandleCancelListing(session, packet);
                break;
            case Mode.RefreshListings:
                HandleRefreshListings(session, packet);
                break;
            case Mode.Purchase:
                HandlePurchase(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleLoad(GameSession session)
    {
        session.Send(MesoMarketPacket.LoadMarket());
        session.Send(MesoMarketPacket.AccountStats(session.Player.Account.MesoMarketDailyListings, session.Player.Account.MesoMarketMonthlyPurchases));

        List<MesoMarketListing> listings = GameServer.MesoMarketManager.GetListingsByAccountId(session.Player.AccountId);
        session.Send(MesoMarketPacket.MyListings(listings));
    }

    private static void HandleCreateListing(GameSession session, PacketReader packet)
    {
        long mesos = packet.ReadLong();
        long price = packet.ReadLong();

        if (session.Player.Account.MesoMarketDailyListings >= int.Parse(ConstantsMetadataStorage.GetConstant("MesoMarketDailyListingsLimit")))
        {
            session.Send(MesoMarketPacket.Error((int) MesoMarketError.ReachedListingLimit));
            return;
        }

        if (!session.Player.Wallet.Meso.Modify(-mesos))
        {
            session.Send(MesoMarketPacket.Error((int) MesoMarketError.NotEnoughMesosToList));
            return;
        }

        MesoMarketListing listing = new(session.Player, price, mesos);

        session.Player.Account.MesoMarketDailyListings++;
        DatabaseManager.Accounts.Update(session.Player.Account);
        session.Send(MesoMarketPacket.CreateListing(listing));
        session.Send(MesoMarketPacket.AccountStats(session.Player.Account.MesoMarketDailyListings, session.Player.Account.MesoMarketMonthlyPurchases));
    }

    private static void HandleCancelListing(GameSession session, PacketReader packet)
    {
        long listingId = packet.ReadLong();

        MesoMarketListing listing = GameServer.MesoMarketManager.GetListingById(listingId);
        if (listing is null)
        {
            session.Send(MesoMarketPacket.Error((int) MesoMarketError.TryAgain));
            return;
        }

        MailHelper.SendMesoMarketCancellation(listing, session.Player.CharacterId);
        session.Send(MesoMarketPacket.CancelListing(listing.Id));
        GameServer.MesoMarketManager.RemoveListing(listing);
        DatabaseManager.MesoMarketListings.Delete(listingId);
    }

    private static void HandleRefreshListings(GameSession session, PacketReader packet)
    {
        // GMS2 has this set at 5m min and max due to it being Meso Tokens instead of mesos
        long minMesoRange = packet.ReadLong();
        long maxMesoRange = packet.ReadLong();

        List<MesoMarketListing> listings = GameServer.MesoMarketManager.GetSearchedListings(minMesoRange, maxMesoRange);
        session.Send(MesoMarketPacket.LoadListings(listings));
    }

    private static void HandlePurchase(GameSession session, PacketReader packet)
    {
        long listingId = packet.ReadLong();

        if (session.Player.Account.MesoMarketMonthlyPurchases >= int.Parse(ConstantsMetadataStorage.GetConstant("MesoMarketMonthlyPurchaseLimit")))
        {
            return;
        }

        MesoMarketListing listing = GameServer.MesoMarketManager.GetListingById(listingId);
        if (listing is null)
        {
            session.Send(MesoMarketPacket.Error((int) MesoMarketError.ItemSoldOut));
            return;
        }

        if (!session.Player.Account.MesoToken.Modify(-listing.Price))
        {
            session.Send(MesoMarketPacket.Error((int) MesoMarketError.NotEnoughMesos));
            return;
        }

        if (listing.OwnerAccountId == session.Player.AccountId)
        {
            session.Send(MesoMarketPacket.Error((int) MesoMarketError.CantPurchaseOwnMeso));
            return;
        }

        session.Player.Account.MesoMarketMonthlyPurchases++;
        DatabaseManager.Accounts.Update(session.Player.Account);
        session.Send(MesoMarketPacket.Purchase(listingId));
        session.Send(MesoMarketPacket.AccountStats(session.Player.Account.MesoMarketDailyListings, session.Player.Account.MesoMarketMonthlyPurchases));

        MailHelper.MesoMarketTransaction(listing, session.Player.CharacterId);
        GameServer.MesoMarketManager.RemoveListing(listing);
        DatabaseManager.MesoMarketListings.Delete(listingId);
    }
}
