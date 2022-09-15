using System.Xml;
using GameDataParser.Files;
using GameDataParser.Files.MetadataExporter;
using GameDataParser.Parsers.Helpers;
using Maple2.File.IO.Crypto.Common;
using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace GameDataParser.Parsers;

public class AnimationParser : Exporter<List<AnimationMetadata>>
{
    public AnimationParser(MetadataResources resources) : base(resources, MetadataName.Animation) { }

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
            XmlNodeList? childNodes = document.DocumentElement?.ChildNodes;
            if (childNodes is null)
            {
                continue;
            }

            foreach (XmlNode animationNode in childNodes)
            {
                AnimationMetadata metadata = new();

                if (animationNode.Name == "kfm")
                {
                    metadata.ActorId = animationNode.Attributes?["name"]?.Value;
                }

                foreach (XmlNode sequenceNode in animationNode)
                {
                    if (sequenceNode.Name != "seq")
                    {
                        continue;
                    }

                    if (ParserHelper.CheckForNull(sequenceNode, "id", "name"))
                    {
                        continue;
                    }

                    SequenceMetadata sequence = new()
                    {
                        SequenceId = short.Parse(sequenceNode.Attributes!["id"]!.Value),
                        SequenceName = sequenceNode.Attributes!["name"]!.Value
                    };

                    foreach (XmlNode keyNode in sequenceNode)
                    {
                        if (ParserHelper.CheckForNull(sequenceNode, "time", "name"))
                        {
                            continue;
                        }

                        KeyMetadata key = new()
                        {
                            KeyName = keyNode.Attributes!["name"]!.Value,
                            KeyTime = float.Parse(keyNode.Attributes["time"]!.Value)
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
