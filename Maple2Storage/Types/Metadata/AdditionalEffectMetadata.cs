using System.Xml.Serialization;
using Maple2Storage.Enums;

namespace Maple2Storage.Types.Metadata;

[XmlType]
public class AdditionalEffectMetadata
{
    [XmlElement(Order = 1)]
    public int Id;
    [XmlElement(Order = 2)]
    public Dictionary<int, AdditionalEffectLevelMetadata> Levels;
}

[XmlType]
public class AdditionalEffectLevelMetadata
{
    [XmlElement(Order = 1)]
    public SkillBeginCondition BeginCondition;
    [XmlElement(Order = 2)]
    public EffectBasicPropertyMetadata Basic;
    [XmlElement(Order = 3)]
    public EffectMotionPropertyMetadata Motion;
    [XmlElement(Order = 4)]
    public EffectCancelEffectMetadata CancelEffect;
    [XmlElement(Order = 5)]
    public EffectImmuneEffectMetadata? ImmuneEffect;
    [XmlElement(Order = 6)]
    public EffectResetSkillCooldownTimeMetadata ResetCoolDownTime;
    [XmlElement(Order = 7)]
    public EffectModifyDurationMetadata ModifyEffectDuration;
    [XmlElement(Order = 8)]
    public EffectModifyOverlapCountMetadata ModifyOverlapCount;
    [XmlElement(Order = 9)]
    public EffectStatusMetadata Status;
    [XmlElement(Order = 10)]
    public EffectFinalStatusMetadata FinalStatus;
    [XmlElement(Order = 11)]
    public EffectOffensiveMetadata Offensive;
    [XmlElement(Order = 12)]
    public EffectDefesiveMetadata Defesive;
    [XmlElement(Order = 13)]
    public EffectRecoveryMetadata Recovery;
    [XmlElement(Order = 14)]
    public EffectExpMetadata Exp;
    [XmlElement(Order = 15)]
    public EffectDotDamageMetadata DotDamage;
    [XmlElement(Order = 16)]
    public EffectDotBuffMetadata DotBuff;
    [XmlElement(Order = 17)]
    public EffectConsumeMetadata Consume;
    [XmlElement(Order = 18)]
    public EffectReflectMetadata Reflect;
    [XmlElement(Order = 19)]
    public EffectUiMetadata Ui;
    [XmlElement(Order = 20)]
    public EffectShieldMetadata Shield;
    [XmlElement(Order = 21)]
    public EffectMesoGuardMetadata MesoGuard;
    [XmlElement(Order = 22)]
    public EffectInvokeMetadata InvokeEffect;
    [XmlElement(Order = 23)]
    public EffectSpecialMetadata SpecialEffect;
    [XmlElement(Order = 24)]
    public EffectRideMetadata Ride;
    [XmlElement(Order = 25)]
    public List<SkillCondition> SplashSkill = new();
    [XmlElement(Order = 26)]
    public List<SkillCondition> ConditionSkill = new();
    [XmlElement(Order = 27)]
    public string Feature;
    [XmlElement(Order = 28)]
    public bool HasStats;
    [XmlElement(Order = 29)]
    public bool HasConditionalStats;
    [XmlElement(Order = 30)]
    public string Name;
    [XmlElement(Order = 31)]
    public string Description;
}

[XmlType]
public class EffectBeginConditionMetadata
{
    [XmlElement(Order = 1)]
    public EffectBeginConditionOwnerMetadata Owner;

    [XmlElement(Order = 2)]
    public float Probability;
}

[XmlType]
public class EffectBeginConditionOwnerMetadata
{
    [XmlElement(Order = 1)]
    public int[] EventSkillIDs;

    [XmlElement(Order = 2)]
    public int[] EventEffectIDs;

    [XmlElement(Order = 3)]
    public int[] HasBuffId;
}

[XmlType]
public class EffectBasicPropertyMetadata
{
    [XmlElement(Order = 1)]
    public int MaxBuffCount;

    [XmlElement(Order = 2)]
    public SkillGroupType SkillGroupType;

    [XmlElement(Order = 3)]
    public int Group;

    [XmlElement(Order = 4)]
    public EffectDotCondition DotCondition;

    [XmlElement(Order = 5)]
    public int DurationTick;

    [XmlElement(Order = 6)]
    public EffectKeepCondition KeepCondition;

    [XmlElement(Order = 7)]
    public int IntervalTick;

    [XmlElement(Order = 8)]
    public BuffType BuffType;

    [XmlElement(Order = 9)]
    public BuffSubType BuffSubType;

    [XmlElement(Order = 10)]
    public BuffCategory BuffCategory;

    [XmlElement(Order = 11)]
    public float CooldownTime;

    [XmlElement(Order = 12)]
    public int DelayTick;

    [XmlElement(Order = 13)]
    public bool UseInGameTime; // count down only if in game

    [XmlElement(Order = 14)]
    public bool InvokeEvent;

    [XmlElement(Order = 15)]
    public bool DeadKeepEffect;

    [XmlElement(Order = 16)]
    public bool LogoutClearEffect;

    [XmlElement(Order = 17)]
    public bool LeaveFieldClearEffect;

    [XmlElement(Order = 18)]
    public CasterIndividualEffect CasterIndividualEffect;

    [XmlElement(Order = 19)]
    public float ClearDistanceFromCaster;

    [XmlElement(Order = 20)]
    public bool ClearEffectFromPvpZone;

    [XmlElement(Order = 21)]
    public bool DoNotClearEffectFromEnterPvpZone;

    [XmlElement(Order = 22)]
    public bool ClearCooldownFromPvpZone;

    [XmlElement(Order = 23)]
    public EffectResetCondition ResetCondition;

    [XmlElement(Order = 24)]
    public int Level;
}

[XmlType]
public class EffectMotionPropertyMetadata
{

}

[XmlType]
public class EffectCancelEffectMetadata
{
    [XmlElement(Order = 1)]
    public bool CancelCheckSameCaster;
    [XmlElement(Order = 2)]
    public bool CancelPassiveEffect;
    [XmlElement(Order = 3)]
    public int[] CancelEffectCodes;
    [XmlElement(Order = 4)]
    public int[] CancelBuffCategories;
}

[XmlType]
public class EffectImmuneEffectMetadata
{
    [XmlElement(Order = 1)]
    public int[] ImmuneEffectCodes;
    [XmlElement(Order = 2)]
    public int[] ImmuneBuffCategories;

    public EffectImmuneEffectMetadata()
    {
        ImmuneEffectCodes = new int[0];
        ImmuneBuffCategories = new int[0];
    }
}

[XmlType]
public class EffectResetSkillCooldownTimeMetadata
{
    [XmlElement(Order = 1)]
    public long[] SkillCodes;

    public EffectResetSkillCooldownTimeMetadata()
    {
        SkillCodes = Array.Empty<long>();
    }
}

[XmlType]
public class EffectModifyDurationMetadata
{
    [XmlElement(Order = 1)]
    public int[] EffectCodes;

    [XmlElement(Order = 2)]
    public float[] DurationFactors;

    [XmlElement(Order = 3)]
    public float[] DurationValues;
}

[XmlType]
public class EffectModifyOverlapCountMetadata
{
    [XmlElement(Order = 1)]
    public int[] EffectCodes;

    [XmlElement(Order = 2)]
    public int[] OffsetCounts;
}

[XmlType]
public class EffectStatusMetadata
{
    [XmlElement(Order = 1)]
    public Dictionary<StatAttribute, EffectStatMetadata> Stats;
}

[XmlType]
public class EffectStatMetadata
{
    [XmlElement(Order = 1)]
    public long Flat;
    [XmlElement(Order = 2)]
    public float Rate;
    [XmlElement(Order = 3)]
    public StatAttributeType AttributeType;
}

[XmlType]
public class EffectFinalStatusMetadata
{

}

[XmlType]
public class EffectOffensiveMetadata
{
    [XmlElement(Order = 1)]
    public bool AlwaysCrit;

    [XmlElement(Order = 2)]
    public int ImmuneBreak;
}

[XmlType]
public class EffectDefesiveMetadata
{
    [XmlElement(Order = 1)]
    public bool Invincible;
}

[XmlType]
public class EffectRecoveryMetadata
{
    [XmlElement(Order = 1)]
    public float RecoveryRate;

    [XmlElement(Order = 2)]
    public float HpRate;

    [XmlElement(Order = 3)]
    public long HpValue;

    [XmlElement(Order = 4)]
    public float SpRate;

    [XmlElement(Order = 5)]
    public long SpValue;

    [XmlElement(Order = 6)]
    public float EpRate;

    [XmlElement(Order = 7)]
    public long EpValue;
}

[XmlType]
public class EffectExpMetadata
{

}

[XmlType]
public class EffectDotDamageMetadata
{
    [XmlElement(Order = 1)]
    public byte DamageType;
    [XmlElement(Order = 2)]
    public float Rate;
    [XmlElement(Order = 3)]
    public float Value;
    [XmlElement(Order = 4)]
    public int Element;
    [XmlElement(Order = 5)]
    public bool UseGrade;
}

[XmlType]
public class EffectDotBuffMetadata
{

}

[XmlType]
public class EffectConsumeMetadata
{

}

[XmlType]
public class EffectReflectMetadata
{

}

[XmlType]
public class EffectUiMetadata
{

}

[XmlType]
public class EffectShieldMetadata
{

}

[XmlType]
public class EffectMesoGuardMetadata
{

}

[XmlType]
public class EffectInvokeMetadata
{
    [XmlElement(Order = 1)]
    public float[] Values;
    [XmlElement(Order = 2)]
    public float[] Rates;
    [XmlElement(Order = 3)]
    public InvokeEffectType[] Types;
    [XmlElement(Order = 4)]
    public int EffectId;
    [XmlElement(Order = 5)]
    public int EffectGroupId;
    [XmlElement(Order = 6)]
    public int SkillId;
    [XmlElement(Order = 7)]
    public int SkillGroupId;
}

[XmlType]
public class EffectSpecialMetadata
{

}

[XmlType]
public class EffectRideMetadata
{

}

[XmlType]
public class EffectTriggerSkillMetadata
{
    [XmlElement(Order = 1)]
    public int[] SkillId;

    [XmlElement(Order = 2)]
    public bool Splash;

    [XmlElement(Order = 3)]
    public int FireCount;

    [XmlElement(Order = 4)]
    public int Interval;

    [XmlElement(Order = 5)]
    public int RemoveDelay;

    [XmlElement(Order = 6)]
    public uint Delay;

    [XmlElement(Order = 7)]
    public EffectBeginConditionMetadata BeginCondition;

    [XmlElement(Order = 8)]
    public int[] SkillLevel;
}
