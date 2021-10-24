using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class QuestMetadataStorage
    {
        private static readonly Dictionary<int, QuestMetadata> map = new Dictionary<int, QuestMetadata>();

        public static void Init()
        {
            using FileStream stream = File.OpenRead($"{Paths.RESOURCES_DIR}/ms2-quest-metadata");
            List<QuestMetadata> items = Serializer.Deserialize<List<QuestMetadata>>(stream);
            foreach (QuestMetadata item in items)
            {
                map[item.Basic.Id] = item;
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
                QuestMetadata questMetadata = item.Value;
                if (level >= questMetadata.Require.Level && questMetadata.Require.RequiredQuests.Count == 0
                    && (questMetadata.Basic.QuestType == QuestType.Epic || questMetadata.Basic.QuestType == QuestType.World))
                {
                    list.Add(questMetadata);
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
