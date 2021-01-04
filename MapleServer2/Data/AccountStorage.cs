using System.Collections.Generic;
using MapleServer2.Types;

namespace MapleServer2.Data
{
    // Class for retrieving and storing account data
    public static class AccountStorage
    {
        // Temp account and character ids
        public const long DEFAULT_ACCOUNT_ID = 1;

        public static int TickCount = 0;

        public static readonly Dictionary<long, List<long>> AccountCharacters = new();
        public static readonly Dictionary<long, Player> Characters = new();

        static AccountStorage()
        {
            // Add temp characters
            long defaultCharId1 = 1;
            long defaultCharId2 = 2;

            AccountCharacters.Add(DEFAULT_ACCOUNT_ID, new List<long> { defaultCharId1, defaultCharId2 });

            Characters.Add(defaultCharId1, Player.Char1(DEFAULT_ACCOUNT_ID, defaultCharId1));
            Characters.Add(defaultCharId2, Player.Char2(DEFAULT_ACCOUNT_ID, defaultCharId2));
        }

        // Retrieves a list of character ids for an account
        public static List<long> ListCharacters(long accountId)
        {
            return AccountCharacters.GetValueOrDefault(accountId, new List<long>());
        }

        // Adds new character
        public static void AddCharacter(Player data)
        {
            Characters.Add(data.CharacterId, data);
        }

        // Retrieves a specific character for an account
        public static Player GetCharacter(long characterId)
        {
            return Characters.GetValueOrDefault(characterId);
        }

        // Updates a character
        public static void UpdateCharacter(Player data)
        {
            Characters.Remove(data.CharacterId);
            Characters.Add(data.CharacterId, data);
        }
    }
}
