using Maple2Storage.Types.Metadata;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class InstrumentHandler : GamePacketHandler<InstrumentHandler>
{
    public override RecvOp OpCode => RecvOp.PlayInstrument;

    private enum Mode : byte
    {
        StartImprovise = 0x0,
        PlayNote = 0x1,
        StopImprovise = 0x2,
        PlayScore = 0x3,
        StopScore = 0x4,
        StartEnsemble = 0x5,
        LeaveEnsemble = 0x6,
        Compose = 0x8,
        Fireworks = 0xE,
        AudienceEmote = 0xF
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.StartImprovise:
                HandleStartImprovise(session, packet);
                break;
            case Mode.PlayNote:
                HandlePlayNote(session, packet);
                break;
            case Mode.StopImprovise:
                HandleStopImprovise(session);
                break;
            case Mode.PlayScore:
                HandlePlayScore(session, packet);
                break;
            case Mode.StopScore:
                HandleStopScore(session);
                break;
            case Mode.StartEnsemble:
                HandleStartEnsemble(session, packet);
                break;
            case Mode.LeaveEnsemble:
                HandleLeaveEnsemble(session);
                break;
            case Mode.Compose:
                HandleCompose(session, packet);
                break;
            case Mode.Fireworks:
                HandleFireworks(session);
                break;
            case Mode.AudienceEmote:
                HandleAudienceEmote(packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleStartImprovise(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();

        if (!session.Player.Inventory.HasItem(itemUid))
        {
            return;
        }

        Item item = session.Player.Inventory.GetByUid(itemUid);

        InstrumentInfoMetadata instrumentInfo = InstrumentInfoMetadataStorage.GetMetadata(item.Function.Id);
        InstrumentCategoryInfoMetadata instrumentCategory = InstrumentCategoryInfoMetadataStorage.GetMetadata(instrumentInfo.Category);

        Instrument instrument = new(instrumentCategory.GMId, instrumentCategory.PercussionId, false, session.Player.FieldPlayer.ObjectId)
        {
            Improvise = true
        };

        session.Player.Instrument = session.FieldManager.RequestFieldObject(instrument);
        session.Player.Instrument.Coord = session.Player.FieldPlayer.Coord;
        session.FieldManager.AddInstrument(session.Player.Instrument);
        session.FieldManager.BroadcastPacket(InstrumentPacket.StartImprovise(session.Player.Instrument));
    }

    private static void HandlePlayNote(GameSession session, PacketReader packet)
    {
        int note = packet.ReadInt();

        session.FieldManager.BroadcastPacket(InstrumentPacket.PlayNote(note, session.Player.FieldPlayer), session);
    }

    private static void HandleStopImprovise(GameSession session)
    {
        if (session.Player.Instrument == null)
        {
            return;
        }

        session.FieldManager.BroadcastPacket(InstrumentPacket.StopImprovise(session.Player.FieldPlayer));
        session.FieldManager.RemoveInstrument(session.Player.Instrument);
        session.Player.Instrument = null;
    }

    private static void HandlePlayScore(GameSession session, PacketReader packet)
    {
        long instrumentItemUid = packet.ReadLong();
        long scoreItemUid = packet.ReadLong();

        if (!session.Player.Inventory.HasItem(scoreItemUid) || !session.Player.Inventory.HasItem(instrumentItemUid))
        {
            return;
        }

        Item instrumentItem = session.Player.Inventory.GetByUid(instrumentItemUid);

        InstrumentInfoMetadata instrumentInfo = InstrumentInfoMetadataStorage.GetMetadata(instrumentItem.Function.Id);
        InstrumentCategoryInfoMetadata instrumentCategory = InstrumentCategoryInfoMetadataStorage.GetMetadata(instrumentInfo.Category);

        Item score = session.Player.Inventory.GetByUid(scoreItemUid);

        if (score.PlayCount <= 0)
        {
            return;
        }

        Instrument instrument = new(instrumentCategory.GMId, instrumentCategory.PercussionId, score.IsCustomScore, session.Player.FieldPlayer.ObjectId)
        {
            InstrumentTick = session.ServerTick,
            Score = score,
            Improvise = false
        };

        score.PlayCount -= 1;
        session.Player.Instrument = session.FieldManager.RequestFieldObject(instrument);
        session.Player.Instrument.Coord = session.Player.FieldPlayer.Coord;
        session.FieldManager.AddInstrument(session.Player.Instrument);
        session.FieldManager.BroadcastPacket(InstrumentPacket.PlayScore(session.Player.Instrument));
        session.Send(InstrumentPacket.UpdateScoreUses(scoreItemUid, score.PlayCount));
    }

    private static void HandleStopScore(GameSession session)
    {
        // get Mastery exp
        ItemMusicMetadata metadata = ItemMetadataStorage.GetMetadata(session.Player.Instrument.Value.Score.Id)?.Music;
        int masteryExpGain = Math.Min(((session.ServerTick - session.Player.Instrument.Value.InstrumentTick) * metadata.MasteryValue) / 1000, metadata.MasteryValueMax);
        session.Player.Levels.GainMasteryExp(MasteryType.Performance, masteryExpGain);

        // get prestige exp
        int prestigeExpGain = (session.ServerTick - session.Player.Instrument.Value.InstrumentTick) / 1000 * 250;
        session.Player.Account.Prestige.GainExp(session, prestigeExpGain);

        //TODO: get exp for normal level

        // remove instrument from field
        session.FieldManager.BroadcastPacket(InstrumentPacket.StopScore(session.Player.Instrument));
        session.FieldManager.RemoveInstrument(session.Player.Instrument);
        session.Player.Instrument = null;
    }

    private static void HandleCompose(GameSession session, PacketReader packet)
    {
        long itemUid = packet.ReadLong();
        int length = packet.ReadInt();
        int instrumentType = packet.ReadInt();
        string scoreName = packet.ReadUnicodeString();
        string scoreNotes = packet.ReadString();

        if (!session.Player.Inventory.HasItem(itemUid))
        {
            return;
        }

        Item item = session.Player.Inventory.GetByUid(itemUid);

        item.Score.Length = length;
        item.Score.Type = instrumentType;
        item.Score.Title = scoreName;
        item.Score.Composer = session.Player.Name;
        item.Score.ComposerCharacterId = session.Player.CharacterId;
        item.Score.Notes = scoreNotes;

        session.Send(InstrumentPacket.Compose(item));
    }

    private static void HandleStartEnsemble(GameSession session, PacketReader packet)
    {
        long instrumentItemUid = packet.ReadLong();
        long scoreItemUid = packet.ReadLong();

        Party party = session.Player.Party;
        if (party == null)
        {
            return;
        }

        if (!session.Player.Inventory.HasItem(scoreItemUid) || !session.Player.Inventory.HasItem(instrumentItemUid))
        {
            return;
        }


        Item score = session.Player.Inventory.GetByUid(scoreItemUid);

        if (score.PlayCount <= 0)
        {
            return;
        }

        Item instrumentItem = session.Player.Inventory.GetByUid(instrumentItemUid);
        InstrumentInfoMetadata instrumentInfo = InstrumentInfoMetadataStorage.GetMetadata(instrumentItem.Function.Id);
        InstrumentCategoryInfoMetadata instrumentCategory = InstrumentCategoryInfoMetadataStorage.GetMetadata(instrumentInfo.Category);
        Instrument instrument = new(instrumentCategory.GMId, instrumentCategory.PercussionId, score.IsCustomScore, session.Player.FieldPlayer.ObjectId)
        {
            Score = score,
            Ensemble = true,
            Improvise = false
        };

        session.Player.Instrument = session.FieldManager.RequestFieldObject(instrument);
        session.Player.Instrument.Coord = session.Player.FieldPlayer.Coord;

        if (session.Player != party.Leader)
        {
            return;
        }

        int instrumentTick = session.ServerTick;
        foreach (Player member in party.Members)
        {
            if (member.Instrument == null)
            {
                continue;
            }

            if (!member.Instrument.Value.Ensemble)
            {
                continue;
            }

            member.Instrument.Value.InstrumentTick = instrumentTick; // set the tick to be all the same
            member.Session.FieldManager.AddInstrument(member.Session.Player.Instrument);
            session.FieldManager.BroadcastPacket(InstrumentPacket.PlayScore(member.Session.Player.Instrument));
            member.Instrument.Value.Score.PlayCount -= 1;
            member.Session.Send(InstrumentPacket.UpdateScoreUses(member.Instrument.Value.Score.Uid, member.Instrument.Value.Score.PlayCount));
            member.Instrument.Value.Ensemble = false;
        }
    }

    private static void HandleLeaveEnsemble(GameSession session)
    {
        session.FieldManager.BroadcastPacket(InstrumentPacket.StopScore(session.Player.Instrument));
        session.FieldManager.RemoveInstrument(session.Player.Instrument);
        session.Player.Instrument = null;
        session.Send(InstrumentPacket.LeaveEnsemble());
    }

    private static void HandleFireworks(GameSession session)
    {
        session.Send(InstrumentPacket.Fireworks(session.Player.FieldPlayer.ObjectId));
    }

    private static void HandleAudienceEmote(PacketReader packet)
    {
        int skillId = packet.ReadInt();
    }
}
