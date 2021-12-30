using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class MushkingRoyaleSystemHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.MUSHKING_ROYALE;

    private enum MushkingRoyaleSystemMode : byte
    {
        EquipMedal = 0x8,
        PurchaseGoldPass = 0x22,
        ClaimRewards = 0x23,
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        MushkingRoyaleSystemMode mode = (MushkingRoyaleSystemMode) packet.ReadByte();

        switch (mode)
        {
            case MushkingRoyaleSystemMode.EquipMedal:
                HandleEquipMedal(session, packet);
                break;
            case MushkingRoyaleSystemMode.PurchaseGoldPass:
                HandlePurchaseGoldPass(session, packet);
                break;
            case MushkingRoyaleSystemMode.ClaimRewards:
                HandleClaimRewards(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleEquipMedal(GameSession session, PacketReader packet)
    {
        MedalSlot slot = (MedalSlot) packet.ReadByte();
        int effectId = packet.ReadInt();

        if (effectId == 0)
        {
            session.Player.Account.UnequipMedal(slot);
            return;
        }

        Medal newMedal = session.Player.Account.Medals.FirstOrDefault(x => x.EffectId == effectId);
        if (newMedal is null)
        {
            return;
        }

        // unequip medal if present
        session.Player.Account.UnequipMedal(slot);

        // equip new medal
        session.Player.Account.EquippedMedals[slot] = newMedal;
        newMedal.IsEquipped = true;
        DatabaseManager.MushkingRoyaleMedals.Update(newMedal);

        session.Send(MushkingRoyaleSystemPacket.LoadMedals(session.Player.Account));
    }



    private static void HandlePurchaseGoldPass(GameSession session, PacketReader packet)
    {
        packet.ReadByte();
        long price = packet.ReadLong();

        SurvivalPeriodMetadata metadata = SurvivalPeriodMetadataStorage.GetMetadata();
        if (metadata is null || metadata.PassPrice != price)
        {
            return;
        }

        if (!session.Player.Account.RemoveMerets(price))
        {
            return;
        }

        session.Player.Account.MushkingRoyaleStats.IsGoldPassActive = true;
        DatabaseManager.MushkingRoyaleStats.Update(session.Player.Account.MushkingRoyaleStats);

        session.Send(MushkingRoyaleSystemPacket.LoadStats(session.Player.Account.MushkingRoyaleStats));
    }

    private static void HandleClaimRewards(GameSession session, PacketReader packet)
    {
        int silverLevelAmount = packet.ReadInt();
        int goldLevelAmount = packet.ReadInt();

        MushkingRoyaleStats stats = session.Player.Account.MushkingRoyaleStats;

        int silverStartLevel = stats.SilverLevelClaimedRewards;
        int goldStartLevel = stats.GoldLevelClaimedRewards;

        List<SurvivalSilverPassRewardMetadata> metadatas = SurvivalSilverPassRewardMetadataStorage.GetMetadatas(silverLevelAmount, stats.SilverLevelClaimedRewards);
        foreach (SurvivalSilverPassRewardMetadata metadata in metadatas)
        {
            GetReward(session, metadata.Type1, metadata.Id1, metadata.Value1, metadata.Count1);
        }
        stats.SilverLevelClaimedRewards += silverLevelAmount;

        if (session.Player.Account.MushkingRoyaleStats.IsGoldPassActive)
        {
            List<SurvivalGoldPassRewardMetadata> goldMetadatas = SurvivalGoldPassRewardMetadataStorage.GetMetadatas(goldLevelAmount, stats.GoldLevelClaimedRewards);
            foreach (SurvivalGoldPassRewardMetadata metadata in goldMetadatas)
            {
                GetReward(session, metadata.Type1, metadata.Id1, metadata.Value1, metadata.Count1);
                if (!metadata.Type2.Equals(""))
                {
                    GetReward(session, metadata.Type2, metadata.Id2, metadata.Value2, metadata.Count2);
                }
            }
            stats.GoldLevelClaimedRewards += goldLevelAmount;
        }

        DatabaseManager.MushkingRoyaleStats.Update(stats);
        session.Send(MushkingRoyaleSystemPacket.ClaimRewards(silverStartLevel, stats.SilverLevelClaimedRewards, goldStartLevel, stats.GoldLevelClaimedRewards));
        session.Send(MushkingRoyaleSystemPacket.LoadStats(stats));
    }

    private static void GetReward(GameSession session, string type, string id, string value, string count)
    {
        switch (type)
        {
            case "item":
                Item item = new(int.Parse(id))
                {
                    Rarity = int.Parse(value),
                    Amount = int.Parse(count)
                };
                session.Player.Inventory.AddItem(session, item, true);
                break;
            case "genderItem":
                List<int> ids = id.Split(",").Select(int.Parse).ToList();
                List<int> values = value.Split(",").Select(int.Parse).ToList();
                List<int> counts = count.Split(",").Select(int.Parse).ToList();
                for (int i = 0; i < ids.Count; i++)
                {
                    Gender gender = ItemMetadataStorage.GetGender(ids[i]);
                    if (gender != session.Player.Gender)
                    {
                        continue;
                    }

                    Item genderItem = new(ids[i])
                    {
                        Rarity = values[i],
                        Amount = counts[i]
                    };
                    session.Player.Inventory.AddItem(session, genderItem, true);
                }
                break;
            case "additionalEffect":
                // TODO: Handle giving player buff
                break;
            default:
                return;
        }
    }
}
