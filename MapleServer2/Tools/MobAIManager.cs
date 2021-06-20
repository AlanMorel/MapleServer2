using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public static class MobAIManager
    {
        private static readonly Dictionary<string, MobAI> AiTable = new Dictionary<string, MobAI>();

        public static void Load(string dirPath, string schemaPath = "")
        {
            foreach (string filePath in Directory.GetFiles(dirPath))
            {
                if (!filePath.EndsWith(".xml"))
                {
                    continue;
                }

                XmlDocument document = new XmlDocument();
                document.Load(filePath);
                if (schemaPath.Length > 0)
                {
                    document.Schemas.Add(null, schemaPath);
                    try
                    {
                        document.Validate(null);
                    }
                    catch (XmlSchemaValidationException)
                    {
                        // TODO: log invalid AI files
                        continue;
                    }
                }
                ParseAI(document);
                // TODO: log parsed AI files
            }
        }

        public static MobAI GetAI(string aiInfo)
        {
            return AiTable.GetValueOrDefault(aiInfo, null);
        }

        private static void ParseAI(XmlDocument document)
        {
            XmlNode behaviorsNode = document.SelectSingleNode("/ai/behavior");

            MobAI ai = new MobAI();
            foreach (XmlNode node in behaviorsNode)
            {
                MobAction newActionValue = GetMobAction(node.Attributes["action"]?.Value);
                MobMovement movementValue = GetMobMovement(node.Attributes["movement"]?.Value);

                if (node.Name == "spawn")
                {
                    ai.Rules.Add(MobState.Spawn, (newActionValue, movementValue, Array.Empty<MobAI.Condition>()));
                }
                else if (node.Name == "normal")
                {
                    ai.Rules.Add(MobState.Normal, (newActionValue, movementValue, Array.Empty<MobAI.Condition>()));
                }
                else if (node.Name == "aggro")
                {
                    ai.Rules.Add(MobState.Aggro, (newActionValue, movementValue, Array.Empty<MobAI.Condition>()));
                }
                // TODO: Rest of the states
            }

            string aiName = Path.GetFileName(document.BaseURI);
            AiTable.Add(aiName, ai);
        }

        private static MobState GetMobState(string stateId)
        {
            return stateId switch
            {
                "spawn" => MobState.Spawn,
                "normal" => MobState.Normal,
                "aggro" => MobState.Aggro,
                "dead" => MobState.Dead,
                _ => MobState.Normal
            };
        }

        private static MobAction GetMobAction(string actionId)
        {
            return actionId switch
            {
                _ => MobAction.None
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
                _ => MobMovement.Hold,
            };
        }
    }
}
