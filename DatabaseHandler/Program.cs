using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace DatabaseHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            using(Maplestory2DBEntities context = new Maplestory2DBEntities()) 
            {
                Character character = new Character() 
                {
                    Account_ID = 1,
                    Name = "DBCharTest",
                    Gender = 0,
                    Level = 70,
                    Job_ID = 50,
                    Map_ID = 2000062
                };
                context.Characters.Add(character);
                context.SaveChanges();
            }
        }
    }
}
