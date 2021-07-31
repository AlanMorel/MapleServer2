using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Servers.Game;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class TutorialHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.TUTORIAL;

        public TutorialHandler(ILogger<TutorialHandler> logger) : base(logger) { }

        public override void Handle(GameSession session, PacketReader packet)
        {
            JobMetadata metadata = JobMetadataStorage.GetJobMetadata((int) session.Player.Job);
            {
                foreach(int taxiMapId in metadata.OpenTaxis)
                {
                    if(session.Player.UnlockedTaxis.Contains(taxiMapId))
                    {
                        continue;
                    }
                    session.Player.UnlockedTaxis.Add(taxiMapId);
                }

                foreach (int openMapId in metadata.OpenMaps)
                {
                    if (session.Player.UnlockedMaps.Contains(openMapId))
                    {
                        continue;
                    }
                    session.Player.UnlockedMaps.Add(openMapId);
                }
            }
        }
    }
}
