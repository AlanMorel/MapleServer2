using Maple2Storage.Enums;
using Maple2Storage.Types.Metadata;
using MapleServer2.Commands.Core;
using MapleServer2.Data.Static;
using MapleServer2.Enums;
using MapleServer2.Packets;
using MapleServer2.Types;

namespace MapleServer2.Commands.Game;

public enum ProgressionTier
{
    All,
    Story,
    StoryUnreleased,
    Chaos,
    ChaosUnreleased,
    Awakening,
    Kritias,
    PostGMS2,
    General
}

public enum KitCategory
{
    All,
    Armor,
    Accessory,
    Named,
    Lapenshard,
    Unreleased,
    PvP,
    Reverse,
    Gemstone
}

public class GearKit
{
    public string Name;
    public List<KitItem> Items;
    public ProgressionTier Tier;
    public KitCategory[] Categories;
    public HashSet<ItemType> HasItemType = new();

    public static Dictionary<ItemType, List<GearKit>> KitsWithItemTypes = new();
    public static Dictionary<ProgressionTier, List<GearKit>> KitsInTier = new();
    public static Dictionary<KitCategory, List<GearKit>> KitsInCategory = new();
    public static Dictionary<int, List<GearKit>> KitsWithItem = new();
    public static Dictionary<string, GearKit> KitsByName = new();

    public GearKit(string name, ProgressionTier tier, List<KitItem> items, params KitCategory[] categories)
    {
        Name = name;
        Tier = tier;
        Items = items;
        Categories = categories;

        KitsInTier.TryAdd(tier, new());
        KitsInTier[tier].Add(this);

        KitsByName.Add(name.ToLower(), this);

        foreach (KitItem item in items)
        {
            if (!HasItemType.Contains(item.Type))
            {
                HasItemType.Add(item.Type);
            }

            KitsWithItem.TryAdd(item.Id, new());
            KitsWithItem[item.Id].Add(this);
        }

        foreach (ItemType type in HasItemType)
        {
            KitsWithItemTypes.TryAdd(type, new());
            KitsWithItemTypes[type].Add(this);
        }

        foreach (KitCategory category in Categories)
        {
            KitsInCategory.TryAdd(category, new());
            KitsInCategory[category].Add(this);
        }
    }

    private static int LapenshardOffset(JobCode job)
    {
        return job switch
        {
            JobCode.Knight => 0,
            JobCode.Berserker => 10,
            JobCode.Wizard => 20,
            JobCode.Priest => 30,
            JobCode.Archer => 40,
            JobCode.HeavyGunner => 50,
            JobCode.Thief => 60,
            JobCode.Assassin => 70,
            JobCode.Runeblade => 80,
            JobCode.Striker => 90,
            JobCode.SoulBinder => 100,
            _ => 0
        };
    }

    private static int JobOffset(JobCode job)
    {
        return job switch
        {
            JobCode.Knight => 0,
            JobCode.Berserker => 1,
            JobCode.Wizard => 2,
            JobCode.Priest => 3,
            JobCode.Archer => 4,
            JobCode.HeavyGunner => 5,
            JobCode.Thief => 6,
            JobCode.Assassin => 7,
            JobCode.Runeblade => 8,
            JobCode.Striker => 9,
            JobCode.SoulBinder => 10,
            _ => 0
        };
    }

    private static int SelectItem(JobCode job, int knight, int berserker, int wizard, int priest, int archer, int hg, int thief, int assassin, int runeblade, int striker, int soulbinder)
    {
        return job switch
        {
            JobCode.Knight => knight,
            JobCode.Berserker => berserker,
            JobCode.Wizard => wizard,
            JobCode.Priest => priest,
            JobCode.Archer => archer,
            JobCode.HeavyGunner => hg,
            JobCode.Thief => thief,
            JobCode.Assassin => assassin,
            JobCode.Runeblade => runeblade,
            JobCode.Striker => striker,
            JobCode.SoulBinder => soulbinder,
            _ => 0
        };
    }

    public static List<GearKit> Kits = new()
    {
        new("4SideGems", ProgressionTier.General, new()
        {
            new(ItemType.None, 4, 10, 40200010), // Wisdom
            new(ItemType.None, 4, 10, 40200110), // Luck
            new(ItemType.None, 4, 10, 40200310), // Life
            new(ItemType.None, 4, 10, 40200410), // Power
            new(ItemType.None, 4, 10, 40200510), // Dexterity
            new(ItemType.None, 4, 10, 40200610), // Accuracy
            new(ItemType.None, 4, 10, 40200710), // Offense
        }, KitCategory.Gemstone),
        new("5SideGems", ProgressionTier.General, new()
        {
            new(ItemType.None, 4, 10, 40200810), // Wisdom
            new(ItemType.None, 4, 10, 40200910), // Luck
            new(ItemType.None, 4, 10, 40201110), // Life
            new(ItemType.None, 4, 10, 40201210), // Power
            new(ItemType.None, 4, 10, 40201310), // Dexterity
            new(ItemType.None, 4, 10, 40201410), // Accuracy
            new(ItemType.None, 4, 10, 40201510), // Offense
        }, KitCategory.Gemstone, KitCategory.Unreleased),
        new("RoughGems", ProgressionTier.General, new()
        {
            new(ItemType.None, 4, 10, 40210010), // Health
            new(ItemType.None, 4, 10, 40210110), // Health
            new(ItemType.None, 4, 10, 40210210), // Health
            new(ItemType.None, 4, 10, 40210310), // Health
            new(ItemType.None, 4, 10, 40210410), // Health
            new(ItemType.None, 4, 10, 40210510), // Health
            new(ItemType.None, 4, 10, 40210610), // Health
            new(ItemType.None, 4, 10, 40210710), // Health
        }, KitCategory.Gemstone, KitCategory.Unreleased),
        new("ElliniumGems", ProgressionTier.General, new()
        {
            new(ItemType.None, 4, 10, 40220123), // Weapon Atk
            new(ItemType.None, 4, 10, 40220133), // Accuracy
            new(ItemType.None, 4, 10, 40220143), // Bonus Atk
            new(ItemType.None, 4, 10, 40220153), // Weapon Atk
            new(ItemType.None, 4, 10, 40220163), // Health
            new(ItemType.None, 4, 10, 40220173), // Bonus Atk
            new(ItemType.None, 4, 10, 40220183), // Health
            new(ItemType.None, 4, 10, 40220193), // Bonus Atk
            new(ItemType.None, 4, 10, 40220203), // Health
            new(ItemType.None, 4, 10, 40220301), // Bonus Atk
        }, KitCategory.Gemstone, KitCategory.Unreleased),
        new("Lapenshards", ProgressionTier.General, new()
        {
            new(ItemType.Lapenshard, 3, 41000020), // Celine
            new(ItemType.Lapenshard, 3, 41000030), // Pride Bolt
            new(ItemType.Lapenshard, 3, 41000040), // Lumarigons
            new(ItemType.Lapenshard, 3, 41000050), // Sword Of Time
            new(ItemType.Lapenshard, 3, 41000060), // Moonlight
            new(ItemType.Lapenshard, 3, 41000070), // Gigantica
            new(ItemType.Lapenshard, 3, 43000011), // Haunting Power
            new(ItemType.Lapenshard, 3, 43000030), // Guardian
            new(ItemType.Lapenshard, 3, 43000040), // Madrakan
            new(ItemType.Lapenshard, 3, 43000050), // Eupheria
            new(ItemType.Lapenshard, 3, 43000060), // Nairin
            new(ItemType.Lapenshard, 3, 43000070), // Wandering
            new(ItemType.Lapenshard, 3, (job) => 42000020 + LapenshardOffset(job)), // Erda
            new(ItemType.Lapenshard, 3, (job) => 42000130 + LapenshardOffset(job)), // Masters Malice
            new(ItemType.Lapenshard, 3, (job) => 42000240 + LapenshardOffset(job)), // Bjorn
            new(ItemType.Lapenshard, 3, (job) => 42000350 + LapenshardOffset(job)), // Pink Bean
            new(ItemType.Lapenshard, 3, (job) => 42000460 + LapenshardOffset(job)), // Space
            new(ItemType.Lapenshard, 3, (job) => 42000570 + LapenshardOffset(job)), // Mason
            new(ItemType.Lapenshard, 3, (job) => 42000680 + LapenshardOffset(job)), // Balrog
            new(ItemType.Lapenshard, 3, (job) => 42000790 + LapenshardOffset(job)), // Zakum
            new(ItemType.Lapenshard, 3, (job) => 42000900 + LapenshardOffset(job)), // Infernog
        }, KitCategory.Lapenshard),
        new("AwakeningLapenshards", ProgressionTier.Awakening, new()
        {
            new(ItemType.Lapenshard, 3, 41000020), // Celine
            new(ItemType.Lapenshard, 3, 41000030), // Pride Bolt
            new(ItemType.Lapenshard, 3, 41000040), // Lumarigons
            new(ItemType.Lapenshard, 3, 41000050), // Sword Of Time
            new(ItemType.Lapenshard, 3, 43000011), // Haunting Power
            new(ItemType.Lapenshard, 3, 43000030), // Guardian
            new(ItemType.Lapenshard, 3, 43000040), // Madrakan
            new(ItemType.Lapenshard, 3, 43000050), // Eupheria
            new(ItemType.Lapenshard, 3, (job) => 42000020 + LapenshardOffset(job)), // Erda
            new(ItemType.Lapenshard, 3, (job) => 42000130 + LapenshardOffset(job)), // Masters Malice
            new(ItemType.Lapenshard, 3, (job) => 42000240 + LapenshardOffset(job)), // Bjorn
            new(ItemType.Lapenshard, 3, (job) => 42000350 + LapenshardOffset(job)), // Pink Bean
            new(ItemType.Lapenshard, 3, (job) => 42000460 + LapenshardOffset(job)), // Space
        }, KitCategory.Lapenshard),
        new("KritiasLapenshards", ProgressionTier.Kritias, new()
        {
            new(ItemType.Lapenshard, 3, 41000060), // Moonlight
            new(ItemType.Lapenshard, 3, 41000070), // Gigantica
            new(ItemType.Lapenshard, 3, 43000060), // Nairin
            new(ItemType.Lapenshard, 3, 43000070), // Wandering
            new(ItemType.Lapenshard, 3, (job) => 42000570 + LapenshardOffset(job)), // Mason
            new(ItemType.Lapenshard, 3, (job) => 42000680 + LapenshardOffset(job)), // Balrog
            new(ItemType.Lapenshard, 3, (job) => 42000790 + LapenshardOffset(job)), // Zakum
            new(ItemType.Lapenshard, 3, (job) => 42000900 + LapenshardOffset(job)), // Infernog
        }, KitCategory.Lapenshard),
        new("FirePrism", ProgressionTier.PostGMS2, new()
        {
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 13200308, 15000312, 15200311, 13300307, 15100304, 15300307, 13100313, 13400306, 15400293, 15500225, 15600227)), // Weapon
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 14100278, 0, 0, 14000269, 0, 0, 13100313, 13400306, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 6, (job) => 11301338 + JobOffset(job)),
            new(ItemType.Clothes, 6, (job) => 11401141 + JobOffset(job)),
            new(ItemType.Pants, 6, (job) => 11501034 + JobOffset(job)),
            new(ItemType.Gloves, 6, (job) => 11601222 + JobOffset(job)),
            new(ItemType.Shoes, 6, (job) => 11701318 + JobOffset(job)),
            new(ItemType.Necklace, 6, 11900127),
            new(ItemType.Earring, 6, 11200105),
            new(ItemType.Ring, 6, 12000116),
            new(ItemType.Cape, 6, 11800149),
            new(ItemType.Belt, 6, 12100117),
        }, KitCategory.Accessory, KitCategory.Armor, KitCategory.Unreleased),
        new("FirePrismArmor", ProgressionTier.PostGMS2, new()
        {
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 13200308, 15000312, 15200311, 13300307, 15100304, 15300307, 13100313, 13400306, 15400293, 15500225, 15600227)), // Weapon
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 14100278, 0, 0, 14000269, 0, 0, 13100313, 13400306, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 6, (job) => 11301338 + JobOffset(job)),
            new(ItemType.Clothes, 6, (job) => 11401141 + JobOffset(job)),
            new(ItemType.Pants, 6, (job) => 11501034 + JobOffset(job)),
            new(ItemType.Gloves, 6, (job) => 11601222 + JobOffset(job)),
            new(ItemType.Shoes, 6, (job) => 11701318 + JobOffset(job)),
        }, KitCategory.Armor, KitCategory.Unreleased),
        new("FirePrismAccessories", ProgressionTier.PostGMS2, new()
        {
            new(ItemType.Necklace, 6, 11900127),
            new(ItemType.Earring, 6, 11200105),
            new(ItemType.Ring, 6, 12000116),
            new(ItemType.Cape, 6, 11800149),
            new(ItemType.Belt, 6, 12100117),
        }, KitCategory.Accessory, KitCategory.Unreleased),
        new("TairenRoyal", ProgressionTier.PostGMS2, new()
        {
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 13200309, 15000313, 15200312, 13300308, 15100305, 15300308, 13100314, 13400307, 15400294, 15500226, 15600228)), // Weapon
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 14100279, 0, 0, 14000270, 0, 0, 13100314, 13400307, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 6, (job) => 11301349 + JobOffset(job)),
            new(ItemType.Clothes, 6, (job) => 11401152 + JobOffset(job)),
            new(ItemType.Pants, 6, (job) => 11501045 + JobOffset(job)),
            new(ItemType.Gloves, 6, (job) => 11601233 + JobOffset(job)),
            new(ItemType.Shoes, 6, (job) => 11701329 + JobOffset(job)),
            new(ItemType.Necklace, 6, 11900121),
            new(ItemType.Earring, 6, 11200096),
            new(ItemType.Ring, 6, 12000110),
            new(ItemType.Cape, 6, 11800123),
            new(ItemType.Belt, 6, 12100111),
        }, KitCategory.Accessory, KitCategory.Armor, KitCategory.Unreleased),
        new("TairenRoyalArmor", ProgressionTier.PostGMS2, new()
        {
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 13200309, 15000313, 15200312, 13300308, 15100305, 15300308, 13100314, 13400307, 15400294, 15500226, 15600228)), // Weapon
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 14100279, 0, 0, 14000270, 0, 0, 13100314, 13400307, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 6, (job) => 11301349 + JobOffset(job)),
            new(ItemType.Clothes, 6, (job) => 11401152 + JobOffset(job)),
            new(ItemType.Pants, 6, (job) => 11501045 + JobOffset(job)),
            new(ItemType.Gloves, 6, (job) => 11601233 + JobOffset(job)),
            new(ItemType.Shoes, 6, (job) => 11701329 + JobOffset(job)),
        }, KitCategory.Armor, KitCategory.Unreleased),
        new("TairenRoyalAccessories", ProgressionTier.PostGMS2, new()
        {
            new(ItemType.Necklace, 6, 11900121),
            new(ItemType.Earring, 6, 11200096),
            new(ItemType.Ring, 6, 12000110),
            new(ItemType.Cape, 6, 11800123),
            new(ItemType.Belt, 6, 12100111),
        }, KitCategory.Accessory, KitCategory.Unreleased),
        new("TairenOfficer", ProgressionTier.Kritias, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13200306, 15000310, 15200309, 13300305, 15100302, 15300305, 13100311, 13400304, 15400291, 15500223, 15600225)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14100276, 0, 0, 14000267, 0, 0, 13100311, 13400304, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 5, (job) => 11301316 + JobOffset(job)),
            new(ItemType.Clothes, 5, (job) => 11401119 + JobOffset(job)),
            new(ItemType.Pants, 5, (job) => 11501012 + JobOffset(job)),
            new(ItemType.Gloves, 5, (job) => 11601200 + JobOffset(job)),
            new(ItemType.Shoes, 5, (job) => 11701296 + JobOffset(job)),
        }, KitCategory.Armor),
        new("AntiqueInfernuke", ProgressionTier.Kritias, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13200307, 15000311, 15200310, 13300306, 15100303, 15300306, 13100312, 13400305, 15400292, 15500224, 15600226)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14100277, 0, 0, 14000268, 0, 0, 13100312, 13400305, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 5, (job) => 11301327 + JobOffset(job)),
            new(ItemType.Clothes, 5, (job) => 11401130 + JobOffset(job)),
            new(ItemType.Pants, 5, (job) => 11501023 + JobOffset(job)),
            new(ItemType.Gloves, 5, (job) => 11601211 + JobOffset(job)),
            new(ItemType.Shoes, 5, (job) => 11701307 + JobOffset(job)),
        }, KitCategory.Armor),
        new("TairenCombat", ProgressionTier.Kritias, new()
        {
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 13200305, 15000309, 15200308, 13300304, 15100301, 15300304, 13100310, 13400303, 15400290, 15500222, 15600224)), // Weapon
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 14100275, 0, 0, 14000266, 0, 0, 13100310, 13400303, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 4, (job) => 11301294 + JobOffset(job)),
            new(ItemType.Clothes, 4, (job) => 11401097 + JobOffset(job)),
            new(ItemType.Pants, 4, (job) => 11500990 + JobOffset(job)),
            new(ItemType.Gloves, 4, (job) => 11601178 + JobOffset(job)),
            new(ItemType.Shoes, 4, (job) => 11701274 + JobOffset(job)),
        }, KitCategory.Armor),
        new("AntiqueArigon", ProgressionTier.Kritias, new()
        {
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 13200304, 15000308, 15200307, 13300303, 15100300, 15300303, 13100309, 13400302, 15400289, 15500221, 15600223)), // Weapon
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 14100274, 0, 0, 14000265, 0, 0, 13100309, 13400302, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 4, (job) => 11301283 + JobOffset(job)),
            new(ItemType.Clothes, 4, (job) => 11401086 + JobOffset(job)),
            new(ItemType.Pants, 4, (job) => 11500979 + JobOffset(job)),
            new(ItemType.Gloves, 4, (job) => 11601167 + JobOffset(job)),
            new(ItemType.Shoes, 4, (job) => 11701263 + JobOffset(job)),
        }, KitCategory.Armor),
        new("Humanitas", ProgressionTier.Kritias, new()
        {
            new(ItemType.Necklace, 5, 11900120),
            new(ItemType.Earring, 5, 11200095),
            new(ItemType.Ring, 5, 12000109),
            new(ItemType.Cape, 5, 11800122),
            new(ItemType.Belt, 5, 12100110),
        }, KitCategory.Accessory),
        new("TairenRensha", ProgressionTier.Kritias, new()
        {
            new(ItemType.Necklace, 4, 11900119),
            new(ItemType.Earring, 4, 11200094),
            new(ItemType.Ring, 4, 12000108),
            new(ItemType.Cape, 4, 11800121),
            new(ItemType.Belt, 4, 12100109),
        }, KitCategory.Accessory),
        new("KritiasNamed", ProgressionTier.Kritias, new()
        {
            new(ItemType.Necklace, 4, 11900122), // Paika
            new(ItemType.Hat, 5, 11300188), // Zakum
            new(ItemType.Ring, 4, 12000111), // Zakum
            new(ItemType.Cape, 4, 11860127), // Infernog
            new(ItemType.Belt, 4, 12100112), // Balrog
        }, KitCategory.Accessory, KitCategory.Named),
        new("Soulrend", ProgressionTier.Awakening, new()
        {
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 13260294, 15060298, 15260297, 13360293, 15160290, 15360293, 13160299, 13460292, 15460178, 15560215, 15660219)), // Weapon
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 14160264, 0, 0, 14060255, 0, 0, 13160299, 13460292, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 6, (job) => 11361220 + JobOffset(job)),
            new(ItemType.Clothes, 6, (job) => 11461038 + JobOffset(job)),
            new(ItemType.Pants, 6, (job) => 11560940 + JobOffset(job)),
            new(ItemType.Gloves, 6, (job) => 11661121 + JobOffset(job)),
            new(ItemType.Shoes, 6, (job) => 11761202 + JobOffset(job)),
            new(ItemType.Necklace, 6, 11960114),
            new(ItemType.Earring, 6, 11260089),
            new(ItemType.Ring, 6, 12060103),
            new(ItemType.Cape, 6, 11860113),
            new(ItemType.Belt, 6, 12160104),
        }, KitCategory.Accessory, KitCategory.Armor),
        new("SoulrendArmor", ProgressionTier.Awakening, new()
        {
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 13260294, 15060298, 15260297, 13360293, 15160290, 15360293, 13160299, 13460292, 15460178, 15560215, 15660219)), // Weapon
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 14160264, 0, 0, 14060255, 0, 0, 13160299, 13460292, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 6, (job) => 11361220 + JobOffset(job)),
            new(ItemType.Clothes, 6, (job) => 11461038 + JobOffset(job)),
            new(ItemType.Pants, 6, (job) => 11560940 + JobOffset(job)),
            new(ItemType.Gloves, 6, (job) => 11661121 + JobOffset(job)),
            new(ItemType.Shoes, 6, (job) => 11761202 + JobOffset(job)),
        }, KitCategory.Armor),
        new("SoulrendAccessories", ProgressionTier.Awakening, new()
        {
            new(ItemType.Necklace, 6, 11960114),
            new(ItemType.Earring, 6, 11260089),
            new(ItemType.Ring, 6, 12060103),
            new(ItemType.Cape, 6, 11860113),
            new(ItemType.Belt, 6, 12160104),
        }, KitCategory.Accessory),
        new("Fractured", ProgressionTier.Awakening, new()
        {
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 13260295, 15060299, 15260298, 13360294, 15160291, 15360294, 13160300, 13460293, 15460179, 15560216, 15660220)), // Weapon
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 14160265, 0, 0, 14060256, 0, 0, 13160300, 13460293, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 6, (job) => 11361231 + JobOffset(job)),
            new(ItemType.Clothes, 6, (job) => 11461049 + JobOffset(job)),
            new(ItemType.Pants, 6, (job) => 11560951 + JobOffset(job)),
            new(ItemType.Gloves, 6, (job) => 11661132 + JobOffset(job)),
            new(ItemType.Shoes, 6, (job) => 11761213 + JobOffset(job)),
            new(ItemType.Necklace, 6, 11260090),
            new(ItemType.Earring, 6, 11960115),
            new(ItemType.Ring, 6, 12060104),
            new(ItemType.Cape, 6, 11860114),
            new(ItemType.Belt, 6, 12160105),
        }, KitCategory.Accessory, KitCategory.Armor, KitCategory.Unreleased),
        new("FracturedArmor", ProgressionTier.Awakening, new()
        {
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 13260295, 15060299, 15260298, 13360294, 15160291, 15360294, 13160300, 13460293, 15460179, 15560216, 15660220)), // Weapon
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 14160265, 0, 0, 14060256, 0, 0, 13160300, 13460293, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 6, (job) => 11361231 + JobOffset(job)),
            new(ItemType.Clothes, 6, (job) => 11461049 + JobOffset(job)),
            new(ItemType.Pants, 6, (job) => 11560951 + JobOffset(job)),
            new(ItemType.Gloves, 6, (job) => 11661132 + JobOffset(job)),
            new(ItemType.Shoes, 6, (job) => 11761213 + JobOffset(job)),
        }, KitCategory.Armor, KitCategory.Unreleased),
        new("FracturedAccessories", ProgressionTier.Awakening, new()
        {
            new(ItemType.Necklace, 6, 11260090),
            new(ItemType.Earring, 6, 11960115),
            new(ItemType.Ring, 6, 12060104),
            new(ItemType.Cape, 6, 11860114),
            new(ItemType.Belt, 6, 12160105),
        }, KitCategory.Accessory, KitCategory.Unreleased),
        new("Centurion", ProgressionTier.Awakening, new()
        {
            new(ItemType.Necklace, 5, 11960117),
            new(ItemType.Earring, 5, 11260092),
            new(ItemType.Ring, 5, 12060106),
            new(ItemType.Cape, 5, 11860117),
            new(ItemType.Belt, 5, 12160107),
        }, KitCategory.Accessory),
        new("Wayward", ProgressionTier.Awakening, new()
        {
            new(ItemType.Necklace, 4, 11960116),
            new(ItemType.Earring, 4, 11260091),
            new(ItemType.Ring, 4, 12060105),
            new(ItemType.Cape, 4, 11860116),
            new(ItemType.Belt, 4, 12160106),
        }, KitCategory.Accessory),
        new("AwakeningNamed", ProgressionTier.Awakening, new()
        {
            new(ItemType.Necklace, 4, 11900118), // Siren
            new(ItemType.Earring, 4, 11260096), // Madrakan
            new(ItemType.Ring, 4, 12000107), // Frost
            new(ItemType.Cape, 4, 11800080), // Madrakan
            new(ItemType.Cape, 4, 11860122), // Ariel
            new(ItemType.Belt, 4, 12100108), // Blizzard
        }, KitCategory.Accessory, KitCategory.Named),
        new("DarkVanguard", ProgressionTier.Awakening, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13260293, 15060297, 15260296, 13360292, 15160289, 15360292, 13160298, 13460291, 15460177, 15560214, 15660218)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14160263, 0, 0, 14060254, 0, 0, 13160298, 13460291, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 5, (job) => 11361209 + JobOffset(job)),
            new(ItemType.Clothes, 5, (job) => 11461027 + JobOffset(job)),
            new(ItemType.Pants, 5, (job) => 11560929 + JobOffset(job)),
            new(ItemType.Gloves, 5, (job) => 11661110 + JobOffset(job)),
            new(ItemType.Shoes, 5, (job) => 11761191 + JobOffset(job)),
        }, KitCategory.Armor),
        new("Enigma", ProgressionTier.Awakening, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13260292, 15060296, 15260295, 13360291, 15160288, 15360291, 13160297, 13460290, 15460176, 15560213, 15660217)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14160262, 0, 0, 14060253, 0, 0, 13160297, 13460290, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 5, (job) => 11361198 + JobOffset(job)),
            new(ItemType.Clothes, 5, (job) => 11461016 + JobOffset(job)),
            new(ItemType.Pants, 5, (job) => 11560918 + JobOffset(job)),
            new(ItemType.Gloves, 5, (job) => 11661099 + JobOffset(job)),
            new(ItemType.Shoes, 5, (job) => 11761180 + JobOffset(job)),
        }, KitCategory.Armor),
        new("Behemoth", ProgressionTier.Awakening, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13260291, 15060295, 15260294, 13360290, 15160287, 15360290, 13160296, 13460289, 15460175, 15560212, 15660216)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14160261, 0, 0, 14060252, 0, 0, 13160296, 13460289, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 5, (job) => 11361187 + JobOffset(job)),
            new(ItemType.Clothes, 5, (job) => 11461005 + JobOffset(job)),
            new(ItemType.Pants, 5, (job) => 11560907 + JobOffset(job)),
            new(ItemType.Gloves, 5, (job) => 11661088 + JobOffset(job)),
            new(ItemType.Shoes, 5, (job) => 11761169 + JobOffset(job)),
        }, KitCategory.Armor),
        new("Demonwing", ProgressionTier.Awakening, new()
        {
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 13260290, 15060294, 15260293, 13360289, 15160286, 15360289, 13160295, 13460288, 15460174, 15560211, 15660215)), // Weapon
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 14160260, 0, 0, 14060251, 0, 0, 13160295, 13460288, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 4, (job) => 11361176 + JobOffset(job)),
            new(ItemType.Clothes, 4, (job) => 11460994 + JobOffset(job)),
            new(ItemType.Pants, 4, (job) => 11560896 + JobOffset(job)),
            new(ItemType.Gloves, 4, (job) => 11661077 + JobOffset(job)),
            new(ItemType.Shoes, 4, (job) => 11761158 + JobOffset(job)),
        }, KitCategory.Armor),
        new("Frontier", ProgressionTier.Awakening, new()
        {
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 13260289, 15060293, 15260292, 13360288, 15160285, 15360288, 13160294, 13460287, 15460173, 15560210, 15660214)), // Weapon
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 14160259, 0, 0, 14060250, 0, 0, 13160294, 13460287, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 4, (job) => 11361165 + JobOffset(job)),
            new(ItemType.Clothes, 4, (job) => 11460983 + JobOffset(job)),
            new(ItemType.Pants, 4, (job) => 11560885 + JobOffset(job)),
            new(ItemType.Gloves, 4, (job) => 11661066 + JobOffset(job)),
            new(ItemType.Shoes, 4, (job) => 11761147 + JobOffset(job)),
        }, KitCategory.Armor),
        new("Tidemaster", ProgressionTier.Awakening, new()
        {
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 13260288, 15060292, 15260291, 13360287, 15160284, 15360287, 13160293, 13460286, 15460172, 15560209, 15660213)), // Weapon
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 14160258, 0, 0, 14060249, 0, 0, 13160293, 13460286, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 4, (job) => 11361154 + JobOffset(job)),
            new(ItemType.Clothes, 4, (job) => 11460972 + JobOffset(job)),
            new(ItemType.Pants, 4, (job) => 11560874 + JobOffset(job)),
            new(ItemType.Gloves, 4, (job) => 11661055 + JobOffset(job)),
            new(ItemType.Shoes, 4, (job) => 11761136 + JobOffset(job)),
        }, KitCategory.Armor),
        new("Pluto", ProgressionTier.Chaos, new()
        {
            new(ItemType.Necklace, 5, 11900099),
            new(ItemType.Earring, 5, 11200072),
            new(ItemType.Ring, 5, 12000088),
            new(ItemType.Cape, 5, 11800092),
            new(ItemType.Belt, 5, 12100088),
        }, KitCategory.Accessory),
        new("Mars", ProgressionTier.Chaos, new()
        {
            new(ItemType.Necklace, 5, 11900100),
            new(ItemType.Earring, 5, 11200073),
            new(ItemType.Ring, 5, 12000089),
            new(ItemType.Cape, 5, 11800093),
            new(ItemType.Belt, 5, 12100089),
        }, KitCategory.Accessory, KitCategory.Unreleased),
        new("Absolute", ProgressionTier.Chaos, new()
        {
            new(ItemType.Necklace, 4, 11930032),
            new(ItemType.Earring, 4, 11250113),
            new(ItemType.Ring, 4, 12030024),
            new(ItemType.Cape, 4, 11850170),
            new(ItemType.Belt, 4, 12130027),
        }, KitCategory.Accessory),
        new("ChaosNamed", ProgressionTier.Chaos, new()
        {
            new(ItemType.Necklace, 4, 11900071), // Kandura
            new(ItemType.Earring, 4, 11200052), // Nutaman
            new(ItemType.Earring, 4, 11200006), // Balrog
            new(ItemType.Hat, 4, 11300140), // Balrog
            new(ItemType.Hat, 4, 11350744), // Varrekant
            new(ItemType.Cape, 4, 11850175), // Varrekant
            new(ItemType.Ring, 4, 12000050), // Pyrros
            new(ItemType.Gloves, 4, 11600301), // Pyrros
            new(ItemType.Belt, 5, 12100049), // Old Fairy King
            new(ItemType.Belt, 4, 12100050), // Old Fairy
        }, KitCategory.Accessory, KitCategory.Named),
        new("ChaosUnreleasedNamed", ProgressionTier.ChaosUnreleased, new()
        {
            new(ItemType.Necklace, 5, 11960118), // Peridot
            new(ItemType.Necklace, 4, 11900088), // Black Covenant
            new(ItemType.Ring, 4, 12000078), // Conspirator
            new(ItemType.Belt, 4, 12100077), // Shadow Scallion
            new(ItemType.Necklace, 4, 11900073), // Shuabritze
            new(ItemType.Ring, 4, 12000063), // Shuabritze
            new(ItemType.Earring, 4, 11200057), // Shuabritze
        }, KitCategory.Accessory, KitCategory.Named, KitCategory.Unreleased),
        new("Eternal", ProgressionTier.ChaosUnreleased, new()
        {
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 13200149, 15000153, 15200152, 13300148, 15100146, 15300146, 13100153, 13400146, 15400091, 15500065, 15600065)), // Weapon
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 14100134, 0, 0, 14000125, 0, 0, 13100153, 13400146, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 6, (job) => SelectItem(job, 11360137, 11360135, 11360148, 11360138, 11360146, 11360136, 11360143, 11360144, 11360432, 11360596, 11360643)),
            new(ItemType.Clothes, 6, (job) => SelectItem(job, 11460075, 11460074, 11460914, 11460915, 11460078, 11460916, 11460076, 11460077, 11460387, 11460512, 11460556)),
            new(ItemType.Pants, 6, (job) => SelectItem(job, 11560076, 11560075, 11560818, 11560819, 11560079, 11560820, 11560077, 11560078, 11560315, 11560426, 11560472)),
            new(ItemType.Gloves, 6, (job) => SelectItem(job, 11660086, 11660084, 11660093, 11660087, 11660091, 11660085, 11660088, 11660089, 11660390, 11660538, 11660581)),
            new(ItemType.Shoes, 6, (job) => SelectItem(job, 11760107, 11760105, 11760115, 11760108, 11760113, 11760106, 11760109, 11760111, 11760417, 11760560, 11760611)),
        }, KitCategory.Armor, KitCategory.Unreleased),
        new("CrimsonBalrog", ProgressionTier.ChaosUnreleased, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13200043, 15000041, 15200142, 13300138, 15100136, 15300136, 13100028, 13400136, 15400064, 15500064, 15600064)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14100126, 0, 0, 14000118, 0, 0, 13100028, 13400136, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 5, (job) => 11301117 + JobOffset(job)),
            new(ItemType.Clothes, 5, (job) => 11400957 + JobOffset(job)),
            new(ItemType.Pants, 5, (job) => 11500861 + JobOffset(job)),
            new(ItemType.Gloves, 5, (job) => 11601030 + JobOffset(job)),
            new(ItemType.Shoes, 5, (job) => 11701105 + JobOffset(job)),
        }, KitCategory.Armor, KitCategory.Unreleased),
        new("Reverse3", ProgressionTier.ChaosUnreleased, new()
        {
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 13260226, 15060230, 15260229, 13360225, 15160222, 15360225, 13160231, 13460224, 15460115, 15560153, 15660142)), // Weapon
            new(ItemType.Longsword, 6, (job) => SelectItem(job, 14160194, 0, 0, 14060185, 0, 0, 13160231, 13460224, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 6, (job) => 11360917 + JobOffset(job)),
            new(ItemType.Clothes, 6, (job) => 11460798 + JobOffset(job)),
            new(ItemType.Pants, 6, (job) => 11560705 + JobOffset(job)),
            new(ItemType.Gloves, 6, (job) => 11660839 + JobOffset(job)),
            new(ItemType.Shoes, 6, (job) => 11760909 + JobOffset(job)),
        }, KitCategory.Armor, KitCategory.Reverse, KitCategory.Unreleased),
        new("Reverse2", ProgressionTier.ChaosUnreleased, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13200227, 15000231, 15200230, 13300226, 15100223, 15300226, 13100232, 13400225, 15400116, 15500154, 15600143)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14100195, 0, 0, 14000186, 0, 0, 13100232, 13400225, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 5, (job) => 11300928 + JobOffset(job)),
            new(ItemType.Clothes, 5, (job) => 11400809 + JobOffset(job)),
            new(ItemType.Pants, 5, (job) => 11500716 + JobOffset(job)),
            new(ItemType.Gloves, 5, (job) => 11600850 + JobOffset(job)),
            new(ItemType.Shoes, 5, (job) => 11700920 + JobOffset(job)),
        }, KitCategory.Armor, KitCategory.Reverse, KitCategory.Unreleased),
        new("Reverse", ProgressionTier.ChaosUnreleased, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13200226, 15000230, 15200229, 13300225, 15100222, 15300225, 13100231, 13400224, 15400115, 15500153, 15600142)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14100194, 0, 0, 14000185, 0, 0, 13100231, 13400224, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 5, (job) => 11300917 + JobOffset(job)),
            new(ItemType.Clothes, 5, (job) => 11400798 + JobOffset(job)),
            new(ItemType.Pants, 5, (job) => 11500705 + JobOffset(job)),
            new(ItemType.Gloves, 5, (job) => 11600839 + JobOffset(job)),
            new(ItemType.Shoes, 5, (job) => 11700909 + JobOffset(job)),
        }, KitCategory.Armor, KitCategory.Reverse, KitCategory.Unreleased),
        new("UnleashedNarubashan", ProgressionTier.ChaosUnreleased, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13200190, 15000194, 15200193, 13300190, 15100186, 15300189, 13100195, 13400187, 15400093, 15500124, 15600118)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14100164, 0, 0, 14000155, 0, 0, 13100195, 13400187, 0, 0, 0)), // Offhand
        }, KitCategory.Armor, KitCategory.Unreleased),
        new("LimitlessNarubashan", ProgressionTier.ChaosUnreleased, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13200189, 15000193, 15200192, 13300189, 15100185, 15300188, 13100194, 13400186, 15400092, 15500123, 15600117)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14100163, 0, 0, 14000154, 0, 0, 13100194, 13400186, 0, 0, 0)), // Offhand
        }, KitCategory.Armor, KitCategory.Unreleased),
        new("Might", ProgressionTier.ChaosUnreleased, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13200188, 15000192, 15200191, 13300188, 15100184, 15300187, 13100193, 13400185, 15400253, 15500122, 15600207)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14130044, 0, 0, 14000030, 0, 0, 13100193, 13400185, 0, 0, 0)), // Offhand
        }, KitCategory.Armor, KitCategory.Unreleased),
        new("Fervor", ProgressionTier.ChaosUnreleased, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13200191, 15000195, 15200194, 13300191, 15100187, 15300190, 13100196, 13400188, 15400094, 15500125, 15600119)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14100194, 0, 0, 14000185, 0, 0, 13100196, 13400188, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 5, (job) => 11300673 + JobOffset(job)),
            new(ItemType.Clothes, 5, (job) => 11400580 + JobOffset(job)),
            new(ItemType.Pants, 5, (job) => 11500496 + JobOffset(job)),
            new(ItemType.Gloves, 5, (job) => 11600607 + JobOffset(job)),
            new(ItemType.Shoes, 5, (job) => 11700637 + JobOffset(job)),
        }, KitCategory.Armor, KitCategory.Unreleased),
        new("Lodestar", ProgressionTier.ChaosUnreleased, new()
        {
            new(ItemType.Hat, 5, (job) => 11351106 + JobOffset(job)),
            new(ItemType.Clothes, 5, (job) => 11450946 + JobOffset(job)),
            new(ItemType.Pants, 5, (job) => 11550850 + JobOffset(job)),
            new(ItemType.Gloves, 5, (job) => 11651019 + JobOffset(job)),
            new(ItemType.Shoes, 5, (job) => 11751094 + JobOffset(job)),
        }, KitCategory.Armor, KitCategory.Unreleased),
        new("Papulatus", ProgressionTier.StoryUnreleased, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13200091, 15000090, 0, 0, 0, 0, 0, 0, 0, 0, 0)), // Weapon
            new(ItemType.Earring, 4, 11200042),
            new(ItemType.Cape, 4, 11800076),
            new(ItemType.Belt, 4, 12100039),
            new(ItemType.Shoes, 4, (job) => 11700218),
        }, KitCategory.Armor, KitCategory.Accessory),
        new("Panic", ProgressionTier.Chaos, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13250294, 15050298, 15250297, 13350293, 15150290, 15350293, 13150299, 13450292, 15450293, 15550533, 15650534)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14150264, 0, 0, 14050255, 0, 0, 13150299, 13450292, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 5, (job) => 11350745 + JobOffset(job)),
            new(ItemType.Clothes, 5, (job) => 11451016 + JobOffset(job)),
            new(ItemType.Pants, 5, (job) => 11550920 + JobOffset(job)),
            new(ItemType.Gloves, 5, (job) => 11650674 + JobOffset(job)),
            new(ItemType.Shoes, 5, (job) => 11750704 + JobOffset(job)),
        }, KitCategory.Armor),
        new("Extreme", ProgressionTier.Chaos, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13250282, 15050286, 15250285, 13350281, 15150278, 15350281, 13150287, 13450280, 15450281, 15550521, 15650510)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14150252, 0, 0, 14050243, 0, 0, 13150287, 13450280, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 5, (job) => 11350684 + JobOffset(job)),
            new(ItemType.Clothes, 5, (job) => SelectItem(job, 11450968, 11450973, 11450969, 11450970, 11450971, 11450972, 11450974, 11450975, 11450976, 11450977, 11450978)),
            new(ItemType.Pants, 5, (job) => SelectItem(job, 11550872, 11550877, 11550873, 11550874, 11550875, 11550876, 11550878, 11550879, 11550880, 11550881, 11550882)),
            new(ItemType.Gloves, 5, (job) => 11650618 + JobOffset(job)),
            new(ItemType.Shoes, 5, (job) => 11750648 + JobOffset(job)),
        }, KitCategory.Armor),
        new("Rage", ProgressionTier.Chaos, new()
        {
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 13250282, 15050286, 15250285, 13350281, 15150278, 15350281, 13150287, 13450280, 15450281, 15550521, 15650510)), // Weapon
            new(ItemType.Longsword, 5, (job) => SelectItem(job, 14150252, 0, 0, 14050243, 0, 0, 13150287, 13450280, 0, 0, 0)), // Offhand
            new(ItemType.Hat, 5, (job) => 11350695 + JobOffset(job)),
            new(ItemType.Clothes, 5, (job) => 11450979 + JobOffset(job)),
            new(ItemType.Pants, 5, (job) => 11550883 + JobOffset(job)),
            new(ItemType.Gloves, 5, (job) => 11650629 + JobOffset(job)),
            new(ItemType.Shoes, 5, (job) => 11750659 + JobOffset(job)),
        }, KitCategory.Armor),
        new("Murpagoth", ProgressionTier.Chaos, new()
        {
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 13250283, 15050287, 15250286, 13350282, 15150279, 15350282, 13150288, 13450281, 15450283, 15550523, 15650524)), // Weapon
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 14150253, 0, 0, 14050244, 0, 0, 13150288, 13450281, 0, 0, 0)), // Offhand
        }, KitCategory.Armor),
        new("AncientRune", ProgressionTier.Chaos, new()
        {
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 13250284, 15050288, 15250287, 13350283, 15150280, 15350283, 13150289, 13450282, 15450284, 15550524, 15650525)), // Weapon
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 14150254, 0, 0, 14050245, 0, 0, 13150289, 13450282, 0, 0, 0)), // Offhand
        }, KitCategory.Armor),
        new("MSLOnyx", ProgressionTier.Chaos, new()
        {
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 13250285, 15050289, 15250288, 13350284, 15150281, 15350284, 13150290, 13450283, 15450284, 15550524, 15650513)), // Weapon
            new(ItemType.Longsword, 4, (job) => SelectItem(job, 14150255, 0, 0, 14050246, 0, 0, 13150290, 13450283, 0, 0, 0)), // Offhand
        }, KitCategory.Armor),
        new("Exquisite", ProgressionTier.Chaos, new()
        {
            new(ItemType.Hat, 4, (job) => 11350728 + JobOffset(job)),
            new(ItemType.Clothes, 4, (job) => 11451005 + JobOffset(job)),
            new(ItemType.Pants, 4, (job) => 11550909 + JobOffset(job)),
            new(ItemType.Gloves, 4, (job) => 11650662 + JobOffset(job)),
            new(ItemType.Shoes, 4, (job) => 11750692 + JobOffset(job)),
        }, KitCategory.Armor),
    };
}

public class KitItem
{
    public ItemType Type;
    public int Id;
    public int Rarity;
    public int Amount;
    public Func<JobCode, int> ItemCallback;

    public KitItem(ItemType type, int rarity, int id)
    {
        Type = type;
        Id = id;
        Rarity = rarity;
        Amount = 1;
        ItemCallback = (code) => Id;
    }

    public KitItem(ItemType type, int rarity, Func<JobCode, int> callback)
    {
        Type = type;
        Id = 0;
        Rarity = rarity;
        Amount = 1;
        ItemCallback = callback;
    }
    public KitItem(ItemType type, int rarity, int amount, int id)
    {
        Type = type;
        Id = id;
        Rarity = rarity;
        Amount = amount;
        ItemCallback = (code) => Id;
    }

    public KitItem(ItemType type, int rarity, int amount, Func<JobCode, int> callback)
    {
        Type = type;
        Id = 0;
        Rarity = rarity;
        Amount = amount;
        ItemCallback = callback;
    }
}

public class KitCommand : InGameCommand
{
    public KitCommand()
    {
        Aliases = new()
        {
            "kit"
        };
        Description = "Gives a whole gear kit for the current class.";
        Parameters = new()
        {
            new Parameter<string>("name", "Name of the kit to give."),
            new Parameter<string>("classOverride", "The name or id of the class to give a kit for if you want a different class's kit."),
        };
        Usage = "/kit name [classOverride]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string kitName = trigger.Get<string>("name");

        if (string.IsNullOrEmpty(kitName))
        {
            trigger.Session.Send(NoticePacket.Notice("Please enter the kit name to give. Use /kitlist to see available kits.", NoticeType.Chat));

            return;
        }

        kitName = kitName.ToLower();

        if (!Enum.TryParse(trigger.Get<string>("classOverride"), true, out JobCode job))
        {
            job = trigger.Session.Player.JobCode;
        }

        if (!GearKit.KitsByName.TryGetValue(kitName, out GearKit? kit))
        {
            trigger.Session.Send(NoticePacket.Notice($"Kit '{kitName}' not found. Please enter the kit name to give. Use /kitlist to see available kits.", NoticeType.Chat));

            return;
        }

        IInventory inventory = trigger.Session.Player.Inventory;

        foreach (KitItem kitItem in kit.Items)
        {
            int id = kitItem.ItemCallback(job);

            if (id == 0)
            {
                continue;
            }

            if (!ItemMetadataStorage.IsValid(id))
            {
                trigger.Session.Send(NoticePacket.Notice($"Kit item {id} doesn't exist.", NoticeType.Chat));

                continue;
            }

            Item item = new(id, Math.Max(1, kitItem.Amount), kitItem.Rarity);

            trigger.Session.Player.Inventory.AddItem(trigger.Session, item, true);
        }
    }
}

public class KitListCommand : InGameCommand
{
    public KitListCommand()
    {
        Aliases = new()
        {
            "kitlist"
        };
        Description = "Gives a list of kits available with a certain category and in a certain tier.";
        Parameters = new()
        {
            new Parameter<string>("category", "Choose a category filter to pick kits from. Use 'list' to see available categories instead. Using an item id will filter for kits with that item."),
            new Parameter<string>("tier", "Filter specific progression tiers to filter from. Tiers available are: Story, Chaos, ChaosUnreleased, Awakening, Kritias, PostGMS2, General"),
        };
        Usage = "/kitlist [category] [tier]";
    }

    public override void Execute(GameCommandTrigger trigger)
    {
        string categoryName = trigger.Get<string>("category");

        if (string.IsNullOrEmpty(categoryName))
        {
            categoryName = "All";
        }

        if (categoryName == "list")
        {
            foreach (string name in Enum.GetNames(typeof(KitCategory)))
            {
                trigger.Session.Send(NoticePacket.Notice(name, NoticeType.Chat));
            }

            return;
        }

        KitCategory category;

        if (!Enum.TryParse(categoryName, true, out category))
        {
            trigger.Session.Send(NoticePacket.Notice($"Unknown gear kit category '{categoryName}'", NoticeType.Chat));

            return;
        }

        ProgressionTier tier = ProgressionTier.All;

        Enum.TryParse(trigger.Get<string>("tier"), true, out tier);

        if (!GearKit.KitsInCategory.TryGetValue(category, out List<GearKit>? kits) && category != KitCategory.All)
        {
            trigger.Session.Send(NoticePacket.Notice($"No gear kits found in category '{categoryName}'", NoticeType.Chat));

            return;
        }

        bool foundKit = false;

        trigger.Session.Send(NoticePacket.Notice($"Kits found matching category '{categoryName}' and tier '{tier}':", NoticeType.Chat));

        foreach (GearKit kit in kits ?? GearKit.Kits)
        {
            if (tier == ProgressionTier.All || kit.Tier == tier)
            {
                foundKit = true;

                trigger.Session.Send(NoticePacket.Notice(kit.Name, NoticeType.Chat));
            }
        }

        if (!foundKit)
        {
            trigger.Session.Send(NoticePacket.Notice($"No gear kits found in category '{categoryName}' and tier '{tier}'", NoticeType.Chat));
        }
    }
}

public class GearBuildCommand : InGameCommand
{
    public GearBuildCommand()
    {
        Aliases = new()
        {
            "gearbuild"
        };
        Description = "Bulk sets the random attributes on an entire set or a specified slot.";
        Parameters = new()
        {
            new Parameter<string>("slot", "Chooses a slot to set lines on. Use the slot enum (RH, PET, etc) for specific slots, 'Armor' for all armor, or 'Accessories' for all accessories."),
            new Parameter<string>("line1", "Sets the stat type of the first random line. It will replace the combat stat on armor if an offensive stat is chosen."),
            new Parameter<float>("value1", "Sets which value in the stat type's possible rolls will be picked. 0 for min roll, 1 for max, and anything between for the nearest matching roll."),
            new Parameter<string>("line2", "Sets the stat type of the first random line. No effect if it's a duplicate line or a second offensive line on armor."),
            new Parameter<float>("value2", "Sets which value in the stat type's possible rolls will be picked."),
            new Parameter<string>("line3", "Sets the stat type of the first random line. No effect if it's a duplicate line or a second offensive line on armor."),
            new Parameter<float>("value3", "Sets which value in the stat type's possible rolls will be picked."),
            new Parameter<string>("line4", "Sets the stat type of the first random line. No effect if it's a duplicate line or not on a legendary pet."),
            new Parameter<float>("value4", "Sets which value in the stat type's possible rolls will be picked."),
        };
        Usage = "/gearbuild slot line1 value1 [line2] [value2] [line3] [value3] [line4] [value4]";
    }

    private readonly string[] Lines = new[] { "line1", "line2", "line3", "line4" };
    private readonly string[] Values = new[] { "value1", "value2", "value3", "value4" };
    private readonly ItemSlot[] ArmorSlots = new[] { ItemSlot.CP, ItemSlot.CL, ItemSlot.PA, ItemSlot.GL, ItemSlot.SH };
    private readonly ItemSlot[] AccessorySlots = new[] { ItemSlot.PD, ItemSlot.EA, ItemSlot.RI, ItemSlot.MT, ItemSlot.BE };

    public override void Execute(GameCommandTrigger trigger)
    {
        string slotName = trigger.Get<string>("slot");

        if (string.IsNullOrEmpty(slotName))
        {
            return;
        }

        slotName = slotName.ToLower();

        List<(ItemSlot, Item)> items = new();
        Player player = trigger.Session.Player;
        IInventory inventory = player.Inventory;

        if (slotName == "armor")
        {
            foreach (ItemSlot slot in ArmorSlots)
            {
                if (inventory.Equips.TryGetValue(slot, out Item? item))
                {
                    items.Add((slot, item));
                }
            }
        }
        else if (slotName == "accessories")
        {
            foreach (ItemSlot slot in AccessorySlots)
            {
                if (inventory.Equips.TryGetValue(slot, out Item? item))
                {
                    items.Add((slot, item));
                }
            }
        }
        else
        {
            foreach (string splitName in slotName.Split(','))
            {
                if (splitName == "pet")
                {
                    if (player.ActivePet is not null)
                    {
                        items.Add((ItemSlot.NONE, player.ActivePet));
                    }
                }
                else if (Enum.TryParse(splitName, true, out ItemSlot slot))
                {
                    if (inventory.Equips.TryGetValue(slot, out Item? item))
                    {
                        items.Add((slot, item));
                    }
                }
            }
        }

        if (player.FieldPlayer is null)
        {
            return;
        }

        List<StatAttribute> usedStats = new();

        foreach ((ItemSlot slot, Item item) in items)
        {
            item.Stats.Randoms.Clear();
        }

        for (int i = 0; i < 3; ++i)
        {
            string statName = trigger.Get<string>(Lines[i]);
            float value = Math.Min(1, Math.Max(0, trigger.Get<float>(Values[i])));

            if (string.IsNullOrEmpty(statName))
            {
                continue;
            }

            if (!Enum.TryParse(statName, true, out StatAttribute stat))
            {
                continue;
            }

            if (usedStats.Contains(stat))
            {
                continue;
            }

            foreach ((ItemSlot slot, Item item) in items)
            {
                int? randomId = ItemMetadataStorage.GetOptionMetadata(item.Id)?.Random;

                if (randomId is null)
                {
                    continue;
                }

                ItemOptionRandom? randomOptions = ItemOptionRandomMetadataStorage.GetMetadata(randomId ?? 0, item.Rarity);
                float? itemLevelFactor = ItemMetadataStorage.GetOptionMetadata(item.Id)?.OptionLevelFactor;

                if (randomOptions is null || itemLevelFactor is null)
                {
                    continue;
                }

                int slots = randomOptions.Slots[1];

                if (i >= slots)
                {
                    continue;
                }

                bool isBasic = randomOptions.Stats.Find((statData) => statData.Attribute == stat) is not null;
                bool isSpecial = !isBasic && randomOptions.SpecialStats.Find((statData) => statData.Attribute == stat) is not null;

                if (!isBasic && !isSpecial)
                {
                    continue;
                }

                List<float> rolls = new();
                int startRoll = itemLevelFactor >= 70 ? 8 : 0;
                StatAttributeType type = StatAttributeType.Rate;

                if (isBasic)
                {
                    if (!RandomStats.GetRange(item.Id).TryGetValue(stat, out List<ParserStat>? stats))
                    {
                        continue;
                    }

                    type = stats[startRoll].AttributeType;

                    for (int j = startRoll; j < 8 + startRoll; ++j)
                    {
                        rolls.Add(stats[j].Value);
                    }
                }
                else
                {
                    if (!RandomStats.GetSpecialRange(item.Id).TryGetValue(stat, out List<ParserSpecialStat>? stats))
                    {
                        continue;
                    }

                    type = stats[startRoll].AttributeType;

                    for (int j = startRoll; j < 8 + startRoll; ++j)
                    {
                        rolls.Add(stats[j].Value);
                    }
                }

                float approximateValue = (1 - value) * rolls[0] + value * rolls[7];
                float closestValue = Math.Abs(rolls[0] - approximateValue);
                int closestIndex = 0;

                for (int j = 1; j < 8; ++j)
                {
                    float current = Math.Abs(rolls[j] - approximateValue);

                    if (current < closestValue)
                    {
                        closestValue = current;
                        closestIndex = j;
                    }
                }

                ItemStat itemStat = isBasic ? new BasicStat(stat, rolls[closestIndex], type) : new SpecialStat(stat, rolls[closestIndex], type);

                if (randomOptions.MultiplyFactor > 0)
                {
                    itemStat.Flat *= (int) Math.Ceiling(randomOptions.MultiplyFactor);
                    itemStat.Rate *= randomOptions.MultiplyFactor;
                }

                item.Stats.Randoms[stat] = itemStat;
            }
        }

        foreach ((ItemSlot slot, Item item) in items)
        {
            if (slot != ItemSlot.NONE)
            {
                trigger.Session.FieldManager.BroadcastPacket(EquipmentPacket.EquipItem(player.FieldPlayer, item, slot));
            }
            else
            {
                player.Inventory.RemoveItem(trigger.Session, item.Uid, out Item _);
                player.Inventory.AddItem(trigger.Session, item, true);

                if (player.FieldPlayer.ActivePet is not null)
                {
                    trigger.Session.FieldManager.RemovePet(player.FieldPlayer.ActivePet);
                }

                trigger.Session.FieldManager.AddPet(trigger.Session, item.Uid);
            }
        }

        player.FieldPlayer?.ComputeStats();
    }
}
