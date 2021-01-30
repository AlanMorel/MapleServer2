using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class PremiumClubHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.PREMIUM_CLUB;

        public PremiumClubHandler(ILogger<PremiumClubHandler> logger) : base(logger) { }

        private enum PremiumClubMode : byte
        {
            Open = 0x1,
            ClaimItems = 0x2,
            OpenPurchaseWindow = 0x3,
            PurchaseMembership = 0x4,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            PremiumClubMode mode = (PremiumClubMode) packet.ReadByte();

            switch (mode)
            {
                case PremiumClubMode.Open:
                    HandleOpen(session);
                    break;
                case PremiumClubMode.ClaimItems:
                    HandleClaimItems(session, packet);
                    break;
                case PremiumClubMode.OpenPurchaseWindow:
                    HandleOpenPurchaseWindow(session);
                    break;
                case PremiumClubMode.PurchaseMembership:
                    HandlePurchaseMembership(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleOpen(GameSession session)
        {
            session.Send(PremiumClubPacket.Open());
        }

        private static void HandleClaimItems(GameSession session, PacketReader packet)
        {
            int benefitId = packet.ReadInt();
            session.Send(PremiumClubPacket.ClaimItem(benefitId));
            // TODO grab data from \table\vipbenefititemtable.xml for item ID, quantity, rank
            // TODO only claim once a day
        }

        private static void HandleOpenPurchaseWindow(GameSession session)
        {
            session.Send(PremiumClubPacket.OpenPurchaseWindow());
        }

        private static void HandlePurchaseMembership(GameSession session, PacketReader packet)
        {
            int packageId = packet.ReadInt();
            session.Send(PremiumClubPacket.PurchaseMembership(packageId));
            // TODO grab data from \table\vipgoodstable.xml for pricing, buff duration, and bonus items
            long expiration = 2592847227; // temporarilyy plugging in expiration time
            session.Send(PremiumClubPacket.ActivatePremium(session.FieldPlayer, expiration));
            session.Player.IsVIP = true;
        }
    }
}
