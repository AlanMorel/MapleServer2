using System.Collections.Generic;
using System.Linq;
using Maple2Storage.Enums;
using Maple2Storage.Tools;
using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.PacketHandlers.Game.Helpers;
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
            MapInteractObject interactObject = MapEntityStorage.GetInteractObject(session.Player.MapId).FirstOrDefault(x => x.Uuid == uuid);
            if (interactObject.Type == InteractObjectType.Gathering)
            {
                // things to do when player starts gathering
            }
        }

        private static void HandleUse(GameSession session, PacketReader packet)
        {
            string uuid = packet.ReadMapleString();
            IFieldObject<InteractObject> interactObject = session.FieldManager.State.InteractObjects[uuid];
            if (interactObject == null)
            {
                return;
            }

            MapInteractObject mapObject = MapEntityStorage.GetInteractObject(session.Player.MapId).FirstOrDefault(x => x.Uuid == uuid);
            int numDrop = 0;

            switch (interactObject.Value.Type)
            {
                case InteractObjectType.Binoculars:
                    QuestHelper.UpdateExplorationQuest(session, mapObject.InteractId.ToString(), "interact_object_rep");
                    break;
                case InteractObjectType.Gathering:
                    RecipeMetadata recipe = RecipeMetadataStorage.GetRecipe(mapObject.RecipeId);

                    session.Player.Levels.GainMasteryExp((MasteryType) recipe.MasteryType, 0);
                    long currentMastery = session.Player.Levels.MasteryExp.FirstOrDefault(x => x.Type == (MasteryType) recipe.MasteryType).CurrentExp;
                    if (currentMastery < recipe.RequireMastery)
                    {
                        return;
                    }

                    session.Player.IncrementGatheringCount(mapObject.RecipeId, 0);
                    int numCount = session.Player.GatheringCount[mapObject.RecipeId].Current;

                    List<RecipeItem> items = RecipeMetadataStorage.GetResult(recipe);
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
                        if (RandomProvider.Get().Next(100) >= prob)
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
                        session.Player.IncrementGatheringCount(mapObject.RecipeId, numDrop);
                        session.Player.Levels.GainMasteryExp((MasteryType) recipe.MasteryType, recipe.RewardMastery);
                    }
                    break;
                case InteractObjectType.AdBalloon:
                    session.Send(PlayerHostPacket.AdBalloonWindow(interactObject));
                    return;
                default:
                    break;
            }
            session.Send(InteractObjectPacket.UseObject(mapObject, (short) (numDrop > 0 ? 0 : 1), numDrop));
            session.Send(InteractObjectPacket.Extra(mapObject));
        }
    }
}
