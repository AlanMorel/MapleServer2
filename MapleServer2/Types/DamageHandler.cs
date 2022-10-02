using Maple2Storage.Enums;
using MapleServer2.Constants;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Servers.Game;

namespace MapleServer2.Types;

public class DamageSourceParameters
{
    public bool IsSkill;
    public bool GuaranteedCrit;
    public bool CanCrit;
    public Element Element;
    public SkillRangeType RangeType;
    public DamageType DamageType;
    public int[] SkillGroups;
    public int EventGroup;
    public float DamageRate;
    public SkillCast ParentSkill;
    public int Id;

    public DamageSourceParameters()
    {
        IsSkill = false;
        GuaranteedCrit = false;
        CanCrit = true;
        Element = Element.None;
        RangeType = SkillRangeType.Special;
        SkillGroups = null;
        EventGroup = 0;
        DamageRate = 0;
        DamageType = DamageType.None;
        ParentSkill = null;
        Id = 0;
    }
}

public class DamageHandler
{
    public IFieldActor Source { get; }
    public IFieldActor Target { get; }
    public double Damage { get; }
    public HitType HitType { get; }

    private DamageHandler(IFieldActor source, IFieldActor target, double damage, HitType hitType)
    {
        Source = source;
        Target = target;
        Damage = damage;
        HitType = hitType;
    }

    public static DamageHandler CalculateDamage(SkillCast skill, IFieldActor source, IFieldActor target)
    {
        if (source is Managers.Actors.Character character)
        {
            if (character.Value.GmFlags.Contains("oneshot"))
            {
                return new(source, target, target.Stats[StatAttribute.Hp].Total, HitType.Critical);
            }
        }

        if (target.AdditionalEffects.Invincible)
        {
            return new(source, target, 0, HitType.Miss);
        }

        DamageSourceParameters parameters = new()
        {
            IsSkill = true,
            GuaranteedCrit = skill.IsGuaranteedCrit() || source.AdditionalEffects.AlwaysCrit,
            Element = skill.GetElement(),
            RangeType = skill.GetRangeType(),
            DamageType = skill.GetSkillDamageType(),
            SkillGroups = skill.GetSkillGroups(),
            DamageRate = skill.GetDamageRate(),
            Id = skill.SkillId
        };

        // get luck coefficient from class. new stat recommended, can be refactored away like isCrit was
        return CalculateDamage(parameters, source, target);
    }

    public static double FetchMultiplier(Stats stats, StatAttribute attribute)
    {
        if (stats.Data.TryGetValue(attribute, out Stat stat))
        {
            return (double) stat.Total / 1000;
        }

        return 0;
    }

    public static void ApplyDotDamage(GameSession session, IFieldActor sourceActor, IFieldActor target, DamageSourceParameters dotParameters)
    {
        if (sourceActor == null)
        {
            return;
        }

        if (target.AdditionalEffects.Invincible)
        {
            target.FieldManager.BroadcastPacket(SkillDamagePacket.DotDamage(sourceActor.ObjectId, target.ObjectId, Environment.TickCount, HitType.Miss, 0));

            return;
        }

        DamageHandler damage = CalculateDamage(dotParameters, sourceActor, target);

        target.Damage(damage, session);

        target.FieldManager.BroadcastPacket(SkillDamagePacket.DotDamage(sourceActor.ObjectId, target.ObjectId, Environment.TickCount, damage.HitType, (int) damage.Damage));
    }

    public static DamageHandler CalculateDamage(DamageSourceParameters parameters, IFieldActor source, IFieldActor target)
    {
        // TODO: get accuracyWeakness from enemy stats from enemy buff. new stat recommended
        const double AccuracyWeakness = 0;
        double hitRate = (source.Stats[StatAttribute.Accuracy].Total + AccuracyWeakness) / Math.Max(target.Stats[StatAttribute.Evasion].Total, 0.1);

        if (Random.Shared.NextDouble() > hitRate)
        {
            return new(source, target, 0, HitType.Miss); // we missed
        }

        double luckCoefficient = 1;
        double attackDamage = 300;

        if (source is IFieldActor<Player> player)
        {
            luckCoefficient = GetClassLuckCoefficient(player.Value.JobCode);

            double bonusAttack = player.Stats[StatAttribute.BonusAtk].Total + Constant.PetAttackMultiplier * player.Stats[StatAttribute.PetBonusAtk].Total;

            // TODO: properly fetch enemy bonus attack weakness from enemy buff. new stat recommended
            const double BonusAttackWeakness = 1;

            double bonusAttackCoeff = BonusAttackWeakness * GetBonusAttackCoefficient(player.Value);
            double minDamage = player.Stats[StatAttribute.MinWeaponAtk].Total + bonusAttackCoeff * bonusAttack;
            double maxDamage = player.Stats[StatAttribute.MaxWeaponAtk].Total + bonusAttackCoeff * bonusAttack;

            attackDamage = minDamage + (maxDamage - minDamage) * Random.Shared.NextDouble();
        }

        bool isCrit = parameters.CanCrit && (parameters.GuaranteedCrit || RollCrit(source, target, luckCoefficient));

        double finalCritDamage = 1;

        if (isCrit)
        {
            // TODO: get critResist from enemy stats from enemy buff. new stat recommended
            const double CritResist = 1;
            double critDamage = 1000 + source.Stats[StatAttribute.CritDamage].Total + source.Stats[StatAttribute.CriticalDamage].Total;
            finalCritDamage = CritResist * ((critDamage / 1000) - 1) + 1;
        }

        double damageBonus = 1 + FetchMultiplier(source.Stats, StatAttribute.TotalDamage) + FetchMultiplier(source.Stats, StatAttribute.Damage);

        damageBonus *= finalCritDamage;

        switch (parameters.Element)
        {
            case Element.Fire:
                damageBonus += FetchMultiplier(source.Stats, StatAttribute.FireDamage);
                break;
            case Element.Ice:
                damageBonus += FetchMultiplier(source.Stats, StatAttribute.IceDamage);
                break;
            case Element.Electric:
                damageBonus += FetchMultiplier(source.Stats, StatAttribute.ElectricDamage);
                break;
            case Element.Holy:
                damageBonus += FetchMultiplier(source.Stats, StatAttribute.HolyDamage);
                break;
            case Element.Dark:
                damageBonus += FetchMultiplier(source.Stats, StatAttribute.DarkDamage);
                break;
            case Element.Poison:
                damageBonus += FetchMultiplier(source.Stats, StatAttribute.PoisonDamage);
                break;
        }

        SkillRangeType rangeType = parameters.RangeType;

        if (rangeType != SkillRangeType.Special)
        {
            damageBonus += FetchMultiplier(source.Stats, rangeType == SkillRangeType.Melee ? StatAttribute.MeleeDamage : StatAttribute.RangedDamage);
        }

        bool isBoss = false;

        if (target is INpc npc)
        {
            isBoss = npc.Value.IsBoss();
        }

        damageBonus += isBoss ? FetchMultiplier(source.Stats, StatAttribute.BossDamage) : 0;

        // TODO: properly fetch enemy attack speed weakness from enemy buff. new stat recommended
        const double AttackSpeedWeakness = 0;

        damageBonus += AttackSpeedWeakness * FetchMultiplier(source.Stats, StatAttribute.AttackSpeed);

        InvokeStatValue skillModifier;

        if (parameters.IsSkill)
        {
            skillModifier = source.Stats.GetSkillStats(parameters.Id, parameters.SkillGroups, InvokeEffectType.IncreaseSkillDamage);
        }
        else
        {
            skillModifier = source.Stats.GetEffectStats(parameters.Id, parameters.EventGroup, InvokeEffectType.IncreaseDotDamage);
        }

        double damageMultiplier = damageBonus * (1 + skillModifier.Rate) * (parameters.DamageRate + skillModifier.Value);

        // TODO: properly fetch enemy pierce resistance from enemy buff. new stat recommended
        const double EnemyPierceResistance = 1;

        double defensePierce = 1 - Math.Min(0.3, EnemyPierceResistance * (FetchMultiplier(source.Stats, StatAttribute.Pierce) - 1));
        damageMultiplier *= 1 / (Math.Max(target.Stats[StatAttribute.Defense].Total, 1) * defensePierce);

        DamageType damageType = parameters.DamageType;

        bool isPhysical = damageType == DamageType.Physical;
        double attackType = 0;

        if (damageType == DamageType.Primary)
        {
            double physAttack = source.Stats[StatAttribute.PhysicalAtk].Total;
            double magAttack = source.Stats[StatAttribute.MagicAtk].Total;

            attackType = Math.Max(physAttack, magAttack) * 0.5f;
            isPhysical = physAttack > magAttack;
        }

        StatAttribute resistanceStat = isPhysical ? StatAttribute.PhysicalRes : StatAttribute.MagicRes;
        StatAttribute attackStat = isPhysical ? StatAttribute.PhysicalAtk : StatAttribute.MagicAtk;
        StatAttribute piercingStat = isPhysical ? StatAttribute.PhysicalPiercing : StatAttribute.MagicPiercing;

        if (damageType != DamageType.Primary)
        {
            attackType = source.Stats[attackStat].Total;
        }

        double targetRes = target.Stats[resistanceStat].Total;
        double resPierce = FetchMultiplier(source.Stats, piercingStat);
        double resistance = (1500.0 - Math.Max(0, targetRes - 1500 * resPierce)) / 1500;

        // does this need to be divided by anything at all to account for raw physical attack?
        damageMultiplier *= attackType * resistance;

        // TODO: apply special standalone multipliers like Spicy Maple Noodles buff? it seems to have had it's own multiplier. new stat recommended
        const double FinalDamageMultiplier = 1;
        damageMultiplier *= FinalDamageMultiplier;

        const double magicNumber = 4; // random constant of an unknown origin, but the formula this was pulled from was always off by a factor of 4

        attackDamage *= damageMultiplier * magicNumber;

        return new(source, target, Math.Max(1, attackDamage), isCrit ? HitType.Critical : HitType.Normal);
    }

    private static bool RollCrit(IFieldActor source, IFieldActor target, double luckCoefficient)
    {
        // used to weigh crit rate in the formula, like how class luck coefficients weigh luck
        const double CritConstant = 5.3;

        // used to convert a percent value to a decimal value
        const double PercentageConversion = 0.015;

        const double MaxCritRate = 0.4;

        double luck = source.Stats[StatAttribute.Luk].Total * luckCoefficient;
        double critRate = source.Stats[StatAttribute.CritRate].Total * CritConstant;
        double critEvasion = Math.Max(target.Stats[StatAttribute.CritEvasion].Total, 1) * 2;
        double critChance = Math.Min(critRate / critEvasion * PercentageConversion, MaxCritRate);

        return Random.Shared.Next(1000) < 1000 * critChance;
    }

    private static double GetRarityBonusAttackMultiplier(Item item)
    {
        return (item?.Rarity ?? 0) switch
        {
            1 => 0.26,
            2 => 0.27,
            3 => 0.2883,
            4 => 0.5,
            5 => 1,
            6 => 1,
            _ => 0
        };
    }

    private static double GetWeaponBonusAttackMultiplier(Player player)
    {
        player.Inventory.Equips.TryGetValue(ItemSlot.RH, out Item rightHand);

        double weaponBonusAttackCoeff = GetRarityBonusAttackMultiplier(rightHand);

        if (rightHand != null && ItemMetadataStorage.GetItemSlots(rightHand.Id).Count < 2)
        {
            player.Inventory.Equips.TryGetValue(ItemSlot.LH, out Item leftHand);

            weaponBonusAttackCoeff = 0.5 * (weaponBonusAttackCoeff + GetRarityBonusAttackMultiplier(rightHand));
        }

        return weaponBonusAttackCoeff;
    }

    private static double GetClassBonusAttackMultiplier(JobCode jobCode)
    {
        return jobCode switch
        {
            JobCode.Beginner => 1.039,
            JobCode.Knight => 1.105,
            JobCode.Berserker => 1.354,
            JobCode.Wizard => 1.398,
            JobCode.Priest => 0.975,
            JobCode.Archer => 1.143,
            JobCode.HeavyGunner => 1.364,
            JobCode.Thief => 1.151,
            JobCode.Assassin => 1.114,
            JobCode.Runeblade => 1.259,
            JobCode.Striker => 1.264,
            JobCode.SoulBinder => 1.177,
            _ => 1,
        };
    }

    private static double GetBonusAttackCoefficient(Player player)
    {
        return 4.96 * GetWeaponBonusAttackMultiplier(player) * GetClassBonusAttackMultiplier(player.JobCode);
    }

    private static double GetClassLuckCoefficient(JobCode jobCode)
    {
        return jobCode switch
        {
            JobCode.Beginner => 1,
            JobCode.Knight => 3.78,
            JobCode.Berserker => 4.305,
            JobCode.Wizard => 3.40375,
            JobCode.Priest => 7.34125,
            JobCode.Archer => 6.4575,
            JobCode.HeavyGunner => 2.03875,
            JobCode.Thief => 0.60375,
            JobCode.Assassin => 0.55125,
            JobCode.Runeblade => 3.78,
            JobCode.Striker => 2.03875,
            JobCode.SoulBinder => 3.40375,
            _ => 1,
        };
    }
}
