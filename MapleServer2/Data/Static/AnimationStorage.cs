using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using MapleServer2.Tools;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public class AnimationData
{
    public AnimationMetadata Metadata;
    public Dictionary<short, SequenceMetadata> SequencesById;
    public Dictionary<string, SequenceMetadata> SequencesByName;

    public SequenceMetadata? GetSequence(short id)
    {
        if (SequencesById.TryGetValue(id, out SequenceMetadata? sequence))
        {
            return sequence;
        }

        return null;
    }

    public SequenceMetadata? GetSequence(string name)
    {
        if (SequencesByName.TryGetValue(name.ToLower(), out SequenceMetadata? sequence))
        {
            return sequence;
        }

        return null;
    }

    public AnimationData(AnimationMetadata metadata, Dictionary<short, SequenceMetadata> sequencesById, Dictionary<string, SequenceMetadata> sequencesByName)
    {
        Metadata= metadata;
        SequencesById = sequencesById;
        SequencesByName = sequencesByName;
    }
}

public static class AnimationStorage
{
    private static readonly Dictionary<string, AnimationData> Animations = new();

    public static void Init()
    {
        using FileStream stream = MetadataHelper.GetFileStream(MetadataName.Animation);
        List<AnimationMetadata> animations = Serializer.Deserialize<List<AnimationMetadata>>(stream);
        foreach (AnimationMetadata animation in animations)
        {
            Dictionary<short, SequenceMetadata> sequencesById = new();
            Dictionary<string, SequenceMetadata> sequencesByName = new();

            foreach (SequenceMetadata sequence in animation.Sequence)
            {
                sequencesById.Add(sequence.SequenceId, sequence);
                sequencesByName.TryAdd(sequence.SequenceName.ToLower(), sequence);
            }

            Animations.Add(animation.ActorId, new(animation, sequencesById, sequencesByName));
        }
    }

    public static IEnumerable<SequenceMetadata>? GetSequencesByActorId(string actorId)
    {
        return Animations.GetValueOrDefault(actorId.ToLower())?.Metadata.Sequence;
    }

    public static short GetSequenceIdBySequenceName(string actorId, string sequenceName)
    {
        IEnumerable<SequenceMetadata>? sequences = GetSequencesByActorId(actorId);
        SequenceMetadata? metadata = sequences?.FirstOrDefault(s => s.SequenceName == sequenceName);

        return metadata?.SequenceId ?? 0;
    }

    public static SequenceMetadata? GetSequenceMetadataByName(string actorId, string sequenceName)
    {
        IEnumerable<SequenceMetadata>? sequences = GetSequencesByActorId(actorId);
        SequenceMetadata? metadata = sequences?.FirstOrDefault(s => s.SequenceName == sequenceName);

        return metadata;
    }

    public static AnimationData? GetAnimationDataByName(string actorId)
    {
        return Animations.GetValueOrDefault(actorId.ToLower());
    }
}
