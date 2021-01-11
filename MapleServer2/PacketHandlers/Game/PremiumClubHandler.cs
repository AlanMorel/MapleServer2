using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class PremiumClubHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.VIP_GOODS_SHOP;

        public PremiumClubHandler(ILogger<ClubHandler> logger) : base(logger) { }

        private enum PremiumClubMode : byte
        {
            OpenUI = 0x1,
            ClaimItems = 0x2,
            PurchaseWindow = 0x3,
            PurchaseMembership = 0x4,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            PremiumClubMode mode = (PremiumClubMode) packet.ReadByte();

            switch (mode)
            {
                case PremiumClubMode.OpenUI:
                    HandleOpenUI(session, packet);
                    break;
                case PremiumClubMode.ClaimItems:
                    HandleClaimItems(session, packet);
                    break;
                case PremiumClubMode.PurchaseWindow:
                    HandlePurchaseWindow(session, packet);
                    break;
                case PremiumClubMode.PurchaseMembership:
                    HandlePurchaseMembership(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private void HandleOpenUI(GameSession session, PacketReader packet)
        {
            session.Send(PremiumClubPacket.OpenUIConfirm());
        }

        private void HandleClaimItems(GameSession session, PacketReader packet)
        {
            int benefitId = packet.ReadInt();
            session.Send(PremiumClubPacket.ClaimItemConfirm(benefitId));
            // TODO grab data from \table\vipbenefititemtable.xml for item ID, quantity, rank
            // TODO only claim once a day
        }

        private void HandlePurchaseWindow(GameSession session, PacketReader packet)
        {
            session.Send(PremiumClubPacket.PurchaseWindowConfirm());
        }

        private void HandlePurchaseMembership(GameSession session, PacketReader packet)
        {
            int packageId = packet.ReadInt();
            session.Send(PremiumClubPacket.PurchaseMembershipConfirm(packageId));
            // TODO grab data from \table\vipgoodstable.xml for pricing, buff duration, and bonus items
            session.Send(PremiumClubPacket.ActivatePremium(session));
        }
    }
}
