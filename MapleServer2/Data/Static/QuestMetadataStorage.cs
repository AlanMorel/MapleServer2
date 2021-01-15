using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class QuestMetadataStorage
    {
        private static readonly Dictionary<int, QuestMetadata> map = new Dictionary<int, QuestMetadata>();

        static QuestMetadataStorage()
        {
            using FileStream stream = File.OpenRead("Maple2Storage/Resources/ms2-quest-metadata");
            List<QuestMetadata> items = Serializer.Deserialize<List<QuestMetadata>>(stream);
            foreach (QuestMetadata item in items)
            {
                map[item.Basic.QuestID] = item;
            }
        }
        public static QuestMetadata GetMetadata(int questId)
        {
            return map.GetValueOrDefault(questId);
        }

        public static int GetQuestsCount()
        {
            return map.Count;
        }

        public static List<QuestMetadata> GetAvailableQuests(int level)
        {
            List<QuestMetadata> list = new List<QuestMetadata>();

            foreach (KeyValuePair<int, QuestMetadata> item in map)
            {
                // Only getting navigator quests to not annoy everyone with quests popping up every restart
                if (level >= item.Value.Require.Level && item.Value.Require.RequiredQuests.Count == 0 && item.Key > 70000000 && item.Key < 70100000) 
                {
                    list.Add(item.Value);
                }
            }

            return list;
        }

        public static Dictionary<int, QuestMetadata> GetAllQuests()
        {
            return map;
        }

        public static bool IsValid(int questId)
        {
            return map.ContainsKey(questId);
        }

    }
}
