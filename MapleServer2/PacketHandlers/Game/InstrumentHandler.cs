using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class InstrumentHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.PLAY_INSTRUMENT;

        public InstrumentHandler(ILogger<InstrumentHandler> logger) : base(logger) { }

        private enum InstrumentMode : byte
        {
            StartImprovise = 0x0,
            PlayNote = 0x1,
            StopImprovise = 0x2,
            PlayScore = 0x3,
            StopScore = 0x4,
            Fireworks = 0xE,
            AudienceEmote = 0xF,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            InstrumentMode mode = (InstrumentMode) packet.ReadByte();

            switch (mode)
            {
                case InstrumentMode.StartImprovise:
                    HandleStartImprovise(session, packet);
                    break;
                case InstrumentMode.PlayNote:
                    HandlePlayNote(session, packet);
                    break;
                case InstrumentMode.StopImprovise:
                    HandleStopImprovise(session);
                    break;
                case InstrumentMode.PlayScore:
                    HandlePlayScore(session, packet);
                    break;
                case InstrumentMode.StopScore:
                    HandleStopScore(session, packet);
                    break;
                case InstrumentMode.Fireworks:
                    HandleFireworks(session);
                    break;
                case InstrumentMode.AudienceEmote:
                    HandleAudienceEmote(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleStartImprovise(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();
            // TODO: Verify if item is an (playable) instrument

            session.FieldManager.BroadcastPacket(InstrumentPacket.StartImprovise(session.Player));
        }

        private static void HandlePlayNote(GameSession session, PacketReader packet)
        {
            int note = packet.ReadInt();

            session.FieldManager.BroadcastPacket(InstrumentPacket.PlayNote(note, session.Player));
        }

        private static void HandleStopImprovise(GameSession session)
        {
            session.FieldManager.BroadcastPacket(InstrumentPacket.StopImprovise(session.Player));
        }

        private static void HandlePlayScore(GameSession session, PacketReader packet)
        {
            long instrumentItemUid = packet.ReadLong();
            long scoreItemUid = packet.ReadLong();

            if (!session.Player.Inventory.Items.ContainsKey(scoreItemUid))
            {
                return;
            }

            Item score = session.Player.Inventory.Items[scoreItemUid];

            if (score.PlayCount <= 0)
            {
                return;
            }

            score.PlayCount -= 1;

            session.Send(InstrumentPacket.PlayScore(session.Player, score.FileName));
            session.FieldManager.BroadcastPacket(InstrumentPacket.PlayScore(session.Player, score.FileName));
            session.Send(InstrumentPacket.ScoreUses(scoreItemUid, score.PlayCount));
        }

        private static void HandleStopScore(GameSession session, PacketReader packet)
        {
            session.Send(InstrumentPacket.StopScore(session.Player));
            session.FieldManager.BroadcastPacket(InstrumentPacket.StopScore(session.Player));
        }

        private static void HandleFireworks(GameSession session)
        {
            session.Send(InstrumentPacket.Fireworks(session.FieldPlayer.ObjectId));
        }

        private static void HandleAudienceEmote(GameSession session, PacketReader packet)
        {
            int skillId = packet.ReadInt();
        }
    }
}
