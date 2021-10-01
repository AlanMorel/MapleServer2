using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class InteractObjectParser : Exporter<List<InteractObjectMetadata>>
    {
        public InteractObjectParser(MetadataResources resources) : base(resources, "interact-object") { }

        protected override List<InteractObjectMetadata> Parse()
        {
            List<InteractObjectMetadata> objects = new List<InteractObjectMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("table/interactobject"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                XmlNodeList interactNodes = document.GetElementsByTagName("interact");
                foreach (XmlNode interactNode in interactNodes)
                {
                    string locale = string.IsNullOrEmpty(interactNode.Attributes["locale"]?.Value) ? "" : interactNode.Attributes["locale"].Value;
                    if (locale != "NA" && locale != "")
                    {
                        continue;
                    }

                    InteractObjectMetadata metadata = new InteractObjectMetadata();

                    metadata.Id = int.Parse(interactNode.Attributes["id"].Value);
                    _ = Enum.TryParse(interactNode.Attributes["type"].Value, out metadata.Type);

                    foreach (XmlNode childNode in interactNode)
                    {
                        if (childNode.Name == "reward")
                        {
                            InteractObjectRewardMetadata reward = new InteractObjectRewardMetadata();
                            reward.Exp = int.Parse(childNode.Attributes["exp"].Value);
                            reward.ExpType = childNode.Attributes["expType"].Value;
                            reward.ExpRate = float.Parse(childNode.Attributes["relativeExpRate"].Value);
                            reward.FirstExpType = childNode.Attributes["firstExpType"].Value;
                            reward.FirstExpRate = float.Parse(childNode.Attributes["firstRelativeExpRate"].Value);
                            metadata.Reward = reward;
                        }
                        else if (childNode.Name == "drop")
                        {
                            InteractObjectDropMetadata drop = new InteractObjectDropMetadata();
                            if (!string.IsNullOrEmpty(childNode.Attributes["objectLevel"].Value))
                            {
                                drop.ObjectLevel = childNode.Attributes["objectLevel"].Value;
                            }
                            if (!string.IsNullOrEmpty(childNode.Attributes["objectDropRank"].Value))
                            {
                                drop.DropRank = int.Parse(childNode.Attributes["objectDropRank"].Value);
                            }
                            if (!string.IsNullOrEmpty(childNode.Attributes["globalDropBoxId"].Value))
                            {
                                drop.GlobalDropBoxId.AddRange(Array.ConvertAll(childNode.Attributes["globalDropBoxId"].Value.Split(","), int.Parse));
                            }
                            if (!string.IsNullOrEmpty(childNode.Attributes["individualDropBoxId"].Value))
                            {
                                drop.IndividualDropBoxId.AddRange(Array.ConvertAll(childNode.Attributes["individualDropBoxId"].Value.Split(","), int.Parse));
                            }
                            metadata.Drop = drop;
                        }
                        else if (childNode.Name == "gathering")
                        {
                            InteractObjectGatheringMetadata gathering = new InteractObjectGatheringMetadata();
                            gathering.RecipeId = int.Parse(childNode.Attributes["receipeID"].Value);
                            metadata.Gathering = gathering;
                        }
                        else if (childNode.Name == "webOpen")
                        {
                            InteractObjectWebMetadata web = new InteractObjectWebMetadata();
                            web.Url = childNode.Attributes["url"].Value;
                            metadata.Web = web;
                        }
                    }

                    objects.Add(metadata);
                }
            }
            return objects;
        }
    }
}
