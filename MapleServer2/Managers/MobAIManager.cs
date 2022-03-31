using System.Xml;
using System.Xml.Schema;
using Maple2Storage.Enums;
using MapleServer2.Enums;
using MapleServer2.Types;
using Serilog;

namespace MapleServer2.Managers;

public static class MobAIManager
{
    private static readonly Dictionary<string, MobAI> AiTable = new();
    private static readonly ILogger Logger = Log.Logger.ForContext(typeof(MobAIManager));

    public static void Load(string dirPath, string schemaPath = null)
    {
        Logger.Information("Loading Mob AI...");
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
                Logger.Warning("Skipping {filename}", filename);
                continue;
            }
            catch (XmlSchemaValidationException e)
            {
                Logger.Warning("{filename} is invalid: {e}", filename, e);
                continue;
            }

            ParseAI(document);
            Logger.Information("Loaded {filename}", filename);
        }
        Logger.Information("Finished loading AI.");
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
