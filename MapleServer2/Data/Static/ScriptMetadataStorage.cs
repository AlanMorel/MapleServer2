using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static
{
    public static class ScriptMetadataStorage
    {
        private static readonly Dictionary<int, ScriptMetadata> QuestScripts = new Dictionary<int, ScriptMetadata>();
        private static readonly Dictionary<int, ScriptMetadata> NpcScripts = new Dictionary<int, ScriptMetadata>();

        static ScriptMetadataStorage()
        {
            using FileStream stream = File.OpenRead("Maple2Storage/Resources/ms2-script-metadata");
            List<ScriptMetadata> items = Serializer.Deserialize<List<ScriptMetadata>>(stream);
            foreach (ScriptMetadata item in items)
            {
                if (item.IsQuestScript)
                {
                    QuestScripts[item.Id] = item;
                }
                else
                {
                    NpcScripts[item.Id] = item;
                }
            }
        }
        public static ScriptMetadata GetQuestScriptMetadata(int value)
        {
            return QuestScripts.GetValueOrDefault(value);
        }

        public static ScriptMetadata GetNpcScriptMetadata(int value)
        {
            return NpcScripts.GetValueOrDefault(value);
        }
    }
}
