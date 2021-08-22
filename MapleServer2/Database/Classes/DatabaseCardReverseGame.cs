using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseCardReverseGame
    {
        private readonly string TableName = "CardReverseGame";

        public long CreateCardReverseGame(CardReverseGame cardReverseGame)
        {
            return DatabaseManager.QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                cardReverseGame.ItemId,
                cardReverseGame.ItemRarity,
                cardReverseGame.ItemAmount
            });
        }

        public List<CardReverseGame> FindAll() => DatabaseManager.QueryFactory.Query(TableName).Get<CardReverseGame>().ToList();

        public void Update(CardReverseGame cardReverseGame)
        {
            DatabaseManager.QueryFactory.Query(TableName).Where("Id", cardReverseGame.Id).Update(new
            {
                cardReverseGame.ItemId,
                cardReverseGame.ItemRarity,
                cardReverseGame.ItemAmount
            });
        }

        public bool Delete(long id) => DatabaseManager.QueryFactory.Query(TableName).Where("Id", id).Delete() == 1;
    }
}
