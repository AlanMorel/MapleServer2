using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class RequestTutorialItemHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.REQUEST_TUTORIAL_ITEM;

        public RequestTutorialItemHandler(ILogger<RequestTutorialItemHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            // [bow] Henesys 47 Bow - 15100216
            // [staff] Modded Student Staff - 15200223
            // [longsword] Sword of tria - 13200220
            // [shield] Shield of tria - 14100190
            // [greatsword] Riena - 15000224
            // [scepter] Saint Mushliel Mace - 13300219
            // [codex] Words of Saint mushliel - 14000181
            // [cannon] Cannon of beginnings - 15300199
            // [dagger] Walker knife - 13100225
            // [star] Rook's Star - 13400218
            // [blade] Runesteel Blade - 15400274
            // [knuckles] Pugilist knuckles - 15500514
            // [orb] Guidance training orb - 15600514

            int[] itemIds = new int[] { 15100216, 15200223, 13200220, 14100190, 15000224, 13300219, 14000181, 15300199, 13100225, 13400218, 15400274, 15500514, 15600514, };
            foreach (int id in itemIds)
            {
                ItemMetadata metadata = ItemMetadataStorage.GetMetadata(id);

                if (!metadata.RecommendJobs.Contains((int) session.Player.Job))
                {
                    continue;
                }

                Item item = new Item(id);
                if (session.Player.Job == Job.Thief || session.Player.Job == Job.Assassin)
                {
                    item.Amount = 2;
                }

                InventoryController.Add(session, item, true);
            }
        }
    }
}
