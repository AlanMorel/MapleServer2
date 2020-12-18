using System.Collections.Generic;
using MapleServer2.Types;

namespace MapleServer2.Data {
    // Class for retrieving and storing account data
    public static class AccountStorage {
        // Temp account and character ids
        public const long DEFAULT_ACCOUNT_ID = 1;

        public static int TickCount = 0;

        // Dictionary of character ids for the account
        public static Dictionary<long, List<long>> accountCharacters = new Dictionary<long, List<long>>();

        // Dictionary of characters for the account
        public static Dictionary<long, Player> characters = new Dictionary<long, Player>();

        static AccountStorage() {
            // Add temp characters
            long defaultCharId1 = 1;
            long defaultCharId2 = 2;

            accountCharacters.Add(DEFAULT_ACCOUNT_ID, new List<long> { defaultCharId1, defaultCharId2 });

            characters.Add(defaultCharId1, Player.Char1(DEFAULT_ACCOUNT_ID, defaultCharId1));
            characters.Add(defaultCharId2, Player.Char2(DEFAULT_ACCOUNT_ID, defaultCharId2));
        }

        // Retrieves a list of character ids for an account
        public static List<long> ListCharacters(long accountId) {
            return accountCharacters.GetValueOrDefault(accountId, new List<long>());
        }

        // Adds new character
        public static void AddCharacter(Player data) {
            characters.Add(data.CharacterId, data);
        }

        // Retrieves a specific character for an account
        public static Player GetCharacter(long characterId) {
            return characters.GetValueOrDefault(characterId);
        }

        // Updates a character
        public static void UpdateCharacter(Player data) {
            characters.Remove(data.CharacterId);
            characters.Add(data.CharacterId, data);
        }
    }
}