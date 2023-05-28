using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using M2dXmlGenerator;
using Maple2.File.Parser.Tools;
using Maple2.File.Parser.Xml.Quest;
using Maple2Storage.Enums;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class QuestParser : Exporter<List<QuestMetadata>>
{
    public QuestParser(MetadataResources resources) : base(resources, MetadataName.Quest) { }

    protected override List<QuestMetadata> Parse()
    {
        List<QuestMetadata> quests = new();
        Filter.Load(Resources.XmlReader, "NA", "Live");
        Maple2.File.Parser.QuestParser questParser = new(Resources.XmlReader);
        foreach ((int id, string name, QuestData data) in questParser.Parse())
        {
            QuestMetadata metadata = new()
            {
                Basic = new()
                {
                    AutoStart = data.basic.autoStart,
                    ChapterID = data.basic.chapterID,
                    Id = id,
                    QuestType = (QuestType) data.basic.questType
                },
                Require = new()
                {
                    Job = data.require.job.Select(x => (short) x).ToList(),
                    Level = data.require.level,
                    MaxLevel = data.require.maxLevel,
                    RequiredQuests = data.require.quest.ToList()
                },
                StartNpc = data.start?.npc ?? 0,
                CompleteNpc = data.complete?.npc ?? 0,
                Condition = data.condition.Select(x => new QuestCondition
                {
                    Code = x._code,
                    Goal = (int) x.value,
                    Target = x._target,
                    Type = x._type
                }).ToList(),
                Dispatch = new()
                {
                    Type = data.dispatch?.type,
                    FieldId = data.dispatch?.field ?? 0,
                    PortalId = (short) (data.dispatch?.portal ?? 0),
                    ScriptId = data.dispatch?.script ?? 0
                },
                Dungeon = new()
                {
                    GoToDungeon = data.gotoDungeon.gotoDungeon,
                    GoToInstanceID = data.gotoDungeon.gotoInstanceID,
                    State = (byte) data.gotoDungeon.state
                },
                Name = name,
                Npc = new()
                {
                    Enable = data.gotoNpc.enable,
                    GoToField = data.gotoNpc.gotoField,
                    GoToPortal = data.gotoNpc.gotoPortal
                },
                ProgressMap = data.progressMap.progressMap.ToList(),
                Reward = new()
                {
                    Exp = data.completeReward.exp,
                    Karma = data.completeReward.karma,
                    Lu = data.completeReward.lu,
                    Money = data.completeReward.money,
                    RelativeExp = data.completeReward._relativeExp
                },
                SummonPortal = new()
                {
                    FieldID = data.summonPortal.fieldID,
                    PortalID = data.summonPortal.portalID
                }
            };

            List<Reward.Item> essentialItem = data.completeReward.essentialItem;
            List<Reward.Item> essentialJobItem = data.completeReward.essentialJobItem;
            List<Reward.Item> acceptEssentialItem = data.acceptReward.essentialItem;

            if (FeatureLocaleFilter.FeatureEnabled("GlobalQuestRewardItem"))
            {
                essentialItem = data.completeReward.globalEssentialItem.Count == 0 ? essentialItem : data.completeReward.globalEssentialItem;
                essentialJobItem = data.completeReward.globalEssentialJobItem.Count == 0 ? essentialJobItem : data.completeReward.globalEssentialJobItem;
                acceptEssentialItem = data.acceptReward.globalEssentialItem.Count == 0 ? acceptEssentialItem : data.acceptReward.globalEssentialItem;
            }

            metadata.RewardItem = essentialItem.Where(x => x.code != 0).Select(x => new QuestRewardItem()
            {
                Code = x.code,
                Count = x.count,
                Rank = (byte) x.rank
            }).ToList();

            metadata.RewardItem.AddRange(essentialJobItem.Where(x => x.code != 0).Select(x => new QuestRewardItem()
            {
                Code = x.code,
                Count = x.count,
                Rank = (byte) x.rank
            }).ToList());

            metadata.AcceptRewardItem = acceptEssentialItem.Where(x => x.code != 0).Select(x => new QuestRewardItem()
            {
                Code = x.code,
                Count = x.count,
                Rank = (byte) x.rank
            }).ToList();

            quests.Add(metadata);
        }


        return quests;
    }
}
