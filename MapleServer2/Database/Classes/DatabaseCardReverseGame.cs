using MapleServer2.Database.Types;
using SqlKata.Execution;

namespace MapleServer2.Database.Classes
{
    public class DatabaseCardReverseGame : DatabaseTable
    {
        public DatabaseCardReverseGame() : base("card_reverse_game") { }

        public long Insert(CardReverseGame cardReverseGame)
        {
            return QueryFactory.Query(TableName).InsertGetId<long>(new
            {
                item_id = cardReverseGame.ItemId,
                item_rarity = cardReverseGame.ItemRarity,
                item_amount = cardReverseGame.ItemAmount
            });
        }

        public List<CardReverseGame> FindAll() => QueryFactory.Query(TableName).Get<CardReverseGame>().ToList();

        public void Update(CardReverseGame cardReverseGame)
        {
            QueryFactory.Query(TableName).Where("id", cardReverseGame.Id).Update(new
            {
                item_id = cardReverseGame.ItemId,
                item_rarity = cardReverseGame.ItemRarity,
                item_amount = cardReverseGame.ItemAmount
            });
        }

        public bool Delete(long id) => QueryFactory.Query(TableName).Where("id", id).Delete() == 1;
    }
}
