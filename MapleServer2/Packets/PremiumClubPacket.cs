using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Types;

namespace MapleServer2.Packets;

public static class PremiumClubPacket
{
    private enum Mode : byte
    {
        ActivatePremium = 0x0,
        LoadItems = 0x1,
        ClaimItem = 0x2,
        OpenPurchaseWindow = 0x3,
        PurchaseMembership = 0x4
    }

    public static PacketWriter LoadItems(List<int> benefitItemIds)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PremiumClub);
        pWriter.Write(Mode.LoadItems);
        pWriter.WriteInt(benefitItemIds.Count);
        foreach (int benefitId in benefitItemIds)
        {
            pWriter.WriteInt(benefitId);
        }
        return pWriter;
    }

    public static PacketWriter ClaimItem(int benefitId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PremiumClub);
        pWriter.Write(Mode.ClaimItem);
        pWriter.WriteInt(benefitId);
        return pWriter;
    }

    public static PacketWriter OpenPurchaseWindow()
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PremiumClub);
        pWriter.Write(Mode.OpenPurchaseWindow);
        pWriter.WriteInt();
        return pWriter;
    }

    public static PacketWriter PurchaseMembership(int packageId)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PremiumClub);
        pWriter.Write(Mode.PurchaseMembership);
        pWriter.WriteInt(packageId);
        return pWriter;
    }

    public static PacketWriter ActivatePremium(IFieldObject<Player> player, long expiration)
    {
        PacketWriter pWriter = PacketWriter.Of(SendOp.PremiumClub);
        pWriter.Write(Mode.ActivatePremium);
        pWriter.WriteInt(player.ObjectId);
        pWriter.WriteLong(expiration);
        return pWriter;
    }
}
