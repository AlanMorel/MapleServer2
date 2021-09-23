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

namespace MapleServer2.PacketHandlers.Game
{
    internal class InteractObjectHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.INTERACT_OBJECT;

        public InteractObjectHandler() : base() { }

        private enum InteractObjectMode : byte
        {
            Cast = 0x0B,
            Interact = 0x0C,
        }

        private static readonly int[] RarityChance = new int[] { 0, 100, 80, 60, 40, 20 };         // drop rate of each rarity level

        public override void Handle(GameSession session, PacketReader packet)
        {
            InteractObjectMode mode = (InteractObjectMode) packet.ReadByte();

            switch (mode)
            {
                case InteractObjectMode.Cast:
                    HandleCast(session, packet);
                    break;
                case InteractObjectMode.Interact:
                    HandleInteract(session, packet);
                    break;
            }
        }

        private static void HandleCast(GameSession session, PacketReader packet)
        {
            string id = packet.ReadMapleString();
            InteractObject interactObject = session.FieldManager.State.InteractObjects[id];
            if (interactObject == null)
            {
                return;
            }

            // TODO: Change state of object only if player succeeds in the cast.
        }

        private static void HandleInteract(GameSession session, PacketReader packet)
        {
            string id = packet.ReadMapleString();
            InteractObject interactObject = session.FieldManager.State.InteractObjects[id];
            if (interactObject == null)
            {
                return;
            }

            InteractObjectMetadata metadata = InteractObjectMetadataStorage.GetInteractObjectMetadata(interactObject.InteractId);

            switch (interactObject.Type)
            {
                case InteractObjectType.Binoculars:
                    session.Send(InteractObjectPacket.Use(interactObject));
                    QuestHelper.UpdateExplorationQuest(session, interactObject.InteractId.ToString(), "interact_object_rep");
                    break;
                case InteractObjectType.Ui:
                    session.Send(InteractObjectPacket.Use(interactObject));
                    break;
                case InteractObjectType.RankBoard:
                    session.Send(WebOpenPacket.Open(metadata.Web.Url));
                    break;
                case InteractObjectType.AdBalloon:
                    session.Send(PlayerHostPacket.AdBalloonWindow((AdBalloon) interactObject));
                    break;
                case InteractObjectType.Gathering:
                    HandleGathering(session, metadata);
                    break;

            }

            session.Send(InteractObjectPacket.Interact(interactObject));
        }

        private static void HandleGathering(GameSession session, InteractObjectMetadata objectMetadata)
        {
            int numDrop = 0;
            RecipeMetadata recipe = RecipeMetadataStorage.GetRecipe(objectMetadata.Gathering.RecipeId);
            if (recipe == null)
            {
                return;
            }

            session.Player.Levels.GainMasteryExp((MasteryType) recipe.MasteryType, 0);
            long currentMastery = session.Player.Levels.MasteryExp.FirstOrDefault(x => x.Type == (MasteryType) recipe.MasteryType).CurrentExp;
            if (currentMastery < recipe.RequireMastery)
            {
                return;
            }

            session.Player.IncrementGatheringCount(recipe.Id, 0);
            int numCount = session.Player.GatheringCount[recipe.Id].Current;

            List<RecipeItem> items = recipe.RewardItems;
            int masteryDiffFactor = numCount switch
            {
                int n when n < recipe.HighPropLimitCount => MasteryFactorMetadataStorage.GetFactor(0),
                int n when n < recipe.NormalPropLimitCount => MasteryFactorMetadataStorage.GetFactor(1),
                int n when n < (int) (recipe.NormalPropLimitCount * 1.3) => MasteryFactorMetadataStorage.GetFactor(2),
                _ => MasteryFactorMetadataStorage.GetFactor(3),
            };

            foreach (RecipeItem item in items)
            {
                int prob = RarityChance[item.Rarity] * masteryDiffFactor / 10000;
                if (RandomProvider.Get().Next(100) >= prob)
                {
                    continue;
                }
                for (int i = 0; i < item.Amount; i++)
                {
                    session.FieldManager.AddItem(session, new Item(item.ItemId)
                    {
                        Rarity = item.Rarity,
                        Amount = item.Amount
                    });
                }
                numDrop += item.Amount;
            }
            if (numDrop <= 0)
            {
                return;
            }

            session.Player.IncrementGatheringCount(recipe.Id, numDrop);
            session.Player.Levels.GainMasteryExp((MasteryType) recipe.MasteryType, recipe.RewardMastery);
        }
    }
}
