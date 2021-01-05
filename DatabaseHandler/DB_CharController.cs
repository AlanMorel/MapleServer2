using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseHandler
{
    public class DB_CharController
    {
        public void CreateChar(long AccID, string name, byte gender, int JobId, int MapId)
        {
            using (Maplestory2DBEntities context = new Maplestory2DBEntities())
            {
                Character character = new Character
                {
                    Account_ID = AccID,
                    Name = name,
                    Gender = gender,
                    Level = (short) JobId,
                    Job_ID = (short) JobId,
                    Map_ID = MapId
                };
            }
        }

        public Character GetCharByID(long charId)
        {
            using (Maplestory2DBEntities context = new Maplestory2DBEntities())
            {
                Character charInfo = context.Characters.FirstOrDefault(r => r.Character_ID == charId);
                return charInfo;
            }
        }
    }
}
