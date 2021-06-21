﻿using System;
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
            Console.WriteLine($"Loading AI...");
            foreach (string filePath in Directory.GetFiles(dirPath))
            {
                if (!filePath.EndsWith(".xml"))
                {
                    continue;
                }

                XmlDocument document = new XmlDocument();
                document.Load(filePath);
                if (schemaPath != null)
                {
                    document.Schemas.Add(null, schemaPath);
                    try
                    {
                        document.Validate(null);
                    }
                    catch (XmlSchemaValidationException e)
                    {
                        Console.WriteLine($"{Path.GetFileName(document.BaseURI)} is invalid:");
                        Console.WriteLine(e);
                        continue;
                    }
                }
                ParseAI(document);
                Console.WriteLine($"Loaded {Path.GetFileName(document.BaseURI)}");
            }
            Console.WriteLine($"Finished loading AI.");
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
                NpcAction newActionValue = GetMobAction(node.Attributes["action"]?.Value);
                MobMovement movementValue = GetMobMovement(node.Attributes["movement"]?.Value);

                if (node.Name == "spawn")
                {
                    ai.Rules.Add(NpcState.Spawn, (newActionValue, movementValue, Array.Empty<MobAI.Condition>()));
                }
                else if (node.Name == "normal")
                {
                    ai.Rules.Add(NpcState.Normal, (newActionValue, movementValue, Array.Empty<MobAI.Condition>()));
                }
                else if (node.Name == "aggro")
                {
                    ai.Rules.Add(NpcState.Aggro, (newActionValue, movementValue, Array.Empty<MobAI.Condition>()));
                }
                // TODO: Rest of the states
            }

            string aiName = Path.GetFileName(document.BaseURI);
            AiTable.Add(aiName, ai);
        }

        private static NpcState GetMobState(string stateId)
        {
            return stateId switch
            {
                "spawn" => NpcState.Spawn,
                "normal" => NpcState.Normal,
                "aggro" => NpcState.Aggro,
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
    }
}
