using MapleServer2.Types;

namespace MapleServer2.Managers;

public class GroupChatManager
{
    private readonly Dictionary<long, GroupChat> GroupChatList;

    public GroupChatManager()
    {
        GroupChatList = new();
    }

    public void AddGroupChat(GroupChat groupChat)
    {
        GroupChatList.Add(groupChat.Id, groupChat);
    }

    public void RemoveGroupChat(GroupChat groupChat)
    {
        GroupChatList.Remove(groupChat.Id);
    }

    public List<GroupChat> GetGroupChatsByMember(long characterId)
    {
        return GroupChatList.Values.Where(x => x.Members.FirstOrDefault(z => z.CharacterId == characterId) != null).ToList();
    }

    public GroupChat GetGroupChatById(int id)
    {
        return GroupChatList.TryGetValue(id, out GroupChat foundGroupChat) ? foundGroupChat : null;
    }
}
