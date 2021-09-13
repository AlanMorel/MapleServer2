using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseCardReverseGame : DatabaseTable
    {
        public DatabaseCardReverseGame() : base("cardreversegame") { }

        public long Insert(CardReverseGame cardReverseGame)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                cardReverseGame.ItemId,
                cardReverseGame.ItemRarity,
                cardReverseGame.ItemAmount
            });
        }

        public List<CardReverseGame> FindAll() => QueryFactory.Query(TableName).Get<CardReverseGame>().ToList();

        public void Update(CardReverseGame cardReverseGame)
        {
            QueryFactory.Query(TableName).Where("id", cardReverseGame.Id).Update(new
            {
                cardReverseGame.ItemId,
                cardReverseGame.ItemRarity,
                cardReverseGame.ItemAmount
            });
        }

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }
}
