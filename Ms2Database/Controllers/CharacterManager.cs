using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ms2Database.DbClasses;

namespace Ms2Database.Controllers
{
    public class CharacterManager
    {
        public void CreateCharacter(long accId, string charName, int jobId) // Creates Character and Inventory
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Character character = new Character() // Set default values here (Refer to character.cs for column names)
                {
                    AccountId = accId,
                    Name = charName,
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

                character = context.Characters.First(c => c.Name.ToLower() == charName.ToLower());
                inventory.CreateInventory(character.CharacterId);
            }
        }

        public void DeleteCharacter(long charId, string charName = "")
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Character character = context.Characters.FirstOrDefault(a => (a.CharacterId == charId) || (a.Name.ToLower() == charName.ToLower()));
                context.Remove(character);
                context.SaveChanges();
            }
        }

        public Character GetCharInfo(long charId)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Character character = context.Characters.FirstOrDefault(a => a.CharacterId == charId);
                return character;
            }
        }

        public void EditCharInfo(Character character)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Character OriginCharacter = character;
                context.SaveChanges();
            }
        }
    }
}
