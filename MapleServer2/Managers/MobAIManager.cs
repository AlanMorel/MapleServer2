using System.Xml;
using System.Xml.Schema;
using Maple2Storage.Enums;
using MapleServer2.Enums;
using MapleServer2.Types;
using NLog;

namespace MapleServer2.Managers;

public static class MobAIManager
{
    private static readonly Dictionary<string, MobAI> AiTable = new();
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

    public static void Load(string dirPath, string schemaPath = null)
    {
        Logger.Info("Loading Mob AI...");
        foreach (string filePath in Directory.GetFiles(dirPath, "*.xml", SearchOption.AllDirectories))
        {
            string filename = Path.GetFileName(filePath);
            XmlDocument document = new();
            try
            {
                document.Load(filePath);
                if (schemaPath != null)
                {
                    document.Schemas.Add(null, schemaPath);
                    document.Validate(null);
                }
            }
            catch (XmlException)
            {
                Logger.Warn($"Skipping {filename}");
                continue;
            }
            catch (XmlSchemaValidationException e)
            {
                Logger.Warn($"{filename} is invalid:");
                Logger.Warn(e);
                continue;
            }

            ParseAI(document);
            Logger.Info($"Loaded {filename}");
        }
        Logger.Info("Finished loading AI.");
    }

    public static MobAI GetAI(string aiInfo)
    {
        return AiTable.GetValueOrDefault(aiInfo, null);
    }

    private static void ParseAI(XmlDocument document)
    {
        XmlNode behaviorsNode = document.SelectSingleNode("/ai/behavior");

        MobAI ai = new();
        foreach (XmlNode node in behaviorsNode)
        {
            NpcState stateValue = GetMobState(node.Name);
            NpcAction newActionValue = GetMobAction(node.Attributes["action"]?.Value);
            MobMovement movementValue = GetMobMovement(node.Attributes["movement"]?.Value);
            IEnumerable<MobAI.Condition> conditions = GetConditions( /*node*/);

            ai.Rules.TryAdd(stateValue, (newActionValue, movementValue, Array.Empty<MobAI.Condition>()));
        }

        string aiName = document.BaseURI.Split("MobAI/")[1];
        AiTable.Add(aiName, ai);
    }

    private static NpcState GetMobState(string stateId)
    {
        return stateId switch
        {
            "spawn" => NpcState.Spawn,
            "normal" => NpcState.Normal,
            "combat" => NpcState.Combat,
            "dead" => NpcState.Dead,
            _ => NpcState.Normal
        };
    }

    private static NpcAction GetMobAction(string actionId)
    {
        return actionId switch
        {
            _ => NpcAction.None
        };
    }

    private static MobMovement GetMobMovement(string movementId)
    {
        return movementId switch
        {
            "patrol" => MobMovement.Patrol,
            "follow" => MobMovement.Follow,
            "strafe" => MobMovement.Strafe,
            "run" => MobMovement.Run,
            "dodge" => MobMovement.Dodge,
            _ => MobMovement.Hold
        };
    }

    private static IEnumerable<MobAI.Condition> GetConditions( /*XmlNode node*/)
    {
        // TODO: Parse actions' conditions
        return Array.Empty<MobAI.Condition>();
    }
}
