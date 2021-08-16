using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseCardReverseGame
    {
        public static long CreateCardReverseGame(CardReverseGame cardReverseGame)
        {
            return DatabaseManager.QueryFactory.Query("CardReverseGame").InsertGetId<long>(new
            {
                cardReverseGame.ItemId,
                cardReverseGame.ItemRarity,
                cardReverseGame.ItemAmount
            });
        }

        public static List<CardReverseGame> FindAll() => DatabaseManager.QueryFactory.Query("cardreversegame").Get<CardReverseGame>().ToList();

        public static void Update(CardReverseGame cardReverseGame)
        {
            DatabaseManager.QueryFactory.Query("CardReverseGame").Where("Id", cardReverseGame.Id).Update(new
            {
                cardReverseGame.ItemId,
                cardReverseGame.ItemRarity,
                cardReverseGame.ItemAmount
            });
        }

        public static bool Delete(long id) => DatabaseManager.QueryFactory.Query("CardReverseGame").Where("Id", id).Delete() == 1;
    }
}
