using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ms2Database.DbClasses;

namespace Ms2Database.Controllers
{
    public class CharacterManager
    {
        public void CreateCharacter(long accountId, string characterName, int jobId) // Creates Character and Inventory
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                Character Character = new Character() // Set default values here (Refer to character.cs for column names)
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
                Context.Characters.Add(Character);
                Context.SaveChanges();

                InventoryManager Inventory = new InventoryManager();

                Character = Context.Characters.First(c => c.Name.ToLower() == characterName.ToLower());
                Inventory.CreateInventory(Character.CharacterId);
            }
        }

        public void DeleteCharacter(long characterId, string characterName = "")
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                Character Character = Context.Characters.FirstOrDefault(a => (a.CharacterId == characterId) || (a.Name.ToLower() == characterName.ToLower()));
                Context.Remove(Character);
                Context.SaveChanges();
            }
        }

        public Character GetCharInfo(long characterId)
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                Character Character = Context.Characters.FirstOrDefault(a => a.CharacterId == characterId);
                return Character;
            }
        }

        public void EditCharInfo(Character character)
        {
            using (Ms2DbContext Context = new Ms2DbContext())
            {
                Character Character = character;
                Context.SaveChanges();
            }
        }
    }
}
