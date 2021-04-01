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
            Compose = 0x8,
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
                    HandleStopScore(session/*, packet*/);
                    break;
                case InstrumentMode.Compose:
                    HandleCompose(session, packet);
                    break;
                case InstrumentMode.Fireworks:
                    HandleFireworks(session);
                    break;
                case InstrumentMode.AudienceEmote:
                    HandleAudienceEmote(/*session,*/ packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleStartImprovise(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();

            if (!session.Player.Inventory.Items.ContainsKey(itemUid))
            {
                return;
            }

            Item item = session.Player.Inventory.Items[itemUid];

            InsturmentInfoMetadata instrument = InstrumentInfoMetadataStorage.GetMetadata(item.Function.Id);
            InstrumentCategoryInfoMetadata instrumentCategory = InstrumentCategoryInfoMetadataStorage.GetMetadata(instrument.Category);
            session.FieldManager.BroadcastPacket(InstrumentPacket.StartImprovise(session.FieldPlayer, instrumentCategory.GMId));
        }

        private static void HandlePlayNote(GameSession session, PacketReader packet)
        {
            int note = packet.ReadInt();

            session.FieldManager.BroadcastPacket(InstrumentPacket.PlayNote(note, session.FieldPlayer));
        }

        private static void HandleStopImprovise(GameSession session)
        {
            session.FieldManager.BroadcastPacket(InstrumentPacket.StopImprovise(session.FieldPlayer));
        }

        private static void HandlePlayScore(GameSession session, PacketReader packet)
        {
            long instrumentItemUid = packet.ReadLong();
            long scoreItemUid = packet.ReadLong();

            if (!session.Player.Inventory.Items.ContainsKey(scoreItemUid) || !session.Player.Inventory.Items.ContainsKey(instrumentItemUid))
            {
                return;
            }

            Item instrument = session.Player.Inventory.Items[instrumentItemUid];

            InsturmentInfoMetadata instrumentInfo = InstrumentInfoMetadataStorage.GetMetadata(instrument.Function.Id);
            InstrumentCategoryInfoMetadata instrumentCategory = InstrumentCategoryInfoMetadataStorage.GetMetadata(instrumentInfo.Category);

            Item score = session.Player.Inventory.Items[scoreItemUid];

            if (score.PlayCount <= 0)
            {
                return;
            }

            score.PlayCount -= 1;

            session.FieldManager.BroadcastPacket(InstrumentPacket.PlayScore(session, score, instrumentCategory.GMId, instrumentCategory.PercussionId));
            session.Send(InstrumentPacket.UpdateScoreUses(scoreItemUid, score.PlayCount));
        }

        private static void HandleStopScore(GameSession session/*, PacketReader packet*/)
        {
            session.Send(InstrumentPacket.StopScore(session.FieldPlayer));
            session.FieldManager.BroadcastPacket(InstrumentPacket.StopScore(session.FieldPlayer));
        }

        private static void HandleCompose(GameSession session, PacketReader packet)
        {
            long itemUid = packet.ReadLong();
            int length = packet.ReadInt();
            int instrumentType = packet.ReadInt();
            string scoreName = packet.ReadUnicodeString();
            string scoreNotes = packet.ReadMapleString();

            if (!session.Player.Inventory.Items.ContainsKey(itemUid))
            {
                return;
            }

            Item item = session.Player.Inventory.Items[itemUid];

            item.Score.Length = length;
            item.Score.Type = instrumentType;
            item.Score.Title = scoreName;
            item.Score.Composer = session.Player.Name;
            item.Score.ComposerCharacterId = session.Player.CharacterId;
            item.Score.Notes = scoreNotes;

            session.Send(InstrumentPacket.Compose(item));
        }

        private static void HandleFireworks(GameSession session)
        {
            session.Send(InstrumentPacket.Fireworks(session.FieldPlayer.ObjectId));
        }

        private static void HandleAudienceEmote(/*GameSession session,*/PacketReader packet)
        {
            int skillId = packet.ReadInt();
        }
    }
}
