﻿using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets.Helpers;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class TradePacket
{
    private enum Mode : byte
    {
        TradeRequest = 0x0,
        Notice = 0x1,
        RequestSent = 0x2,
        RequestDeclined = 0x4,
        RequestAccepted = 0x5,
        TradeStatus = 0x6,
        AddItemToTrade = 0x8,
        RemoveItemToTrade = 0x9,
        AddMesosToTrade = 0xA,
        FinalizeOffer = 0xB,
        OfferedAltered = 0xC,
        FinalizeTrade = 0xD
    }

    public static PacketWriter TradeRequest(string playerName, long playerCharacterId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trade);
        pWriter.Write(Mode.TradeRequest);
        pWriter.WriteUnicodeString(playerName);
        pWriter.WriteLong(playerCharacterId);
        return pWriter;
    }

    public static PacketWriter RequestSent()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trade);
        pWriter.Write(Mode.RequestSent);
        return pWriter;
    }

    public static PacketWriter RequestDeclined(string characterName)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trade);
        pWriter.Write(Mode.RequestDeclined);
        pWriter.WriteUnicodeString(characterName);
        return pWriter;
    }

    public static PacketWriter RequestAccepted(long playerCharacterId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trade);
        pWriter.Write(Mode.RequestAccepted);
        pWriter.WriteLong(playerCharacterId);
        return pWriter;
    }

    public static PacketWriter TradeStatus(bool tradeIsComplete)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trade);
        pWriter.Write(Mode.TradeStatus);
        pWriter.WriteBool(tradeIsComplete);
        return pWriter;
    }

    public static PacketWriter AddItemToTrade(Item item, int index, bool selfInventory)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trade);
        pWriter.Write(Mode.AddItemToTrade);
        pWriter.WriteBool(selfInventory);
        pWriter.WriteInt(item.Id);
        pWriter.WriteLong(item.Uid);
        pWriter.WriteInt(item.Rarity);
        pWriter.WriteInt(index);
        pWriter.WriteInt(item.Amount);
        pWriter.WriteInt(index);
        pWriter.WriteItem(item);
        return pWriter;
    }

    public static PacketWriter RemoveItemToTrade(Item item, int index, bool selfInventory)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trade);
        pWriter.Write(Mode.RemoveItemToTrade);
        pWriter.WriteBool(selfInventory);
        pWriter.WriteInt(index);
        pWriter.WriteLong(item.Uid);
        return pWriter;
    }

    public static PacketWriter AddMesosToTrade(long mesos, bool selfInventory)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trade);
        pWriter.Write(Mode.AddMesosToTrade);
        pWriter.WriteBool(selfInventory);
        pWriter.WriteLong(mesos);
        return pWriter;
    }

    public static PacketWriter FinalizeOffer(bool selfInventory)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trade);
        pWriter.Write(Mode.FinalizeOffer);
        pWriter.WriteBool(selfInventory);
        return pWriter;
    }

    public static PacketWriter OfferedAltered(bool self)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trade);
        pWriter.Write(Mode.OfferedAltered);
        pWriter.WriteBool(self);
        return pWriter;
    }

    public static PacketWriter FinalizeTrade(bool selfInventory)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.Trade);
        pWriter.Write(Mode.FinalizeTrade);
        pWriter.WriteBool(selfInventory);
        return pWriter;
    }
}
