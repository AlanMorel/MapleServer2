﻿using System;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
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

            if (!PremiumClubDailyBenefitMetadataStorage.IsValid(benefitId))
            {
                return;
            }

            PremiumClubDailyBenefitMetadata benefit = PremiumClubDailyBenefitMetadataStorage.GetMetadata(benefitId);

            Item benefitRewardItem = new(benefit.ItemId)
            {
                Rarity = benefit.ItemRarity,
                Amount = benefit.ItemAmount,
            };

            InventoryController.Add(session, benefitRewardItem, true);

            // TODO only claim once a day
        }

        private static void HandleOpenPurchaseWindow(GameSession session)
        {
            session.Send(PremiumClubPacket.OpenPurchaseWindow());
        }

        private static void HandlePurchaseMembership(GameSession session, PacketReader packet)
        {
            int vipId = packet.ReadInt();

            if (!PremiumClubPackageMetadataStorage.IsValid(vipId))
            {
                return;
            }

            PremiumClubPackageMetadata vipPackage = PremiumClubPackageMetadataStorage.GetMetadata(vipId);

            if (!session.Player.Account.RemoveMerets(session, vipPackage.Price))
            {
                return;
            }

            session.Send(PremiumClubPacket.PurchaseMembership(vipId));

            foreach (BonusItem item in vipPackage.BonusItem)
            {
                Item bonusItem = new(item.Id)
                {
                    Rarity = item.Rarity,
                    Amount = item.Amount
                };
                InventoryController.Add(session, bonusItem, true);
            }

            int vipTime = vipPackage.VipPeriod * 3600; // Convert to seconds, as vipPeriod is given as hours

            ActivatePremium(session, vipTime);
        }

        public static void ActivatePremium(GameSession session, long vipTime)
        {
            long expiration = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + vipTime;

            if (!session.Player.IsVip())
            {
                session.Player.VIPExpiration = expiration;
                session.Send(NoticePacket.Notice(SystemNotice.PremiumActivated, NoticeType.ChatAndFastText));
            }
            else
            {
                session.Player.VIPExpiration += vipTime;
                session.Send(NoticePacket.Notice(SystemNotice.PremiumExtended, NoticeType.ChatAndFastText));
            }
            session.Send(BuffPacket.SendBuff(0, new Status(100000014, session.FieldPlayer.ObjectId, session.FieldPlayer.ObjectId, 1, (int) vipTime, 1)));
            session.Send(PremiumClubPacket.ActivatePremium(session.FieldPlayer, session.Player.VIPExpiration));
        }
    }
}
