using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;
using MapleServer2.Types;

namespace MapleServer2.PacketHandlers.Game.Helpers;

public static class QuestHelper
{
    public static void GetNewQuests(Player player)
    {
        List<QuestMetadata> questList = QuestMetadataStorage.GetAvailableQuests(player.Levels.Level, player.JobCode);
        foreach (QuestMetadata quest in questList)
        {
            if (player.QuestData.ContainsKey(quest.Basic.Id))
            {
                continue;
            }

            player.QuestData.Add(quest.Basic.Id, new(player.CharacterId, quest.Basic.Id));
        }
    }
}
