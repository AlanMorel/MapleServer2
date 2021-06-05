using System.Collections.Generic;
using MapleServer2.Types;

namespace MapleServer2.Database
{
    public static class MapleopolySeeding
    {
        public static void Seed()
        {
            List<MapleopolyTile> tiles = new List<MapleopolyTile>
            {
                new MapleopolyTile
                {
                    TilePosition = 1,
                    Type = MapleopolyTileType.Start
                },
                new MapleopolyTile
                {
                    TilePosition = 2,
                    Type = MapleopolyTileType.Item,
                    ItemId = 20001130,
                    ItemRarity = 1,
                    ItemAmount = 10
                },
                new MapleopolyTile
                {
                    TilePosition = 3,
                    Type = MapleopolyTileType.Item,
                    ItemId = 20301360,
                    ItemRarity = 3,
                    ItemAmount = 3
                },
                new MapleopolyTile
                {
                    TilePosition = 4,
                    Type = MapleopolyTileType.MoveForward,
                    TileParameter = 5
                },
                new MapleopolyTile
                {
                    TilePosition = 5,
                    Type = MapleopolyTileType.Item,
                    ItemId = 40100023,
                    ItemRarity = 1,
                    ItemAmount = 300
                },
                new MapleopolyTile
                {
                    TilePosition = 6,
                    Type = MapleopolyTileType.Item,
                    ItemId = 20000279,
                    ItemRarity = 1,
                    ItemAmount = 2
                },
                new MapleopolyTile
                {
                    TilePosition = 7,
                    Type = MapleopolyTileType.Item,
                    ItemId = 20301360,
                    ItemRarity = 3,
                    ItemAmount = 3
                },
                new MapleopolyTile
                {
                    TilePosition = 8,
                    Type = MapleopolyTileType.TreasureTrove,
                    ItemId = 20000569,
                    ItemRarity = 1,
                    ItemAmount = 1
                },
                new MapleopolyTile
                {
                    TilePosition = 9,
                    Type = MapleopolyTileType.Item,
                    ItemId = 40100036,
                    ItemRarity = 1,
                    ItemAmount = 300
                },
                new MapleopolyTile
                {
                    TilePosition = 10,
                    Type = MapleopolyTileType.Item,
                    ItemId = 40100037,
                    ItemRarity = 1,
                    ItemAmount = 300
                },
                new MapleopolyTile
                {
                    TilePosition = 11,
                    Type = MapleopolyTileType.GoToStart
                },
                new MapleopolyTile
                {
                    TilePosition = 12,
                    Type = MapleopolyTileType.Item,
                    ItemId = 20001130,
                    ItemRarity = 1,
                    ItemAmount = 10
                },
                new MapleopolyTile
                {
                    TilePosition = 13,
                    Type = MapleopolyTileType.Item,
                    ItemId = 20300855,
                    ItemRarity = 1,
                    ItemAmount = 1
                },
                new MapleopolyTile
                {
                    TilePosition = 14,
                    Type = MapleopolyTileType.Item,
                    ItemId = 20301493,
                    ItemRarity = 1,
                    ItemAmount = 5
                },
                new MapleopolyTile
                {
                    TilePosition = 15,
                    Type = MapleopolyTileType.RoundTrip
                },
                new MapleopolyTile
                {
                    TilePosition = 16,
                    Type = MapleopolyTileType.Item,
                    ItemId = 40100037,
                    ItemRarity = 1,
                    ItemAmount = 300
                },
                new MapleopolyTile
                {
                    TilePosition = 17,
                    Type = MapleopolyTileType.Backtrack,
                    TileParameter = 5
                },
                new MapleopolyTile
                {
                    TilePosition = 18,
                    Type = MapleopolyTileType.Item,
                    ItemId = 20302777,
                    ItemRarity = 1,
                    ItemAmount = 10
                },
                new MapleopolyTile
                {
                    TilePosition = 19,
                    Type = MapleopolyTileType.Item,
                    ItemId = 40100036,
                    ItemRarity = 1,
                    ItemAmount = 300
                },
                new MapleopolyTile
                {
                    TilePosition = 20,
                    Type = MapleopolyTileType.Item,
                    ItemId = 20000280,
                    ItemRarity = 1,
                    ItemAmount = 2
                },
                new MapleopolyTile
                {
                    TilePosition = 21,
                    Type = MapleopolyTileType.MoveForward,
                    TileParameter = 3
                },
                new MapleopolyTile
                {
                    TilePosition = 22,
                    Type = MapleopolyTileType.TreasureTrove,
                    ItemId = 20000569,
                    ItemRarity = 1,
                    ItemAmount = 1
                },
                new MapleopolyTile
                {
                    TilePosition = 23,
                    Type = MapleopolyTileType.Item,
                    ItemId = 40100037,
                    ItemRarity = 1,
                    ItemAmount = 300
                },
                new MapleopolyTile
                {
                    TilePosition = 24,
                    Type = MapleopolyTileType.Item,
                    ItemId = 20001130,
                    ItemRarity = 1,
                    ItemAmount = 10
                },
                new MapleopolyTile
                {
                    TilePosition = 25,
                    Type = MapleopolyTileType.Item,
                    ItemId = 20300078,
                    ItemRarity = 1,
                    ItemAmount = 10
                },
                new MapleopolyTile
                {
                    TilePosition = 26,
                    Type = MapleopolyTileType.Backtrack,
                    TileParameter = 3
                },
                new MapleopolyTile
                {
                    TilePosition = 27,
                    Type = MapleopolyTileType.Item,
                    ItemId = 20301360,
                    ItemRarity = 3,
                    ItemAmount = 5
                },
                new MapleopolyTile
                {
                    TilePosition = 28,
                    Type = MapleopolyTileType.Item,
                    ItemId = 40100037,
                    ItemRarity = 1,
                    ItemAmount = 300
                },
            };

            DatabaseManager.InsertMapleopoly(tiles);
        }
    }
}
