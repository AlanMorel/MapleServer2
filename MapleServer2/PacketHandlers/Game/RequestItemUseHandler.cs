using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Maple2Storage.Types.Metadata;
using Microsoft.Extensions.Logging;
using MapleServer2.Data.Static;
using System.Collections.Generic;
using System;
using Maple2Storage.Types;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestItemUseHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_ITEM_USE;

        public RequestItemUseHandler(ILogger<RequestItemUseHandler> logger) : base(logger) { }
        private Random rng = new Random();

        private enum BoxType : byte
        {
            OPEN = 0x00,
            SELECT = 0x01
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            long boxUid = packet.ReadLong();
            BoxType boxType = (BoxType) packet.ReadShort();

            int index = 0;
            if (boxType == BoxType.SELECT)
            {
                index = packet.ReadShort() - 0x30; // Starts at 0x30 for some reason
            }

            if (!session.Player.Inventory.Items.ContainsKey(boxUid))
            {
                return;
            }

            // Get the box item
            Item box = session.Player.Inventory.Items[boxUid];

            // Remove box if amount is 1 or less
            if (box.Amount <= 1)
            {
                InventoryController.Remove(session, boxUid, out Item removed);
            }
            // Decrement box amount to otherwise
            else
            {
                box.Amount -= 1;
                InventoryController.Update(session, boxUid, box.Amount);
            }

            // Handle selection box
            if (boxType == BoxType.SELECT)
            {
                if (index >= box.Content.Count)
                {
                    return;
                }

                OpenBox(session, box.Content[index]);
                return;
            }

            List<int> groupCount = new List<int>();

            foreach (ItemContent item in box.Content)
            {
                if (!groupCount.Contains(item.DropGroup))
                {
                    groupCount.Add(item.DropGroup);
                }
            }

            if (groupCount.Count == 1)
            {
                int smartDropRate = box.Content[0].SmartDropRate;

                if (smartDropRate == 0)
                {
                    int rand = rng.Next(0, box.Content.Count);
                    OpenBox(session, box.Content[rand]);
                }
                else if (smartDropRate == 100)
                {
                    foreach (ItemContent content in box.Content)
                    {
                        if (ItemMetadataStorage.GetRecommendJobs(content.Id).Contains(session.Player.JobGroupId) || ItemMetadataStorage.GetRecommendJobs(content.Id).Contains(0))
                        {
                            OpenBox(session, content);
                        }
                    }
                }
                else
                {
                    bool success = rng.Next(0, 100) > smartDropRate;

                    foreach (ItemContent item in box.Content)
                    {
                        if (success)
                        {
                            if (ItemMetadataStorage.GetRecommendJobs(item.Id).Contains(session.Player.JobGroupId)) // maybe this need to a random call instead to get the next item on Content
                            {
                                OpenBox(session, item);
                                break;
                            }
                        }
                        else
                        {
                            if (ItemMetadataStorage.GetRecommendJobs(item.Id).Contains(session.Player.JobGroupId))
                            {
                                box.Content.Remove(item);
                                break;
                            }
                        }
                    }

                    if (!success)
                    {
                        int rand = rng.Next(0, box.Content.Count);
                        OpenBox(session, box.Content[rand]);
                    }
                }
            }
            else
            {
                foreach (ItemContent content in box.Content)
                {
                    if (ItemMetadataStorage.GetRecommendJobs(content.Id).Contains(session.Player.JobGroupId) || ItemMetadataStorage.GetRecommendJobs(content.Id).Contains(0))
                    {
                        OpenBox(session, content);
                    }
                }
            }
        }

        private void OpenBox(GameSession session, ItemContent content)
        {
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
                InventoryController.Add(session, item, true);

                if ((item.RecommendJobs.Contains(70) || item.RecommendJobs.Contains(80)) && item.ItemSlot == ItemSlot.OH)
                {
                    item = new Item(content.Id)
                    {
                        Amount = rng.Next(content.MinAmount, content.MaxAmount),
                        Rarity = content.Rarity,
                        Enchants = content.EnchantLevel,
                    };
                    InventoryController.Add(session, item, true);
                }

                if (content.Id2 != 0)
                {
                    item = new Item(content.Id2)
                    {
                        Amount = rng.Next(content.MinAmount, content.MaxAmount),
                        Rarity = content.Rarity,
                        Enchants = content.EnchantLevel,
                    };
                    InventoryController.Add(session, item, true);
                }
            }
        }
    }
}
