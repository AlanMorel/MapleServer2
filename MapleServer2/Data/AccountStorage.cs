using System.Collections.Generic;
using MapleServer2.Types;
using Maple2Storage.Types;

namespace MapleServer2.Data {
    // Class for retrieving and storing account data
    public static class AccountStorage {
        // Temp account and character ids
        public const long DEFAULT_ACCOUNT = 0x1111111111111111;
        public const long DEFAULT_CHAR1 = 0x7777777777777777;
        public const long DEFAULT_CHAR2 = 0x1212121212121212;

        public const long SECONDARY_ACCOUNT = 0x2222222222222222;

        // Dictionary of character ids for the account
        public static Dictionary<long, List<long>> accountCharacters = new Dictionary<long, List<long>>();

        // Dictionary of characters for the account
        public static Dictionary<long, Player> characters = new Dictionary<long, Player>();

        static AccountStorage () {
            // Add temp characters
            accountCharacters.Add(DEFAULT_ACCOUNT, new List<long> { DEFAULT_CHAR1, DEFAULT_CHAR2 });
            characters.Add(DEFAULT_CHAR1, Player.Default(DEFAULT_ACCOUNT, DEFAULT_CHAR1));
            characters.Add(DEFAULT_CHAR2, Player.MaleDefault(DEFAULT_ACCOUNT, DEFAULT_CHAR2));
        }

        // Retrieves a list of character ids for an account
        public static List<long> ListCharacters(long accountId) {
            return accountCharacters.GetValueOrDefault(accountId, new List<long>());
        }

        // Adds new character
        public static bool AddCharacter(Player data) {
            characters.Add(data.CharacterId, data);
            return true;
        }

        // Retrieves a specific character for an account
        public static Player GetCharacter(long characterId) {
            return characters.GetValueOrDefault(characterId);
        }

        // Updates a character
        public static bool UpdateCharacter(Player data) {
            characters.Remove(data.CharacterId);
            characters.Add(data.CharacterId, data);
            return true;
        }
    }
}