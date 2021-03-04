using System;
using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    class InteractObjectHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.INTERACT_OBJECT;

        public InteractObjectHandler(ILogger<InteractObjectHandler> logger) : base(logger) { }

        private enum InteractObjectMode : byte
        {
            Cast = 0x0B,
            Use = 0x0C,
        }

        private static readonly int[] RarityChance = new int[] { 100, 80, 60, 40, 20 };

        public override void Handle(GameSession session, PacketReader packet)
        {
            InteractObjectMode mode = (InteractObjectMode) packet.ReadByte();

            switch (mode)
            {
                case InteractObjectMode.Cast:
                    HandleStart(session, packet);
                    break;
                case InteractObjectMode.Use:
                    HandleUse(session, packet);
                    break;
            }
        }

        private static void HandleStart(GameSession session, PacketReader packet)
        {
            string uuid = packet.ReadMapleString();
            MapInteractActor actor = MapEntityStorage.GetInteractActors(session.Player.MapId).FirstOrDefault(x => x.Uuid == uuid);
            if (actor.Type == InteractActorType.Gathering)
            {
                // things to do when player starts gathering
            }
        }

        private static void HandleUse(GameSession session, PacketReader packet)
        {
            string uuid = packet.ReadMapleString();
            MapInteractActor actor = MapEntityStorage.GetInteractActors(session.Player.MapId).FirstOrDefault(x => x.Uuid == uuid);
            int numDrop = 0;

            if (actor == null)
            {
                return;
            }
            if (actor.Type == InteractActorType.Binoculars)
            {
                List<QuestStatus> questList = session.Player.QuestList;
                foreach (QuestStatus item in questList.Where(x => x.Basic.QuestID >= 72000000 && x.Condition != null))
                {
                    QuestCondition condition = item.Condition.FirstOrDefault(x => x.Code != "" && int.Parse(x.Code) == actor.InteractId);
                    if (condition == null)
                    {
                        continue;
                    }

                    item.Completed = true;
                    item.CompleteTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    session.Send(QuestPacket.CompleteExplorationGoal(item.Basic.QuestID));
                    session.Send(QuestPacket.CompleteQuest(item.Basic.QuestID));
                    break;
                }
                session.Send(InteractActorPacket.UseObject(actor));
            }
            else if (actor.Type == InteractActorType.Gathering)
            {
                RecipeMetadata recipe = RecipeMetadataStorage.GetRecipe(actor.RecipeId);
                long requireMastery = int.Parse(recipe.RequireMastery);
                Enums.MasteryType type = (Enums.MasteryType) int.Parse(recipe.MasteryType);

                session.Player.Levels.GainMasteryExp(type, 0);
                long currentMastery = session.Player.Levels.MasteryExp.FirstOrDefault(x => x.Type == (byte) type).CurrentExp;
                if (currentMastery < requireMastery)
                {
                    return;
                }

                session.Player.ConsumeGatheringCount(actor.RecipeId, 0);
                int remCount = session.Player.GatheringCount[actor.RecipeId].Current;

                List<RecipeItem> items = RecipeMetadataStorage.GetResult(recipe);
                Random rand = new Random();

                float probMultiplier = remCount > 3 ? 1 : remCount > 0 ? 0.5f : 0;
                foreach (RecipeItem item in items)
                {
                    int prob = (int) (RarityChance[item.Rarity]/* * probMultiplier*/);
                    if (rand.Next(100) < prob)
                    {
                        for (int i = 0; i < item.Amount; i++)
                        {
                            session.FieldManager.AddItem(session, new Item(item.Id));
                        }
                        numDrop += item.Amount;
                    }
                }
                if (numDrop > 0)
                {
                    session.Player.ConsumeGatheringCount(actor.RecipeId, numDrop);
                    session.Player.Levels.GainMasteryExp(type, recipe.RewardMastery);
                }
            }
            session.Send(InteractActorPacket.UseObject(actor, numDrop > 0 ? 0 : 1, numDrop));
            session.Send(InteractActorPacket.Extra(actor));
        }
    }
}
