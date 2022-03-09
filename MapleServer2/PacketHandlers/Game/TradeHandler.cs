using Maple2Storage.Tools;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class TradeHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.TRADE;

    private enum TradeModeType : byte
    {
        SendRequest = 0x00,
        RequestRespond = 0x02,
        AcceptRequest = 0x03,
        DeclineRequest = 0x04,
        CloseTrade = 0x07,
        AddItemToTrade = 0x08,
        RemoveItemToTrade = 0x09,
        ModifyMesosToTrade = 0x0A,
        LockOffer = 0x0B,
        FinalizeTrade = 0x0D,
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        TradeModeType mode = (TradeModeType) packet.ReadByte();
        switch (mode)
        {
            case TradeModeType.SendRequest:
                HandleSendRequest(session, packet);
                break;
            case TradeModeType.RequestRespond:
                HandleRequestRespond(session, packet);
                break;
            case TradeModeType.AcceptRequest:
                HandleAcceptRequest(session, packet);
                break;
            case TradeModeType.DeclineRequest:
                HandleDeclineRequest(session, packet);
                break;
            case TradeModeType.CloseTrade:
                HandleCloseTrade(session);
                break;
            case TradeModeType.AddItemToTrade:
                HandleAddItemToTrade(session, packet);
                break;
            case TradeModeType.RemoveItemToTrade:
                HandleRemoveItemToTrade(session, packet);
                break;
            case TradeModeType.ModifyMesosToTrade:
                HandleModifyMesosToTrade(session, packet);
                break;
            case TradeModeType.LockOffer:
                HandleLockOffer(session);
                break;
            case TradeModeType.FinalizeTrade:
                HandleFinalizeTrade(session);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleSendRequest(GameSession session, PacketReader packet)
    {
        long otherCharacterId = packet.ReadLong();
        Player player = GameServer.PlayerManager.GetPlayerById(otherCharacterId);
        if (player is null)
        {
            return;
        }

        session.Send(TradePacket.RequestSent());
        player.Session?.Send(TradePacket.TradeRequest(session.Player.Name, session.Player.CharacterId));
    }

    private static void HandleRequestRespond(GameSession session, PacketReader packet)
    {
        long otherCharacterId = packet.ReadLong();
        // no idea what this does
    }

    private static void HandleAcceptRequest(GameSession session, PacketReader packet)
    {
        long otherCharacterId = packet.ReadLong();
        Player otherPlayer = GameServer.PlayerManager.GetPlayerById(otherCharacterId);
        if (otherPlayer is null)
        {
            return;
        }

        // Create Trade inventory
        session.Player.TradeInventory = new(otherPlayer);
        otherPlayer.TradeInventory = new(session.Player);

        session.Send(TradePacket.RequestAccepted(otherCharacterId));
        otherPlayer.Session?.Send(TradePacket.RequestAccepted(session.Player.CharacterId));
    }

    private static void HandleDeclineRequest(GameSession session, PacketReader packet)
    {
        long otherCharacterId = packet.ReadLong();
        Player player = GameServer.PlayerManager.GetPlayerById(otherCharacterId);
        player?.Session?.Send(TradePacket.RequestDeclined(session.Player.Name));
    }

    private static void HandleCloseTrade(GameSession session)
    {
        TradeInventory tradeInventory = session.Player.TradeInventory;
        if (tradeInventory is null)
        {
            return;
        }

        Player otherPlayer = tradeInventory.OtherPlayer;

        // Return items back to player's inventory
        session.Player.TradeInventory.SendItems(session.Player, false);
        session.Player.TradeInventory = null;

        if (otherPlayer?.TradeInventory is null)
        {
            return;
        }

        session.Send(TradePacket.TradeStatus(false));
        otherPlayer.Session?.Send(TradePacket.TradeStatus(false));
    }

    private static void HandleAddItemToTrade(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        int amount = packet.ReadInt();

        if (!session.Player.Inventory.HasItem(itemUid) || session.Player.TradeInventory.IsLocked)
        {
            return;
        }

        Item item = session.Player.Inventory.GetByUid(itemUid);

        if (item.IsBound())
        {
            return;
        }

        // Split item
        if (item.Amount > amount)
        {
            item.TrySplit(amount, out Item splitItem);
            session.Send(ItemInventoryPacket.UpdateAmount(itemUid, item.Amount));
            item = splitItem;
        }
        else
        {
            session.Player.Inventory.RemoveItem(session, itemUid, out Item removedItem);
            item = removedItem;
        }

        session.Player.TradeInventory.AddItem(session, item);
    }

    private static void HandleRemoveItemToTrade(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        int index = packet.ReadInt();

        if (session.Player.TradeInventory.IsLocked)
        {
            return;
        }

        session.Player.TradeInventory.RemoveItem(session, itemUid, index);
    }

    private static void HandleModifyMesosToTrade(GameSession session, PacketReader packet)
    {
        long mesoAmount = packet.ReadLong();

        if (mesoAmount > session.Player.Wallet.Meso.Amount || session.Player.TradeInventory.IsLocked)
        {
            return;
        }

        Player player = session.Player.TradeInventory.OtherPlayer;
        session.Player.TradeInventory.AlterTrade(session);

        session.Player.TradeInventory.Mesos = mesoAmount;

        session.Send(TradePacket.AddMesosToTrade(mesoAmount, true));
        player.Session?.Send(TradePacket.AddMesosToTrade(mesoAmount, false));
    }

    private static void HandleLockOffer(GameSession session)
    {
        TradeInventory tradeInventory = session.Player.TradeInventory;

        tradeInventory.IsLocked = true;

        session.Send(TradePacket.FinalizeOffer(true));
        tradeInventory.OtherPlayer.Session?.Send(TradePacket.FinalizeOffer(false));
    }

    private static void HandleFinalizeTrade(GameSession session)
    {
        TradeInventory tradeInventory = session.Player.TradeInventory;
        Player otherPlayer = tradeInventory.OtherPlayer;
        if (!otherPlayer.Session.Connected())
        {
            HandleCloseTrade(session);
            return;
        }

        TradeInventory otherPlayerTradeInventory = tradeInventory.OtherPlayer?.TradeInventory;
        if (otherPlayerTradeInventory is null)
        {
            return;
        }

        if (!otherPlayerTradeInventory.IsLocked && !tradeInventory.IsLocked)
        {
            return;
        }

        session.Send(TradePacket.FinalizeTrade(true));
        otherPlayer.Session.Send(TradePacket.FinalizeTrade(false));

        // Trade items
        GetSumMesos(session.Player, -tradeInventory.Mesos, otherPlayerTradeInventory.Mesos);
        otherPlayerTradeInventory.SendItems(session.Player, true);
        GetSumMesos(otherPlayer, -otherPlayerTradeInventory.Mesos, tradeInventory.Mesos);
        tradeInventory.SendItems(otherPlayer, true);

        session.Send(TradePacket.TradeStatus(true));
        otherPlayer.Session?.Send(TradePacket.TradeStatus(true));

        session.Player.TradeInventory = null;
        otherPlayer.TradeInventory = null;
    }

    private static void GetSumMesos(Player player, long incomingMesos, long outgoingMesos)
    {
        long sumMesos = incomingMesos + outgoingMesos;
        if (sumMesos > 0)
        {
            float feeRate = int.Parse(ConstantsMetadataStorage.GetConstant("TradeFeePercent")) / 100f;
            long tax = (long) (feeRate * sumMesos);
            sumMesos -= tax;
        }

        if (sumMesos != 0)
        {
            player.Wallet.Meso.Modify(sumMesos);
        }
    }
}
