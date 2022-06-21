using System.Xml.Serialization;
using Maple2Storage.Enums;
using MapleServer2.Enums;

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
    public EffectBeginConditionMetadata BeginCondition;
    [XmlElement(Order = 2)]
    public EffectBasicPropertyMetadata Basic;
    [XmlElement(Order = 3)]
    public EffectMotionPropertyMetadata Motion;
    [XmlElement(Order = 4)]
    public EffectCancelEffectMetadata CancelEffect;
    [XmlElement(Order = 5)]
    public EffectImmuneEffectMetadata ImmuneEffect;
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
    public List<EffectTriggerSkillMetadata> SplashSkill;
    [XmlElement(Order = 26)]
    public List<EffectTriggerSkillMetadata> ConditionSkill;
}

[XmlType]
public class EffectBeginConditionMetadata
{

}

[XmlType]
public class EffectBasicPropertyMetadata
{
    [XmlElement(Order = 1)]
    public int MaxBuffCount;
}

[XmlType]
public class EffectMotionPropertyMetadata
{

}

[XmlType]
public class EffectCancelEffectMetadata
{

}

[XmlType]
public class EffectImmuneEffectMetadata
{

}

[XmlType]
public class EffectResetSkillCooldownTimeMetadata
{

}

[XmlType]
public class EffectModifyDurationMetadata
{

}

[XmlType]
public class EffectModifyOverlapCountMetadata
{

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

}

[XmlType]
public class EffectDefesiveMetadata
{

}

[XmlType]
public class EffectRecoveryMetadata
{

}

[XmlType]
public class EffectExpMetadata
{

}

[XmlType]
public class EffectDotDamageMetadata
{

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

}
