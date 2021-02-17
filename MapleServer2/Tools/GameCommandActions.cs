using System;
using System.Collections.Generic;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public static class GameCommandActions
    {
        public static void Process(GameSession session, string command)
        {
            string[] args = command.ToLower().Split(" ", 2);
            switch (args[0])
            {
                case "sethandicraft":
                    session.Player.Levels.GainMasteryExp(MasteryType.Handicraft, ParseInt(session, args.Length > 1 ? args[1] : ""));
                    break;
                case "setprestigelevel":
                    session.Player.Levels.SetPrestigeLevel(ParseInt(session, args.Length > 1 ? args[1] : ""));
                    break;
                case "setlevel":
                    session.Player.Levels.SetLevel(ParseShort(session, args.Length > 1 ? args[1] : ""));
                    break;
                case "gainprestigeexp":
                    session.Player.Levels.GainPrestigeExp(ParseLong(session, args.Length > 1 ? args[1] : ""));
                    break;
                case "gainexp":
                    session.Player.Levels.GainExp(ParseInt(session, args.Length > 1 ? args[1] : ""));
                    break;
                case "setvalor":
                    session.Player.Wallet.ValorToken.SetAmount(ParseLong(session, args.Length > 1 ? args[1] : ""));
                    break;
                case "settreva":
                    session.Player.Wallet.Treva.SetAmount(ParseLong(session, args.Length > 1 ? args[1] : ""));
                    break;
                case "setrue":
                    session.Player.Wallet.Rue.SetAmount(ParseLong(session, args.Length > 1 ? args[1] : ""));
                    break;
                case "sethavi":
                    session.Player.Wallet.HaviFruit.SetAmount(ParseLong(session, args.Length > 1 ? args[1] : ""));
                    break;
                case "setmeso":
                    session.Player.Wallet.Meso.SetAmount(ParseLong(session, args.Length > 1 ? args[1] : ""));
                    break;
                case "setmeret":
                    session.Player.Wallet.Meret.SetAmount(ParseLong(session, args.Length > 1 ? args[1] : ""));
                    break;
                case "item":
                    ProcessItemCommand(session, args.Length > 1 ? args[1] : "");
                    break;
                case "npc":
                    ProcessNpcCommand(session, args.Length > 1 ? args[1] : "");
                    break;
                case "mob":
                    ProcessMobCommand(session, args.Length > 1 ? args[1] : "");
                    break;
                case "map":
                    ProcessMapCommand(session, args.Length > 1 ? args[1] : "");
                    break;
                case "coord":
                    session.SendNotice(session.FieldPlayer.Coord.ToString());
                    break;
                case "battleoff":
                    session.Send(UserBattlePacket.UserBattle(session.FieldPlayer, false));
                    break;
                case "notice":
                    if (args.Length <= 1)
                    {
                        break;
                    }
                    MapleServer.BroadcastPacketAll(NoticePacket.Notice(args[1]));
                    break;
                case "test":
                    session.Send(ShopPacket.Open(11003463, 168, "guildtokenetc"));
                    break;
            }
        }

        // Example: "item id:20000027"
        private static void ProcessItemCommand(GameSession session, string command)
        {
            Dictionary<string, string> config = command.ToMap();
            if (!int.TryParse(config.GetValueOrDefault("id", "20000027"), out int itemId))
            {
                return;
            }
            if (!ItemMetadataStorage.IsValid(itemId))
            {
                session.SendNotice("Invalid item: " + itemId);
                return;
            }

            _ = int.TryParse(config.GetValueOrDefault("rarity", "5"), out int rarity);
            _ = int.TryParse(config.GetValueOrDefault("amount", "1"), out int amount);

            Item item = new Item(itemId)
            {
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                TransferFlag = TransferFlag.Splitable | TransferFlag.Tradeable,
                PlayCount = itemId.ToString().StartsWith("35") ? 10 : 0,
                Rarity = rarity,
                Amount = amount,
                Stats = new ItemStats(itemId, rarity)
            };

            // Simulate looting item
            InventoryController.Add(session, item, true);
        }

        // Example: "map -> return current map id"
        // Example: "map id:200001 -> teleport to map"
        private static void ProcessMapCommand(GameSession session, string command)
        {
            Dictionary<string, string> config = command.ToMap();
            if (!int.TryParse(config.GetValueOrDefault("id", "0"), out int mapId))
            {
                return;
            }
            if (mapId == 0)
            {
                session.SendNotice($"Current map id:{session.Player.MapId}");
                return;
            }

            if (session.Player.MapId == mapId)
            {
                session.SendNotice("You are already on that map.");
                return;
            }

            MapPlayerSpawn spawn = MapEntityStorage.GetRandomPlayerSpawn(mapId);

            if (spawn != null)
            {
                session.Player.MapId = mapId;
                session.Player.Coord = spawn.Coord.ToFloat();
                session.Player.Rotation = spawn.Rotation.ToFloat();
                session.Send(FieldPacket.RequestEnter(session.FieldPlayer));
            }
            else
            {
                session.SendNotice("Could not find coordinates to spawn on that map.");
            }
        }

        private static void ProcessNpcCommand(GameSession session, string command)
        {
            Dictionary<string, string> config = command.ToMap();
            if (!int.TryParse(config.GetValueOrDefault("id", "11003146"), out int npcId))
            {
                return;
            }
            Npc npc = new Npc(npcId);
            if (byte.TryParse(config.GetValueOrDefault("ani", "-1"), out byte animation))
            {
                npc.Animation = animation;
            }
            if (short.TryParse(config.GetValueOrDefault("dir", "2700"), out short rotation))
            {
                npc.ZRotation = rotation;
            }

            IFieldObject<Npc> fieldNpc = session.FieldManager.RequestFieldObject(npc);
            if (TryParseCoord(config.GetValueOrDefault("coord", ""), out CoordF coord))
            {
                fieldNpc.Coord = coord;
            }
            else
            {
                fieldNpc.Coord = session.FieldPlayer.Coord;
            }

            session.FieldManager.AddNpc(fieldNpc);
        }

        private static void ProcessMobCommand(GameSession session, string command)
        {
            Dictionary<string, string> config = command.ToMap();
            if (!int.TryParse(config.GetValueOrDefault("id", "21000001"), out int mobId))
            {
                return;
            }
            Mob mob = new Mob(mobId);
            if (byte.TryParse(config.GetValueOrDefault("ani", "-1"), out byte animation))
            {
                mob.Animation = animation;
            }
            if (short.TryParse(config.GetValueOrDefault("dir", "2700"), out short rotation))
            {
                mob.ZRotation = rotation;
            }

            IFieldObject<Mob> fieldMob = session.FieldManager.RequestFieldObject(mob);
            if (TryParseCoord(config.GetValueOrDefault("coord", ""), out CoordF coord))
            {
                fieldMob.Coord = coord;
            }
            else
            {
                fieldMob.Coord = session.FieldPlayer.Coord;
            }

            session.FieldManager.AddMob(fieldMob);
        }

        private static Dictionary<string, string> ToMap(this string command)
        {
            string[] args = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            Dictionary<string, string> map = new Dictionary<string, string>();
            foreach (string arg in args)
            {
                string[] entry = arg.Split(new[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (entry.Length != 2)
                {
                    Console.WriteLine($"Invalid map entry: \"{arg}\" was ignored.");
                    continue;
                }

                map[entry[0]] = entry[1];
            }

            return map;
        }

        private static bool TryParseCoord(string s, out CoordF result)
        {
            string[] values = s.Split(",");
            if (values.Length == 3 && float.TryParse(values[0], out float x)
                                   && float.TryParse(values[1], out float y)
                                   && float.TryParse(values[2], out float z))
            {
                result = CoordF.From(x, y, z);
                return true;
            }

            result = default;
            return false;
        }

        private static long ParseLong(GameSession session, string s)
        {
            try
            {
                return long.Parse(s);
            }
            catch (FormatException)
            {
                session.SendNotice("The input is not type long.");
                return -1;
            }
            catch (OverflowException)
            {
                session.SendNotice("You entered a number too big or too small.");
                return -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static int ParseInt(GameSession session, string s)
        {
            try
            {
                return int.Parse(s);
            }
            catch (FormatException)
            {
                session.SendNotice("The input is not type int.");
                return -1;
            }
            catch (OverflowException)
            {
                session.SendNotice("You entered a number too big or too small.");
                return -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static short ParseShort(GameSession session, string s)
        {
            try
            {
                return short.Parse(s);
            }
            catch (FormatException)
            {
                session.SendNotice("The input is not type short.");
                return -1;
            }
            catch (OverflowException)
            {
                session.SendNotice("You entered a number too big or too small.");
                return -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }
}
