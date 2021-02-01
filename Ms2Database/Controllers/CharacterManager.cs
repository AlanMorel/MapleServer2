using System.Linq;
using Ms2Database.DbClasses;

namespace Ms2Database.Controllers
{
    public class CharacterManager
    {
        public static void CreateCharacter(long accountId, string characterName, int jobId) // Creates Character and Inventory
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Character character = new Character() // Set default values here (Refer to character.cs for column names)
                {
                    AccountId = accountId,
                    Name = characterName,
                    JobId = jobId,
                    MapId = 2000062,
                    CoordX = 2850,
                    CoordY = 2550,
                    CoordZ = 1800,
                    Level = 70,
                    PrestigeLvl = 100,
                    Motto = "This works!",
                    HomeName = "My Home Works!",
                    InsigniaId = 29,
                    TitleId = 10000292
                };
                context.Characters.Add(character);
                context.SaveChanges();

                InventoryManager inventory = new InventoryManager();

                character = context.Characters.First(c => c.Name.ToLower() == characterName.ToLower());
                InventoryManager.CreateInventory(character.CharacterId);
            }
        }

        public static void DeleteCharacter(long characterId, string characterName = "")
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Character character = context.Characters.FirstOrDefault(a => (a.CharacterId == characterId) || (a.Name.ToLower() == characterName.ToLower()));
                context.Remove(character);
                context.SaveChanges();
            }
        }

        public static Character GetCharInfo(long characterId)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Character character = context.Characters.FirstOrDefault(a => a.CharacterId == characterId);
                return character;
            }
        }

        public static void EditCharInfo(/*Character character*/)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                context.SaveChanges();
            }
        }
    }
}
