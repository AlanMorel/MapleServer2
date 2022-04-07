using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class AnimationStorage
{
    private static readonly Dictionary<string, AnimationMetadata> Animations = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Animation);
        List<AnimationMetadata> animations = Serializer.Deserialize<List<AnimationMetadata>>(stream);
        foreach (AnimationMetadata animation in animations)
        {
            Animations.Add(animation.ActorId, animation);
        }
    }

    public static IEnumerable<SequenceMetadata> GetSequencesByActorId(string actorId)
    {
        return Animations.GetValueOrDefault(actorId.ToLower()).Sequence;
    }

    public static short GetSequenceIdBySequenceName(string actorId, string sequenceName)
    {
        IEnumerable<SequenceMetadata> sequences = GetSequencesByActorId(actorId);
        SequenceMetadata metadata = sequences.FirstOrDefault(s => s.SequenceName == sequenceName);

        if (metadata != default)
        {
            return metadata.SequenceId;
        }

        return 0;
    }
}
