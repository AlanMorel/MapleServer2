using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ms2Database.DbClasses;
using Ms2Database.Controllers;

namespace Ms2Database
{
    public class Program
    {
        static void Main(string[] args)
        {
            AccountManager accmanage = new AccountManager();
            CharacterManager charmanage = new CharacterManager();

            accmanage.CreateAccount("localhost", ""); // Creates Account (username, password)
            charmanage.CreateCharacter(1, "Char1", 50); // Creates character (accId, name, jobid)

            Console.WriteLine("Database has been created");
            Console.WriteLine("Test Data has been loaded");
        }
    }
}
