using System.Collections.Generic;
using Maple2Storage.Types;
using MapleServer2.Types;
using MapleServer2.Database;

namespace MapleServer2.Data.Static {
    public class StaticAccountStorage : IAccountStorage {
        private readonly Dictionary<long, List<long>> accountCharacters = new Dictionary<long, List<long>>();
        private readonly Dictionary<long, Player> characters = new Dictionary<long, Player>();

        public const long DEFAULT_ACCOUNT = 0x1111111111111111;
        public const long DEFAULT_CHARACTER = 0x7777777777777777;
        public const long DEFAULT_CHARACTER2 = 0x1212121212121212;
        public const int DEFAULT_MAPID = 2000062;

        public const long SECONDARY_ACCOUNT = 0x2222222222222222;
        public const long SECONDARY_CHARACTER1 = 0x5555555555555555;
        public const long SECONDARY_CHARACTER2 = 0x6666666666666666;
        public const long SECONDARY_CHARACTER3 = 0x6767676767676767;

        public StaticAccountStorage() {
            // Connection to DB
            /*DBConnect dBConnect = new DBConnect();
            DEFAULT_MAPID = dBConnect.Select();*/

            // Default Account
            accountCharacters.Add(DEFAULT_ACCOUNT, new List<long> { DEFAULT_CHARACTER, DEFAULT_CHARACTER2 });
            characters[DEFAULT_CHARACTER] = Player.Default(DEFAULT_ACCOUNT, DEFAULT_CHARACTER, DEFAULT_MAPID);
            characters[DEFAULT_CHARACTER2] = Player.MaleDefault(DEFAULT_ACCOUNT, DEFAULT_CHARACTER2, DEFAULT_MAPID);

            // Secondary Account, has a character at tutorial
            accountCharacters.Add(SECONDARY_ACCOUNT, new List<long> { SECONDARY_CHARACTER1, SECONDARY_CHARACTER2 });
            characters[SECONDARY_CHARACTER1] = Player.Default(SECONDARY_ACCOUNT, SECONDARY_CHARACTER1, DEFAULT_MAPID, "Name2");
            Player tutorialChar = Player.Default(SECONDARY_ACCOUNT, SECONDARY_CHARACTER2, DEFAULT_MAPID, "Noob");
            tutorialChar.MapId = 52000065; // Tutorial
            tutorialChar.Coord = CoordF.From(-675, 525, 600);
            characters[SECONDARY_CHARACTER2] = tutorialChar;
            Player tutorialChar2 = Player.Default(SECONDARY_ACCOUNT, SECONDARY_CHARACTER3, DEFAULT_MAPID, "Wizard");
            tutorialChar2.MapId = 52000101; // Tutorial
            tutorialChar2.Coord = CoordF.From(-1350, -1050, 1650);
            characters[SECONDARY_CHARACTER3] = tutorialChar2;
        }

        public List<long> ListCharacters(long accountId) {
            return accountCharacters.GetValueOrDefault(accountId, new List<long>());
        }

        public Player GetCharacter(long characterId) {
            return characters.GetValueOrDefault(characterId);
        }

        // Cannot save to static storage
        public bool SaveCharacter(long characterId, Player data) {
            return false;
        }
    }
}