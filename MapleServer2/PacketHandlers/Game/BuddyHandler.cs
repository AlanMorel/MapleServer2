using MaplePacketLib2.Tools;
using MapleServer2.Constants;
using MapleServer2.Database;
using MapleServer2.Managers;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game;

public class BuddyHandler : GamePacketHandler<BuddyHandler>
{
    public override RecvOp OpCode => RecvOp.Buddy;

    private enum Mode : byte
    {
        SendRequest = 0x2,
        Accept = 0x3,
        Decline = 0x4,
        Block = 0x5,
        Unblock = 0x6,
        RemoveFriend = 0x7,
        EditBlockReason = 0xA,
        CancelRequest = 0x11
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
        DeclinedRequest = 0x9
    }

    public override void Handle(GameSession session, PacketReader packet)
    {
        Mode mode = (Mode) packet.ReadByte();

        switch (mode)
        {
            case Mode.SendRequest:
                HandleSendRequest(session, packet);
                break;
            case Mode.Accept:
                HandleAccept(session, packet);
                break;
            case Mode.Decline:
                HandleDecline(session, packet);
                break;
            case Mode.Block:
                HandleBlock(session, packet);
                break;
            case Mode.Unblock:
                HandleUnblock(session, packet);
                break;
            case Mode.RemoveFriend:
                HandleRemoveFriend(session, packet);
                break;
            case Mode.EditBlockReason:
                HandleEditBlockReason(session, packet);
                break;
            case Mode.CancelRequest:
                HandleCancelRequest(session, packet);
                break;
            default:
                LogUnknownMode(mode);
                break;
        }
    }

    private static void HandleSendRequest(GameSession session, PacketReader packet)
    {
        string otherPlayerName = packet.ReadUnicodeString();
        string message = packet.ReadUnicodeString();

        if (!DatabaseManager.Characters.NameExists(otherPlayerName))
        {
            session.Send(BuddyPacket.Notice((byte) BuddyNotice.CharacterNotFound, otherPlayerName));
            return;
        }

        Player targetPlayer = GameServer.PlayerManager.GetPlayerByName(otherPlayerName);
        if (targetPlayer == null) // If the player is not online, get player data from database
        {
            targetPlayer = DatabaseManager.Characters.FindPartialPlayerByName(otherPlayerName);
            targetPlayer.BuddyList = GameServer.BuddyManager.GetBuddies(targetPlayer.CharacterId);
        }

        if (targetPlayer.CharacterId == session.Player.CharacterId)
        {
            session.Send(BuddyPacket.Notice((byte) BuddyNotice.CannotAddSelf, targetPlayer.Name));
            return;
        }

        if (session.Player.BuddyList.Count(b => !b.Blocked) >= 100) // 100 is friend limit
        {
            session.Send(BuddyPacket.Notice((byte) BuddyNotice.CannotAddFriends, targetPlayer.Name));
            return;
        }

        if (targetPlayer.BuddyList.Count(b => !b.Blocked) >= 100)
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
            return;
        }

        long id = GuidGenerator.Long();
        Buddy buddy = new(id, session.Player.CharacterId, targetPlayer, message, true, false);
        Buddy buddyTargetPlayer = new(id, targetPlayer.CharacterId, session.Player, message, false, true);
        GameServer.BuddyManager.AddBuddy(buddy);
        GameServer.BuddyManager.AddBuddy(buddyTargetPlayer);
        session.Player.BuddyList.Add(buddy);

        session.Send(BuddyPacket.Notice((byte) BuddyNotice.RequestSent, targetPlayer.Name));
        session.Send(BuddyPacket.AddToList(buddy));

        if (targetPlayer.Session != null && targetPlayer.Session.Connected())
        {
            targetPlayer.BuddyList.Add(buddyTargetPlayer);
            targetPlayer.Session.Send(BuddyPacket.AddToList(buddyTargetPlayer));
        }
    }

    private static void HandleRemoveFriend(GameSession session, PacketReader packet)
    {
        long buddyId = packet.ReadLong();

        Buddy buddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(session.Player, buddyId);
        Buddy buddyFriend = GameServer.BuddyManager.GetBuddyByPlayerAndId(buddy.Friend, buddyId);

        session.Send(BuddyPacket.RemoveFromList(buddy));

        Player otherPlayer = GameServer.PlayerManager.GetPlayerByName(buddy.Friend.Name);
        if (otherPlayer != null)
        {
            otherPlayer.Session.Send(BuddyPacket.RemoveFromList(buddyFriend));
            otherPlayer.BuddyList.Remove(buddyFriend);
        }

        GameServer.BuddyManager.RemoveBuddy(buddy);
        GameServer.BuddyManager.RemoveBuddy(buddyFriend);
        session.Player.BuddyList.Remove(buddy);
        DatabaseManager.Buddies.Delete(buddy.Id);
        DatabaseManager.Buddies.Delete(buddyFriend.Id);
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
        DatabaseManager.Buddies.Update(buddy);
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
        session.Send(BuddyPacket.LoginLogoutNotification(buddy));
        DatabaseManager.Buddies.Update(buddy);
        DatabaseManager.Buddies.Update(buddyFriend);

        Player otherPlayer = GameServer.PlayerManager.GetPlayerByName(buddy.Friend.Name);
        if (otherPlayer != null)
        {
            otherPlayer.Session.Send(BuddyPacket.UpdateBuddy(buddyFriend));
            otherPlayer.Session.Send(BuddyPacket.AcceptNotification(buddyFriend));
        }
    }

    private static void HandleDecline(GameSession session, PacketReader packet)
    {
        long buddyId = packet.ReadLong();

        Buddy buddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(session.Player, buddyId);
        Buddy buddyFriend = GameServer.BuddyManager.GetBuddyByPlayerAndId(buddy.Friend, buddyId);

        session.Send(BuddyPacket.DeclineRequest(buddy));

        Player otherPlayer = GameServer.PlayerManager.GetPlayerByName(buddy.Friend.Name);
        if (otherPlayer != null)
        {
            otherPlayer.Session.Send(BuddyPacket.RemoveFromList(buddyFriend));
            otherPlayer.BuddyList.Remove(buddyFriend);
        }

        GameServer.BuddyManager.RemoveBuddy(buddy);
        GameServer.BuddyManager.RemoveBuddy(buddyFriend);
        session.Player.BuddyList.Remove(buddy);
        DatabaseManager.Buddies.Delete(buddy.Id);
        DatabaseManager.Buddies.Delete(buddyFriend.Id);
    }

    private static void HandleBlock(GameSession session, PacketReader packet)
    {
        long buddyId = packet.ReadLong();
        string targetName = packet.ReadUnicodeString();
        string message = packet.ReadUnicodeString();

        if (session.Player.BuddyList.Count(b => b.Blocked) >= 100) // 100 is block limit
        {
            session.Send(BuddyPacket.Notice((byte) BuddyNotice.CannotBlock, targetName));
            return;
        }

        if (!DatabaseManager.Characters.NameExists(targetName))
        {
            session.Send(BuddyPacket.Notice((byte) BuddyNotice.CharacterNotFound, targetName));
            return;
        }

        Player targetPlayer = GameServer.PlayerManager.GetPlayerByName(targetName);
        if (targetPlayer == null) // If the player is not online, get player data from database
        {
            targetPlayer = DatabaseManager.Characters.FindPartialPlayerByName(targetName);
            targetPlayer.BuddyList = GameServer.BuddyManager.GetBuddies(targetPlayer.CharacterId);
        }

        if (buddyId == 0) // if buddy doesn't exist, create Buddy
        {
            long id = GuidGenerator.Long();
            Buddy buddy = new(id, session.Player.CharacterId, targetPlayer, message, false, false, true);
            GameServer.BuddyManager.AddBuddy(buddy);
            session.Player.BuddyList.Add(buddy);

            session.Send(BuddyPacket.AddToList(buddy));
            session.Send(BuddyPacket.Block(buddy));
        }
        else
        {
            Buddy buddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(session.Player, buddyId);
            Buddy buddyFriend = GameServer.BuddyManager.GetBuddyByPlayerAndId(buddy.Friend, buddyId);

            if (targetPlayer.Session != null && targetPlayer.Session.Connected())
            {
                targetPlayer.BuddyList.Remove(buddyFriend);
                targetPlayer.Session.Send(BuddyPacket.RemoveFromList(buddyFriend));
            }

            GameServer.BuddyManager.RemoveBuddy(buddyFriend);

            buddy.BlockReason = message;
            buddy.Blocked = true;
            session.Send(BuddyPacket.UpdateBuddy(buddy));
            session.Send(BuddyPacket.Block(buddy));
            DatabaseManager.Buddies.Update(buddy);
            DatabaseManager.Buddies.Delete(buddyFriend.Id);
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
        DatabaseManager.Buddies.Delete(buddy.Id);
    }

    private static void HandleCancelRequest(GameSession session, PacketReader packet)
    {
        long buddyId = packet.ReadLong();

        Buddy buddy = GameServer.BuddyManager.GetBuddyByPlayerAndId(session.Player, buddyId);
        Buddy buddyFriend = GameServer.BuddyManager.GetBuddyByPlayerAndId(buddy.Friend, buddyId);

        session.Send(BuddyPacket.CancelRequest(buddy));

        Player targetPlayer = GameServer.PlayerManager.GetPlayerByName(buddy.Friend.Name);

        if (targetPlayer != null)
        {
            targetPlayer.Session.Send(BuddyPacket.RemoveFromList(buddyFriend));
            targetPlayer.BuddyList.Remove(buddyFriend);
        }

        GameServer.BuddyManager.RemoveBuddy(buddy);
        GameServer.BuddyManager.RemoveBuddy(buddyFriend);
        session.Player.BuddyList.Remove(buddy);
        DatabaseManager.Buddies.Delete(buddy.Id);
        DatabaseManager.Buddies.Delete(buddyFriend.Id);
    }
}
