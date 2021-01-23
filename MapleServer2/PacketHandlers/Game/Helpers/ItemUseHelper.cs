using System;
using System.Collections.Generic;
using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game.Helpers
{
    public static class ItemUseHelper
    {
        private enum BoxType : byte
        {
            OPEN = 0x00,
            SELECT = 0x01
        }

        public static void OpenBox(GameSession session, List<ItemContent> content)
        {
            Random rng = new Random();
            bool oneGroup = true;

            foreach (ItemContent item in content)
            {
                if (item.DropGroup != content[0].DropGroup)
                {
                    oneGroup = false;
                    break;
                }
            }

            if (!oneGroup)
            {
                int smartDropRate = content[0].SmartDropRate;

                if (smartDropRate == 0)
                {
                    int rand = rng.Next(0, content.Count);
                    GiveItem(session, content[rand]);
                }
                else if (smartDropRate == 100)
                {
                    foreach (ItemContent item in content)
                    {
                        if (ItemMetadataStorage.GetRecommendJobs(item.Id).Contains(session.Player.JobGroupId) || ItemMetadataStorage.GetRecommendJobs(item.Id).Contains(0))
                        {
                            GiveItem(session, item);
                        }
                    }
                }
                else
                {
                    bool success = rng.Next(0, 100) > smartDropRate;

                    foreach (ItemContent item in content)
                    {
                        if (success)
                        {
                            if (ItemMetadataStorage.GetRecommendJobs(item.Id).Contains(session.Player.JobGroupId)) // maybe this need to a random call instead to get the next item on Content
                            {
                                GiveItem(session, item);
                                break;
                            }
                        }
                        else
                        {
                            if (ItemMetadataStorage.GetRecommendJobs(item.Id).Contains(session.Player.JobGroupId))
                            {
                                content.Remove(item);
                                break;
                            }
                        }
                    }

                    if (!success)
                    {
                        int rand = rng.Next(0, content.Count);
                        GiveItem(session, content[rand]);
                    }
                }
            }
            else
            {
                foreach (ItemContent item in content)
                {
                    if (ItemMetadataStorage.GetRecommendJobs(item.Id).Contains(session.Player.JobGroupId) || ItemMetadataStorage.GetRecommendJobs(item.Id).Contains(0))
                    {
                        GiveItem(session, item);
                    }
                }
            }
        }

        public static void GiveItem(GameSession session, ItemContent content)
        {
            Random rng = new Random();

            // Currency
            if (content.Id.ToString().StartsWith("9"))
            {
                switch (content.Id)
                {
                    case 90000001: // Meso
                        session.Player.Wallet.Meso.Modify(rng.Next(content.MinAmount, content.MaxAmount));
                        break;
                    case 90000004: // Meret
                    case 90000011: // Meret
                    case 90000015: // Meret
                    case 90000016: // Meret
                        session.Player.Wallet.Meret.Modify(rng.Next(content.MinAmount, content.MaxAmount));
                        break;
                }
            }
            // Items
            else
            {
                Item item = new Item(content.Id)
                {
                    Amount = rng.Next(content.MinAmount, content.MaxAmount),
                    Rarity = content.Rarity,
                    Enchants = content.EnchantLevel,
                };
                if (item.SlotMax == 1)
                {
                    item.Amount = content.MaxAmount;
                }
                InventoryController.Add(session, item, true);

                if (content.Id2 != 0)
                {
                    item = new Item(content.Id2)
                    {
                        Amount = rng.Next(content.MinAmount, content.MaxAmount),
                        Rarity = content.Rarity
                    };
                    InventoryController.Add(session, item, true);
                }
            }
        }
    }
}
