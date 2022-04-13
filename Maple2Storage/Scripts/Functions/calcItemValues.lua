-- Job values
GlobalJob = 0
Beginner = 1
Knight = 10
Berserker = 20
Wizard = 30
Priest = 40
Archer = 50
HeavyGunner = 60
Thief = 70
Assassin = 80
RuneBlader = 90
Striker = 100
SoulBinder = 110

-- Armor and Weapon values
Earring = 12
Hat = 13
Clothes = 14
Pants = 15
Gloves = 16
Shoes = 17
Cape = 18
Necklace = 19
Ring = 20
Belt = 21
Overall = 22
Bludgeon = 30
Dagger = 31
Longsword = 32
Scepter = 33
ThrowingStar = 34
SpellBook = 40
Shield = 41
Greatsword = 50
Bow = 51
Staff = 52
Cannon = 53
Blade = 54
Knuckle = 55
Orb = 56

-- Rarities
NoRarity = 0
CommonRarity = 1
RareRarity = 2
ExceptionalRarity = 3
EpicRarity = 4
LegendaryRarity = 5
AscendantRarity = 6

ItemLevelRarityCoefficient = {}
ItemLevelRarityCoefficient[Bludgeon] = 1
ItemLevelRarityCoefficient[Dagger] = 1
ItemLevelRarityCoefficient[Longsword] = 1
ItemLevelRarityCoefficient[Scepter] = 1
ItemLevelRarityCoefficient[ThrowingStar] = 1
ItemLevelRarityCoefficient[SpellBook] = 1
ItemLevelRarityCoefficient[Shield] = 1
ItemLevelRarityCoefficient[Greatsword] = 1
ItemLevelRarityCoefficient[Bow] = 1
ItemLevelRarityCoefficient[Staff] = 1
ItemLevelRarityCoefficient[Cannon] = 1
ItemLevelRarityCoefficient[Blade] = 1
ItemLevelRarityCoefficient[Knuckle] = 1
ItemLevelRarityCoefficient[Orb] = 1
ItemLevelRarityCoefficient[Hat] = 1
ItemLevelRarityCoefficient[Clothes] = 1
ItemLevelRarityCoefficient[Pants] = 1
ItemLevelRarityCoefficient[Gloves] = 1
ItemLevelRarityCoefficient[Shoes] = 1
ItemLevelRarityCoefficient[Overall] = 1
ItemLevelRarityCoefficient[Earring] = 0
ItemLevelRarityCoefficient[Cape] = 0
ItemLevelRarityCoefficient[Necklace] = 0
ItemLevelRarityCoefficient[Ring] = 0
ItemLevelRarityCoefficient[Belt] = 1

ItemLevelCoefficient = {}
ItemLevelCoefficient[Bludgeon] = 1
ItemLevelCoefficient[Dagger] = 1
ItemLevelCoefficient[Longsword] = 1
ItemLevelCoefficient[Scepter] = 1
ItemLevelCoefficient[ThrowingStar] = 1
ItemLevelCoefficient[SpellBook] = 1
ItemLevelCoefficient[Shield] = 1
ItemLevelCoefficient[Greatsword] = 1
ItemLevelCoefficient[Bow] = 1
ItemLevelCoefficient[Staff] = 1
ItemLevelCoefficient[Cannon] = 1
ItemLevelCoefficient[Blade] = 1
ItemLevelCoefficient[Knuckle] = 1
ItemLevelCoefficient[Orb] = 1
ItemLevelCoefficient[Hat] = 0.21
ItemLevelCoefficient[Clothes] = 0.32
ItemLevelCoefficient[Pants] = 0.3
ItemLevelCoefficient[Gloves] = 0.06
ItemLevelCoefficient[Shoes] = 0.06
ItemLevelCoefficient[Overall] = 0.62
ItemLevelCoefficient[Earring] = 0.2
ItemLevelCoefficient[Cape] = 0.2
ItemLevelCoefficient[Necklace] = 0.2
ItemLevelCoefficient[Ring] = 0.2
ItemLevelCoefficient[Belt] = 0.2

LimitBreakItemLevelCoefficient = {}
LimitBreakItemLevelCoefficient[0] = 0
LimitBreakItemLevelCoefficient[1] = 0.02
LimitBreakItemLevelCoefficient[2] = 0.04
LimitBreakItemLevelCoefficient[3] = 0.06
LimitBreakItemLevelCoefficient[4] = 0.08
LimitBreakItemLevelCoefficient[5] = 0.1
LimitBreakItemLevelCoefficient[6] = 0.14
LimitBreakItemLevelCoefficient[7] = 0.18
LimitBreakItemLevelCoefficient[8] = 0.23
LimitBreakItemLevelCoefficient[9] = 0.29
LimitBreakItemLevelCoefficient[10] = 0.44
LimitBreakItemLevelCoefficient[11] = 0.74
LimitBreakItemLevelCoefficient[12] = 1.05
LimitBreakItemLevelCoefficient[13] = 1.36
LimitBreakItemLevelCoefficient[14] = 1.68
LimitBreakItemLevelCoefficient[15] = 2

LimitBreakItemLevelCoefficientNA = {}
LimitBreakItemLevelCoefficientNA[0] = 0
LimitBreakItemLevelCoefficientNA[1] = 0.02
LimitBreakItemLevelCoefficientNA[2] = 0.04
LimitBreakItemLevelCoefficientNA[3] = 0.07
LimitBreakItemLevelCoefficientNA[4] = 0.1
LimitBreakItemLevelCoefficientNA[5] = 0.14
LimitBreakItemLevelCoefficientNA[6] = 0.19
LimitBreakItemLevelCoefficientNA[7] = 0.25
LimitBreakItemLevelCoefficientNA[8] = 0.32
LimitBreakItemLevelCoefficientNA[9] = 0.4
LimitBreakItemLevelCoefficientNA[10] = 0.5
LimitBreakItemLevelCoefficientNA[11] = 0.64
LimitBreakItemLevelCoefficientNA[12] = 0.84
LimitBreakItemLevelCoefficientNA[13] = 1.12
LimitBreakItemLevelCoefficientNA[14] = 1.5
LimitBreakItemLevelCoefficientNA[15] = 2

LevelScoreFactorNA = {}
for i = 0, 99 do
    LevelScoreFactorNA[i] = 0
end
LevelScoreFactorNA[57] = 2.899
LevelScoreFactorNA[60] = 3.4442
LevelScoreFactorNA[67] = 12.538
LevelScoreFactorNA[70] = 14.15
LevelScoreFactorNA[80] = 45.91
LevelScoreFactorNA[90] = 140.13

RarityScoreFactor = {}
RarityScoreFactor[NoRarity] = 1
RarityScoreFactor[CommonRarity] = 1
RarityScoreFactor[RareRarity] = 1
RarityScoreFactor[ExceptionalRarity] = 1
RarityScoreFactor[EpicRarity] = 0.558
RarityScoreFactor[LegendaryRarity] = 1.2
RarityScoreFactor[AscendantRarity] = 1.9

ArmorConstantSlotCoefficient = {}
ArmorConstantSlotCoefficient[Overall] = 0.62
ArmorConstantSlotCoefficient[Clothes] = 0.32
ArmorConstantSlotCoefficient[Pants] = 0.3
ArmorConstantSlotCoefficient[Hat] = 0.21
ArmorConstantSlotCoefficient[Gloves] = 0.06
ArmorConstantSlotCoefficient[Shoes] = 0.06
ArmorConstantSlotCoefficient[Earring] = 0.05
ArmorConstantSlotCoefficient[Cape] = 0.05
ArmorConstantSlotCoefficient[Necklace] = 0.05
ArmorConstantSlotCoefficient[Ring] = 0.05
ArmorConstantSlotCoefficient[Belt] = 0.05
ArmorConstantSlotCoefficient[Shield] = 0.15

ArmorConstantJobCoefficient = {}
ArmorConstantJobCoefficient[GlobalJob] = 0.8
ArmorConstantJobCoefficient[Beginner] = 0.9
ArmorConstantJobCoefficient[Knight] = 1.1
ArmorConstantJobCoefficient[Berserker] = 1
ArmorConstantJobCoefficient[Wizard] = 0.9
ArmorConstantJobCoefficient[Priest] = 0.88
ArmorConstantJobCoefficient[Archer] = 0.93
ArmorConstantJobCoefficient[HeavyGunner] = 0.95
ArmorConstantJobCoefficient[Thief] = 0.95
ArmorConstantJobCoefficient[Assassin] = 0.9
ArmorConstantJobCoefficient[RuneBlader] = 0.97
ArmorConstantJobCoefficient[Striker] = 1
ArmorConstantJobCoefficient[SoulBinder] = 0.9

ArmorConstantRarityCoefficient = {}
ArmorConstantRarityCoefficient[1] = {}
ArmorConstantRarityCoefficient[1][CommonRarity] = 0.9
ArmorConstantRarityCoefficient[1][RareRarity] = 0.98
ArmorConstantRarityCoefficient[1][ExceptionalRarity] = 1.06
ArmorConstantRarityCoefficient[1][EpicRarity] = 1.14
ArmorConstantRarityCoefficient[1][LegendaryRarity] = 1.21
ArmorConstantRarityCoefficient[1][AscendantRarity] = 1.4
ArmorConstantRarityCoefficient[2] = {}
ArmorConstantRarityCoefficient[2][CommonRarity] = 1
ArmorConstantRarityCoefficient[2][RareRarity] = 1.1
ArmorConstantRarityCoefficient[2][ExceptionalRarity] = 1.2
ArmorConstantRarityCoefficient[2][EpicRarity] = 1.3
ArmorConstantRarityCoefficient[2][LegendaryRarity] = 1.45
ArmorConstantRarityCoefficient[2][AscendantRarity] = 1.6

WeaponRarityAddndd = {}
WeaponRarityAddndd[CommonRarity] = 0
WeaponRarityAddndd[RareRarity] = 0
WeaponRarityAddndd[ExceptionalRarity] = 0
WeaponRarityAddndd[EpicRarity] = 0.9846
WeaponRarityAddndd[LegendaryRarity] = 2.061
WeaponRarityAddndd[AscendantRarity] = 2.061

WeaponRarityAddndd50NA = {}
WeaponRarityAddndd50NA[CommonRarity] = 0
WeaponRarityAddndd50NA[RareRarity] = 0
WeaponRarityAddndd50NA[ExceptionalRarity] = 0
WeaponRarityAddndd50NA[EpicRarity] = 0.9846
WeaponRarityAddndd50NA[LegendaryRarity] = 1.426
WeaponRarityAddndd50NA[AscendantRarity] = 1.795

WeaponRarityAddndd60 = {}
WeaponRarityAddndd60[CommonRarity] = 0
WeaponRarityAddndd60[RareRarity] = 0
WeaponRarityAddndd60[ExceptionalRarity] = 0
WeaponRarityAddndd60[EpicRarity] = 1.531
WeaponRarityAddndd60[LegendaryRarity] = 1.833
WeaponRarityAddndd60[AscendantRarity] = 2.132

WeaponRarityAddndd60KR = {}
WeaponRarityAddndd60KR[CommonRarity] = 0
WeaponRarityAddndd60KR[RareRarity] = 0
WeaponRarityAddndd60KR[ExceptionalRarity] = 0
WeaponRarityAddndd60KR[EpicRarity] = 0.6486
WeaponRarityAddndd60KR[LegendaryRarity] = 1.123
WeaponRarityAddndd60KR[AscendantRarity] = 1.5408

WeaponRarityAddndd60NA = {}
WeaponRarityAddndd60NA[CommonRarity] = 0
WeaponRarityAddndd60NA[RareRarity] = 0
WeaponRarityAddndd60NA[ExceptionalRarity] = 0
WeaponRarityAddndd60NA[EpicRarity] = 2.016
WeaponRarityAddndd60NA[LegendaryRarity] = 2.22
WeaponRarityAddndd60NA[AscendantRarity] = 2.442

WeaponRarityAddndd70 = {}
WeaponRarityAddndd70[CommonRarity] = 0
WeaponRarityAddndd70[RareRarity] = 0
WeaponRarityAddndd70[ExceptionalRarity] = 0
WeaponRarityAddndd70[EpicRarity] = 3.248
WeaponRarityAddndd70[LegendaryRarity] = 3.105
WeaponRarityAddndd70[AscendantRarity] = 3.127

WeaponRarityAddndd70KR = {}
WeaponRarityAddndd70KR[CommonRarity] = 0
WeaponRarityAddndd70KR[RareRarity] = 0
WeaponRarityAddndd70KR[ExceptionalRarity] = 0
WeaponRarityAddndd70KR[EpicRarity] = 1.2445
WeaponRarityAddndd70KR[LegendaryRarity] = 1.6125
WeaponRarityAddndd70KR[AscendantRarity] = 1.947

WeaponRarityAddndd70NA = {}
WeaponRarityAddndd70NA[CommonRarity] = 0
WeaponRarityAddndd70NA[RareRarity] = 0
WeaponRarityAddndd70NA[ExceptionalRarity] = 0
WeaponRarityAddndd70NA[EpicRarity] = 3.174
WeaponRarityAddndd70NA[LegendaryRarity] = 3.084
WeaponRarityAddndd70NA[AscendantRarity] = 3.129

WeaponRarityAddndd80 = {}
WeaponRarityAddndd80[CommonRarity] = 0
WeaponRarityAddndd80[RareRarity] = 0
WeaponRarityAddndd80[ExceptionalRarity] = 0
WeaponRarityAddndd80[EpicRarity] = 4.841
WeaponRarityAddndd80[LegendaryRarity] = 4.344
WeaponRarityAddndd80[AscendantRarity] = 4.13

WeaponRarityAddndd80KR = {}
WeaponRarityAddndd80KR[CommonRarity] = 0
WeaponRarityAddndd80KR[RareRarity] = 0
WeaponRarityAddndd80KR[ExceptionalRarity] = 0
WeaponRarityAddndd80KR[EpicRarity] = 2.085
WeaponRarityAddndd80KR[LegendaryRarity] = 2.2669
WeaponRarityAddndd80KR[AscendantRarity] = 2.4771

WeaponRarityAddndd80NA = {}
WeaponRarityAddndd80NA[CommonRarity] = 0
WeaponRarityAddndd80NA[RareRarity] = 0
WeaponRarityAddndd80NA[ExceptionalRarity] = 0
WeaponRarityAddndd80NA[EpicRarity] = 4.7672
WeaponRarityAddndd80NA[LegendaryRarity] = 4.2895
WeaponRarityAddndd80NA[AscendantRarity] = 4.0858

WeaponRarityAddndd90 = {}
WeaponRarityAddndd90[CommonRarity] = 0
WeaponRarityAddndd90[RareRarity] = 0
WeaponRarityAddndd90[ExceptionalRarity] = 0
WeaponRarityAddndd90[EpicRarity] = 7.0691
WeaponRarityAddndd90[LegendaryRarity] = 6.0235
WeaponRarityAddndd90[AscendantRarity] = 5.4645

WeaponRarityAddndd90KR = {}
WeaponRarityAddndd90KR[CommonRarity] = 0
WeaponRarityAddndd90KR[RareRarity] = 0
WeaponRarityAddndd90KR[ExceptionalRarity] = 0
WeaponRarityAddndd90KR[EpicRarity] = 3.262
WeaponRarityAddndd90KR[LegendaryRarity] = 3.1535
WeaponRarityAddndd90KR[AscendantRarity] = 3.182

WeaponRarityAddndd90NA = {}
WeaponRarityAddndd90NA[CommonRarity] = 0
WeaponRarityAddndd90NA[RareRarity] = 0
WeaponRarityAddndd90NA[ExceptionalRarity] = 0
WeaponRarityAddndd90NA[EpicRarity] = 6.9675
WeaponRarityAddndd90NA[LegendaryRarity] = 5.947
WeaponRarityAddndd90NA[AscendantRarity] = 5.4043

WeaponRarityAddwap = {}
WeaponRarityAddwap[CommonRarity] = 0
WeaponRarityAddwap[RareRarity] = 0
WeaponRarityAddwap[ExceptionalRarity] = 0
WeaponRarityAddwap[EpicRarity] = 0.9828
WeaponRarityAddwap[LegendaryRarity] = 2.0589
WeaponRarityAddwap[AscendantRarity] = 2.0589

WeaponRarityAddwap50NA = {}
WeaponRarityAddwap50NA[CommonRarity] = 0
WeaponRarityAddwap50NA[RareRarity] = 0
WeaponRarityAddwap50NA[ExceptionalRarity] = 0
WeaponRarityAddwap50NA[EpicRarity] = 0.9828
WeaponRarityAddwap50NA[LegendaryRarity] = 1.4215
WeaponRarityAddwap50NA[AscendantRarity] = 1.7975

WeaponRarityAddwap60 = {}
WeaponRarityAddwap60[CommonRarity] = 0
WeaponRarityAddwap60[RareRarity] = 0
WeaponRarityAddwap60[ExceptionalRarity] = 0
WeaponRarityAddwap60[EpicRarity] = 1.531
WeaponRarityAddwap60[LegendaryRarity] = 1.8285
WeaponRarityAddwap60[AscendantRarity] = 2.118

WeaponRarityAddwap60KR = {}
WeaponRarityAddwap60KR[CommonRarity] = 0
WeaponRarityAddwap60KR[RareRarity] = 0
WeaponRarityAddwap60KR[ExceptionalRarity] = 0
WeaponRarityAddwap60KR[EpicRarity] = 0.6486
WeaponRarityAddwap60KR[LegendaryRarity] = 1.123
WeaponRarityAddwap60KR[AscendantRarity] = 1.5408

WeaponRarityAddwap60NA = {}
WeaponRarityAddwap60NA[CommonRarity] = 0
WeaponRarityAddwap60NA[RareRarity] = 0
WeaponRarityAddwap60NA[ExceptionalRarity] = 0
WeaponRarityAddwap60NA[EpicRarity] = 2.0159
WeaponRarityAddwap60NA[LegendaryRarity] = 2.2145
WeaponRarityAddwap60NA[AscendantRarity] = 2.4356

WeaponRarityAddwap70 = {}
WeaponRarityAddwap70[CommonRarity] = 0
WeaponRarityAddwap70[RareRarity] = 0
WeaponRarityAddwap70[ExceptionalRarity] = 0
WeaponRarityAddwap70[EpicRarity] = 2.4979
WeaponRarityAddwap70[LegendaryRarity] = 2.5491
WeaponRarityAddwap70[AscendantRarity] = 2.6866

WeaponRarityAddwap70KR = {}
WeaponRarityAddwap70KR[CommonRarity] = 0
WeaponRarityAddwap70KR[RareRarity] = 0
WeaponRarityAddwap70KR[ExceptionalRarity] = 0
WeaponRarityAddwap70KR[EpicRarity] = 1.2422
WeaponRarityAddwap70KR[LegendaryRarity] = 1.6133
WeaponRarityAddwap70KR[AscendantRarity] = 1.9477

WeaponRarityAddwap70NA = {}
WeaponRarityAddwap70NA[CommonRarity] = 0
WeaponRarityAddwap70NA[RareRarity] = 0
WeaponRarityAddwap70NA[ExceptionalRarity] = 0
WeaponRarityAddwap70NA[EpicRarity] = 3.1675
WeaponRarityAddwap70NA[LegendaryRarity] = 3.0833
WeaponRarityAddwap70NA[AscendantRarity] = 3.1261

WeaponRarityAddwap80 = {}
WeaponRarityAddwap80[CommonRarity] = 0
WeaponRarityAddwap80[RareRarity] = 0
WeaponRarityAddwap80[ExceptionalRarity] = 0
WeaponRarityAddwap80[EpicRarity] = 3.8106
WeaponRarityAddwap80[LegendaryRarity] = 3.5679
WeaponRarityAddwap80[AscendantRarity] = 3.5112

WeaponRarityAddwap80KR = {}
WeaponRarityAddwap80KR[CommonRarity] = 0
WeaponRarityAddwap80KR[RareRarity] = 0
WeaponRarityAddwap80KR[ExceptionalRarity] = 0
WeaponRarityAddwap80KR[EpicRarity] = 2.0836
WeaponRarityAddwap80KR[LegendaryRarity] = 2.2663
WeaponRarityAddwap80KR[AscendantRarity] = 2.4763

WeaponRarityAddwap80NA = {}
WeaponRarityAddwap80NA[CommonRarity] = 0
WeaponRarityAddwap80NA[RareRarity] = 0
WeaponRarityAddwap80NA[ExceptionalRarity] = 0
WeaponRarityAddwap80NA[EpicRarity] = 4.76
WeaponRarityAddwap80NA[LegendaryRarity] = 4.2835
WeaponRarityAddwap80NA[AscendantRarity] = 4.0804

WeaponRarityAddwap90 = {}
WeaponRarityAddwap90[CommonRarity] = 0
WeaponRarityAddwap90[RareRarity] = 0
WeaponRarityAddwap90[ExceptionalRarity] = 0
WeaponRarityAddwap90[EpicRarity] = 5.6481
WeaponRarityAddwap90[LegendaryRarity] = 4.9527
WeaponRarityAddwap90[AscendantRarity] = 4.6126

WeaponRarityAddwap90KR = {}
WeaponRarityAddwap90KR[CommonRarity] = 0
WeaponRarityAddwap90KR[RareRarity] = 0
WeaponRarityAddwap90KR[ExceptionalRarity] = 0
WeaponRarityAddwap90KR[EpicRarity] = 3.2613
WeaponRarityAddwap90KR[LegendaryRarity] = 3.1538
WeaponRarityAddwap90KR[AscendantRarity] = 3.1822

WeaponRarityAddwap90NA = {}
WeaponRarityAddwap90NA[CommonRarity] = 0
WeaponRarityAddwap90NA[RareRarity] = 0
WeaponRarityAddwap90NA[ExceptionalRarity] = 0
WeaponRarityAddwap90NA[EpicRarity] = 6.9602
WeaponRarityAddwap90NA[LegendaryRarity] = 5.9416
WeaponRarityAddwap90NA[AscendantRarity] = 5.399

--
AccConstantRarityCoefficient = {}
AccConstantRarityCoefficient[CommonRarity] = 1
AccConstantRarityCoefficient[RareRarity] = 1.1
AccConstantRarityCoefficient[ExceptionalRarity] = 1.2
AccConstantRarityCoefficient[EpicRarity] = 1.3
AccConstantRarityCoefficient[LegendaryRarity] = 1.4
AccConstantRarityCoefficient[AscendantRarity] = 1.4

ShieldConstantRarityCoefficient = {}
ShieldConstantRarityCoefficient[CommonRarity] = 0.1
ShieldConstantRarityCoefficient[RareRarity] = 0.2
ShieldConstantRarityCoefficient[ExceptionalRarity] = 0.3
ShieldConstantRarityCoefficient[EpicRarity] = 0.4
ShieldConstantRarityCoefficient[LegendaryRarity] = 0.5
ShieldConstantRarityCoefficient[AscendantRarity] = 0.5

--
WeaponRarityHPCorrection = {}
WeaponRarityHPCorrection[1] = {}
WeaponRarityHPCorrection[1][CommonRarity] = 0.3
WeaponRarityHPCorrection[1][RareRarity] = 0.4
WeaponRarityHPCorrection[1][ExceptionalRarity] = 0.5
WeaponRarityHPCorrection[1][EpicRarity] = 0.6
WeaponRarityHPCorrection[1][LegendaryRarity] = 0.7
WeaponRarityHPCorrection[1][AscendantRarity] = 0.8

WeaponHPSlotCorrection = {}
WeaponHPSlotCorrection[Bludgeon] = 0.1
WeaponHPSlotCorrection[Dagger] = 0.3
WeaponHPSlotCorrection[Longsword] = 0.04
WeaponHPSlotCorrection[Scepter] = 0.1
WeaponHPSlotCorrection[ThrowingStar] = 0.3
WeaponHPSlotCorrection[Greatsword] = 0.2
WeaponHPSlotCorrection[Bow] = 0.1
WeaponHPSlotCorrection[Staff] = 0.1
WeaponHPSlotCorrection[Cannon] = 0.2
WeaponHPSlotCorrection[Blade] = 0.1
WeaponHPSlotCorrection[Knuckle] = 0.1
WeaponHPSlotCorrection[Orb] = 0.2

WeaponSlotCoefficient = {}
WeaponSlotCoefficient[Bludgeon] = 1
WeaponSlotCoefficient[Dagger] = 1.03
WeaponSlotCoefficient[Longsword] = 0.95
WeaponSlotCoefficient[Scepter] = 0.92
WeaponSlotCoefficient[ThrowingStar] = 0.95
WeaponSlotCoefficient[Greatsword] = 1.1
WeaponSlotCoefficient[Bow] = 1.03
WeaponSlotCoefficient[Staff] = 1.2
WeaponSlotCoefficient[Cannon] = 1.05
WeaponSlotCoefficient[Blade] = 1.045
WeaponSlotCoefficient[Knuckle] = 1.13
WeaponSlotCoefficient[Orb] = 0.96

WeaponAttackSpeedCoefficient = {}
WeaponAttackSpeedCoefficient[Bludgeon] = 0.95
WeaponAttackSpeedCoefficient[Dagger] = 1.05
WeaponAttackSpeedCoefficient[Longsword] = 1
WeaponAttackSpeedCoefficient[Scepter] = 1.1
WeaponAttackSpeedCoefficient[ThrowingStar] = 1
WeaponAttackSpeedCoefficient[Greatsword] = 0.95
WeaponAttackSpeedCoefficient[Bow] = 1.05
WeaponAttackSpeedCoefficient[Staff] = 1
WeaponAttackSpeedCoefficient[Cannon] = 0.9
WeaponAttackSpeedCoefficient[Blade] = 0.975
WeaponAttackSpeedCoefficient[Knuckle] = 1.05
WeaponAttackSpeedCoefficient[Orb] = 0.95

WeaponRarityCoefficient = {}
WeaponRarityCoefficient[1] = {}
WeaponRarityCoefficient[1][CommonRarity] = 0.9
WeaponRarityCoefficient[1][RareRarity] = 0.98
WeaponRarityCoefficient[1][ExceptionalRarity] = 1.06
WeaponRarityCoefficient[1][EpicRarity] = 1.14
WeaponRarityCoefficient[1][LegendaryRarity] = 1.21
WeaponRarityCoefficient[1][AscendantRarity] = 1.4
WeaponRarityCoefficient[2] = {}
WeaponRarityCoefficient[2][CommonRarity] = 0.9
WeaponRarityCoefficient[2][RareRarity] = 0.98
WeaponRarityCoefficient[2][ExceptionalRarity] = 1.06
WeaponRarityCoefficient[2][EpicRarity] = 1.14
WeaponRarityCoefficient[2][LegendaryRarity] = 1.21
WeaponRarityCoefficient[2][AscendantRarity] = 1.4

WeaponSlotDeviation = {}
WeaponSlotDeviation[Bludgeon] = {}
WeaponSlotDeviation[Bludgeon][1] = 0.05
WeaponSlotDeviation[Bludgeon][2] = 0.1
WeaponSlotDeviation[Dagger] = {}
WeaponSlotDeviation[Dagger][1] = 0.15
WeaponSlotDeviation[Dagger][2] = 0.3
WeaponSlotDeviation[Longsword] = {}
WeaponSlotDeviation[Longsword][1] = 0.02
WeaponSlotDeviation[Longsword][2] = 0.04
WeaponSlotDeviation[Scepter] = {}
WeaponSlotDeviation[Scepter][1] = 0.05
WeaponSlotDeviation[Scepter][2] = 0.1
WeaponSlotDeviation[ThrowingStar] = {}
WeaponSlotDeviation[ThrowingStar][1] = 0.15
WeaponSlotDeviation[ThrowingStar][2] = 0.3
WeaponSlotDeviation[Greatsword] = {}
WeaponSlotDeviation[Greatsword][1] = 0.1
WeaponSlotDeviation[Greatsword][2] = 0.2
WeaponSlotDeviation[Bow] = {}
WeaponSlotDeviation[Bow][1] = 0.05
WeaponSlotDeviation[Bow][2] = 0.1
WeaponSlotDeviation[Staff] = {}
WeaponSlotDeviation[Staff][1] = 0.05
WeaponSlotDeviation[Staff][2] = 0.1
WeaponSlotDeviation[Cannon] = {}
WeaponSlotDeviation[Cannon][1] = 0.1
WeaponSlotDeviation[Cannon][2] = 0.2
WeaponSlotDeviation[Blade] = {}
WeaponSlotDeviation[Blade][1] = 0.1
WeaponSlotDeviation[Blade][2] = 0.2
WeaponSlotDeviation[Knuckle] = {}
WeaponSlotDeviation[Knuckle][1] = 0.05
WeaponSlotDeviation[Knuckle][2] = 0.1
WeaponSlotDeviation[Orb] = {}
WeaponSlotDeviation[Orb][1] = 0.1
WeaponSlotDeviation[Orb][2] = 0.2

StaticArmorRarityCoefficient = {}
StaticArmorRarityCoefficient[CommonRarity] = 0.1
StaticArmorRarityCoefficient[RareRarity] = 0.12
StaticArmorRarityCoefficient[ExceptionalRarity] = 0.14
StaticArmorRarityCoefficient[EpicRarity] = 0.16
StaticArmorRarityCoefficient[LegendaryRarity] = 0.18
StaticArmorRarityCoefficient[AscendantRarity] = 0.2

StaticAccRarityCoefficient = {}
StaticAccRarityCoefficient[CommonRarity] = 0.4
StaticAccRarityCoefficient[RareRarity] = 0.55
StaticAccRarityCoefficient[ExceptionalRarity] = 0.7
StaticAccRarityCoefficient[EpicRarity] = 0.85
StaticAccRarityCoefficient[LegendaryRarity] = 1
StaticAccRarityCoefficient[AscendantRarity] = 1

WeaponCapSlotRarityValue = {}
WeaponCapSlotRarityValue[1] = {}
WeaponCapSlotRarityValue[1][CommonRarity] = 6
WeaponCapSlotRarityValue[1][RareRarity] = 7
WeaponCapSlotRarityValue[1][ExceptionalRarity] = 8
WeaponCapSlotRarityValue[1][EpicRarity] = 9
WeaponCapSlotRarityValue[1][LegendaryRarity] = 10
WeaponCapSlotRarityValue[1][AscendantRarity] = 10
WeaponCapSlotRarityValue[2] = {}
WeaponCapSlotRarityValue[2][CommonRarity] = 12
WeaponCapSlotRarityValue[2][RareRarity] = 14
WeaponCapSlotRarityValue[2][ExceptionalRarity] = 16
WeaponCapSlotRarityValue[2][EpicRarity] = 18
WeaponCapSlotRarityValue[2][LegendaryRarity] = 20
WeaponCapSlotRarityValue[2][AscendantRarity] = 20

WeaponRarityDoombookMAPCorrection = {}
WeaponRarityDoombookMAPCorrection[CommonRarity] = 0.925
WeaponRarityDoombookMAPCorrection[RareRarity] = 1.0175
WeaponRarityDoombookMAPCorrection[ExceptionalRarity] = 1.11
WeaponRarityDoombookMAPCorrection[EpicRarity] = 1.2025
WeaponRarityDoombookMAPCorrection[LegendaryRarity] = 1.2825
WeaponRarityDoombookMAPCorrection[AscendantRarity] = 1.2825

StaticWapmaxCoefficient = {}
StaticWapmaxCoefficient[CommonRarity] = 0.1
StaticWapmaxCoefficient[RareRarity] = 0.12
StaticWapmaxCoefficient[ExceptionalRarity] = 0.14
StaticWapmaxCoefficient[EpicRarity] = 0.16
StaticWapmaxCoefficient[LegendaryRarity] = 0.18
StaticWapmaxCoefficient[AscendantRarity] = 0.2

StaticRarityCoefficient = {}
StaticRarityCoefficient[CommonRarity] = 0.4
StaticRarityCoefficient[RareRarity] = 0.55
StaticRarityCoefficient[ExceptionalRarity] = 0.7
StaticRarityCoefficient[EpicRarity] = 0.85
StaticRarityCoefficient[LegendaryRarity] = 1
StaticRarityCoefficient[AscendantRarity] = 1

UpgradeFactor = 0.06

function round(firstValue, secondValue)
    if secondValue == nil then
        secondValue = 0
    end
    return math.floor(firstValue * (10 ^ secondValue) + 0.5 + 1e-06) / (10 ^ secondValue)
end

function clipValue(clipMinValue, floorValue, clipMaxValue)
    if clipMinValue < floorValue then
        clipMinValue = floorValue
    end
    if clipMaxValue < clipMinValue then
        clipMinValue = clipMaxValue
    end
    return clipMinValue
end

function calcItemGearScore(gearScoreFactorValue, rarity, itemSlot, enchantLevel, limitBreakLevel)
    local maxEnchantValue = 15
    enchantLevel = clipValue(enchantLevel, 0, maxEnchantValue)
    local scoreResult = 0
    local itemGearScore = 0
    local addItemGearScore = 0
    if gearScoreFactorValue > 0 then
        if limitBreakLevel < 60 then
            if rarity > 3 and gearScoreFactorValue >= 50 then
                if ItemLevelRarityCoefficient[itemSlot] > 0 then
                    if rarity >= 5 then
                        scoreResult = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                            ItemLevelCoefficient[itemSlot] * 2 * (math.max)(rarity - 3, 1) +
                            (math.max)((gearScoreFactorValue - 50) * 100 * ItemLevelCoefficient[itemSlot], 0)
                    else
                        scoreResult = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                            ItemLevelCoefficient[itemSlot] * 2 * (math.max)(rarity - 3, 1)
                    end
                else
                    if itemSlot == Earring or itemSlot == Cape or itemSlot == Necklace or itemSlot == Ring then
                        if rarity >= 5 then
                            scoreResult = (10 * gearScoreFactorValue + (math.max)((rarity - 2) * 5, 0)) *
                                ItemLevelCoefficient[itemSlot] * 2 * (math.max)(rarity - 4, 1) +
                                (math.max)((gearScoreFactorValue - 50) * 100 * ItemLevelCoefficient[itemSlot], 0)
                        else
                            scoreResult = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                                ItemLevelCoefficient[itemSlot]
                        end
                    else
                        scoreResult = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                            ItemLevelCoefficient[itemSlot]
                    end
                end
            else
                scoreResult = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) * ItemLevelCoefficient[itemSlot]
            end
            if rarity > 3 and gearScoreFactorValue >= 50 then
                if ItemLevelRarityCoefficient[itemSlot] > 0 then
                    scoreResult = (2 + LevelScoreFactorNA[gearScoreFactorValue]) * 1030 * RarityScoreFactor[rarity] *
                        ItemLevelCoefficient[itemSlot]
                else
                    if itemSlot == Earring or itemSlot == Cape or itemSlot == Necklace or itemSlot == Ring then
                        scoreResult = (2 + LevelScoreFactorNA[gearScoreFactorValue]) * 1030 * RarityScoreFactor[rarity] *
                            ItemLevelCoefficient[itemSlot]
                    else
                        scoreResult = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                            ItemLevelCoefficient[itemSlot]
                    end
                end
            else
                scoreResult = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                    ItemLevelCoefficient[itemSlot]
            end
        else
            if limitBreakLevel < 70 then
                if rarity > 3 and gearScoreFactorValue >= 50 then
                    if ItemLevelRarityCoefficient[itemSlot] > 0 then
                        scoreResult = (2 + LevelScoreFactorNA[gearScoreFactorValue]) * 1030 * RarityScoreFactor[rarity] *
                            ItemLevelCoefficient[itemSlot]
                    else
                        if itemSlot == Earring or itemSlot == Cape or itemSlot == Necklace or itemSlot == Ring then
                            scoreResult = (2 + LevelScoreFactorNA[gearScoreFactorValue]) * 1030 * RarityScoreFactor[rarity] *
                                ItemLevelCoefficient[itemSlot]
                        else
                            scoreResult = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                                ItemLevelCoefficient[itemSlot]
                        end
                    end
                else
                    scoreResult = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                        ItemLevelCoefficient[itemSlot]
                end
            else
                if rarity > 3 and gearScoreFactorValue >= 50 then
                    local rarityFactor = RarityScoreFactor[rarity]
                    if ItemLevelRarityCoefficient[itemSlot] > 0 then
                        scoreResult = (2 + LevelScoreFactorNA[gearScoreFactorValue]) * 1030 * rarityFactor *
                            ItemLevelCoefficient[itemSlot]
                    else
                        if itemSlot == Earring or itemSlot == Cape or itemSlot == Necklace or itemSlot == Ring then
                            scoreResult = (2 + LevelScoreFactorNA[gearScoreFactorValue]) * 1030 * rarityFactor *
                                ItemLevelCoefficient[itemSlot]
                        else
                            scoreResult = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                                ItemLevelCoefficient[itemSlot]
                        end
                    end
                else
                    do
                        scoreResult = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                            ItemLevelCoefficient[itemSlot]
                        scoreResult = 0
                        if limitBreakLevel < 60 then
                            itemGearScore = scoreResult
                        else
                            if limitBreakLevel < 70 then
                                itemGearScore = scoreResult
                            else
                                itemGearScore = scoreResult
                            end
                        end
                        if limitBreakLevel < 60 then
                            if rarity >= 4 then
                                addItemGearScore = itemGearScore * LimitBreakItemLevelCoefficientNA[enchantLevel]
                            else
                                addItemGearScore = itemGearScore * LimitBreakItemLevelCoefficient[enchantLevel]
                            end
                        else
                            if limitBreakLevel < 70 then
                                if rarity >= 4 then
                                    addItemGearScore = itemGearScore * LimitBreakItemLevelCoefficientNA[enchantLevel]
                                else
                                    addItemGearScore = itemGearScore * LimitBreakItemLevelCoefficient[enchantLevel]
                                end
                            else
                                if limitBreakLevel < 80 then
                                    if rarity >= 4 then
                                        addItemGearScore = itemGearScore * LimitBreakItemLevelCoefficientNA[enchantLevel]
                                    else
                                        addItemGearScore = itemGearScore * LimitBreakItemLevelCoefficient[enchantLevel]
                                    end
                                else
                                    if rarity >= 4 then
                                        addItemGearScore = itemGearScore * LimitBreakItemLevelCoefficientNA[enchantLevel]
                                    else
                                        addItemGearScore = itemGearScore * LimitBreakItemLevelCoefficient[enchantLevel]
                                    end
                                end
                            end
                        end
                    end
                end
            end
        end
    end
    itemGearScore = scoreResult
    return itemGearScore, addItemGearScore
end

function constant_value_hp(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local hpValue = 0

    if itemSlot == Earring or itemSlot == Cape or itemSlot == Necklace or itemSlot == Ring or itemSlot == Belt then
        if optionLevelFactor > 50 then
            if itemLevel < 60 then
                if rarity == 4 then
                    hpValue = round((360 * 0.6) * ((1 + 0.06) ^ (optionLevelFactor - 50)), 0)
                elseif rarity == 5 then
                    hpValue = round((600 * 0.6 * ((1 + 0.06) ^ (optionLevelFactor - 50))), 0)
                elseif rarity >= 6 then
                    hpValue = round((757.44 * 0.6 * ((1 + 0.06) ^ (optionLevelFactor - 50))), 0)
                else
                    hpValue = round((((optionLevelFactor - 50) / 2) * 13), 0)
                end
            elseif rarity == 4 then
                hpValue = round(((360 * 0.6) * ((1 + 0.06) ^ (optionLevelFactor - (53 + ((optionLevelFactor / 10) - 6) * 6)))), 0)
            elseif rarity == 5 then
                hpValue = round(((600 * 0.6) * ((1 + 0.06) ^ (optionLevelFactor - (53 + ((optionLevelFactor / 10) - 6) * 6)))), 0)
            elseif rarity >= 6 then
                hpValue = round((757.44 * 0.6) * ((1 + 0.06) ^ (optionLevelFactor - (53 + (((optionLevelFactor / 10) - 6) * 6)))), 0)
            else
                hpValue = round((((optionLevelFactor - 50) / 2) * 13), 0)
            end
        else
            hpValue = 55
        end
    elseif itemSlot > 30 and itemSlot < 40 then
        if optionLevelFactor > 5 then
            hpValue = round(((1.2884 * optionLevelFactor) - ((6.56 * WeaponRarityHPCorrection[1][rarity]) * WeaponHPSlotCorrection[itemSlot])), 0)
        elseif optionLevelFactor < 6 then
            hpValue = 1
        else
            hpValue = 0
        end
    elseif itemSlot > 49 and itemSlot < 60 then
        if optionLevelFactor > 5 then
            hpValue = round(((1.2884 * optionLevelFactor) - (((6.56 * WeaponRarityHPCorrection[1][rarity]) * WeaponHPSlotCorrection[itemSlot]) * 2)), 0)
        elseif optionLevelFactor < 6 then
            hpValue = 2
        else
            hpValue = 0
        end
    else
        hpValue = 0
    end
    return hpValue
end

function constant_value_addndd(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local itemRarityFactor
    local itemRarityFactor2
    local baseAddNddValue
    local NddValueRoundedResult
    local NddValueRoundedResult2
    local addNddValue
    itemRarityFactor = 0
    itemRarityFactor2 = 0
    if itemLevel < 60 then
        itemRarityFactor = WeaponRarityAddndd50NA[rarity]
        itemRarityFactor2 = WeaponRarityAddndd50NA[4]
    elseif itemLevel < 70 then
        itemRarityFactor = WeaponRarityAddndd60NA[rarity]
        itemRarityFactor2 = WeaponRarityAddndd60NA[4]
    elseif itemLevel < 80 then
        itemRarityFactor = WeaponRarityAddndd70NA[rarity]
        itemRarityFactor2 = WeaponRarityAddndd70NA[4]
    elseif itemLevel < 90 then
        itemRarityFactor = WeaponRarityAddndd80NA[rarity]
        itemRarityFactor2 = WeaponRarityAddndd80NA[4]
    else
        itemRarityFactor = WeaponRarityAddndd90NA[rarity]
        itemRarityFactor2 = WeaponRarityAddndd90NA[4]
    end
    baseAddNddValue = 0
    NddValueRoundedResult = 0
    NddValueRoundedResult2 = 0
    if optionLevelFactor == 1 then
        baseAddNddValue = 9
    else
        addNddValue = 9
        for x = 2, optionLevelFactor do
            if x > 49 then
                armorLevelBaseCurrentLevelPlus = addNddValue * UpgradeFactor
            else
                armorLevelBaseCurrentLevelPlus = math.max((1 + ((x / 10) * 4)), 0)
            end
            addNddValue = addNddValue + armorLevelBaseCurrentLevelPlus
            baseAddNddValue = addNddValue
        end
    end
    if itemSlot == Belt then
        addNddValue = round((baseAddNddValue) * ArmorConstantSlotCoefficient[itemSlot] * ArmorConstantJobCoefficient[GlobalJob] *
            ArmorConstantRarityCoefficient[deviationValue][rarity], 0)
        NddValueRoundedResult = addNddValue * itemRarityFactor
        addNddValue = round((baseAddNddValue) * ArmorConstantSlotCoefficient[itemSlot] * ArmorConstantJobCoefficient[GlobalJob] *
            ArmorConstantRarityCoefficient[deviationValue][4], 0)
        NddValueRoundedResult2 = addNddValue * itemRarityFactor2
    else
        addNddValue = round((baseAddNddValue) * ArmorConstantSlotCoefficient[itemSlot] * ArmorConstantJobCoefficient[itemJob] *
            ArmorConstantRarityCoefficient[deviationValue][rarity], 0)
        NddValueRoundedResult = addNddValue * itemRarityFactor
        addNddValue = round((baseAddNddValue) * ArmorConstantSlotCoefficient[itemSlot] * ArmorConstantJobCoefficient[itemJob] *
            ArmorConstantRarityCoefficient[deviationValue][4], 0)
        NddValueRoundedResult2 = addNddValue * itemRarityFactor2
    end
    addNddValue = 0
    if itemLevel > 49 and rarity > 3 then
        addNddValue = round((NddValueRoundedResult + NddValueRoundedResult2 * (rarity - 4)), 0)
    end
    do
        return math.max(addNddValue, 0)
    end
end

function constant_value_ndd(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local nddValue
    local baseNddValue
    local addNddValue
    nddValue = 0
    baseNddValue = 0
    if optionLevelFactor == 1 then
        baseNddValue = 9
    else
        addNddValue = 9
        for x = 2, optionLevelFactor do
            if x > 49 then
                armorLevelBaseCurrentLevelPlus = addNddValue * UpgradeFactor
            else
                armorLevelBaseCurrentLevelPlus = math.max((1 + ((x / 10) * 4)), 0)
            end
            addNddValue = addNddValue + armorLevelBaseCurrentLevelPlus
        end
        baseNddValue = addNddValue
    end
    addNddValue = 0
    addNddValue = constant_value_addndd(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    if itemSlot == Belt then
        nddValue = round((baseNddValue) * ArmorConstantSlotCoefficient[itemSlot] * ArmorConstantJobCoefficient[GlobalJob] *
            ArmorConstantRarityCoefficient[deviationValue][rarity], 0) + addNddValue
    elseif itemLevel >= 70 and currentStatValue > 0 then
        nddValue = round((baseNddValue) * ArmorConstantSlotCoefficient[itemSlot] * ArmorConstantJobCoefficient[itemJob] *
            ArmorConstantRarityCoefficient[deviationValue][rarity] * (currentStatValue / 100), 0) + round(addNddValue * (currentStatValue / 100), 0)
    else
        nddValue = round((baseNddValue) * ArmorConstantSlotCoefficient[itemSlot] * ArmorConstantJobCoefficient[itemJob] *
            ArmorConstantRarityCoefficient[deviationValue][rarity], 0) + addNddValue
    end
    return nddValue
end

function constant_value_mar(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local marValue = 0
    if itemSlot == Earring then
        marValue = round((math.max(0, ((((4.5 + (1 * (optionLevelFactor - 12)))) / 1.5) * AccConstantRarityCoefficient[rarity]))), 0)
    elseif itemSlot == Ring then
        marValue = round((math.max(0, (((4 + (1.5 * ((optionLevelFactor - 12)))) / 1.5) * AccConstantRarityCoefficient[rarity]))), 0)
    elseif itemSlot == SpellBook or itemSlot == Shield then
        if rarity == 1 then
            marValue = 2
        elseif rarity == 2 then
            marValue = 4
        elseif rarity == 3 then
            marValue = 7
        elseif rarity == 4 then
            marValue = 9
        elseif rarity >= 5 then
            marValue = 12
        else
            marValue = 0
        end
    else
        marValue = 0
    end
    return marValue
end

function constant_value_par(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local parValue = 0
    if itemSlot == Cape then
        parValue = round(math.max(0, (3 + (((1 * (optionLevelFactor - 12)) / 1.5) * AccConstantRarityCoefficient[rarity]))), 0)
    elseif itemSlot == Necklace then
        parValue = round(math.max(0, (3 + (((1.5 * (optionLevelFactor - 12)) / 1.5) * AccConstantRarityCoefficient[rarity]))), 0)
    elseif itemSlot == SpellBook then
        if rarity == 1 then
            parValue = 2
        elseif rarity == 2 then
            parValue = 4
        elseif rarity == 3 then
            parValue = 7
        elseif rarity == 4 then
            parValue = 9
        elseif rarity >= 5 then
            parValue = 12
        else
            parValue = 0
        end
    else
        parValue = 0
    end
    return parValue
end

function constant_value_map(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local mapValue
    local fractionedFactor
    mapValue = 0
    if optionLevelFactor < 58 then
        mapValue = round(((round(((0.5502 * optionLevelFactor) + 0.1806), 0)) * WeaponRarityDoombookMAPCorrection[rarity]), 0)
    elseif optionLevelFactor > 57 then
        fractionedFactor = optionLevelFactor % 2
        if fractionedFactor == 1 then
            mapValue = round(((((0.2751 * (optionLevelFactor - 1)) + 16.136)) * WeaponRarityDoombookMAPCorrection[rarity]), 0)
        else
            mapValue = round(((((0.2751 * optionLevelFactor) + 16.136)) * WeaponRarityDoombookMAPCorrection[rarity]), 0)
        end
    else
        mapValue = 0
    end
    return mapValue
end

function constant_value_cap(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local capValue = 0
    if itemSlot > 29 and itemSlot < 40 then
        capValue = (WeaponCapSlotRarityValue[1])[rarity]
    else
        if itemSlot > 39 and itemSlot < 60 then
            capValue = (WeaponCapSlotRarityValue[2])[rarity]
        else
            capValue = 0
        end
    end
    return capValue
end

function constant_value_str(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local strValue = 0
    if itemLevel < 51 then
        if itemJob == Knight or itemJob == Berserker or itemJob == RuneBlader then
            if itemSlot == Overall then
                strValue = round(1.8 * (itemLevel - 20) / 3, 0)
            else
                strValue = round((itemLevel - 20) / 3, 0)
            end
        elseif itemJob == GlobalJob then
            if itemSlot == Overall then
                strValue = round(1.8 * (itemLevel - 20) / 6, 0)
            else
                strValue = round((itemLevel - 20) / 6, 0)
            end
        else
            strValue = 0
        end
    else
        if itemJob == Knight or itemJob == Berserker or itemJob == RuneBlader then
            if itemSlot == Overall then
                strValue = round(1.8 * (2 * itemLevel - 90) / 2, 0)
            else
                strValue = round(2 * itemLevel - 90, 0)
            end
        elseif itemJob == GlobalJob then
            if itemSlot == Overall then
                strValue = round(1.8 * (2 * itemLevel - 90) / 6, 0)
            else
                strValue = round((2 * itemLevel - 90) / 3, 0)
            end
        else
            strValue = 0
        end
    end
    return strValue
end

function constant_value_int(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local intValue = 0
    if itemLevel < 51 then
        if itemJob == Wizard or itemJob == Priest or itemJob == SoulBinder or itemJob == Striker and l_14_7 == 1 then
            if itemSlot == Overall then
                intValue = round(1.8 * (itemLevel - 20) / 3, 0)
            else
                intValue = round((itemLevel - 20) / 3, 0)
            end
        elseif itemJob == GlobalJob then
            if itemSlot == Overall then
                intValue = round(1.8 * (itemLevel - 20) / 6, 0)
            else
                intValue = round((itemLevel - 20) / 6, 0)
            end
        else
            intValue = 0
        end
    else
        if itemJob == Wizard or itemJob == Priest or itemJob == SoulBinder or itemJob == Striker and l_14_7 == 1 then
            if itemSlot == Overall then
                intValue = round(1.8 * (2 * itemLevel - 90) / 2, 0)
            else
                intValue = round(2 * itemLevel - 90, 0)
            end
        elseif itemJob == GlobalJob then
            if itemSlot == Overall then
                intValue = round(1.8 * (2 * itemLevel - 90) / 6, 0)
            else
                intValue = round((2 * itemLevel - 90) / 3, 0)
            end
        else
            intValue = 0
        end
    end
    return intValue
end

function constant_value_dex(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local dexValue = 0
    if itemLevel < 51 then
        if itemJob == Archer or itemJob == HeavyGunner or itemJob == Striker then
            if itemSlot == Overall then
                dexValue = round(1.8 * (itemLevel - 20) / 3, 0)
            else
                dexValue = round((itemLevel - 20) / 3, 0)
            end
        elseif itemJob == GlobalJob then
            if itemSlot == Overall then
                dexValue = round(1.8 * (itemLevel - 20) / 6, 0)
            else
                dexValue = round((itemLevel - 20) / 6, 0)
            end
        else
            dexValue = 0
        end
    elseif itemJob == Archer or itemJob == HeavyGunner or itemJob == Striker then
        if itemSlot == Overall then
            dexValue = round(1.8 * (2 * itemLevel - 90) / 2, 0)
        else
            dexValue = round(2 * itemLevel - 90, 0)
        end
    elseif itemJob == GlobalJob then
        if itemSlot == Overall then
            dexValue = round(1.8 * (2 * itemLevel - 90) / 6, 0)
        else
            dexValue = round((2 * itemLevel - 90) / 3, 0)
        end
    else
        dexValue = 0
    end
    return dexValue
end

function constant_value_luk(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local lukValue = 0
    if itemLevel < 51 then
        if itemJob == Thief or itemJob == Assassin then
            if itemSlot == Overall then
                lukValue = round(((1.8 * (itemLevel - 20)) / 3), 0)
            else
                lukValue = round(((itemLevel - 20) / 3), 0)
            end
        elseif itemJob == GlobalJob then
            if itemSlot == Overall then
                lukValue = round(((1.8 * (itemLevel - 20)) / 6), 0)
            else
                lukValue = round(((itemLevel - 20) / 6), 0)
            end
        else
            lukValue = 0
        end
    elseif itemJob == Thief or itemJob == Assassin then
        if itemSlot == Overall then
            lukValue = round(((1.8 * ((2 * itemLevel) - 90)) / 2), 0)
        else
            lukValue = round(((2 * itemLevel) - 90), 0)
        end
    elseif itemJob == GlobalJob then
        if itemSlot == Overall then
            lukValue = round(((1.8 * (((2 * itemLevel) - 90))) / 6), 0)
        else
            lukValue = round((((2 * itemLevel) - 90) / 3), 0)
        end
    else
        lukValue = 0
    end
    return lukValue
end

function constant_value_addwap(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local weaponRarityFactor
    local weaponRarityFactor2
    local addWapValue = 0
    local weaponRoundedWapValue = 0
    local roundedWeaponWapValue1 = 0
    local roundedWeaponWapValue2 = 0
    local weaponRarityFactorResult = 0
    local roundedWeaponWapValue3 = 0
    local roundedWeaponWapValue4 = 0
    local weaponRarityFactor2Result = 0
    local minAddWapValue
    local maxAddWapValue
    weaponRarityFactor = 0
    weaponRarityFactor2 = 0
    if itemLevel < 60 then
        weaponRarityFactor = WeaponRarityAddwap[rarity]
        weaponRarityFactor2 = WeaponRarityAddwap[4]
    elseif itemLevel < 70 then
        weaponRarityFactor = WeaponRarityAddwap60NA[rarity]
        weaponRarityFactor2 = WeaponRarityAddwap60NA[4]
    elseif itemLevel < 80 then
        weaponRarityFactor = WeaponRarityAddwap70NA[rarity]
        weaponRarityFactor2 = WeaponRarityAddwap70NA[4]
    elseif itemLevel < 90 then
        weaponRarityFactor = WeaponRarityAddwap80NA[rarity]
        weaponRarityFactor2 = WeaponRarityAddwap80NA[4]
    else
        weaponRarityFactor = WeaponRarityAddwap90NA[rarity]
        weaponRarityFactor2 = WeaponRarityAddwap90NA[4]
    end
    if optionLevelFactor == 1 then
        addWapValue = 5
    else
        minAddWapValue = 5
        maxAddWapValue = 0
        for x = 2, optionLevelFactor do
            if x > 49 then
                maxAddWapValue = minAddWapValue * UpgradeFactor
            else
                maxAddWapValue = math.max((((x / 30) * 20) - 0.8), 0)
            end
            minAddWapValue = minAddWapValue + maxAddWapValue
            addWapValue = minAddWapValue
        end
    end
    weaponRoundedWapValue = round((((addWapValue) * WeaponSlotCoefficient[itemSlot]) / WeaponAttackSpeedCoefficient[itemSlot]), 1)
    roundedWeaponWapValue1 = round(((weaponRoundedWapValue * (WeaponRarityCoefficient[deviationValue][rarity])) * (1 - WeaponSlotDeviation[itemSlot][deviationValue])), 0)
    roundedWeaponWapValue2 = round(((weaponRoundedWapValue * WeaponRarityCoefficient[deviationValue][rarity]) * (1 + WeaponSlotDeviation[itemSlot][deviationValue])), 0)
    weaponRarityFactorResult = ((roundedWeaponWapValue1 + roundedWeaponWapValue2) / 2) * weaponRarityFactor
    roundedWeaponWapValue3 = round(((weaponRoundedWapValue * WeaponRarityCoefficient[deviationValue][4]) * (1 - WeaponSlotDeviation[itemSlot][deviationValue])), 0)
    roundedWeaponWapValue4 = round(((weaponRoundedWapValue * WeaponRarityCoefficient[deviationValue][4]) * (1 + WeaponSlotDeviation[itemSlot][deviationValue])), 0)
    weaponRarityFactor2Result = ((roundedWeaponWapValue3 + roundedWeaponWapValue4) / 2) * weaponRarityFactor2
    if itemLevel > 49 and rarity > 3 then
        minAddWapValue = round(weaponRarityFactorResult + weaponRarityFactor2Result * (rarity - 4), 0)
    end
    do
        return math.max(minAddWapValue, 0)
    end
end

function constant_value_wapmin(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local wapMinValue
    local weaponRoundedWapValue
    local maxWapMinValue
    local addWapValue
    local minWapMinValue
    wapMinValue = 0
    weaponRoundedWapValue = 0
    maxWapMinValue = 0
    if optionLevelFactor == 1 then
        wapMinValue = 5
    else
        addWapValue = 5
        minWapMinValue = 0
        for x = 2, optionLevelFactor do
            do
                if x > 49 then
                    minWapMinValue = addWapValue * UpgradeFactor
                else
                    minWapMinValue = math.max((((x / 30) * 20) - 0.8), 0)
                end
            end
            addWapValue = addWapValue + minWapMinValue
            wapMinValue = addWapValue
        end
    end
    weaponRoundedWapValue = round((((wapMinValue) * WeaponSlotCoefficient[itemSlot]) / WeaponAttackSpeedCoefficient[itemSlot]), 1)
    addWapValue = 0
    addWapValue = constant_value_addwap(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    if itemLevel < 60 then
        minWapMinValue = round(weaponRoundedWapValue * WeaponRarityCoefficient[deviationValue][rarity] * (1 - WeaponSlotDeviation[itemSlot][deviationValue]), 0)
        maxWapMinValue = minWapMinValue + addWapValue
    elseif itemLevel >= 70 and currentStatValue > 0 then
        maxWapMinValue = round(weaponRoundedWapValue * WeaponRarityCoefficient[deviationValue][rarity] * (1 - WeaponSlotDeviation[itemSlot][deviationValue]) * (currentStatValue / 100), 0) +
            round(addWapValue * (1 - WeaponSlotDeviation[itemSlot][deviationValue]) * (currentStatValue / 100), 0)
    else
        maxWapMinValue = round(weaponRoundedWapValue * WeaponRarityCoefficient[deviationValue][rarity] * (1 - WeaponSlotDeviation[itemSlot][deviationValue]), 0) +
            round(addWapValue * (1 - WeaponSlotDeviation[itemSlot][deviationValue]), 0)
    end
    minWapMinValue = maxWapMinValue
    return maxWapMinValue
end

function constant_value_wapmax(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local wapMaxValue
    local weaponRoundedWapValue
    local maxWapMaxValue
    local addWapValue
    local minWapMaxValue
    wapMaxValue = 0
    weaponRoundedWapValue = 0
    maxWapMaxValue = 0
    if optionLevelFactor == 1 then
        wapMaxValue = 5
    else
        addWapValue = 5
        minWapMaxValue = 0
        for x = 1, optionLevelFactor do
            do
                if x > 49 then
                    minWapMaxValue = addWapValue * UpgradeFactor
                else
                    minWapMaxValue = math.max((((x / 30) * 20) - 0.8), 0)
                end
            end
            addWapValue = addWapValue + minWapMaxValue
            wapMaxValue = addWapValue
        end
    end
    weaponRoundedWapValue = round((((wapMaxValue) * WeaponSlotCoefficient[itemSlot]) / WeaponAttackSpeedCoefficient[itemSlot]), 1)
    addWapValue = 0
    addWapValue = constant_value_addwap(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    if itemLevel < 60 then
        minWapMaxValue = round(weaponRoundedWapValue * WeaponRarityCoefficient[deviationValue][rarity] * (1 + WeaponSlotDeviation[itemSlot][deviationValue]), 0)
        maxWapMaxValue = minWapMaxValue + addWapValue
    elseif itemLevel >= 70 and currentStatValue > 0 then
        minWapMaxValue = round(weaponRoundedWapValue * WeaponRarityCoefficient[deviationValue][rarity] * (1 + WeaponSlotDeviation[itemSlot][deviationValue]) * (currentStatValue / 100), 0)
        maxWapMaxValue = minWapMaxValue + round(addWapValue * (1 + WeaponSlotDeviation[itemSlot][deviationValue]) * (currentStatValue / 100), 0)
    else
        minWapMaxValue = round(weaponRoundedWapValue * WeaponRarityCoefficient[deviationValue][rarity] * (1 + WeaponSlotDeviation[itemSlot][deviationValue]), 0)
        maxWapMaxValue = minWapMaxValue + round(addWapValue * (1 + WeaponSlotDeviation[itemSlot][deviationValue]), 0)
    end
    minWapMaxValue = maxWapMaxValue
    return minWapMaxValue
end

function static_value_hp(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local minHpValue = 0
    local maxHpValue = 0
    local staticMinHpValue = 0
    local staticMaxHpValue = 0

    if itemSlot == Overall then
        minHpValue = round(((math.max(((((-7e-005 * (optionLevelFactor ^ 3)) + (0.0162 * (optionLevelFactor ^ 2))) + (0.1656 * optionLevelFactor)) - 0.5098), 1)) * 1.8), 0)
    else
        minHpValue = round((math.max(((((-7e-005 * (optionLevelFactor ^ 3)) + (0.0162 * (optionLevelFactor ^ 2))) + (0.1656 * optionLevelFactor)) - 0.5098), 1)), 0)
    end
    if minHpValue < 7 then
        maxHpValue = minHpValue + 4
    elseif minHpValue > 6 and minHpValue < 9 then
        maxHpValue = minHpValue + 5
    elseif minHpValue > 8 and minHpValue < 11 then
        maxHpValue = minHpValue + 6
    elseif minHpValue > 10 and minHpValue < 14 then
        maxHpValue = minHpValue + 7
    elseif minHpValue > 13 and minHpValue < 16 then
        maxHpValue = minHpValue + 8
    elseif minHpValue > 15 and minHpValue < 20 then
        maxHpValue = minHpValue + 9
    elseif minHpValue > 19 and minHpValue < 22 then
        maxHpValue = minHpValue + 10
    elseif minHpValue > 21 and minHpValue < 25 then
        maxHpValue = minHpValue + 11
    elseif minHpValue > 24 and minHpValue < 28 then
        maxHpValue = minHpValue + 12
    elseif minHpValue > 27 and minHpValue < 32 then
        maxHpValue = minHpValue + 13
    elseif minHpValue > 31 and minHpValue < 37 then
        maxHpValue = minHpValue + 14
    elseif minHpValue > 36 and minHpValue < 42 then
        maxHpValue = minHpValue + 15
    elseif minHpValue > 41 and minHpValue < 46 then
        maxHpValue = minHpValue + 16
    elseif minHpValue > 45 and minHpValue < 53 then
        maxHpValue = minHpValue + 17
    elseif minHpValue > 52 and minHpValue < 63 then
        maxHpValue = minHpValue + 18
    elseif minHpValue > 62 and minHpValue < 75 then
        maxHpValue = minHpValue + 19
    elseif minHpValue > 74 then
        maxHpValue = minHpValue + 20
    else
        maxHpValue = minHpValue
    end
    if currentStatValue == 0 then
        staticMinHpValue = minHpValue
        staticMaxHpValue = maxHpValue
    else
        staticMinHpValue = round((maxHpValue * (currentStatValue / 100)), 0)
        staticMaxHpValue = round((maxHpValue * (currentStatValue / 100)), 0)
    end
    return staticMinHpValue, staticMaxHpValue
end

function static_value_addndd(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local itemRarityFactor = 0
    local rarityFactor = 0
    local armorLevelBaseCurrentLevelPlus
    local baseNddValue = 0
    local maxAddNddValue = 0
    local minAddNddValue = 0
    if itemLevel < 60 then
        itemRarityFactor = WeaponRarityAddndd50NA[rarity]
        rarityFactor = WeaponRarityAddndd50NA[4]
    elseif itemLevel < 70 then
        itemRarityFactor = WeaponRarityAddndd60NA[rarity]
        rarityFactor = WeaponRarityAddndd60NA[4]
    elseif itemLevel < 80 then
        itemRarityFactor = WeaponRarityAddndd70NA[rarity]
        rarityFactor = WeaponRarityAddndd70NA[4]
    elseif itemLevel < 90 then
        itemRarityFactor = WeaponRarityAddndd80NA[rarity]
        rarityFactor = WeaponRarityAddndd80NA[4]
    else
        itemRarityFactor = WeaponRarityAddndd90NA[rarity]
        rarityFactor = WeaponRarityAddndd90NA[4]
    end
    if optionLevelFactor == 1 then
        baseNddValue = 9
    else
        local addNddValue = 9
        for x = 2, optionLevelFactor do
            if x > 49 then
                armorLevelBaseCurrentLevelPlus = addNddValue * UpgradeFactor
            else
                armorLevelBaseCurrentLevelPlus = math.max((1 + x / 10 * 4), 0)
            end
            do
                addNddValue = addNddValue + armorLevelBaseCurrentLevelPlus
                baseNddValue = addNddValue
            end
        end
    end
    do
        if itemSlot == Belt then
            maxAddNddValue = math.max(round((baseNddValue) * ArmorConstantSlotCoefficient[itemSlot] * ArmorConstantJobCoefficient[GlobalJob] * StaticArmorRarityCoefficient[rarity], 0), 4) * itemRarityFactor
            minAddNddValue = math.max(round((baseNddValue) * ArmorConstantSlotCoefficient[itemSlot] * ArmorConstantJobCoefficient[GlobalJob] * StaticArmorRarityCoefficient[4], 0), 4) * rarityFactor
        else
            do
                if itemSlot == Earring or itemSlot == Cape or itemSlot == Necklace or itemSlot == Ring then
                    maxAddNddValue = math.max(round((baseNddValue) * ArmorConstantSlotCoefficient[itemSlot] * StaticAccRarityCoefficient[rarity], 0), 0) * itemRarityFactor
                    minAddNddValue = math.max(round((baseNddValue) * ArmorConstantSlotCoefficient[itemSlot] * StaticAccRarityCoefficient[4], 0), 0) * rarityFactor
                else
                    maxAddNddValue = math.max(round((baseNddValue) * ArmorConstantSlotCoefficient[itemSlot] * ArmorConstantJobCoefficient[itemJob] * StaticArmorRarityCoefficient[rarity], 0), 4) * itemRarityFactor
                    minAddNddValue = math.max(round((baseNddValue) * ArmorConstantSlotCoefficient[itemSlot] * ArmorConstantJobCoefficient[itemJob] * StaticArmorRarityCoefficient[4], 0), 4) * rarityFactor
                end
            end
        end
        local addNddValueResult = 0
        if itemLevel > 49 and rarity > 3 then
            if currentStatValue == 0 then
                addNddValueResult = round(maxAddNddValue + minAddNddValue * (rarity - 4), 0)
            else
                addNddValueResult = round((maxAddNddValue + minAddNddValue * (rarity - 4)) * (currentStatValue / 100), 0)
            end
        end
        do
            return math.max(addNddValueResult, 0)
        end
    end
end

function static_value_ndd(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local minNddValue = 0
    local maxNddValue = 0
    local minNddValueResult = 0
    local maxNddValueResult = 0
    local preNddValue = 0
    if optionLevelFactor == 1 then
        preNddValue = 9
    else
        local baseNddValue = 9
        for x = 2, optionLevelFactor do
            if x > 49 then
                armorLevelBaseCurrentLevelPlus = baseNddValue * UpgradeFactor
            else
                armorLevelBaseCurrentLevelPlus = (math.max)(1 + x / 10 * 4, 0)
            end
            baseNddValue = baseNddValue + armorLevelBaseCurrentLevelPlus
            preNddValue = baseNddValue
        end
    end
    do
        local addNddValue = 0
        addNddValue = static_value_addndd(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
        if itemSlot == Belt then
            maxNddValue = (math.max)(round((preNddValue) * ArmorConstantSlotCoefficient[itemSlot] * ArmorConstantJobCoefficient[GlobalJob] * StaticArmorRarityCoefficient[rarity], 0), 4)
        else
            if itemSlot == Earring or itemSlot == Cape or itemSlot == Necklace or itemSlot == Ring then
                maxNddValue = (math.max)(round((preNddValue) * ArmorConstantSlotCoefficient[itemSlot] * StaticAccRarityCoefficient[rarity], 0), 0)
            else
                maxNddValue = (math.max)(round((preNddValue) * ArmorConstantSlotCoefficient[itemSlot] * ArmorConstantJobCoefficient[itemJob] * StaticArmorRarityCoefficient[rarity], 0), 4)
            end
        end
        if maxNddValue < 466 then
            minNddValue = round(maxNddValue * (math.max)(0.0598 * (math.log)(maxNddValue) + 0.432, 0.5), 0)
        else
            minNddValue = (math.max)(round(maxNddValue * 0.8, 0), 1)
        end
        if currentStatValue == 0 then
            minNddValueResult = minNddValue + addNddValue
            maxNddValueResult = maxNddValue + addNddValue
        else
            minNddValueResult = round(maxNddValue * (currentStatValue / 100), 0) + addNddValue
            maxNddValueResult = round(maxNddValue * (currentStatValue / 100), 0) + addNddValue
        end
        return minNddValueResult, maxNddValueResult
    end
end

function static_value_mar(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local minMarValue = 0
    local maxVarValue = 0
    local staticMinMarValue = 0
    local staticMaxMarValue = 0

    if itemSlot == SpellBook then
        minMarValue = round(((((1e-005 * (optionLevelFactor ^ 3)) - (0.003 * optionLevelFactor ^ 2)) + (0.367 * optionLevelFactor)) + 4.8841), 0)
    else
        minMarValue = round((((((1e-005 * (optionLevelFactor ^ 3)) - (0.003 * (optionLevelFactor ^ 2))) + (0.367 * optionLevelFactor)) + 4.8841) * StaticRarityCoefficient[rarity]), 0)
    end
    if minMarValue < 5 then
        maxVarValue = minMarValue + 3
    elseif minMarValue == 5 then
        maxVarValue = minMarValue + 4
    elseif minMarValue > 5 then
        maxVarValue = minMarValue + 5
    else
        maxVarValue = minMarValue
    end
    if currentStatValue == 0 then
        staticMinMarValue = minMarValue
        staticMaxMarValue = maxVarValue
    else
        staticMinMarValue = round((maxVarValue * (currentStatValue / 100)), 0)
        staticMaxMarValue = round((maxVarValue * (currentStatValue / 100)), 0)
    end
    return staticMinMarValue, staticMaxMarValue
end

function static_value_par(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local minParValue = 0
    local maxParValue = 0
    local staticMinParValue = 0
    local staticMaxParValue = 0

    minParValue = round(((((1e-005 * (optionLevelFactor ^ 3)) - (0.003 * (optionLevelFactor ^ 2))) + (0.367 * optionLevelFactor)) + 4.8841), 0)
    if minParValue < 5 then
        maxParValue = minParValue + 3
    elseif minParValue == 5 then
        maxParValue = minParValue + 4
    elseif minParValue > 5 then
        maxParValue = minParValue + 5
    else
        maxParValue = minParValue
    end
    if currentStatValue == 0 then
        staticMinParValue = minParValue
        staticMaxParValue = maxParValue
    else
        staticMinParValue = round((maxParValue * (currentStatValue / 100)), 0)
        staticMaxParValue = round((maxParValue * (currentStatValue / 100)), 0)
    end
    return staticMinParValue, staticMaxParValue
end

function static_value_map(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local minMapValue = 0
    local maxMapValue = 0
    local staticMinMapValue = 0
    local staticMaxMapValue = 0

    if itemSlot > 29 and itemSlot < 50 then
        maxMapValue = round((((-3.8e-006 * (optionLevelFactor ^ 3)) + (0.0009 * (optionLevelFactor ^ 2))) + (0.0294 * optionLevelFactor)), 0) + 3
    elseif itemSlot > 49 and itemSlot < 60 then
        maxMapValue = round(((((-3.8e-006 * (optionLevelFactor ^ 3)) + (0.0009 * (optionLevelFactor ^ 2))) + (0.0294 * optionLevelFactor)) * 1.8), 0) + 3
    else
        minMapValue = 0
        maxMapValue = 0
    end
    if currentStatValue == 0 then
        staticMinMapValue = minMapValue
        staticMaxMapValue = maxMapValue
    else
        staticMinMapValue = round((maxMapValue * (currentStatValue / 100)), 0)
        staticMaxMapValue = round((maxMapValue * (currentStatValue / 100)), 0)
    end
    return staticMinMapValue, staticMaxMapValue
end

function static_value_pap(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local minPapValue = 0
    local maxPapValue = 0
    local staticMinPapValue = 0
    local staticMaxPapValue = 0
    local l_17_10
    local l_17_11
    local l_17_12
    if itemSlot > 29 and itemSlot < 50 then
        minPapValue = round((((-3.8e-006 * (optionLevelFactor ^ 3)) + (0.0009 * (optionLevelFactor ^ 2))) + (0.0294 * optionLevelFactor)), 0)
        maxPapValue = minPapValue + 3
    elseif itemSlot > 49 and itemSlot < 60 then
        minPapValue = round((((((-3.8e-006 * (optionLevelFactor ^ 3)) + (0.0009 * (optionLevelFactor ^ 2))) + (0.0294 * optionLevelFactor))) * 1.8), 0)
        maxPapValue = minPapValue + 3
    else
        minPapValue = 0
        maxPapValue = 0
    end
    if currentStatValue == 0 then
        staticMinPapValue = minPapValue
        staticMaxPapValue = maxPapValue
    else
        staticMinPapValue = round((maxPapValue * (currentStatValue / 100)), 0)
        staticMaxPapValue = round((maxPapValue * (currentStatValue / 100)), 0)
    end
    return staticMinPapValue, staticMaxPapValue
end

function static_rate_abp(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local minAbpRate = 0
    local maxAbpRate = 0
    local staticMinAbpRate = 0
    local staticMaxAbpRate = 0

    minAbpRate = round(((0.0016 * optionLevelFactor) + 0.0624), 3)
    maxAbpRate = minAbpRate + 0.013
    if currentStatValue == 0 then
        staticMinAbpRate = minAbpRate
        staticMaxAbpRate = maxAbpRate
    else
        staticMinAbpRate = round(((maxAbpRate) * (currentStatValue / 100)), 3)
        staticMaxAbpRate = round(((maxAbpRate) * (currentStatValue / 100)), 3)
    end
    return staticMinAbpRate, staticMaxAbpRate
end

function static_value_addwap(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local itemRarityFactor
    local rarityFactor
    local baseAddWapValue = 0
    local addWapValue = 0
    local maxAddWapValue = 0
    local minAddWapValue = 0
    local addWapValueResult
    local WapUpgradeFactor
    itemRarityFactor = 0
    rarityFactor = 0
    if itemLevel < 60 then
        itemRarityFactor = WeaponRarityAddwap50NA[rarity]
        rarityFactor = WeaponRarityAddwap50NA[4]
    elseif itemLevel < 70 then
        itemRarityFactor = WeaponRarityAddwap60NA[rarity]
        rarityFactor = WeaponRarityAddwap60NA[4]
    elseif itemLevel < 80 then
        itemRarityFactor = WeaponRarityAddwap70NA[rarity]
        rarityFactor = WeaponRarityAddwap70NA[4]
    elseif itemLevel < 90 then
        itemRarityFactor = WeaponRarityAddwap80NA[rarity]
        rarityFactor = WeaponRarityAddwap80NA[4]
    else
        itemRarityFactor = WeaponRarityAddwap90NA[rarity]
        rarityFactor = WeaponRarityAddwap90NA[4]
    end
    if optionLevelFactor == 1 then
        baseAddWapValue = 5
    else
        addWapValueResult = 5
        WapUpgradeFactor = 0
        for x = 2, optionLevelFactor do
            if x > 49 then
                WapUpgradeFactor = addWapValueResult * UpgradeFactor
            else
                WapUpgradeFactor = math.max((((x / 30) * 20) - 0.8), 0)
            end
            addWapValueResult = addWapValueResult + WapUpgradeFactor
            baseAddWapValue = addWapValueResult
        end
    end
    if itemSlot == Blade then
        addWapValue = round((((baseAddWapValue) * WeaponSlotCoefficient[Staff]) / WeaponAttackSpeedCoefficient[Staff]), 1)
    else
        addWapValue = round((((baseAddWapValue) * WeaponSlotCoefficient[itemSlot]) / WeaponAttackSpeedCoefficient[itemSlot]), 1)
    end
    maxAddWapValue = math.max(((addWapValue * StaticWapmaxCoefficient[rarity]) * (1 + WeaponSlotDeviation[itemSlot][1])), 2)
    minAddWapValue = math.max(((addWapValue * StaticWapmaxCoefficient[4]) * (1 + WeaponSlotDeviation[itemSlot][1])), 2)
    addWapValueResult = 0
    if itemLevel > 49 and rarity > 3 then
        addWapValueResult = round(maxAddWapValue + minAddWapValue * (rarity - 4), 0)
    end
    do
        return math.max(addWapValueResult, 0)
    end
end

function static_value_wapmax(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel)
    local baseWapValue = 0
    local wapValueResult = 0
    local addWapValue = 0
    local staticWapMinValue = 0
    local staticWapMaxValue = 0
    local finalWapMinValue = 0
    local finalWapMaxValue = 0
    if optionLevelFactor == 1 then
        baseWapValue = 5
    else
        local addWapValueResult = 5
        local wapUpgradeFactor = 0
        for x = 2, optionLevelFactor do
            if x > 49 then
                wapUpgradeFactor = addWapValueResult * UpgradeFactor
            else
                wapUpgradeFactor = (math.max)(x / 30 * 20 - 0.8, 0)
            end
            addWapValueResult = addWapValueResult + wapUpgradeFactor
            baseWapValue = addWapValueResult
        end
    end
    do
        addWapValue = static_value_addwap(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, itemLevel, l_15_7)
        if itemSlot == Blade then
            wapValueResult = round((baseWapValue) * WeaponSlotCoefficient[Staff] / WeaponAttackSpeedCoefficient[Staff], 1)
            staticWapMaxValue = (math.max)(wapValueResult * StaticWapmaxCoefficient[rarity] * (1 + (WeaponSlotDeviation[itemSlot])[1]), 2) + addWapValue
        else
            wapValueResult = round((baseWapValue) * WeaponSlotCoefficient[itemSlot] / WeaponAttackSpeedCoefficient[itemSlot], 1)
            staticWapMaxValue = (math.max)(wapValueResult * StaticWapmaxCoefficient[rarity] * (1 + (WeaponSlotDeviation[itemSlot])[1]), 2) + addWapValue
        end
        staticWapMinValue = (math.max)(staticWapMaxValue * 0.78, 1)
        if currentStatValue == 0 then
            finalWapMinValue = staticWapMinValue
            finalWapMaxValue = staticWapMaxValue
        else
            finalWapMinValue = round(staticWapMaxValue * (currentStatValue / 100), 0)
            finalWapMaxValue = round(staticWapMaxValue * (currentStatValue / 100), 0)
        end
        return finalWapMinValue, finalWapMaxValue
    end
end