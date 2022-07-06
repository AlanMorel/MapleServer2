using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Database;
using MapleServer2.Database.Types;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class MapleopolyHandler : GamePacketHandler<MapleopolyHandler>
{
    public override RecvOp OpCode => RecvOp.Mapleopoly;

    private enum Mode : byte
    {
        Open = 0x0,
        Roll = 0x1,
        ProcessTile = 0x3
    }

    private enum MapleopolyNotice : byte
    {
        NotEnoughTokens = 0x1,
        DiceAlreadyRolled = 0x4,
        YouCannotRollRightNow = 0x5
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        BlueMarble mapleopolyEvent = DatabaseManager.Events.FindMapleopolyEvent();
        if (mapleopolyEvent is null)
        {
            // TODO: Find an error packet to send if event is not active
            return;
        }

        GameEventUserValue totalTileValue = GameEventHelper.GetUserValue(session.Player, mapleopolyEvent.Id, mapleopolyEvent.EndTimestamp,
            GameEventUserValueType.MapleopolyTotalTileCount);
        GameEventUserValue freeRollValue = GameEventHelper.GetUserValue(session.Player, mapleopolyEvent.Id, mapleopolyEvent.EndTimestamp,
            GameEventUserValueType.MapleopolyFreeRollAmount);
        GameEventUserValue totalTripValue = GameEventHelper.GetUserValue(session.Player, mapleopolyEvent.Id, mapleopolyEvent.EndTimestamp,
            GameEventUserValueType.MapleopolyTotalTrips);

        switch (mode)
        {
            case Mode.Open:
                HandleOpen(session, totalTileValue, freeRollValue);
                break;
            case Mode.Roll:
                HandleRoll(session, totalTileValue, freeRollValue);
                break;
            case Mode.ProcessTile:
                HandleProcessTile(session, totalTileValue, freeRollValue, totalTripValue);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleOpen(GameSession session, GameEventUserValue totalTileValue, GameEventUserValue freeRollValue)
    {
        List<MapleopolyTile> tiles = DatabaseManager.Mapleopoly.FindAllTiles();
        if (tiles.Count == 0)
        {
            return;
        }

        int tokenAmount = 0;
        int tokenItemId = int.Parse(ConstantsMetadataStorage.GetConstant("MapleopolyTokenItemId"));
        Item token = session.Player.Inventory.GetById(tokenItemId);
        if (token != null)
        {
            tokenAmount = token.Amount;
        }

        int.TryParse(totalTileValue.EventValue, out int totalTiles);
        int.TryParse(freeRollValue.EventValue, out int freeRolls);
        session.Send(MapleopolyPacket.Open(totalTiles, freeRolls, tiles, tokenItemId, tokenAmount));
    }

    private static void HandleRoll(GameSession session, GameEventUserValue totalTileValue, GameEventUserValue freeRollValue)
    {
        // Check if player can roll
        int tokenItemId = int.Parse(ConstantsMetadataStorage.GetConstant("MapleopolyTokenItemId"));
        int tokenCost = int.Parse(ConstantsMetadataStorage.GetConstant("MapleopolyTokenCost"));

        Item token = session.Player.Inventory.GetById(tokenItemId);

        int.TryParse(freeRollValue.EventValue, out int freeRolls);

        if (freeRolls > 0)
        {
            freeRolls--;
        }
        else if (token != null && token.Amount >= tokenCost)
        {
            session.Player.Inventory.ConsumeItem(session, token.Uid, tokenCost);
        }
        else
        {
            session.Send(MapleopolyPacket.Notice((byte) MapleopolyNotice.NotEnoughTokens));
            return;
        }

        Random rnd = Random.Shared;

        // roll two dice
        int roll1 = rnd.Next(1, 6);
        int roll2 = rnd.Next(1, 6);
        int totalRoll = roll1 + roll2;

        int.TryParse(totalTileValue.EventValue, out int totalTiles);
        totalTiles += totalRoll;
        if (roll1 == roll2)
        {
            freeRolls++;
        }

        // update user event values
        freeRollValue.UpdateValue(session, freeRolls);
        totalTileValue.UpdateValue(session, totalTiles);

        session.Send(MapleopolyPacket.Roll(totalTiles, roll1, roll2));
    }

    private static void HandleProcessTile(GameSession session, GameEventUserValue totalTileValue, GameEventUserValue freeRollValue,
        GameEventUserValue totalTripValue)
    {
        int.TryParse(freeRollValue.EventValue, out int freeRolls);
        int.TryParse(totalTileValue.EventValue, out int totalTiles);
        int currentTilePosition = totalTiles % MapleopolyTile.TILE_AMOUNT;

        MapleopolyTile currentTile = DatabaseManager.Mapleopoly.FindTileByPosition(currentTilePosition + 1);

        switch (currentTile.Type)
        {
            case MapleopolyTileType.Item:
            case MapleopolyTileType.TreasureTrove:
                Item item = new(currentTile.ItemId, currentTile.ItemAmount, currentTile.ItemRarity);
                session.Player.Inventory.AddItem(session, item, true);
                break;
            case MapleopolyTileType.Backtrack:
                totalTiles -= currentTile.TileParameter;
                break;
            case MapleopolyTileType.MoveForward:
                totalTiles += currentTile.TileParameter;
                break;
            case MapleopolyTileType.RoundTrip:
                totalTiles += MapleopolyTile.TILE_AMOUNT;
                break;
            case MapleopolyTileType.GoToStart:
                int tileToStart = MapleopolyTile.TILE_AMOUNT - currentTilePosition;
                totalTiles += tileToStart;
                break;
            case MapleopolyTileType.Start:
                break;
            default:
                Logger.Warning("Unsupported tile");
                break;
        }

        ProcessTrip(session, totalTripValue, totalTiles); // Check if player passed Start
        totalTileValue.UpdateValue(session, totalTiles);
        session.Send(MapleopolyPacket.ProcessTile(totalTiles, freeRolls, currentTile));
    }

    private static void ProcessTrip(GameSession session, GameEventUserValue totalTripValue, int totalTiles)
    {
        int.TryParse(totalTripValue.EventValue, out int totalTrips);
        int newTotalTrips = totalTiles / MapleopolyTile.TILE_AMOUNT;
        if (newTotalTrips <= totalTrips)
        {
            return;
        }

        int difference = newTotalTrips - totalTrips;

        List<BlueMarbleReward> items = DatabaseManager.Events.FindMapleopolyEvent().Rewards;
        for (int i = 0; i < difference; i++)
        {
            totalTrips++;

            // Check if there's any item to give for every 1 trip
            BlueMarbleReward mapleopolyItem1 = items.FirstOrDefault(x => x.TripAmount == 0);
            if (mapleopolyItem1 != null)
            {
                Item item1 = new(mapleopolyItem1.ItemId, mapleopolyItem1.ItemAmount, mapleopolyItem1.ItemRarity);
                session.Player.Inventory.AddItem(session, item1, true);
            }

            // Check if there's any other item to give for hitting a specific number of trips
            BlueMarbleReward mapleopolyItem2 = items.FirstOrDefault(x => x.TripAmount == totalTrips);
            if (mapleopolyItem2 == null)
            {
                continue;
            }

            Item item2 = new(mapleopolyItem2.ItemId, mapleopolyItem2.ItemAmount, mapleopolyItem2.ItemRarity);
            session.Player.Inventory.AddItem(session, item2, true);
        }

        totalTripValue.UpdateValue(session, totalTrips);
    }
}
