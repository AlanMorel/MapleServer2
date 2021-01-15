using System;
using Ms2Database.DbClasses;
using Ms2Database.Controllers;

namespace Ms2Database
{
    class Program
    {
        static void Main(string[] args)
        {
            AccountManager AccountManage = new AccountManager();
            CharacterManager CharacterManage = new CharacterManager();

            AccountManage.CreateAccount("localhost", "");
            CharacterManage.CreateCharacter(1, "Char2", 15);
        }
    }
}
