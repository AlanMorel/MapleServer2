using System.Collections.Generic;
using Maple2Storage.Types;
using MapleServer2.Database;
using Maple2.Data.Types;

namespace MapleServer2.GameData.Static {
    public class StaticAccountStorage : IAccountStorage {
        private readonly Dictionary<long, List<long>> accountCharacters = new Dictionary<long, List<long>>();
        private readonly Dictionary<long, Character> characters = new Dictionary<long, Character>();

        public const long DEFAULT_ACCOUNT = 0x1111111111111111;
        public const long DEFAULT_CHARACTER = 0x7777777777777777;
        public int DEFAULT_MAPID = 0;

        public const long SECONDARY_ACCOUNT = 0x2222222222222222;
        public const long SECONDARY_CHARACTER1 = 0x5555555555555555;
        public const long SECONDARY_CHARACTER2 = 0x6666666666666666;
        public const long SECONDARY_CHARACTER3 = 0x6767676767676767;

        public StaticAccountStorage() {
            // Connection to DB
            DBConnect dBConnect = new DBConnect();
            DEFAULT_MAPID = dBConnect.Select();


            // Default Account
            accountCharacters.Add(DEFAULT_ACCOUNT, new List<long> { DEFAULT_CHARACTER });
            characters[DEFAULT_CHARACTER] = Character.Default(DEFAULT_CHARACTER, Maple2Storage.Enums.JobType.Archer ,"Spark");


        }

        public List<long> ListCharacters(long accountId) {
            return accountCharacters.GetValueOrDefault(accountId, new List<long>());
        }

        public Character GetCharacter(long characterId) {
            return characters.GetValueOrDefault(characterId);
        }

        // Cannot save to static storage
        public bool SaveCharacter(long characterId, Character data) {
            return false;
        }
    }
}