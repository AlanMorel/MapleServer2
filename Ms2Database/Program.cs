using Ms2Database.Controllers;

namespace Ms2Database
{
    class Program
    {
        static void Main(/*string[] args*/)
        {
            /*
            AccountManager accountManage = new AccountManager();
            CharacterManager characterManage = new CharacterManager();
            */

            AccountManager.CreateAccount("localhost", "");
            CharacterManager.CreateCharacter(1, "Char1", 15);
        }
    }
}
