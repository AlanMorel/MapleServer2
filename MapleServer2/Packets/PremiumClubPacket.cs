using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Packets
{
    public static class PremiumClubPacket
    {
        private enum PremiumClubPacketMode : byte
        {
            ActivatePremium = 0x0,
            Open = 0x1,
            ClaimItem = 0x2,
            OpenPurchaseWindow = 0x3,
            PurchaseMembership = 0x4,
        }

        public static Packet Open()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PREMIUM_CLUB);
            pWriter.WriteMode(PremiumClubPacketMode.Open);
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet ClaimItem(int benefitId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PREMIUM_CLUB);
            pWriter.WriteMode(PremiumClubPacketMode.ClaimItem);
            pWriter.WriteInt(benefitId);
            return pWriter;
        }

        public static Packet OpenPurchaseWindow()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PREMIUM_CLUB);
            pWriter.WriteMode(PremiumClubPacketMode.OpenPurchaseWindow);
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet PurchaseMembership(int packageId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PREMIUM_CLUB);
            pWriter.WriteMode(PremiumClubPacketMode.PurchaseMembership);
            pWriter.WriteInt(packageId);
            return pWriter;
        }

        public static Packet ActivatePremium(IFieldObject<Player> player, long expiration)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.PREMIUM_CLUB);
            pWriter.WriteMode(PremiumClubPacketMode.ActivatePremium);
            pWriter.WriteInt(player.ObjectId);
            pWriter.WriteLong(expiration);
            return pWriter;
        }
    }
}
