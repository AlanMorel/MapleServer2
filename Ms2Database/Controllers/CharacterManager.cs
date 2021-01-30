using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
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

                character = context.Characters.First(column => column.Name.ToLower() == characterName.ToLower());
                inventory.CreateInventory(character.CharacterId);
            }
        }

        public void DeleteCharacter(long characterId)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Character character = context.Characters.FirstOrDefault(column => column.CharacterId == characterId);
                context.Remove(character);
                context.SaveChanges();
            }
        }

        public Character GetCharacterInfo(long characterId)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Character character = context.Characters.FirstOrDefault(column => column.CharacterId == characterId);
                return character;
            }
        }

        public void UpdateCharInfo(Character characterObject)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Character character = characterObject;
                context.SaveChanges();
            }
        }

        public List<Character> GetCharacterList(long accountId)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                List<Character> characters = context.Characters.Include(table => table.Account)
                                                               .Where(column => column.Account.AccountId == accountId)
                                                               .ToList();
                return characters;
            }
        }
    }
}
