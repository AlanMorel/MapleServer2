using System.Collections.Generic;
using MapleServer2.Database.Types;

namespace MapleServer2.Database
{
    public static class CardReverseGameSeeding
    {
        public static void Seed()
        {
            List<CardReverseGame> cards = new List<CardReverseGame>
            {
                new CardReverseGame
                {
                    ItemId = 30000946,
                    ItemAmount = 1,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 30000937,
                    ItemAmount = 1,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20000258,
                    ItemAmount = 1,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 40100001,
                    ItemAmount = 300,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 40100042,
                    ItemAmount = 200,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20300312,
                    ItemAmount = 1,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20001138,
                    ItemAmount = 5,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 40100024,
                    ItemAmount = 30,
                    ItemRarity = 4
                },
                new CardReverseGame
                {
                    ItemId = 20000589,
                    ItemAmount = 1,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20000671,
                    ItemAmount = 5,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20301360,
                    ItemAmount = 5,
                    ItemRarity = 3
                },
                new CardReverseGame
                {
                    ItemId = 20301498,
                    ItemAmount = 5,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20300055,
                    ItemAmount = 1,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20300055,
                    ItemAmount = 1,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20300311,
                    ItemAmount = 1,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20300416,
                    ItemAmount = 3,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20300429,
                    ItemAmount = 4,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20300546,
                    ItemAmount = 4,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20000939,
                    ItemAmount = 20,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20001443,
                    ItemAmount = 1,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20001689,
                    ItemAmount = 10,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20001691,
                    ItemAmount = 3,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20001690,
                    ItemAmount = 3,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20300078,
                    ItemAmount = 10,
                    ItemRarity = 1
                },
                new CardReverseGame
                {
                    ItemId = 20300417,
                    ItemAmount = 3,
                    ItemRarity = 1
                },
            };

            DatabaseManager.InsertCardReverseGame(cards);
        }
    }
}
