using System;
using System.Collections.Generic;
using Maple2Storage.Types;
using Maple2Storage.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using Maple2.Data.Types.Items;
using MapleServer2.Types.FieldObjects;
using MapleServer2.Types.Npcs;

namespace MapleServer2.Tools {
    public static class GameCommandActions {
        public static void Process(GameSession session, string command) {
            string[] args = command.ToLower().Split(" ", 2);
            switch (args[0]) {
                case "item":
                    ProcessItemCommand(session, args.Length > 1 ? args[1] : "");
                    break;
                case "npc":
                    ProcessNpcCommand(session, args.Length > 1 ? args[1] : "");
                    break;
                case "coord":
                    session.SendNotice(session.FieldPlayer.Coord.ToString());
                    break;
            }
        }

        // Example: "item id:20000027"
        private static void ProcessItemCommand(GameSession session, string command) {
            Dictionary<string, string> config = command.ToMap();
            int.TryParse(config.GetValueOrDefault("id", "20000027"), out int itemId);
            if (!ItemMetadataStorage.IsValid(itemId)) {
                session.SendNotice("Invalid item: " + itemId);
                return;
            }

            // Add some bonus attributes to equips and pets
            var stats = new ItemStats();
            if (ItemMetadataStorage.GetTab(itemId) == InventoryType.Gear
                    || ItemMetadataStorage.GetTab(itemId) == InventoryType.Pets) {
                var rng = new Random();
                stats.BonusAttributes.Add(ItemStat.Of((ItemAttribute) rng.Next(35), 0.01f));
                stats.BonusAttributes.Add(ItemStat.Of((ItemAttribute) rng.Next(35), 0.01f));
            }

            var item = new Item(itemId) {
                Uid = Environment.TickCount64,
                CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                Transfer = TransferFlag.Splitable | TransferFlag.Tradeable,
                Stats = stats
            };
            int.TryParse(config.GetValueOrDefault("rarity", "5"), out item.Rarity);
            int.TryParse(config.GetValueOrDefault("amount", "1"), out item.Amount);

            // Simulate looting item
            if (session.Inventory.Add(item)) {
                session.Send(ItemInventoryPacket.Add(item));
                session.Send(ItemInventoryPacket.MarkItemNew(item));
            }
        }

        private static void ProcessNpcCommand(GameSession session, string command) {
            Dictionary<string, string> config = command.ToMap();
            int.TryParse(config.GetValueOrDefault("id", "11003146"), out int npcId);
            var npc = new Npc(npcId);
            byte.TryParse(config.GetValueOrDefault("ani", "-1"), out npc.Animation);
            short.TryParse(config.GetValueOrDefault("dir", "2700"), out npc.Rotation);

            IFieldObject<Npc> fieldNpc = session.FieldManager.RequestFieldObject(npc);
            if (TryParseCoord(config.GetValueOrDefault("coord", ""), out CoordF coord)) {
                fieldNpc.Coord = coord;
            } else {
                fieldNpc.Coord = session.FieldPlayer.Coord;
            }

            session.FieldManager.AddTestNpc(fieldNpc);
        }

        private static Dictionary<string, string> ToMap(this string command) {
            string[] args = command.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries);

            var map = new Dictionary<string, string>();
            foreach (string arg in args) {
                string[] entry = arg.Split(new[] {':', '='}, StringSplitOptions.RemoveEmptyEntries);
                if (entry.Length != 2) {
                    Console.WriteLine($"Invalid map entry: \"{arg}\" was ignored.");
                    continue;
                }

                map[entry[0]] = entry[1];
            }

            return map;
        }

        private static bool TryParseCoord(string s, out CoordF result) {
            string[] values = s.Split(",");
            if (values.Length == 3 && float.TryParse(values[0], out float x)
                                   && float.TryParse(values[1], out float y)
                                   && float.TryParse(values[2], out float z)) {
                result = CoordF.From(x, y, z);
                return true;
            }

            result = default;
            return false;
        }
    }
}