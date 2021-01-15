using System;
using Ms2Database.DbClasses;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms2Database.Controllers
{
    public class CharacterManager
    {
        public void CreateCharacter(long accId, string charName, int jobId)
        {
            using(Ms2DbContext context = new Ms2DbContext())
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

        public void SetCharInfo(Character CharIn)
        {
            using (Ms2DbContext context = new Ms2DbContext())
            {
                Character character = CharIn;
                context.SaveChanges();
            }
        }
    }
}
