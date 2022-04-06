using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;
using ProtoBuf;

namespace MapleServer2.Data.Static;

public static class AnimationStorage
{
    private static readonly Dictionary<string, AnimationMetadata> Animations = new();

    public static void Init()
    {
        string path = Path.Combine(Paths.RESOURCES_DIR, $"ms2-{MetadataName.Animation}-metadata");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("Metadata not found. Re-run GameDataParser");
        }

        using FileStream stream = File.OpenRead(path);
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
