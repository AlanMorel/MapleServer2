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

        private static readonly int[] RarityChance = new int[] { 0, 100, 80, 60, 40, 20 };         // drop rate of each rarity level

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
                foreach (QuestStatus quest in questList.Where(x => x.Basic.Id >= 72000000 && x.Condition != null))
                {
                    QuestCondition condition = quest.Condition.Where(x => x.Type == "interact_object_rep").FirstOrDefault(x => x.Code != "" && int.Parse(x.Code) == actor.InteractId);
                    if (condition == null)
                    {
                        continue;
                    }

                    quest.Completed = true;
                    quest.CompleteTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    session.Player.Levels.GainExp(quest.Reward.Exp);
                    session.Player.Wallet.Meso.Modify(quest.Reward.Money);
                    session.Send(QuestPacket.CompleteExplorationGoal(quest.Basic.Id));
                    session.Send(QuestPacket.CompleteQuest(quest.Basic.Id));
                    break;
                }
            }
            else if (actor.Type == InteractActorType.Gathering)
            {
                RecipeMetadata recipe = RecipeMetadataStorage.GetRecipe(actor.RecipeId);
                long requireMastery = int.Parse(recipe.RequireMastery);
                Enums.MasteryType type = (Enums.MasteryType) int.Parse(recipe.MasteryType);

                session.Player.Levels.GainMasteryExp(type, 0);
                long currentMastery = session.Player.Levels.MasteryExp.FirstOrDefault(x => x.Type == type).CurrentExp;
                if (currentMastery < requireMastery)
                {
                    return;
                }

                session.Player.IncrementGatheringCount(actor.RecipeId, 0);
                int numCount = session.Player.GatheringCount[actor.RecipeId].Current;

                List<RecipeItem> items = RecipeMetadataStorage.GetResult(recipe);
                Random rand = new Random();
                int masteryDiffFactor = numCount switch
                {
                    int n when n < recipe.HighPropLimitCount => MasteryFactorMetadataStorage.GetFactor(0),
                    int n when n < recipe.NormalPropLimitCount => MasteryFactorMetadataStorage.GetFactor(1),
                    int n when n < (int) (recipe.NormalPropLimitCount * 1.3) => MasteryFactorMetadataStorage.GetFactor(2),
                    _ => MasteryFactorMetadataStorage.GetFactor(3),
                };

                foreach (RecipeItem item in items)
                {
                    int prob = (int) (RarityChance[item.Rarity] * masteryDiffFactor) / 10000;
                    if (rand.Next(100) >= prob)
                    {
                        continue;
                    }
                    for (int i = 0; i < item.Amount; i++)
                    {
                        session.FieldManager.AddItem(session, new Item(item.Id));
                    }
                    numDrop += item.Amount;
                }
                if (numDrop > 0)
                {
                    session.Player.IncrementGatheringCount(actor.RecipeId, numDrop);
                    session.Player.Levels.GainMasteryExp(type, recipe.RewardMastery);
                }
            }
            session.Send(InteractActorPacket.UseObject(actor, (short) (numDrop > 0 ? 0 : 1), numDrop));
            session.Send(InteractActorPacket.Extra(actor));
        }
    }
}
