using System.Collections.Generic;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public class GroupChatManager
    {
        private readonly Dictionary<long, GroupChat> GroupChatList;

        public GroupChatManager()
        {
            GroupChatList = new Dictionary<long, GroupChat>();
        }

        public void AddGroupChat(GroupChat groupChat)
        {
            GroupChatList.Add(groupChat.Id, groupChat);
        }

        public void RemoveGroupChat(GroupChat groupChat)
        {
            GroupChatList.Remove(groupChat.Id);
        }

        public GroupChat GetGroupChatById(int id)
        {
            return GroupChatList.TryGetValue(id, out GroupChat foundGroupChat) ? foundGroupChat : null;
        }
    }
}
