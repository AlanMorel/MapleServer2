using System.Collections.Generic;
using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers
{
    public class AnimationParser : Exporter<List<AnimationMetadata>>
    {
        public AnimationParser(MetadataResources resources) : base(resources, "animation") { }

        protected override List<AnimationMetadata> Parse()
        {
            List<AnimationMetadata> animations = new List<AnimationMetadata>();
            foreach (PackFileEntry entry in Resources.XmlReader.Files)
            {
                if (!entry.Name.StartsWith("anikeytext"))
                {
                    continue;
                }

                XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
                foreach (XmlNode animationNode in document.DocumentElement.ChildNodes)
                {
                    AnimationMetadata metadata = new AnimationMetadata();

                    if (animationNode.Name == "kfm")
                    {
                        metadata.ActorId = animationNode.Attributes["name"].Value;
                    }
                    foreach (XmlNode sequenceNode in animationNode)
                    {
                        if (sequenceNode.Name != "seq")
                        {
                            continue;
                        }

                        SequenceMetadata sequence = new SequenceMetadata();
                        sequence.SequenceId = short.Parse(sequenceNode.Attributes["id"].Value);
                        sequence.SequenceName = sequenceNode.Attributes["name"].Value;
                        foreach (XmlNode keyNode in sequenceNode)
                        {
                            KeyMetadata key = new KeyMetadata();
                            key.KeyName = keyNode.Attributes["name"].Value;
                            key.KeyTime = float.Parse(keyNode.Attributes["time"].Value);
                            sequence.Keys.Add(key);
                        }
                        metadata.Sequence.Add(sequence);
                    }
                    animations.Add(metadata);
                }
            }
            return animations;
        }
    }
}
