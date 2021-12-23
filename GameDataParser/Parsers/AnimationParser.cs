using System.Xml;
using GameDataParser.Files;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class AnimationParser : Exporter<List<AnimationMetadata>>
{
    public AnimationParser(MetadataResources resources) : base(resources, "animation") { }

    protected override List<AnimationMetadata> Parse()
    {
        List<AnimationMetadata> animations = new();
        foreach (PackFileEntry entry in Resources.XmlReader.Files)
        {
            if (!entry.Name.StartsWith("anikeytext"))
            {
                continue;
            }

            XmlDocument document = Resources.XmlReader.GetXmlDocument(entry);
            foreach (XmlNode animationNode in document.DocumentElement.ChildNodes)
            {
                AnimationMetadata metadata = new();

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

                    SequenceMetadata sequence = new()
                    {
                        SequenceId = short.Parse(sequenceNode.Attributes["id"].Value),
                        SequenceName = sequenceNode.Attributes["name"].Value
                    };
                    foreach (XmlNode keyNode in sequenceNode)
                    {
                        KeyMetadata key = new()
                        {
                            KeyName = keyNode.Attributes["name"].Value,
                            KeyTime = float.Parse(keyNode.Attributes["time"].Value)
                        };
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
