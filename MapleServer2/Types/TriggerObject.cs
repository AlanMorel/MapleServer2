using Maple2Storage.Types;
using Maple2Storage.Types.Metadata;

namespace MapleServer2.Types;

public class TriggerObject
{
    public int Id;

    public TriggerObject(int id)
    {
        Id = id;
    }
}
public class TriggerMesh : TriggerObject
{
    public bool IsVisible;
    public int Animation;
    public TriggerMesh(int id, bool isVisible) : base(id)
    {
        IsVisible = isVisible;
        Animation = 3; //TODO : Find this value somewhere in the flat block. Default animation time is 3
    }
}
public class TriggerEffect : TriggerObject
{
    public bool IsVisible;
    public TriggerEffect(int id, bool isVisible) : base(id)
    {
        IsVisible = isVisible;
    }
}
public class TriggerCamera : TriggerObject
{
    public bool IsEnabled;
    public TriggerCamera(int id, bool isEnabled) : base(id)
    {
        IsEnabled = isEnabled;
    }
}
public class TriggerActor : TriggerObject
{
    public bool IsVisible;
    public string StateName;
    public TriggerActor(int id, bool isVisible, string stateName) : base(id)
    {
        IsVisible = isVisible;
        StateName = stateName;
    }
}
public class TriggerLadder : TriggerObject
{
    public bool IsVisible;
    public bool AnimationEffect;
    public int AnimationDelay;
    public TriggerLadder(int id, bool isVisible) : base(id)
    {
        IsVisible = isVisible;
        AnimationEffect = false;
        AnimationDelay = 3;
    }
}
public class TriggerRope : TriggerObject
{
    public bool IsVisible;
    public bool AnimationEffect;
    public int AnimationDelay;
    public TriggerRope(int id, bool isVisible) : base(id)
    {
        IsVisible = isVisible;
        AnimationEffect = false;
        AnimationDelay = 3;
    }
}
public class TriggerCube : TriggerObject
{
    public bool IsVisible;
    public TriggerCube(int id, bool isVisible) : base(id)
    {
        IsVisible = isVisible;
    }
}
public class TriggerSound : TriggerObject
{
    public bool IsEnabled;
    public TriggerSound(int id, bool isEnabled) : base(id)
    {
        IsEnabled = isEnabled;
    }
}
public class TriggerSkill : TriggerObject
{
    public CoordF Position;
    public byte Count;
    public short SkillLevel;
    public int SkillId;
    public SkillCondition Condition;

    public TriggerSkill(int id, int skillId, short skillLevel, byte count, CoordF position, SkillCondition condition) : base(id)
    {
        SkillId = skillId;
        SkillLevel = skillLevel;
        Count = count;
        Position = position;
        Condition = condition;
    }
}
