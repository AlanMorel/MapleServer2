namespace MapleServer2.Types
{
    public class Mapleopoly
    {
        public int FreeRollAmount;
        public int TotalTileCount;
        public int TotalTrips;

        public const int TILE_AMOUNT = 28;
        // Temporarily hardcoding the item and cost
        public const int TOKEN_ITEM_ID = 30001445; // Aetherine Coin
        public const int TOKEN_COST = 2;

        public Mapleopoly() { }
    }

    public class MapleopolyTile
    {
        public int TilePosition;
        public MapleopolyTileType Type;
        public int TileParameter;
        public int ItemId;
        public byte ItemRarity;
        public int ItemAmount;

        public MapleopolyTile() { }
    }

    public enum MapleopolyTileType : short
    {
        Item = 0x0,
        Lose = 0x1,
        Backtrack = 0x2,
        MoveForward = 0x3,
        GoToStart = 0x4,
        RollAgain = 0x5,
        Start = 0x6,
        Trap = 0x7,
        RoundTrip = 0x8,
        TreasureTrove = 0x9,
    }
}
