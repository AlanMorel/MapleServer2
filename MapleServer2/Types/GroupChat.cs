using MaplePacketLib2.Tools;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types;

public class GroupChat
{
    public int Id { get; }
    public byte MaxMembers { get; }
    public List<Player> Members { get; }

    public GroupChat(Player player)
    {
        Id = GuidGenerator.Int();
        MaxMembers = 20;
        Members = new();

        AddMember(player);
    }

    public void AddMember(Player player)
    {
        Members.Add(player);
        player.GroupChats.Add(this);
    }

    public void RemoveMember(Player player)
    {
        Members.Remove(player);
        player.GroupChats.Remove(this);
        CheckDisband();
    }

    public void CheckDisband()
    {
        if (Members.Count >= 1)
        {
            return;
        }

        GameServer.GroupChatManager.RemoveGroupChat(this);
    }
    
    public void CheckOfflineGroupChat()
    {
        List<GameSession> sessions = GetSessions();
        if (sessions.Count == 0)
        {
            GameServer.GroupChatManager.RemoveGroupChat(this);
            return;
        }
    }

    public void BroadcastPacketGroupChat(PacketWriter packet, GameSession sender = null)
    {
        BroadcastGroupChat(session =>
        {
            if (session == sender)
            {
                return;
            }

            session.Send(packet);
        });
    }

    public void BroadcastGroupChat(Action<GameSession> action)
    {
        IEnumerable<GameSession> sessions = GetSessions();
        lock (sessions)
        {
            foreach (GameSession session in sessions)
            {
                action?.Invoke(session);
            }
        }
    }

    public List<GameSession> GetSessions()
    {
        List<GameSession> sessions = new();
        foreach (Player member in Members)
        {
            GameSession playerSession = GameServer.PlayerManager.GetPlayerById(member.CharacterId)?.Session;
            if (playerSession != null)
            {
                sessions.Add(playerSession);
            }
        }
        return sessions;
    }
}
