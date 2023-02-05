using Maple2Storage.Types.Metadata;
using MapleServer2.Data.Static;

namespace MapleServer2.Types;

public class AnimationHandler
{
    public IFieldActor Parent;
    public string ActorId { get; private set; }
    public AnimationData? Metadata { get; private set; }
    public SequenceMetadata? SequenceData { get; private set; }
    public long SequenceStart { get; private set; }
    public long SequenceEnd { get; private set; }
    public long LastTick { get; private set; }
    public SkillCast? CurrentSkill { get; private set; }
    public int SkillMotion { get; private set; }

    public AnimationHandler(IFieldActor parent, string actorId = "")
    {
        Parent = parent;
        ActorId = "";

        SetActorId(actorId);
    }

    public void SetActorId(string id)
    {
        if (ActorId == id)
        {
            return;
        }

        Metadata = AnimationStorage.GetAnimationDataByName(id);

        ResetSequence();

        if (Metadata is null)
        {
            return;
        }

        ActorId = id;
    }

    private void SetSequence(SequenceMetadata? sequenceData, SkillCast? skillCast, int skillMotion)
    {
        if (sequenceData is null)
        {
            ResetSequence();

            return;
        }

        SequenceData = sequenceData;
        SequenceStart = Parent.FieldManager?.TickCount64 ?? 0;
        SequenceEnd = SequenceStart + (long) (1000 * SequenceData.Keys.First((key) => key.KeyName == "end").KeyTime);
        LastTick = SequenceStart;
        CurrentSkill = skillCast;
        SkillMotion = skillMotion;
    }

    public void SetSequence(string name, SkillCast? skillCast = null, int skillMotion = 0)
    {
        if (Metadata is null)
        {
            return;
        }

        SetSequence(Metadata.GetSequence(name), skillCast, skillMotion);
    }

    public void SetSequence(short id, SkillCast? skillCast = null, int skillMotion = 0)
    {
        if (Metadata is null)
        {
            return;
        }

        SetSequence(Metadata.GetSequence(id), skillCast, skillMotion);
    }

    public void LoopSequence(SequenceMetadata? sequenceData, SkillCast? skillCast, int skillMotion)
    {
        if (sequenceData is null || (skillCast is not null && skillCast != CurrentSkill))
        {
            return;
        }

        if (SequenceData != sequenceData)
        {
            return;
        }

        KeyMetadata? loopStart = sequenceData.Keys.FirstOrDefault((key) => key.KeyName == "loopstart");
        KeyMetadata? loopEnd = sequenceData.Keys.FirstOrDefault((key) => key.KeyName == "loopend");
        KeyMetadata end = SequenceData.Keys.First((key) => key.KeyName == "end");

        if (SkillMotion == skillMotion)
        {
            if (loopStart is not null && loopEnd is not null)
            {
                SequenceEnd = (Parent.FieldManager?.TickCount64 ?? 0) + (long) (1000 * (end.KeyTime - loopStart.KeyTime));
            }
        }
    }

    public void LoopSequence(string name, SkillCast? skillCast = null, int skillMotion = 0)
    {
        if (Metadata is null)
        {
            return;
        }

        LoopSequence(Metadata.GetSequence(name), skillCast, skillMotion);
    }

    public void LoopSequence(short id, SkillCast? skillCast = null, int skillMotion = 0)
    {
        if (Metadata is null)
        {
            return;
        }

        LoopSequence(Metadata.GetSequence(id), skillCast, skillMotion);
    }

    public void Update()
    {
        if (SequenceData is null)
        {
            return;
        }

        long currentTick = Parent.FieldManager?.TickCount64 ?? 0;

        if (currentTick > SequenceEnd)
        {
            ResetSequence();

            return;
        }
    }

    public void ResetSequence()
    {
        if (SequenceData is null)
        {
            return;
        }

        SequenceData = null;
        SequenceStart = 0;
        SequenceEnd = 0;
        LastTick = 0;
        CurrentSkill = null;
        SkillMotion = 0;
    }
}
