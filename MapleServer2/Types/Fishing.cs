using Maple2Storage.Tools;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class Fishing
{
    public int FishId;
    public int TotalCaught;
    public int TotalPrizeFish;
    public int LargestFish;
    public Fishing() { }

    public static void AddExistingFish(GameSession session, FishMetadata fish, int size)
    {
        session.Player.FishAlbum[fish.Id].TotalCaught += 1;

        if (size > session.Player.FishAlbum[fish.Id].LargestFish)
        {
            session.Player.FishAlbum[fish.Id].LargestFish = size;
        }

        AddMastery(session, fish, false);

        if (size >= fish.BigSize[0])
        {
            session.Player.FishAlbum[fish.Id].LargestFish = size;
        }
    }

    public static void AddNewFish(GameSession session, FishMetadata fish, int size)
    {
        session.Player.FishAlbum[fish.Id] = new()
        {
            FishId = fish.Id, TotalCaught = 1, LargestFish = size
        };

        AddMastery(session, fish, true);

        if (size >= fish.BigSize[0])
        {
            session.Player.FishAlbum[fish.Id].LargestFish = size;
        }
    }

    public static void AddMastery(GameSession session, FishMetadata fish, bool newFish)
    {
        if (newFish)
        {
            int exp = fish.Rarity * 2;
            session.Player.Levels.GainMasteryExp(MasteryType.Fishing, exp);
            session.Send(FishingPacket.IncreaseMastery(MasteryType.Fishing, fish.Id, exp));
            return;
        }

        int expChance;
        Random rnd = RandomProvider.Get();
        MasteryExp masteryExp = session.Player.Levels.MasteryExp.FirstOrDefault(x => x.Type == MasteryType.Fishing);
        FishingSpotMetadata fishingSpot = FishingSpotMetadataStorage.GetMetadata(session.Player.MapId);

        if (masteryExp.CurrentExp > fishingSpot.MaxMastery)
        {
            expChance = rnd.Next(0, 200); // decrease chance of gaining mastery by half
        }
        else
        {
            expChance = rnd.Next(0, 100);
        }

        if (expChance <= 10)
        {
            int exp = rnd.Next(1, 3);
            session.Player.Levels.GainMasteryExp(MasteryType.Fishing, exp);
            session.Send(FishingPacket.IncreaseMastery(MasteryType.Fishing, fish.Id, exp));
        }
    }
}
