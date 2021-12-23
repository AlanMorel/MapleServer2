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

public class TrophyHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.TROPHY;

    private enum TrophyHandlerMode : byte
    {
        ClaimReward = 0x03,
        Favorite = 0x04
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        TrophyHandlerMode mode = (TrophyHandlerMode) packet.ReadByte();

        switch (mode)
        {
            case TrophyHandlerMode.ClaimReward:
                HandleClaimReward(session, packet);
                break;
            case TrophyHandlerMode.Favorite:
                HandleFavorite(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleClaimReward(GameSession session, PacketReader packet)
    {
        int id = packet.ReadInt();
        if (!session.Player.TrophyData.TryGetValue(id, out Trophy trophy))
        {
            return;
        }

        TrophyMetadata metadata = TrophyMetadataStorage.GetMetadata(trophy.Id);
        List<TrophyGradeMetadata> grades = metadata.Grades.Where(x => x.Grade <= trophy.LastReward).ToList();
        foreach (TrophyGradeMetadata grade in grades)
        {
            if (grade.Grade >= trophy.LastReward)
            {
                ProvideReward(grade, session);
                trophy.LastReward++;
            }
        }
        session.Send(TrophyPacket.WriteUpdate(trophy));
        DatabaseManager.Trophies.Update(trophy);
    }

    private static void HandleFavorite(GameSession session, PacketReader packet)
    {
        int id = packet.ReadInt();
        bool favorited = packet.ReadBool();
        if (!session.Player.TrophyData.TryGetValue(id, out Trophy trophy))
        {
            return;
        }
        trophy.Favorited = favorited;
        session.Send(TrophyPacket.ToggleFavorite(trophy, favorited));
        DatabaseManager.Trophies.Update(trophy);
    }

    private static void ProvideReward(TrophyGradeMetadata grade, GameSession session)
    {
        switch (grade.RewardType)
        {
            // this cases don't require any handling.
            case RewardType.None:
            case RewardType.itemcoloring:
            case RewardType.beauty_hair:
            case RewardType.skillPoint:
            case RewardType.beauty_makeup:
            case RewardType.shop_build:
            case RewardType.shop_weapon:
            case RewardType.dynamicaction:
            case RewardType.etc:
            case RewardType.beauty_skin:
            case RewardType.statPoint:
            case RewardType.shop_ride:
            default:
                break;
            case RewardType.item:
                session.Player.Inventory.AddItem(session, new(grade.RewardCode, grade.RewardValue), true);
                break;
            case RewardType.title:
                session.Player.Titles.Add(grade.RewardCode);
                session.Send(UserEnvPacket.AddTitle(grade.RewardCode));
                break;
        }
    }
}
