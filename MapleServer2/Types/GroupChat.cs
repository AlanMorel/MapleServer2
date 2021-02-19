using System;
using System.Collections.Generic;
using System.Linq;
using MaplePacketLib2.Tools;
using MapleServer2.Servers.Game;
using MapleServer2.Tools;

namespace MapleServer2.Types
{
    public class GroupChat
    {
        public int Id { get; }
        public byte MaxMembers { get; }
        public List<Player> Members { get; }

        public GroupChat(Player player)
        {
            Id = GuidGenerator.Int();
            MaxMembers = 20;
            Members = new List<Player> { player };
        }

        public void AddMember(Player player)
        {
            Members.Add(player);

            for (int i = 0; i < 3; i++)
            {
                if (player.GroupChatId[i] == 0)
                {
                    player.GroupChatId[i] = Id;
                    break;
                }
            }
        }

        public void RemoveMember(Player player)
        {
            Members.Remove(player);
            for (int i = 0; i < 3; i++)
            {
                if (player.GroupChatId[i] == Id)
                {
                    player.GroupChatId[i] = 0;
                    break;
                }
            }

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

        public void BroadcastPacketGroupChat(Packet packet, GameSession sender = null)
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

        private List<GameSession> GetSessions()
        {
            return Members.Where(member => member.Session.Connected()).Select(member => member.Session).ToList();
        }
    }
}
