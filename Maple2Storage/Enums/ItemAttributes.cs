namespace Maple2Storage.Enums
{
    public enum ItemAttribute : short
    {
        Strength = 0,
        Dexterity = 1,
        Intelligence = 2,
        Luck = 3,
        Health = 4,
        HpRegen = 5,
        HpRegenInterval = 6,
        Spirit = 7,
        SpRegen = 8,
        SpRegenInterval = 9,
        Stamina = 10,
        StaminaRegen = 11,
        StaminaRegenInterval = 12,
        AttackSpeed = 13,
        MovementSpeed = 14,
        Accuracy = 15,
        Evasion = 16,
        CriticalRate = 17,
        CriticalDamage = 18,
        CriticalEvasion = 19,
        Defense = 20,
        PerfectGuard = 21,
        JumpHeight = 22,
        PhysicalAtk = 23,
        MagicalAtk = 24,
        PhysicalRes = 25,
        MagicalRes = 26,
        MinWeaponAtk = 27,
        MaxWeaponAtk = 28,
        MinDamage = 29,
        MaxDamage = 30,
        Piercing = 31,
        MountMovementSpeed = 32,
        BonusAtk = 33,
        PetBonusAtk = 34
    }

    // TODO: Complete this enum
    public enum SpecialItemAttribute : short
    {
        None = 0,
        ExpBonus = 1,
        MesoBonus = 2,
        SwimSpeed = 3,
        DashDistance = 4,
        TonicDropRate = 5,
        GearDropRate = 6,
        TotalDamage = 7,
        CriticalDamage = 8,
        Damage = 9,
        LeaderDamage = 10,
        EliteDamage = 11,
        BossDamage = 12,
        HpOnKill = 13,
        SpiritOnKill = 14,
        StaminaOnKill = 15,
        Heal = 16,
        AllyRecovery = 17,
        IceDamage = 18,
        FireDamage = 19,
        DarkDamage = 20,
        HolyDamage = 21,
        PoisonDamage = 22,
        ElectricDamage = 23,
        MeleeDamage = 24,
        RangedDamage = 25,
        PhysicalPiercing = 26,
        MagicPiercing = 27,
        IceDamageReduce = 28,
        FireDamageReduce = 29,
        DarkDamageReduce = 30,
        HolyDamageReduce = 31,
        PoisonDamageReduce = 32,
        ElectricDamageReduce = 33,
        StunReduce = 34,
        CooldownReduce = 35,
        DebuffDurationReduce = 36,
        MeleeDamageReduce = 37,
        RangedDamageReduce = 38,
        KnockbackReduce = 39,
        MeleeStun = 40, //melee chance to stun
        MeeleeKnockback = 42, //chance of knockback after meele att
        RangedKnockback = 43, //chance of knockback after ranged att
        RangedImmob = 45, //ranged chance to immob
        MeleeAoeDamage = 46, //melee chance to do aoe damage
        RangedAoeDamage = 47, //ranged chance to do aoe damage
        DropRate = 48,
        QuestExp = 49,
        QuestMeso = 50,
        PvpDamage = 54,
        PvpDefense = 55,
        GuildExp = 56,
        GuildCoin = 57,
        McKayXpOrb = 58, //mc-kay experience orb value bonus
        FishingExp = 59,
        ArcadeExp = 60,
        MusicExp = 61,
        AssistantMood = 62, //assistant mood improvement rate
        AssistantDiscount = 63,
        BlackMarketReduce = 64,
        EnchantCatalystDiscount = 65,
        MeretReviveFee = 66,
        MiningBonus = 67,
        RanchingBonus = 68,
        SmithingExp = 69,
        HandicraftMastery = 70,
        ForagingBonus = 71,
        FarmingBonus = 72,
        AlchemyMastery = 73,
        CookingMastery = 74,
        ForagingExp = 75,
        CraftingExp = 76,

        //techs
        TECH = 77, //level 1 skill
        TECH_2 = 78, //2nd level 1 skill
        TECH_10 = 79, //lv 10 skill
        TECH_13 = 80, //lv 13 skill
        TECH_19 = 81,
        TECH_22 = 82,
        TECH_3 = 83
    }
}
