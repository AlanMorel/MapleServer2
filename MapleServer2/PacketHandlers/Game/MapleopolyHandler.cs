using Maple2Storage.Tools;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class MapleopolyHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.MAPLEOPOLY;

        public MapleopolyHandler() : base() { }

        private enum MapleopolyMode : byte
        {
            Open = 0x0,
            Roll = 0x1,
            ProcessTile = 0x3,
        }

        private enum MapleopolyNotice : byte
        {
            NotEnoughTokens = 0x1,
            DiceAlreadyRolled = 0x4,
            YouCannotRollRightNow = 0x5,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            MapleopolyMode mode = (MapleopolyMode) packet.ReadByte();

            switch (mode)
            {
                case MapleopolyMode.Open:
                    HandleOpen(session);
                    break;
                case MapleopolyMode.Roll:
                    HandleRoll(session);
                    break;
                case MapleopolyMode.ProcessTile:
                    HandleProcessTile(session);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleOpen(GameSession session)
        {
            List<MapleopolyTile> tiles = DatabaseManager.Mapleopoly.FindAllTiles();

            int tokenAmount = 0;
            Item token = session.Player.Inventory.Items.FirstOrDefault(x => x.Value.Id == Mapleopoly.TOKEN_ITEM_ID).Value;
            if (token != null)
            {
                tokenAmount = token.Amount;
            }
            session.Send(MapleopolyPacket.Open(session.Player.Mapleopoly, tiles, tokenAmount));
        }

        private static void HandleRoll(GameSession session)
        {
            // Check if player can roll
            Item token = session.Player.Inventory.Items.FirstOrDefault(x => x.Value.Id == Mapleopoly.TOKEN_ITEM_ID).Value;

            if (session.Player.Mapleopoly.FreeRollAmount > 0)
            {
                session.Player.Mapleopoly.FreeRollAmount--;
            }
            else if (token != null && token.Amount >= Mapleopoly.TOKEN_COST)
            {
                session.Player.Inventory.ConsumeItem(session, token.Uid, Mapleopoly.TOKEN_COST);
            }
            else
            {
                session.Send(MapleopolyPacket.Notice((byte) MapleopolyNotice.NotEnoughTokens));
                return;
            }

            Random rnd = RandomProvider.Get();

            // roll two dice
            int roll1 = rnd.Next(1, 6);
            int roll2 = rnd.Next(1, 6);
            int totalRoll = roll1 + roll2;

            session.Player.Mapleopoly.TotalTileCount += totalRoll;
            if (roll1 == roll2)
            {
                session.Player.Mapleopoly.FreeRollAmount++;
            }
            session.Send(MapleopolyPacket.Roll(session.Player.Mapleopoly.TotalTileCount, roll1, roll2));
        }

        private static void HandleProcessTile(GameSession session)
        {
            int currentTilePosition = session.Player.Mapleopoly.TotalTileCount % Mapleopoly.TILE_AMOUNT;

            MapleopolyTile currentTile = DatabaseManager.Mapleopoly.FindTileByPosition(currentTilePosition + 1);

            switch (currentTile.Type)
            {
                case MapleopolyTileType.Item:
                case MapleopolyTileType.TreasureTrove:
                    Item item = new Item(currentTile.ItemId)
                    {
                        Amount = currentTile.ItemAmount,
                        Rarity = currentTile.ItemRarity
                    };
                    session.Player.Inventory.AddItem(session, item, true);
                    break;
                case MapleopolyTileType.Backtrack:
                    session.Player.Mapleopoly.TotalTileCount -= currentTile.TileParameter;
                    break;
                case MapleopolyTileType.MoveForward:
                    session.Player.Mapleopoly.TotalTileCount += currentTile.TileParameter;
                    break;
                case MapleopolyTileType.RoundTrip:
                    session.Player.Mapleopoly.TotalTileCount += Mapleopoly.TILE_AMOUNT;
                    break;
                case MapleopolyTileType.GoToStart:
                    int tileToStart = Mapleopoly.TILE_AMOUNT - currentTilePosition;
                    session.Player.Mapleopoly.TotalTileCount += tileToStart;
                    break;
                case MapleopolyTileType.Start:
                    break;
                default:
                    Logger.Warn("Unsupported tile");
                    break;
            }

            ProcessTrip(session); // Check if player passed Start
            session.Send(MapleopolyPacket.ProcessTile(session.Player.Mapleopoly, currentTile));
        }

        private static void ProcessTrip(GameSession session)
        {
            int newTotalTrips = session.Player.Mapleopoly.TotalTileCount / Mapleopoly.TILE_AMOUNT;
            if (newTotalTrips <= session.Player.Mapleopoly.TotalTrips)
            {
                return;
            }

            int difference = newTotalTrips - session.Player.Mapleopoly.TotalTrips;

            List<MapleopolyEvent> items = DatabaseManager.Events.FindAllMapleopolyEvents();
            for (int i = 0; i < difference; i++)
            {
                session.Player.Mapleopoly.TotalTrips++;

                // Check if there's any item to give for every 1 trip
                MapleopolyEvent mapleopolyItem1 = items.FirstOrDefault(x => x.TripAmount == 0);
                if (mapleopolyItem1 != null)
                {
                    Item item1 = new Item(mapleopolyItem1.ItemId)
                    {
                        Amount = mapleopolyItem1.ItemAmount,
                        Rarity = mapleopolyItem1.ItemRarity
                    };
                    session.Player.Inventory.AddItem(session, item1, true);
                }

                // Check if there's any other item to give for hitting a specific number of trips
                MapleopolyEvent mapleopolyItem2 = items.FirstOrDefault(x => x.TripAmount == session.Player.Mapleopoly.TotalTrips);
                if (mapleopolyItem2 == null)
                {
                    continue;
                }
                Item item2 = new Item(mapleopolyItem2.ItemId)
                {
                    Amount = mapleopolyItem2.ItemAmount,
                    Rarity = mapleopolyItem2.ItemRarity
                };
                session.Player.Inventory.AddItem(session, item2, true);
            }
        }
    }
}
