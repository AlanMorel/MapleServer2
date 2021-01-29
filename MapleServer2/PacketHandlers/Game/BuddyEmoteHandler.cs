﻿using System;
using System.Collections.Generic;
using Maple2Storage.Types;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class BuddyEmoteHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.BUDDY_EMOTE;

        public BuddyEmoteHandler(ILogger<BuddyEmoteHandler> logger) : base(logger) { }

        private enum BuddyEmoteMode : byte
        {
            InviteBuddyEmote = 0x0,
            InviteBuddyEmoteConfirm = 0x1,
            LearnEmote = 0x2,
            AcceptEmote = 0x3,
            DeclineEmote = 0x4,
            StopEmote = 0x6,
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

        private void HandleInviteBuddyEmote(GameSession session, PacketReader packet)
        {
            int buddyEmoteId = packet.ReadInt();
            long characterId = packet.ReadLong();

            Player buddy = GameServer.Storage.GetPlayerById(characterId);
            if (buddy == null)
            {
                return;
            }

            buddy.Session.Send(BuddyEmotePacket.SendRequest(buddyEmoteId, session.Player));
        }

        private void HandleInviteBuddyEmoteConfirm(GameSession session, PacketReader packet)
        {
            long senderCharacterId = packet.ReadLong();

            Player buddy = GameServer.Storage.GetPlayerById(senderCharacterId);
            if (buddy == null)
            {
                return;
            }

            buddy.Session.Send(BuddyEmotePacket.ConfirmSendRequest(session.Player));
        }

        private void HandleLearnEmote(GameSession session, PacketReader packet)
        {
            long emoteItemUid = packet.ReadLong();
            // TODO grab emoteId from emoteItemUid
            session.Send(BuddyEmotePacket.LearnEmote());
        }

        private void HandleAcceptEmote(GameSession session, PacketReader packet)
        {
            int buddyEmoteId = packet.ReadInt();
            long senderCharacterId = packet.ReadLong();
            CoordF senderCoords = packet.Read<CoordF>();
            CoordF selfCoords = packet.Read<CoordF>();
            int rotation = packet.ReadInt();

            Player buddy = GameServer.Storage.GetPlayerById(senderCharacterId);
            if (buddy == null)
            {
                return;
            }

            buddy.Session.Send(BuddyEmotePacket.SendAccept(buddyEmoteId, session.Player));
            session.Send(BuddyEmotePacket.StartEmote(buddyEmoteId, buddy.Session.Player, session.Player, selfCoords, rotation));
            buddy.Session.Send(BuddyEmotePacket.StartEmote(buddyEmoteId, buddy.Session.Player, session.Player, selfCoords, rotation));
        }

        private void HandleDeclineEmote(GameSession session, PacketReader packet)
        {
            int buddyEmoteId = packet.ReadInt();
            long senderCharacterId = packet.ReadLong();

            Player other = GameServer.Storage.GetPlayerById(senderCharacterId);
            if (other == null)
            {
                return;
            }

            other.Session.Send(BuddyEmotePacket.DeclineEmote(buddyEmoteId, session.Player));
        }

        private void HandleStopEmote(GameSession session, PacketReader packet)
        {
            int buddyEmoteId = packet.ReadInt();
            long target = packet.ReadLong();

            Player buddy = GameServer.Storage.GetPlayerById(target);
            if (buddy == null)
            {
                return;
            }

            buddy.Session.Send(BuddyEmotePacket.StopEmote(buddyEmoteId, session.Player));
        }
    }
}
