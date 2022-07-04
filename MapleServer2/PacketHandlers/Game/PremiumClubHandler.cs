using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class PremiumClubHandler : GamePacketHandler<PremiumClubHandler>
{
    public override RecvOp OpCode => RecvOp.PremiumClub;

    private enum PremiumClubMode : byte
    {
        Open = 0x1,
        ClaimItems = 0x2,
        OpenPurchaseWindow = 0x3,
        PurchaseMembership = 0x4
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
                LogUnknownMode(mode);
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

        Item benefitRewardItem = new(benefit.ItemId, benefit.ItemAmount, benefit.ItemRarity);

        session.Player.Inventory.AddItem(session, benefitRewardItem, true);

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

        if (!session.Player.Account.RemoveMerets(vipPackage.Price))
        {
            return;
        }

        session.Send(PremiumClubPacket.PurchaseMembership(vipId));

        foreach (BonusItem item in vipPackage.BonusItem)
        {
            Item bonusItem = new(item.Id, item.Amount, item.Rarity);
            session.Player.Inventory.AddItem(session, bonusItem, true);
        }

        int vipTime = vipPackage.VipPeriod * 3600; // Convert to seconds, as vipPeriod is given as hours

        ActivatePremium(session, vipTime);
    }

    public static void ActivatePremium(GameSession session, long vipTime)
    {
        long expiration = TimeInfo.Now() + vipTime;

        Account account = session.Player.Account;
        if (!account.IsVip())
        {
            account.VIPExpiration = expiration;
            session.Send(NoticePacket.Notice(SystemNotice.PremiumActivated, NoticeType.Chat | NoticeType.FastText));
        }
        else
        {
            account.VIPExpiration += vipTime;
            session.Send(NoticePacket.Notice(SystemNotice.PremiumExtended, NoticeType.Chat | NoticeType.FastText));
        }

        List<PremiumClubEffectMetadata> effectMetadatas = PremiumClubEffectMetadataStorage.GetBuffs();
        foreach (PremiumClubEffectMetadata effect in effectMetadatas)
        {
            session.Player.FieldPlayer.AdditionalEffects.AddEffect(new(effect.EffectId, effect.EffectLevel)
            {
                Duration = (int) (Math.Min(account.VIPExpiration - TimeInfo.Now(), 0x0FFFFFFF)),
                IsBuff = true
            });
        }

        session.Send(PremiumClubPacket.ActivatePremium(session.Player.FieldPlayer, account.VIPExpiration));
    }
}
