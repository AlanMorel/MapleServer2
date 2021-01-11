using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;

namespace MapleServer2.Packets
{
    public static class PremiumClubPacket
    {
        private enum PremiumClubPacketMode : byte
        {
            ActivatePremium = 0x0,
            OpenUIConfirm = 0x1,
            ClaimItemConfirm = 0x2,
            PurchaseWindowConfirm = 0x3,
            PurchaseMembershipConfirm = 0x4,
        }

        public static Packet OpenUIConfirm()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.VIP_GOODS_SHOP);
            pWriter.WriteMode(PremiumClubPacketMode.OpenUIConfirm);
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet ClaimItemConfirm(int benefitId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.VIP_GOODS_SHOP);
            pWriter.WriteMode(PremiumClubPacketMode.ClaimItemConfirm);
            pWriter.WriteInt(benefitId);
            return pWriter;
        }

        public static Packet PurchaseWindowConfirm()
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.VIP_GOODS_SHOP);
            pWriter.WriteMode(PremiumClubPacketMode.PurchaseWindowConfirm);
            pWriter.WriteInt();
            return pWriter;
        }

        public static Packet PurchaseMembershipConfirm(int packageId)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.VIP_GOODS_SHOP);
            pWriter.WriteMode(PremiumClubPacketMode.PurchaseMembershipConfirm);
            pWriter.WriteInt(packageId);
            return pWriter;
        }

        public static Packet ActivatePremium(GameSession session)
        {
            PacketWriter pWriter = PacketWriter.Of(SendOp.VIP_GOODS_SHOP);
            pWriter.WriteMode(PremiumClubPacketMode.ActivatePremium);
            pWriter.WriteInt(session.FieldPlayer.ObjectId);
            pWriter.WriteLong(2592847227); // expiration timestamp
            return pWriter;
        }
    }
}
