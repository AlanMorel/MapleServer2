using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class BuddyEmoteHandler : GamePacketHandler
{
    public override RecvOp OpCode => RecvOp.BuddyEmote;

    private enum BuddyEmoteMode : byte
    {
        InviteBuddyEmote = 0x0,
        InviteBuddyEmoteConfirm = 0x1,
        LearnEmote = 0x2,
        AcceptEmote = 0x3,
        DeclineEmote = 0x4,
        StopEmote = 0x6
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        BuddyEmoteMode mode = (BuddyEmoteMode) packet.ReadByte();

        switch (mode)
        {
            case BuddyEmoteMode.InviteBuddyEmote:
                HandleInviteBuddyEmote(session, packet);
                break;
            case BuddyEmoteMode.InviteBuddyEmoteConfirm:
                HandleInviteBuddyEmoteConfirm(session, packet);
                break;
            case BuddyEmoteMode.LearnEmote:
                HandleLearnEmote(session, packet);
                break;
            case BuddyEmoteMode.AcceptEmote:
                HandleAcceptEmote(session, packet);
                break;
            case BuddyEmoteMode.DeclineEmote:
                HandleDeclineEmote(session, packet);
                break;
            case BuddyEmoteMode.StopEmote:
                HandleStopEmote(session, packet);
                break;
            default:
                IPacketHandler<GameSession>.LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleInviteBuddyEmote(GameSession session, PacketReader packet)
    {
        int buddyEmoteId = packet.ReadInt();
        long characterId = packet.ReadLong();

        Player buddy = GameServer.PlayerManager.GetPlayerById(characterId);
        if (buddy == null)
        {
            return;
        }

        buddy.Session.Send(BuddyEmotePacket.SendRequest(buddyEmoteId, session.Player));
    }

    private static void HandleInviteBuddyEmoteConfirm(GameSession session, PacketReader packet)
    {
        long senderCharacterId = packet.ReadLong();

        Player buddy = GameServer.PlayerManager.GetPlayerById(senderCharacterId);
        if (buddy == null)
        {
            return;
        }

        buddy.Session.Send(BuddyEmotePacket.ConfirmSendRequest(session.Player));
    }

    private static void HandleLearnEmote(GameSession session, PacketReader packet)
    {
        long emoteItemUid = packet.ReadLong();
        // TODO grab emoteId from emoteItemUid
        session.Send(BuddyEmotePacket.LearnEmote());
    }

    private static void HandleAcceptEmote(GameSession session, PacketReader packet)
    {
        int buddyEmoteId = packet.ReadInt();
        long senderCharacterId = packet.ReadLong();
        CoordF senderCoords = packet.Read<CoordF>();
        CoordF selfCoords = packet.Read<CoordF>();
        int rotation = packet.ReadInt();

        Player buddy = GameServer.PlayerManager.GetPlayerById(senderCharacterId);
        if (buddy == null)
        {
            return;
        }

        buddy.Session.Send(BuddyEmotePacket.SendAccept(buddyEmoteId, session.Player));
        session.Send(BuddyEmotePacket.StartEmote(buddyEmoteId, buddy.Session.Player, session.Player, selfCoords, rotation));
        buddy.Session.Send(BuddyEmotePacket.StartEmote(buddyEmoteId, buddy.Session.Player, session.Player, selfCoords, rotation));
    }

    private static void HandleDeclineEmote(GameSession session, PacketReader packet)
    {
        int buddyEmoteId = packet.ReadInt();
        long senderCharacterId = packet.ReadLong();

        Player other = GameServer.PlayerManager.GetPlayerById(senderCharacterId);
        if (other == null)
        {
            return;
        }

        other.Session.Send(BuddyEmotePacket.DeclineEmote(buddyEmoteId, session.Player));
    }

    private static void HandleStopEmote(GameSession session, PacketReader packet)
    {
        int buddyEmoteId = packet.ReadInt();
        long target = packet.ReadLong();

        Player buddy = GameServer.PlayerManager.GetPlayerById(target);
        if (buddy == null)
        {
            return;
        }

        buddy.Session.Send(BuddyEmotePacket.StopEmote(buddyEmoteId, session.Player));
    }
}
