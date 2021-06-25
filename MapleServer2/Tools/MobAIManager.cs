using System;
using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using Maple2Storage.Enums;
using MapleServer2.Types;

namespace MapleServer2.Tools
{
    public static class MobAIManager
    {
        private static readonly Dictionary<string, MobAI> AiTable = new Dictionary<string, MobAI>();

        public static void Load(string dirPath, string schemaPath = null)
        {
            Console.WriteLine("Loading AI...");
            foreach (string filePath in Directory.GetFiles(dirPath, "*.xml", SearchOption.AllDirectories))
            {
                string filename = filePath.Split(@"MobAI\")[1];
                XmlDocument document = new XmlDocument();
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
                    Console.WriteLine($"Skipping {filename}");
                    continue;
                }
                catch (XmlSchemaValidationException e)
                {
                    Console.WriteLine($"{filename} is invalid:");
                    Console.WriteLine(e);
                    continue;
                }

                ParseAI(document);
                Console.WriteLine($"Loaded {filename}");
            }
            Console.WriteLine("Finished loading AI.");
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
                NpcState stateValue = GetMobState(node.Name);
                NpcAction newActionValue = GetMobAction(node.Attributes["action"]?.Value);
                MobMovement movementValue = GetMobMovement(node.Attributes["movement"]?.Value);
                MobAI.Condition[] conditions = GetConditions(node);

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
                _ => MobMovement.Hold,
            };
        }

        private static MobAI.Condition[] GetConditions(XmlNode node)
        {
            // TODO: Parse actions' conditions
            return Array.Empty<MobAI.Condition>();
        }
    }
}
