﻿using Maple2Storage.Enums;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class MeretMarketHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.MERET_MARKET;

    public MeretMarketHandler() : base() { }

    private enum MeretMarketMode : byte
    {
        Initialize = 0x16,
        OpenPremium = 0x1B,
        SendMarketRequest = 0x1D,
        Purchase = 0x1E,
        Home = 0x65,
        LoadCart = 0x6B
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        MeretMarketMode mode = (MeretMarketMode) packet.ReadByte();

        switch (mode)
        {
            case MeretMarketMode.Initialize:
                HandleInitialize(session);
                break;
            case MeretMarketMode.OpenPremium:
                HandleOpenPremium(session, packet);
                break;
            case MeretMarketMode.Purchase:
                HandlePurchase(session, packet);
                break;
            case MeretMarketMode.Home:
                HandleHome(session);
                break;
            case MeretMarketMode.LoadCart:
                HandleLoadCart(session);
                break;
            case MeretMarketMode.SendMarketRequest:
                HandleSendMarketRequest(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleInitialize(GameSession session)
    {
        session.Send(MeretMarketPacket.Initialize());
    }

    private static void HandleOpenPremium(GameSession session, PacketReader packet)
    {
        MeretMarketCategory category = (MeretMarketCategory) packet.ReadInt();
        List<MeretMarketItem> marketItems = DatabaseManager.MeretMarket.FindAllByCategoryId(category);
        if (marketItems == null)
        {
            return;
        }
        session.Send(MeretMarketPacket.Premium(marketItems));
    }

    private static void HandlePurchase(GameSession session, PacketReader packet)
    {
        byte quantity = packet.ReadByte();
        int marketItemId = packet.ReadInt();
        byte[] unk = packet.ReadBytes(12);
        int childMarketItemId = packet.ReadInt();
        long unk2 = packet.ReadLong();
        int itemIndex = packet.ReadInt();
        int totalQuantity = packet.ReadInt();
        int unk3 = packet.ReadInt();
        byte unk4 = packet.ReadByte();
        string unk5 = packet.ReadUnicodeString();
        string unk6 = packet.ReadUnicodeString();
        long price = packet.ReadLong();

        MeretMarketItem marketItem = DatabaseManager.MeretMarket.FindById(marketItemId);
        if (marketItem == null)
        {
            return;
        }

        if (childMarketItemId == 0)
        {
            HandleMarketItemPay(session, marketItem, itemIndex, totalQuantity);
            return;
        }

        MeretMarketItem childItem = marketItem.AdditionalQuantities.FirstOrDefault(x => x.MarketId == childMarketItemId);
        if (childItem == null)
        {
            return;
        }

        HandleMarketItemPay(session, childItem, itemIndex, totalQuantity);
    }

    private static void HandleMarketItemPay(GameSession session, MeretMarketItem marketItem, int itemIndex, int totalQuantity)
    {
        switch (marketItem.TokenType)
        {
            case MeretMarketCurrencyType.Meret:
                if (!session.Player.Account.RemoveMerets(marketItem.SalePrice))
                {
                    return;
                }
                break;
            case MeretMarketCurrencyType.Meso:
                if (!session.Player.Wallet.Meso.Modify(marketItem.SalePrice))
                {
                    return;
                }
                break;
        }

        Item item = new(marketItem.ItemId)
        {
            Amount = marketItem.Quantity + marketItem.BonusQuantity,
            Rarity = marketItem.Rarity
        };
        if (marketItem.Duration != 0)
        {
            item.ExpiryTime = TimeInfo.Now() + Environment.TickCount + marketItem.Duration * 24 * 60 * 60;
        }
        session.Player.Inventory.AddItem(session, item, true);
        session.Send(MeretMarketPacket.Purchase(marketItem, itemIndex, totalQuantity));
    }

    private static void HandleHome(GameSession session)
    {
        List<MeretMarketItem> marketItems = DatabaseManager.MeretMarket.FindAllByCategoryId(MeretMarketCategory.Promo);
        if (marketItems == null)
        {
            return;
        }
        session.Send(MeretMarketPacket.Promos(marketItems));
    }

    private static void HandleLoadCart(GameSession session)
    {
        session.Send(MeretMarketPacket.LoadCart());
    }

    private static void HandleSendMarketRequest(GameSession session, PacketReader packet)
    {
        packet.ReadByte(); //constant 1
        int meretMarketItemUid = packet.ReadInt();
        List<MeretMarketItem> meretMarketItems = new()
        {
            DatabaseManager.MeretMarket.FindById(meretMarketItemUid)
        };
        session.Send(MeretMarketPacket.Premium(meretMarketItems));
    }
}
