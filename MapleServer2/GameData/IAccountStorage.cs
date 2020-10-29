using System.Collections.Generic;
using Maple2.Data.Types;

namespace MapleServer2.GameData {
    // Interface for retrieving account data
    public interface IAccountStorage {

        // Retrieves a list of character ids for an account
        public List<long> ListCharacters(long accountId);

        // Retrieves a specific character for an account
        public Character GetCharacter(long characterId);

        public bool SaveCharacter(long characterId, Character data);
    }
}