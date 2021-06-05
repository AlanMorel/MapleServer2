using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;
using Microsoft.Extensions.Logging;

namespace MapleServer2.PacketHandlers.Game
{
    public class BuddyHandler : GamePacketHandler
    {
        public override RecvOp OpCode => RecvOp.BUDDY;

        public BuddyHandler(ILogger<BuddyHandler> logger) : base(logger) { }

        private enum BuddyMode : byte
        {
            SendRequest = 0x2,
            Accept = 0x3,
            Decline = 0x4,
            Block = 0x5,
            Unblock = 0x6,
            RemoveFriend = 0x7,
            EditBlockReason = 0xA,
            CancelRequest = 0x11,
        }

        public enum BuddyNotice : byte
        {
            RequestSent = 0x0,
            CharacterNotFound = 0x1,
            RequestAlreadySent = 0x2,
            AlreadyFriends = 0x3,
            CannotAddSelf = 0x4,
            CannotSendRequest = 0x5,
            CannotBlock = 0x6,
            CannotAddFriends = 0x7,
            OtherUserCannotAddFriends = 0x8,
            DeclinedRequest = 0x9,
        }

        public override void Handle(GameSession session, PacketReader packet)
        {
            BuddyMode mode = (BuddyMode) packet.ReadByte();

            switch (mode)
            {
                case BuddyMode.SendRequest:
                    HandleSendRequest(session, packet);
                    break;
                case BuddyMode.Accept:
                    HandleAccept(session, packet);
                    break;
                case BuddyMode.Decline:
                    HandleDecline(session, packet);
                    break;
                case BuddyMode.Block:
                    HandleBlock(session, packet);
                    break;
                case BuddyMode.Unblock:
                    HandleUnblock(session, packet);
                    break;
                case BuddyMode.RemoveFriend:
                    HandleRemoveFriend(session, packet);
                    break;
                case BuddyMode.EditBlockReason:
                    HandleEditBlockReason(session, packet);
                    break;
                case BuddyMode.CancelRequest:
                    HandleCancelRequest(session, packet);
                    break;
                default:
                    IPacketHandler<GameSession>.LogUnknownMode(mode);
                    break;
            }
        }

        private static void HandleSendRequest(GameSession session, PacketReader packet)
        {
            string otherPlayerName = packet.ReadUnicodeString();
            string message = packet.ReadUnicodeString();

            Player targetPlayer = GameServer.Storage.GetPlayerByName(otherPlayerName);
            if (targetPlayer == null) // TODO: Change this so it checks if player exists
            {
                return;
            }

            if (targetPlayer == session.Player)
            {
                session.Send(BuddyPacket.Notice((byte) BuddyNotice.CannotAddSelf, targetPlayer.Name));
                return;
            }

            if ((byte) session.Player.BuddyList.Count(b => !b.Blocked) >= 100) // 100 is friend limit
            {
                session.Send(BuddyPacket.Notice((byte) BuddyNotice.CannotAddFriends, targetPlayer.Name));
            }

            if ((byte) targetPlayer.BuddyList.Count(b => !b.Blocked) >= 100)
            {
                session.Send(BuddyPacket.Notice((byte) BuddyNotice.OtherUserCannotAddFriends, targetPlayer.Name));
                return;
            }

            if (BuddyManager.IsBlocked(session.Player, targetPlayer))
            {
                session.Send(BuddyPacket.Notice((byte) BuddyNotice.DeclinedRequest, targetPlayer.Name));
                return;
            }

            if (BuddyManager.IsFriend(session.Player, targetPlayer))
            {
                session.Send(BuddyPacket.Notice((byte) BuddyNotice.AlreadyFriends, targetPlayer.Name));
            }

            long id = GuidGenerator.Long();
            Buddy buddy = new Buddy(id, targetPlayer, session.Player, message, true, false);
            Buddy buddyTargetPlayer = new Buddy(id, session.Player, targetPlayer, message, false, true);
            GameServer.BuddyManager.AddBuddy(buddy);
            GameServer.BuddyManager.AddBuddy(buddyTargetPlayer);

            session.Send(BuddyPacket.Notice((byte) BuddyNotice.RequestSent, targetPlayer.Name));

            if (targetPlayer != null) // TODO: Change this to only send if player exists
            {
                session.Send(BuddyPacket.AddToList(buddy));
            }

            if (targetPlayer != null) // TODO: Change to send if online
            {
                targetPlayer.Session.Send(BuddyPacket.AddToList(buddyTargetPlayer));
                return;
            }
        }

        private static void HandleRemoveFriend(GameSession session, PacketReader packet)
        {
            long buddyId = packet.ReadLong();

            Buddy buddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(session.Player, buddyId);
            Buddy buddyFriend = GameServer.BuddyManager.GetBuddyByPlayerAndId(buddy.Friend, buddyId);

            session.Send(BuddyPacket.RemoveFromList(buddy));

            if (buddy.Friend != null) // TODO: Change to send if online
            {
                buddy.Friend.Session.Send(BuddyPacket.RemoveFromList(buddyFriend));
            }

            GameServer.BuddyManager.RemoveBuddy(buddy);
            GameServer.BuddyManager.RemoveBuddy(buddyFriend);
            session.Player.BuddyList.Remove(buddy);
            buddy.Friend.BuddyList.Remove(buddyFriend);
        }

        private static void HandleEditBlockReason(GameSession session, PacketReader packet)
        {
            long buddyId = packet.ReadLong();
            string otherPlayerName = packet.ReadUnicodeString();
            string newBlockReason = packet.ReadUnicodeString();

            Buddy buddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(session.Player, buddyId);
            if (buddy == null || otherPlayerName != buddy.Friend.Name)
            {
                return;
            }

            buddy.Message = newBlockReason;
            session.Send(BuddyPacket.EditBlockReason(buddy));
        }

        private static void HandleAccept(GameSession session, PacketReader packet)
        {
            long buddyId = packet.ReadLong();

            Buddy buddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(session.Player, buddyId);
            Buddy buddyFriend = GameServer.BuddyManager.GetBuddyByPlayerAndId(buddy.Friend, buddyId);

            buddy.IsFriendRequest = false;
            buddyFriend.IsPending = false;

            session.Send(BuddyPacket.AcceptRequest(buddy));
            session.Send(BuddyPacket.UpdateBuddy(buddy));
            session.Send(BuddyPacket.LoginLogoutNotifcation(buddy));

            if (buddy.Friend != null) // TODO: Change to send if online
            {
                buddy.Friend.Session.Send(BuddyPacket.UpdateBuddy(buddyFriend));
                buddy.Friend.Session.Send(BuddyPacket.AcceptNotification(buddyFriend));
            }
        }

        private static void HandleDecline(GameSession session, PacketReader packet)
        {
            long buddyId = packet.ReadLong();

            Buddy buddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(session.Player, buddyId);
            Buddy buddyFriend = GameServer.BuddyManager.GetBuddyByPlayerAndId(buddy.Friend, buddyId);

            session.Send(BuddyPacket.DeclineRequest(buddy));

            if (buddy.Friend != null) // TODO: Change to send if online
            {
                buddy.Friend.Session.Send(BuddyPacket.RemoveFromList(buddyFriend));
            }

            GameServer.BuddyManager.RemoveBuddy(buddy);
            GameServer.BuddyManager.RemoveBuddy(buddyFriend);
            session.Player.BuddyList.Remove(buddy);
            buddy.Friend.BuddyList.Remove(buddyFriend);
        }

        private static void HandleBlock(GameSession session, PacketReader packet)
        {
            long buddyId = packet.ReadLong();
            string target = packet.ReadUnicodeString();
            string message = packet.ReadUnicodeString();

            if ((byte) session.Player.BuddyList.Count(b => b.Blocked) >= 100)  // 100 is block limit
            {
                session.Send(BuddyPacket.Notice((byte) BuddyNotice.CannotBlock, target));
                return;
            }

            Player targetPlayer = GameServer.Storage.GetPlayerByName(target);

            if (buddyId == 0) // if buddy doesn't exist, create Buddy
            {
                long id = GuidGenerator.Long();
                Buddy buddy = new Buddy(id, targetPlayer, session.Player, message, false, false, true);
                GameServer.BuddyManager.AddBuddy(buddy);

                session.Send(BuddyPacket.AddToList(buddy));
                session.Send(BuddyPacket.Block(buddy));
            }
            else
            {
                Buddy buddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(session.Player, buddyId);
                Buddy buddyFriend = GameServer.BuddyManager.GetBuddyByPlayerAndId(buddy.Friend, buddyId);
                if (buddy.Friend != null)  // TODO: Change to send if online
                {
                    buddy.Friend.Session.Send(BuddyPacket.RemoveFromList(buddyFriend));
                }

                GameServer.BuddyManager.RemoveBuddy(buddyFriend);
                buddy.Friend.BuddyList.Remove(buddyFriend);

                buddy.BlockReason = message;
                buddy.Blocked = true;
                session.Send(BuddyPacket.UpdateBuddy(buddy));
                session.Send(BuddyPacket.Block(buddy));
            }
        }

        private static void HandleUnblock(GameSession session, PacketReader packet)
        {
            long buddyId = packet.ReadLong();

            Buddy buddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(session.Player, buddyId);

            session.Send(BuddyPacket.Unblock(buddy));
            session.Send(BuddyPacket.RemoveFromList(buddy));

            GameServer.BuddyManager.RemoveBuddy(buddy);
            session.Player.BuddyList.Remove(buddy);
        }

        private static void HandleCancelRequest(GameSession session, PacketReader packet)
        {
            long buddyId = packet.ReadLong();

            Buddy buddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(session.Player, buddyId);
            Buddy buddyFriend = GameServer.BuddyManager.GetBuddyByPlayerAndId(buddy.Friend, buddyId);

            session.Send(BuddyPacket.CancelRequest(buddy));

            if (buddy.Friend != null)  // TODO: Change to send if online
            {
                buddy.Friend.Session.Send(BuddyPacket.RemoveFromList(buddyFriend));
            }

            GameServer.BuddyManager.RemoveBuddy(buddy);
            GameServer.BuddyManager.RemoveBuddy(buddyFriend);
            session.Player.BuddyList.Remove(buddy);
            buddy.Friend.BuddyList.Remove(buddyFriend);
        }

    }
}
