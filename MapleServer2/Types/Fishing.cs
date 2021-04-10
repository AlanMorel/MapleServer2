using System;
using Maple2Storage.Types.Metadata;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types
{
    public class Fishing
    {
        public int FishId;
        public int TotalCaught;
        public int TotalPrizeFish;
        public int LargestFish;
        public Fishing() { }

        public void AddExistingFish(GameSession session, FishMetadata fish, int size)
        {
            session.Player.FishAlbum[fish.Id].TotalCaught += 1;

            if (size > session.Player.FishAlbum[fish.Id].LargestFish)
            {
                session.Player.FishAlbum[fish.Id].LargestFish = size;
            }
            // TODO: Lower exp chance if player's fishing exp is higher than FishingSpot maxMastery
            AddMastery(session, fish, false);

            if (size >= fish.BigSize[0])
            {
                session.Player.FishAlbum[fish.Id].LargestFish = size;
            }
        }

        public void AddNewFish(GameSession session, FishMetadata fish, int size)
        {
            session.Player.FishAlbum[fish.Id].FishId = fish.Id;
            session.Player.FishAlbum[fish.Id].TotalCaught = 1;
            session.Player.FishAlbum[fish.Id].LargestFish = size;

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

            Random rnd = new Random();
            int expChance = rnd.Next(0, 100);
            if (expChance <= 10)
            {
                int exp = rnd.Next(1, 3);
                session.Player.Levels.GainMasteryExp(MasteryType.Fishing, exp);
                session.Send(FishingPacket.IncreaseMastery(MasteryType.Fishing, fish.Id, exp));
            }
        }
    }
}
