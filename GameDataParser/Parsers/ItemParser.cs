using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Xml;
using GameDataParser.Files;
using GameDataParser.Crypto.Common;
using Maple2Storage.Types.Metadata;
using Maple2Storage.Types;
using ProtoBuf;

namespace GameDataParser.Parsers
{
    public static class ItemParser
    {
        
        public static List<ItemMetadata> Parse(MemoryMappedFile m2dFile, IEnumerable<PackFileEntry> entries)
        {
            List<ItemMetadata> items = new List<ItemMetadata>();
            foreach (PackFileEntry entry in entries)
            {
                if (!entry.Name.StartsWith("item/")) continue;

                ItemMetadata metadata = new ItemMetadata();
                string itemId = Path.GetFileNameWithoutExtension(entry.Name);
                metadata.Id = int.Parse(itemId);
                Debug.Assert(metadata.Id > 0, $"Invalid Id {metadata.Id} from {itemId}");

                using XmlReader reader = m2dFile.GetReader(entry.FileHeader);
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == "slot")
                        {
                            bool result = Enum.TryParse<ItemSlot>(reader["name"], out metadata.Slot);
                            if (!result && !string.IsNullOrEmpty(reader["name"]))
                            {
                                throw new ArgumentException("Failed to parse item slot:" + reader["name"]);
                            }
                        }
                        else if (reader.Name == "property")
                        {
                            try
                            {
                                byte type = byte.Parse(reader["type"]);
                                byte subType = byte.Parse(reader["subtype"]);
                                bool skin = byte.Parse(reader["skin"]) != 0;
                                metadata.Tab = GetTab(type, subType, skin);
                                metadata.IsTemplate = byte.Parse(reader["skinType"] ?? "0") == 99;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Failed {itemId}: {e.Message}");
                            }

                            metadata.SlotMax = int.Parse(reader["slotMax"]);
                        }
                    }
                }

                items.Add(metadata);
            }

            return items;
        }

        public static void Write(List<ItemMetadata> items)
        {
            using (FileStream writeStream = File.OpenWrite(VariableDefines.OUTPUT + "ms2-item-metadata"))
            {
                Serializer.Serialize(writeStream, items);
            }
            using (FileStream readStream = File.OpenRead(VariableDefines.OUTPUT + "ms2-item-metadata"))
            {
                // Ensure the file is read equivalent
                // Debug.Assert(items.SequenceEqual(Serializer.Deserialize<List<ItemMetadata>>(readStream)));
            }
            Console.WriteLine("Successfully parsed item metadata!");
        }

        // This is an approximation and may not be 100% correct
        public static InventoryType GetTab(byte type, byte subType, bool skin = false)
        {
            if (skin)
            {
                return InventoryType.Outfit;
            }

            switch (type)
            {
                case 0: // Unknown
                    return InventoryType.Misc;
                case 1:
                    return InventoryType.Gear;
                case 2: // "Usable"
                    switch (subType)
                    {
                        case 2:
                            return InventoryType.Consumable;
                        case 8: // Skill book for mount?
                            return InventoryType.Mount;
                        case 14: // Emote
                            return InventoryType.Misc;
                    }

                    break;
                case 3:
                    return InventoryType.Quest;
                case 4:
                    return InventoryType.Misc;
                case 5: // Air mount
                    return InventoryType.Mount;
                case 6: // Furnishing shows up in FishingMusic
                    return InventoryType.FishingMusic;
                case 7:
                    return InventoryType.Badge;
                case 9: // Ground mount
                    return InventoryType.Mount;
                case 10:
                    switch (subType)
                    {
                        case 0:
                        case 4:
                        case 5: // Ad Balloon
                        case 11: // Tail Medal
                        case 15: // Voucher
                        case 17: // Packages
                        case 18: // Packages
                        case 19:
                            return InventoryType.Misc;
                        case 20: // Fishing Pole / Instrument
                            return InventoryType.FishingMusic;
                    }

                    break;
                case 11:
                    return InventoryType.Pets;
                case 12: // Music Score
                    return InventoryType.FishingMusic;
                case 13:
                    return InventoryType.Gemstone;
                case 14: // Gem dust
                    return InventoryType.Catalyst;
                case 15:
                    return InventoryType.Catalyst;
                case 16:
                    return InventoryType.LifeSkill;
                case 19:
                    return InventoryType.Misc;
                case 20:
                    return InventoryType.Currency;
                case 21:
                    return InventoryType.Currency;
                case 22: // Blueprint
                    return InventoryType.Misc;
            }

            throw new ArgumentException($"Unknown Tab for: {type},{subType}");
        }
    }
}