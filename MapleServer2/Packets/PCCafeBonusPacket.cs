using MaplePacketLib2.Tools;
using MapleServer2.Constants;

namespace MapleServer2.Packets;

public static class PCCafeBonusPacket
{
    private enum PCCafeBonusPacketMode : byte
    {
        LoadBenefits = 0x0,
        ClaimLoginTimeReward = 0x1,
        ClaimPCCafeItem = 0x2,
        Unk = 0x3
    }

    public static PacketWriter LoadBenefits()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PCCafeBonus);
        pWriter.Write(PCCafeBonusPacketMode.LoadBenefits);
        pWriter.WriteByte();
        pWriter.WriteBool(true); // player is PCCafe user 
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt(100000006); // benefit buff ID
        pWriter.WriteInt(1); // login time reward count
        
        // loop
        pWriter.WriteInt(20001752); // item ID
        pWriter.WriteShort(1); // item rarity
        pWriter.WriteInt(3); // item quantity
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteInt(50);
        pWriter.WriteInt(60000); // login time required in ms
        pWriter.WriteInt();
        // end loop
        
        pWriter.WriteInt(1); // pc cafe only item count
        
        // loop
        pWriter.WriteInt(20300298); // item ID
        pWriter.WriteShort(2); // item rarity
        pWriter.WriteInt(4); // item quantity
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteByte();
        pWriter.WriteInt();
        // end loop
        
        pWriter.WriteByte(1); // PCCafe enabled ?
        return pWriter;
    }
    
    public static PacketWriter ClaimLoginTimeReward(byte index)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PCCafeBonus);
        pWriter.Write(PCCafeBonusPacketMode.ClaimLoginTimeReward);
        pWriter.WriteByte(index);
        pWriter.WriteInt();
        return pWriter;
    }

    public static PacketWriter ClaimPCCafeItem(int index)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PCCafeBonus);
        pWriter.Write(PCCafeBonusPacketMode.ClaimPCCafeItem);
        pWriter.WriteInt(index);
        return pWriter;
    }
    
    public static PacketWriter Unk()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PCCafeBonus);
        pWriter.Write(PCCafeBonusPacketMode.Unk);
        pWriter.WriteBool(true); // is PC Cafe user
        pWriter.WriteInt();
        pWriter.WriteInt();
        pWriter.WriteInt();
        return pWriter;
    }
}
