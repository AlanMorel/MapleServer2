FloatEpsilon = 1e-006
g_locale = ""
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

local round = function(firstValue, secondValue)
    if secondValue == nil then
        secondValue = 0
    end
    return math.floor(firstValue * (10 ^ secondValue) + 0.5 + FloatEpsilon) / (10 ^ secondValue)
end

local clip_value = function(clipMinValue, floorValue, clipMaxValue)
    if clipMinValue < floorValue then
        clipMinValue = floorValue
    end
    if clipMaxValue < clipMinValue then
        clipMinValue = clipMaxValue
    end
    return clipMinValue
end

-- itemLevelRarityCoefficient
itemLevelRarityCoefficient = {}
itemLevelRarityCoefficient[Bludgeon] = 1
itemLevelRarityCoefficient[Dagger] = 1
itemLevelRarityCoefficient[Longsword] = 1
itemLevelRarityCoefficient[Scepter] = 1
itemLevelRarityCoefficient[ThrowingStar] = 1
itemLevelRarityCoefficient[SpellBook] = 1
itemLevelRarityCoefficient[Shield] = 1
itemLevelRarityCoefficient[Greatsword] = 1
itemLevelRarityCoefficient[Bow] = 1
itemLevelRarityCoefficient[Staff] = 1
itemLevelRarityCoefficient[Cannon] = 1
itemLevelRarityCoefficient[Blade] = 1
itemLevelRarityCoefficient[Knuckle] = 1
itemLevelRarityCoefficient[Orb] = 1
itemLevelRarityCoefficient[Hat] = 1
itemLevelRarityCoefficient[Clothes] = 1
itemLevelRarityCoefficient[Pants] = 1
itemLevelRarityCoefficient[Gloves] = 1
itemLevelRarityCoefficient[Shoes] = 1
itemLevelRarityCoefficient[Overall] = 1
itemLevelRarityCoefficient[Earring] = 0
itemLevelRarityCoefficient[Cape] = 0
itemLevelRarityCoefficient[Necklace] = 0
itemLevelRarityCoefficient[Ring] = 0
itemLevelRarityCoefficient[Belt] = 1

--itemLevelCoefficient
itemLevelCoefficient = {}
itemLevelCoefficient[Bludgeon] = 1
itemLevelCoefficient[Dagger] = 1
itemLevelCoefficient[Longsword] = 1
itemLevelCoefficient[Scepter] = 1
itemLevelCoefficient[ThrowingStar] = 1
itemLevelCoefficient[SpellBook] = 1
itemLevelCoefficient[Shield] = 1
itemLevelCoefficient[Greatsword] = 1
itemLevelCoefficient[Bow] = 1
itemLevelCoefficient[Staff] = 1
itemLevelCoefficient[Cannon] = 1
itemLevelCoefficient[Blade] = 1
itemLevelCoefficient[Knuckle] = 1
itemLevelCoefficient[Orb] = 1
itemLevelCoefficient[Hat] = 0.21
itemLevelCoefficient[Clothes] = 0.32
itemLevelCoefficient[Pants] = 0.3
itemLevelCoefficient[Gloves] = 0.06
itemLevelCoefficient[Shoes] = 0.06
itemLevelCoefficient[Overall] = 0.62
itemLevelCoefficient[Earring] = 0.2
itemLevelCoefficient[Cape] = 0.2
itemLevelCoefficient[Necklace] = 0.2
itemLevelCoefficient[Ring] = 0.2
itemLevelCoefficient[Belt] = 0.2

-- limitBreakItemLevelCoefficient
limitBreakItemLevelCoefficient = {}
limitBreakItemLevelCoefficient[0] = 0
limitBreakItemLevelCoefficient[1] = 0.02
limitBreakItemLevelCoefficient[2] = 0.04
limitBreakItemLevelCoefficient[3] = 0.06
limitBreakItemLevelCoefficient[4] = 0.08
limitBreakItemLevelCoefficient[5] = 0.1
limitBreakItemLevelCoefficient[6] = 0.14
limitBreakItemLevelCoefficient[7] = 0.18
limitBreakItemLevelCoefficient[8] = 0.23
limitBreakItemLevelCoefficient[9] = 0.29
limitBreakItemLevelCoefficient[10] = 0.44
limitBreakItemLevelCoefficient[11] = 0.74
limitBreakItemLevelCoefficient[12] = 1.05
limitBreakItemLevelCoefficient[13] = 1.36
limitBreakItemLevelCoefficient[14] = 1.68
limitBreakItemLevelCoefficient[15] = 2

-- limitBreakItemLevelCoefficientNA
limitBreakItemLevelCoefficientNA = {}
limitBreakItemLevelCoefficientNA[0] = 0
limitBreakItemLevelCoefficientNA[1] = 0.02
limitBreakItemLevelCoefficientNA[2] = 0.04
limitBreakItemLevelCoefficientNA[3] = 0.07
limitBreakItemLevelCoefficientNA[4] = 0.1
limitBreakItemLevelCoefficientNA[5] = 0.14
limitBreakItemLevelCoefficientNA[6] = 0.19
limitBreakItemLevelCoefficientNA[7] = 0.25
limitBreakItemLevelCoefficientNA[8] = 0.32
limitBreakItemLevelCoefficientNA[9] = 0.4
limitBreakItemLevelCoefficientNA[10] = 0.5
limitBreakItemLevelCoefficientNA[11] = 0.64
limitBreakItemLevelCoefficientNA[12] = 0.84
limitBreakItemLevelCoefficientNA[13] = 1.12
limitBreakItemLevelCoefficientNA[14] = 1.5
limitBreakItemLevelCoefficientNA[15] = 2

-- levelScoreFactorNA
levelScoreFactorNA = {}
for i = 0, 99 do
    levelScoreFactorNA[i] = 0
end
levelScoreFactorNA[57] = 2.899
levelScoreFactorNA[60] = 3.4442
levelScoreFactorNA[67] = 12.538
levelScoreFactorNA[70] = 14.15
levelScoreFactorNA[80] = 45.91
levelScoreFactorNA[90] = 140.13

-- rarityScoreFactor
rarityScoreFactor = {}
rarityScoreFactor[NoRarity] = 1
rarityScoreFactor[CommonRarity] = 1
rarityScoreFactor[RareRarity] = 1
rarityScoreFactor[ExceptionalRarity] = 1
rarityScoreFactor[EpicRarity] = 0.558
rarityScoreFactor[LegendaryRarity] = 1.2
rarityScoreFactor[AscendantRarity] = 1.9

-- armorConstantSlotCoefficient
armorConstantSlotCoefficient = {}
armorConstantSlotCoefficient[Overall] = 0.62
armorConstantSlotCoefficient[Clothes] = 0.32
armorConstantSlotCoefficient[Pants] = 0.3
armorConstantSlotCoefficient[Hat] = 0.21
armorConstantSlotCoefficient[Gloves] = 0.06
armorConstantSlotCoefficient[Shoes] = 0.06
armorConstantSlotCoefficient[Earring] = 0.05
armorConstantSlotCoefficient[Cape] = 0.05
armorConstantSlotCoefficient[Necklace] = 0.05
armorConstantSlotCoefficient[Ring] = 0.05
armorConstantSlotCoefficient[Belt] = 0.05
armorConstantSlotCoefficient[Shield] = 0.15

-- armorConstantJobCoefficient
armorConstantJobCoefficient = {}
armorConstantJobCoefficient[GlobalJob] = 0.8
armorConstantJobCoefficient[Beginner] = 0.9
armorConstantJobCoefficient[Knight] = 1.1
armorConstantJobCoefficient[Berserker] = 1
armorConstantJobCoefficient[Wizard] = 0.9
armorConstantJobCoefficient[Priest] = 0.88
armorConstantJobCoefficient[Archer] = 0.93
armorConstantJobCoefficient[HeavyGunner] = 0.95
armorConstantJobCoefficient[Thief] = 0.95
armorConstantJobCoefficient[Assassin] = 0.9
armorConstantJobCoefficient[RuneBlader] = 0.97
armorConstantJobCoefficient[Striker] = 1
armorConstantJobCoefficient[SoulBinder] = 0.9

-- armorConstantGradeCoefficient
armorConstantGradeCoefficient = {}
armorConstantGradeCoefficient[1] = {}
armorConstantGradeCoefficient[1][CommonRarity] = 0.9
armorConstantGradeCoefficient[1][RareRarity] = 0.98
armorConstantGradeCoefficient[1][ExceptionalRarity] = 1.06
armorConstantGradeCoefficient[1][EpicRarity] = 1.14
armorConstantGradeCoefficient[1][LegendaryRarity] = 1.21
armorConstantGradeCoefficient[1][AscendantRarity] = 1.4
armorConstantGradeCoefficient[2] = {}
armorConstantGradeCoefficient[2][CommonRarity] = 1
armorConstantGradeCoefficient[2][RareRarity] = 1.1
armorConstantGradeCoefficient[2][ExceptionalRarity] = 1.2
armorConstantGradeCoefficient[2][EpicRarity] = 1.3
armorConstantGradeCoefficient[2][LegendaryRarity] = 1.45
armorConstantGradeCoefficient[2][AscendantRarity] = 1.6

-- weaponGradeAddndd
weaponGradeAddndd = {}
weaponGradeAddndd[CommonRarity] = 0
weaponGradeAddndd[RareRarity] = 0
weaponGradeAddndd[ExceptionalRarity] = 0
weaponGradeAddndd[EpicRarity] = 0.9846
weaponGradeAddndd[LegendaryRarity] = 2.061
weaponGradeAddndd[AscendantRarity] = 2.061

-- weaponGradeAddndd50NA
weaponGradeAddndd50NA = {}
weaponGradeAddndd50NA[CommonRarity] = 0
weaponGradeAddndd50NA[RareRarity] = 0
weaponGradeAddndd50NA[EpicRarity] = 0
weaponGradeAddndd50NA[EpicRarity] = 0.9846
weaponGradeAddndd50NA[LegendaryRarity] = 1.426
weaponGradeAddndd50NA[AscendantRarity] = 1.795

-- weaponGradeAddndd60
weaponGradeAddndd60 = {}
weaponGradeAddndd60[CommonRarity] = 0
weaponGradeAddndd60[RareRarity] = 0
weaponGradeAddndd60[ExceptionalRarity] = 0
weaponGradeAddndd60[EpicRarity] = 1.531
weaponGradeAddndd60[LegendaryRarity] = 1.833
weaponGradeAddndd60[AscendantRarity] = 2.132

-- weaponGradeAddndd60KR
weaponGradeAddndd60KR = {}
weaponGradeAddndd60KR[CommonRarity] = 0
weaponGradeAddndd60KR[RareRarity] = 0
weaponGradeAddndd60KR[ExceptionalRarity] = 0
weaponGradeAddndd60KR[EpicRarity] = 0.6486
weaponGradeAddndd60KR[LegendaryRarity] = 1.123
weaponGradeAddndd60KR[AscendantRarity] = 1.5408

weaponGradeAddndd60NA = {}
weaponGradeAddndd60NA[CommonRarity] = 0
weaponGradeAddndd60NA[RareRarity] = 0
weaponGradeAddndd60NA[ExceptionalRarity] = 0
weaponGradeAddndd60NA[EpicRarity] = 2.016
weaponGradeAddndd60NA[LegendaryRarity] = 2.22
weaponGradeAddndd60NA[AscendantRarity] = 2.442

weaponGradeAddndd70 = {}
weaponGradeAddndd70[CommonRarity] = 0
weaponGradeAddndd70[RareRarity] = 0
weaponGradeAddndd70[ExceptionalRarity] = 0
weaponGradeAddndd70[EpicRarity] = 3.248
weaponGradeAddndd70[LegendaryRarity] = 3.105
weaponGradeAddndd70[AscendantRarity] = 3.127

weaponGradeAddndd70KR = {}
weaponGradeAddndd70KR[CommonRarity] = 0
weaponGradeAddndd70KR[RareRarity] = 0
weaponGradeAddndd70KR[ExceptionalRarity] = 0
weaponGradeAddndd70KR[EpicRarity] = 1.2445
weaponGradeAddndd70KR[LegendaryRarity] = 1.6125
weaponGradeAddndd70KR[AscendantRarity] = 1.947

weaponGradeAddndd70NA = {}
weaponGradeAddndd70NA[CommonRarity] = 0
weaponGradeAddndd70NA[RareRarity] = 0
weaponGradeAddndd70NA[ExceptionalRarity] = 0
weaponGradeAddndd70NA[EpicRarity] = 3.174
weaponGradeAddndd70NA[LegendaryRarity] = 3.084
weaponGradeAddndd70NA[AscendantRarity] = 3.129

weaponGradeAddndd80 = {}
weaponGradeAddndd80[CommonRarity] = 0
weaponGradeAddndd80[RareRarity] = 0
weaponGradeAddndd80[ExceptionalRarity] = 0
weaponGradeAddndd80[EpicRarity] = 4.841
weaponGradeAddndd80[LegendaryRarity] = 4.344
weaponGradeAddndd80[AscendantRarity] = 4.13

weaponGradeAddndd80KR = {}
weaponGradeAddndd80KR[CommonRarity] = 0
weaponGradeAddndd80KR[RareRarity] = 0
weaponGradeAddndd80KR[ExceptionalRarity] = 0
weaponGradeAddndd80KR[EpicRarity] = 2.085
weaponGradeAddndd80KR[LegendaryRarity] = 2.2669
weaponGradeAddndd80KR[AscendantRarity] = 2.4771

weaponGradeAddndd80NA = {}
weaponGradeAddndd80NA[CommonRarity] = 0
weaponGradeAddndd80NA[RareRarity] = 0
weaponGradeAddndd80NA[ExceptionalRarity] = 0
weaponGradeAddndd80NA[EpicRarity] = 4.7672
weaponGradeAddndd80NA[LegendaryRarity] = 4.2895
weaponGradeAddndd80NA[AscendantRarity] = 4.0858

-- weaponGradeAddndd90
weaponGradeAddndd90 = {}
weaponGradeAddndd90[CommonRarity] = 0
weaponGradeAddndd90[RareRarity] = 0
weaponGradeAddndd90[ExceptionalRarity] = 0
weaponGradeAddndd90[EpicRarity] = 7.0691
weaponGradeAddndd90[LegendaryRarity] = 6.0235
weaponGradeAddndd90[AscendantRarity] = 5.4645

-- weaponGradeAddndd90KR
weaponGradeAddndd90KR = {}
weaponGradeAddndd90KR[CommonRarity] = 0
weaponGradeAddndd90KR[RareRarity] = 0
weaponGradeAddndd90KR[ExceptionalRarity] = 0
weaponGradeAddndd90KR[EpicRarity] = 3.262
weaponGradeAddndd90KR[LegendaryRarity] = 3.1535
weaponGradeAddndd90KR[AscendantRarity] = 3.182

-- weaponGradeAddndd90NA
weaponGradeAddndd90NA = {}
weaponGradeAddndd90NA[CommonRarity] = 0
weaponGradeAddndd90NA[RareRarity] = 0
weaponGradeAddndd90NA[ExceptionalRarity] = 0
weaponGradeAddndd90NA[EpicRarity] = 6.9675
weaponGradeAddndd90NA[LegendaryRarity] = 5.947
weaponGradeAddndd90NA[AscendantRarity] = 5.4043

weaponGradeAddwap = {}
weaponGradeAddwap[CommonRarity] = 0
weaponGradeAddwap[RareRarity] = 0
weaponGradeAddwap[ExceptionalRarity] = 0
weaponGradeAddwap[EpicRarity] = 0.9828
weaponGradeAddwap[LegendaryRarity] = 2.0589
weaponGradeAddwap[AscendantRarity] = 2.0589

-- weaponGradeAddWap
weaponGradeAddwap50NA = {}
weaponGradeAddwap50NA[CommonRarity] = 0
weaponGradeAddwap50NA[RareRarity] = 0
weaponGradeAddwap50NA[ExceptionalRarity] = 0
weaponGradeAddwap50NA[EpicRarity] = 0.9828
weaponGradeAddwap50NA[LegendaryRarity] = 1.4215
weaponGradeAddwap50NA[AscendantRarity] = 1.7975

weaponGradeAddwap60 = {}
weaponGradeAddwap60[CommonRarity] = 0
weaponGradeAddwap60[RareRarity] = 0
weaponGradeAddwap60[ExceptionalRarity] = 0
weaponGradeAddwap60[EpicRarity] = 1.531
weaponGradeAddwap60[LegendaryRarity] = 1.8285
weaponGradeAddwap60[AscendantRarity] = 2.118

weaponGradeAddwap60KR = {}
weaponGradeAddwap60KR[CommonRarity] = 0
weaponGradeAddwap60KR[RareRarity] = 0
weaponGradeAddwap60KR[ExceptionalRarity] = 0
weaponGradeAddwap60KR[EpicRarity] = 0.6486
weaponGradeAddwap60KR[LegendaryRarity] = 1.123
weaponGradeAddwap60KR[AscendantRarity] = 1.5408

weaponGradeAddwap60NA = {}
weaponGradeAddwap60NA[CommonRarity] = 0
weaponGradeAddwap60NA[RareRarity] = 0
weaponGradeAddwap60NA[ExceptionalRarity] = 0
weaponGradeAddwap60NA[EpicRarity] = 2.0159
weaponGradeAddwap60NA[LegendaryRarity] = 2.2145
weaponGradeAddwap60NA[AscendantRarity] = 2.4356

weaponGradeAddwap70 = {}
weaponGradeAddwap70[CommonRarity] = 0
weaponGradeAddwap70[RareRarity] = 0
weaponGradeAddwap70[ExceptionalRarity] = 0
weaponGradeAddwap70[EpicRarity] = 2.4979
weaponGradeAddwap70[LegendaryRarity] = 2.5491
weaponGradeAddwap70[AscendantRarity] = 2.6866

weaponGradeAddwap70KR = {}
weaponGradeAddwap70KR[CommonRarity] = 0
weaponGradeAddwap70KR[RareRarity] = 0
weaponGradeAddwap70KR[ExceptionalRarity] = 0
weaponGradeAddwap70KR[EpicRarity] = 1.2422
weaponGradeAddwap70KR[LegendaryRarity] = 1.6133
weaponGradeAddwap70KR[AscendantRarity] = 1.9477

weaponGradeAddwap70NA = {}
weaponGradeAddwap70NA[CommonRarity] = 0
weaponGradeAddwap70NA[RareRarity] = 0
weaponGradeAddwap70NA[ExceptionalRarity] = 0
weaponGradeAddwap70NA[EpicRarity] = 3.1675
weaponGradeAddwap70NA[LegendaryRarity] = 3.0833
weaponGradeAddwap70NA[AscendantRarity] = 3.1261

weaponGradeAddwap80 = {}
weaponGradeAddwap80[CommonRarity] = 0
weaponGradeAddwap80[RareRarity] = 0
weaponGradeAddwap80[ExceptionalRarity] = 0
weaponGradeAddwap80[EpicRarity] = 3.8106
weaponGradeAddwap80[LegendaryRarity] = 3.5679
weaponGradeAddwap80[AscendantRarity] = 3.5112

weaponGradeAddwap80KR = {}
weaponGradeAddwap80KR[CommonRarity] = 0
weaponGradeAddwap80KR[RareRarity] = 0
weaponGradeAddwap80KR[ExceptionalRarity] = 0
weaponGradeAddwap80KR[EpicRarity] = 2.0836
weaponGradeAddwap80KR[LegendaryRarity] = 2.2663
weaponGradeAddwap80KR[AscendantRarity] = 2.4763

weaponGradeAddwap80NA = {}
weaponGradeAddwap80NA[CommonRarity] = 0
weaponGradeAddwap80NA[RareRarity] = 0
weaponGradeAddwap80NA[ExceptionalRarity] = 0
weaponGradeAddwap80NA[EpicRarity] = 4.76
weaponGradeAddwap80NA[LegendaryRarity] = 4.2835
weaponGradeAddwap80NA[AscendantRarity] = 4.0804

weaponGradeAddwap90 = {}
weaponGradeAddwap90[CommonRarity] = 0
weaponGradeAddwap90[RareRarity] = 0
weaponGradeAddwap90[ExceptionalRarity] = 0
weaponGradeAddwap90[EpicRarity] = 5.6481
weaponGradeAddwap90[LegendaryRarity] = 4.9527
weaponGradeAddwap90[AscendantRarity] = 4.6126

weaponGradeAddwap90KR = {}
weaponGradeAddwap90KR[CommonRarity] = 0
weaponGradeAddwap90KR[RareRarity] = 0
weaponGradeAddwap90KR[ExceptionalRarity] = 0
weaponGradeAddwap90KR[EpicRarity] = 3.2613
weaponGradeAddwap90KR[LegendaryRarity] = 3.1538
weaponGradeAddwap90KR[AscendantRarity] = 3.1822

weaponGradeAddwap90NA = {}
weaponGradeAddwap90NA[CommonRarity] = 0
weaponGradeAddwap90NA[RareRarity] = 0
weaponGradeAddwap90NA[ExceptionalRarity] = 0
weaponGradeAddwap90NA[EpicRarity] = 6.9602
weaponGradeAddwap90NA[LegendaryRarity] = 5.9416
weaponGradeAddwap90NA[AscendantRarity] = 5.399

--
accConstantGradeCoefficient = {}
accConstantGradeCoefficient[CommonRarity] = 1
accConstantGradeCoefficient[RareRarity] = 1.1
accConstantGradeCoefficient[ExceptionalRarity] = 1.2
accConstantGradeCoefficient[EpicRarity] = 1.3
accConstantGradeCoefficient[LegendaryRarity] = 1.4
accConstantGradeCoefficient[AscendantRarity] = 1.4

ShieldConstantGradeCoefficient = {}
ShieldConstantGradeCoefficient[CommonRarity] = 0.1
ShieldConstantGradeCoefficient[RareRarity] = 0.2
ShieldConstantGradeCoefficient[ExceptionalRarity] = 0.3
ShieldConstantGradeCoefficient[EpicRarity] = 0.4
ShieldConstantGradeCoefficient[LegendaryRarity] = 0.5
ShieldConstantGradeCoefficient[AscendantRarity] = 0.5

--
weaponGradeHPCorrection = {}
weaponGradeHPCorrection[1] = {}
weaponGradeHPCorrection[1][CommonRarity] = 0.3
weaponGradeHPCorrection[1][RareRarity] = 0.4
weaponGradeHPCorrection[1][ExceptionalRarity] = 0.5
weaponGradeHPCorrection[1][EpicRarity] = 0.6
weaponGradeHPCorrection[1][LegendaryRarity] = 0.7
weaponGradeHPCorrection[1][AscendantRarity] = 0.8

weaponHPSlotCorrection = {}
weaponHPSlotCorrection[Bludgeon] = 0.1
weaponHPSlotCorrection[Dagger] = 0.3
weaponHPSlotCorrection[Longsword] = 0.04
weaponHPSlotCorrection[Scepter] = 0.1
weaponHPSlotCorrection[ThrowingStar] = 0.3
weaponHPSlotCorrection[Greatsword] = 0.2
weaponHPSlotCorrection[Bow] = 0.1
weaponHPSlotCorrection[Staff] = 0.1
weaponHPSlotCorrection[Cannon] = 0.2
weaponHPSlotCorrection[Blade] = 0.1
weaponHPSlotCorrection[Knuckle] = 0.1
weaponHPSlotCorrection[Orb] = 0.2

weaponSlotCoefficient = {}
weaponSlotCoefficient[Bludgeon] = 1
weaponSlotCoefficient[Dagger] = 1.03
weaponSlotCoefficient[Longsword] = 0.95
weaponSlotCoefficient[Scepter] = 0.92
weaponSlotCoefficient[ThrowingStar] = 0.95
weaponSlotCoefficient[Greatsword] = 1.1
weaponSlotCoefficient[Bow] = 1.03
weaponSlotCoefficient[Staff] = 1.2
weaponSlotCoefficient[Cannon] = 1.05
weaponSlotCoefficient[Blade] = 1.045
weaponSlotCoefficient[Knuckle] = 1.13
weaponSlotCoefficient[Orb] = 0.96

weaponAttackSpeedCoefficient = {}
weaponAttackSpeedCoefficient[Bludgeon] = 0.95
weaponAttackSpeedCoefficient[Dagger] = 1.05
weaponAttackSpeedCoefficient[Longsword] = 1
weaponAttackSpeedCoefficient[Scepter] = 1.1
weaponAttackSpeedCoefficient[ThrowingStar] = 1
weaponAttackSpeedCoefficient[Greatsword] = 0.95
weaponAttackSpeedCoefficient[Bow] = 1.05
weaponAttackSpeedCoefficient[Staff] = 1
weaponAttackSpeedCoefficient[Cannon] = 0.9
weaponAttackSpeedCoefficient[Blade] = 0.975
weaponAttackSpeedCoefficient[Knuckle] = 1.05
weaponAttackSpeedCoefficient[Orb] = 0.95

weaponGradeCoefficient = {}
weaponGradeCoefficient[1] = {}
weaponGradeCoefficient[1][CommonRarity] = 0.9
weaponGradeCoefficient[1][RareRarity] = 0.98
weaponGradeCoefficient[1][ExceptionalRarity] = 1.06
weaponGradeCoefficient[1][EpicRarity] = 1.14
weaponGradeCoefficient[1][LegendaryRarity] = 1.21
weaponGradeCoefficient[1][AscendantRarity] = 1.4
weaponGradeCoefficient[2] = {}
weaponGradeCoefficient[2][CommonRarity] = 0.9
weaponGradeCoefficient[2][RareRarity] = 0.98
weaponGradeCoefficient[2][ExceptionalRarity] = 1.06
weaponGradeCoefficient[2][EpicRarity] = 1.14
weaponGradeCoefficient[2][LegendaryRarity] = 1.21
weaponGradeCoefficient[2][AscendantRarity] = 1.4

weaponSlotDeviation = {}
weaponSlotDeviation[Bludgeon] = {}
weaponSlotDeviation[Bludgeon][1] = 0.05
weaponSlotDeviation[Bludgeon][2] = 0.1
weaponSlotDeviation[Dagger] = {}
weaponSlotDeviation[Dagger][1] = 0.15
weaponSlotDeviation[Dagger][2] = 0.3
weaponSlotDeviation[Longsword] = {}
weaponSlotDeviation[Longsword][1] = 0.02
weaponSlotDeviation[Longsword][2] = 0.04
weaponSlotDeviation[Scepter] = {}
weaponSlotDeviation[Scepter][1] = 0.05
weaponSlotDeviation[Scepter][2] = 0.1
weaponSlotDeviation[ThrowingStar] = {}
weaponSlotDeviation[ThrowingStar][1] = 0.15
weaponSlotDeviation[ThrowingStar][2] = 0.3
weaponSlotDeviation[Greatsword] = {}
weaponSlotDeviation[Greatsword][1] = 0.1
weaponSlotDeviation[Greatsword][2] = 0.2
weaponSlotDeviation[Bow] = {}
weaponSlotDeviation[Bow][1] = 0.05
weaponSlotDeviation[Bow][2] = 0.1
weaponSlotDeviation[Staff] = {}
weaponSlotDeviation[Staff][1] = 0.05
weaponSlotDeviation[Staff][2] = 0.1
weaponSlotDeviation[Cannon] = {}
weaponSlotDeviation[Cannon][1] = 0.1
weaponSlotDeviation[Cannon][2] = 0.2
weaponSlotDeviation[Blade] = {}
weaponSlotDeviation[Blade][1] = 0.1
weaponSlotDeviation[Blade][2] = 0.2
weaponSlotDeviation[Knuckle] = {}
weaponSlotDeviation[Knuckle][1] = 0.05
weaponSlotDeviation[Knuckle][2] = 0.1
weaponSlotDeviation[Orb] = {}
weaponSlotDeviation[Orb][1] = 0.1
weaponSlotDeviation[Orb][2] = 0.2

staticArmorGradeCoefficient = {}
staticArmorGradeCoefficient[CommonRarity] = 0.1
staticArmorGradeCoefficient[RareRarity] = 0.12
staticArmorGradeCoefficient[ExceptionalRarity] = 0.14
staticArmorGradeCoefficient[EpicRarity] = 0.16
staticArmorGradeCoefficient[LegendaryRarity] = 0.18
staticArmorGradeCoefficient[AscendantRarity] = 0.2

staticAccGradeCoefficient = {}
staticAccGradeCoefficient[CommonRarity] = 0.4
staticAccGradeCoefficient[RareRarity] = 0.55
staticAccGradeCoefficient[ExceptionalRarity] = 0.7
staticAccGradeCoefficient[EpicRarity] = 0.85
staticAccGradeCoefficient[LegendaryRarity] = 1
staticAccGradeCoefficient[AscendantRarity] = 1

upgradefactor = 0.06

constant_value_ndd = function(l_5_0, l_5_1, l_5_2, l_5_3, l_5_4, l_5_5, l_5_6, l_5_7)
    local l_5_8 = nil
    local l_5_9 = nil
    local l_5_10 = nil
    local l_5_11 = nil
    local l_5_12 = nil
    local l_5_13 = nil
    local l_5_14 = nil
    local l_5_15 = nil
    local l_5_16 = nil
    local l_5_17 = nil
    local l_5_18 = nil
    local l_5_19 = nil
    l_5_8 = 0
    -- DECOMPILER ERROR: Confused at declaration of local variable

    l_5_9 = 0
    -- DECOMPILER ERROR: Confused at declaration of local variable

    if l_5_4 == 1 then
        l_5_9 = 9
    else
        l_5_10 = 9
        -- DECOMPILER ERROR: Confused at declaration of local variable

        l_5_11 = 2
        l_5_12 = l_5_4
        l_5_13 = 1
        for l_5_14 = l_5_11, l_5_12, l_5_13 do
            -- DECOMPILER ERROR: Confused at declaration of local variable

            l_5_15 = g_locale
            l_5_16 = Locale_KR
            if l_5_15 == l_5_16 then
                if l_5_6 < 60 then
                    l_5_15 = math
                    l_5_15 = l_5_15.max
                    l_5_16 = l_5_14 / 10
                    l_5_16 = l_5_16 * 4
                    l_5_16 = 1 + l_5_16
                    l_5_17 = 0
                    l_5_15 = l_5_15(l_5_16, l_5_17)
                    armorLevelBaseCurrentLevelPlus = l_5_15
                elseif l_5_14 > 49 then
                    l_5_15 = upgradefactor
                    l_5_15 = l_5_10 * l_5_15
                    armorLevelBaseCurrentLevelPlus = l_5_15
                else
                    l_5_15 = math
                    l_5_15 = l_5_15.max
                    l_5_16 = l_5_14 / 10
                    l_5_16 = l_5_16 * 4
                    l_5_16 = 1 + l_5_16
                    l_5_17 = 0
                    l_5_15 = l_5_15(l_5_16, l_5_17)
                    armorLevelBaseCurrentLevelPlus = l_5_15
                end
            elseif l_5_14 > 49 then
                l_5_15 = upgradefactor
                l_5_15 = l_5_10 * l_5_15
                armorLevelBaseCurrentLevelPlus = l_5_15
            else
                l_5_15 = math
                l_5_15 = l_5_15.max
                l_5_16 = l_5_14 / 10
                l_5_16 = l_5_16 * 4
                l_5_16 = 1 + l_5_16
                l_5_17 = 0
                l_5_15 = l_5_15(l_5_16, l_5_17)
                armorLevelBaseCurrentLevelPlus = l_5_15
            end
        end
        l_5_15 = armorLevelBaseCurrentLevelPlus
        l_5_10 = l_5_10 + l_5_15
        l_5_9 = l_5_10
    end
    l_5_10 = 0
    -- DECOMPILER ERROR: Confused at declaration of local variable

    if g_locale == Locale_KR then
        if l_5_6 < 60 then
            l_5_10 = 0
        else
            l_5_10 = constant_value_addndd(l_5_0, l_5_1, l_5_2, l_5_3, l_5_4, l_5_5, l_5_6, l_5_7)
        end
    else
        l_5_10 = constant_value_addndd(l_5_0, l_5_1, l_5_2, l_5_3, l_5_4, l_5_5, l_5_6, l_5_7)
    end
    if l_5_2 == 21 then
        l_5_8 = round((l_5_9) * armorConstantSlotCoefficient[l_5_2] * armorConstantJobCoefficient[GlobalJob] *
            armorConstantGradeCoefficient[l_5_1][l_5_5], 0) + l_5_10
    elseif l_5_6 >= 70 and l_5_0 > 0 then
        l_5_8 = round((l_5_9) * armorConstantSlotCoefficient[l_5_2] * armorConstantJobCoefficient[l_5_3] *
            armorConstantGradeCoefficient[l_5_1][l_5_5] * (l_5_0 / 100), 0) + round(l_5_10 * (l_5_0 / 100), 0)
    else
        l_5_8 = round((l_5_9) * armorConstantSlotCoefficient[l_5_2] * f[l_5_3] *
            armorConstantGradeCoefficient[l_5_1][l_5_5], 0) + l_5_10
    end
    return l_5_8, l_5_8
    -- DECOMPILER ERROR: Confused about usage of registers for local variables.

end

constant_value_addndd = function(l_4_0, l_4_1, l_4_2, l_4_3, l_4_4, l_4_5, l_4_6, l_4_7)
    local l_4_8 = nil
    local l_4_9 = nil
    local l_4_10 = nil
    local l_4_11 = nil
    local l_4_12 = nil
    local l_4_13 = nil
    local l_4_14 = nil
    local l_4_15 = nil
    local l_4_16 = nil
    local l_4_17 = nil
    local l_4_18 = nil
    local l_4_19 = nil
    local l_4_20 = nil
    l_4_8 = 0
    -- DECOMPILER ERROR: Confused at declaration of local variable

    l_4_9 = 0
    -- DECOMPILER ERROR: Confused at declaration of local variable

    if l_4_6 < 60 then
        l_4_10 = g_locale
        l_4_11 = Locale_KR
        if l_4_10 ~= l_4_11 then
            l_4_10 = g_locale
            l_4_11 = Locale_CN
            if l_4_10 == l_4_11 then
            end
            l_4_10 = weaponGradeAddndd
            l_4_8 = l_4_10[l_4_5]
            l_4_10 = weaponGradeAddndd
            l_4_9 = l_4_10[4]
        else
            l_4_10 = weaponGradeAddndd50NA
            l_4_8 = l_4_10[l_4_5]
            l_4_10 = weaponGradeAddndd50NA
            l_4_9 = l_4_10[4]
        end
    elseif l_4_6 < 70 then
        l_4_10 = g_locale
        l_4_11 = Locale_KR
        if l_4_10 == l_4_11 then
            l_4_10 = weaponGradeAddndd60KR
            l_4_8 = l_4_10[l_4_5]
            l_4_10 = weaponGradeAddndd60KR
            l_4_9 = l_4_10[4]
        else
            l_4_10 = g_locale
            l_4_11 = Locale_CN
            if l_4_10 == l_4_11 then
                l_4_10 = weaponGradeAddndd60
                l_4_8 = l_4_10[l_4_5]
                l_4_10 = weaponGradeAddndd60
                l_4_9 = l_4_10[4]
            else
                l_4_10 = weaponGradeAddndd60NA
                l_4_8 = l_4_10[l_4_5]
                l_4_10 = weaponGradeAddndd60NA
                l_4_9 = l_4_10[4]
            end
        end
    elseif l_4_6 < 80 then
        l_4_10 = g_locale
        l_4_11 = Locale_KR
        if l_4_10 == l_4_11 then
            l_4_10 = weaponGradeAddndd70KR
            l_4_8 = l_4_10[l_4_5]
            l_4_10 = weaponGradeAddndd70KR
            l_4_9 = l_4_10[4]
        else
            l_4_10 = g_locale
            l_4_11 = Locale_CN
            if l_4_10 == l_4_11 then
                l_4_10 = weaponGradeAddndd70
                l_4_8 = l_4_10[l_4_5]
                l_4_10 = weaponGradeAddndd70
                l_4_9 = l_4_10[4]
            else
                l_4_10 = weaponGradeAddndd70NA
                l_4_8 = l_4_10[l_4_5]
                l_4_10 = weaponGradeAddndd70NA
                l_4_9 = l_4_10[4]
            end
        end
    elseif l_4_6 < 90 then
        l_4_10 = g_locale
        l_4_11 = Locale_KR
        if l_4_10 == l_4_11 then
            l_4_10 = weaponGradeAddndd80KR
            l_4_8 = l_4_10[l_4_5]
            l_4_10 = weaponGradeAddndd80KR
            l_4_9 = l_4_10[4]
        else
            l_4_10 = g_locale
            l_4_11 = Locale_CN
            if l_4_10 == l_4_11 then
                l_4_10 = weaponGradeAddndd80
                l_4_8 = l_4_10[l_4_5]
                l_4_10 = weaponGradeAddndd80
                l_4_9 = l_4_10[4]
            else
                l_4_10 = weaponGradeAddndd80NA
                l_4_8 = l_4_10[l_4_5]
                l_4_10 = weaponGradeAddndd80NA
                l_4_9 = l_4_10[4]
            end
        end
    else
        l_4_10 = g_locale
        l_4_11 = Locale_KR
        if l_4_10 == l_4_11 then
            l_4_10 = weaponGradeAddndd90KR
            l_4_8 = l_4_10[l_4_5]
            l_4_10 = weaponGradeAddndd90KR
            l_4_9 = l_4_10[4]
        else
            l_4_10 = g_locale
            l_4_11 = Locale_CN
            if l_4_10 == l_4_11 then
                l_4_10 = weaponGradeAddndd90
                l_4_8 = l_4_10[l_4_5]
                l_4_10 = weaponGradeAddndd90
                l_4_9 = l_4_10[4]
            else
                l_4_10 = weaponGradeAddndd90NA
                l_4_8 = l_4_10[l_4_5]
                l_4_10 = weaponGradeAddndd90NA
                l_4_9 = l_4_10[4]
            end
        end
    end
    l_4_10 = 0
    -- DECOMPILER ERROR: Confused at declaration of local variable

    l_4_11 = 0
    -- DECOMPILER ERROR: Confused at declaration of local variable

    l_4_12 = 0
    -- DECOMPILER ERROR: Confused at declaration of local variable

    if l_4_4 == 1 then
        l_4_10 = 9
    else
        l_4_13 = 9
        -- DECOMPILER ERROR: Confused at declaration of local variable

        l_4_14 = 2
        l_4_15 = l_4_4
        l_4_16 = 1
        for l_4_17 = l_4_14, l_4_15, l_4_16 do
            -- DECOMPILER ERROR: Confused at declaration of local variable

            if l_4_17 > 49 then
                l_4_18 = upgradefactor
                l_4_18 = l_4_13 * l_4_18
                armorLevelBaseCurrentLevelPlus = l_4_18
            else
                l_4_18 = math
                l_4_18 = l_4_18.max
                l_4_19 = l_4_17 / 10
                l_4_19 = l_4_19 * 4
                l_4_19 = 1 + l_4_19
                -- DECOMPILER ERROR: Confused at declaration of local variable

                l_4_20 = 0
                -- DECOMPILER ERROR: Confused at declaration of local variable

                l_4_18 = l_4_18(l_4_19, l_4_20)
                armorLevelBaseCurrentLevelPlus = l_4_18
            end
            l_4_18 = armorLevelBaseCurrentLevelPlus
            -- DECOMPILER ERROR: Confused at declaration of local variable

            l_4_13 = l_4_13 + l_4_18
            l_4_10 = l_4_13
        end
        -- DECOMPILER ERROR: Confused about usage of registers for local variables.

    end
    if l_4_2 == 21 then
        l_4_13 = round
        l_4_13 = l_4_13((l_4_10) * armorConstantSlotCoefficient[l_4_2] * armorConstantJobCoefficient[GlobalJob] *
            armorConstantGradeCoefficient[l_4_1][l_4_5], 0)
        l_4_11 = l_4_13 * l_4_8
        l_4_13 = round
        l_4_13 = l_4_13((l_4_10) * armorConstantSlotCoefficient[l_4_2] * armorConstantJobCoefficient[GlobalJob] *
            armorConstantGradeCoefficient[l_4_1][4], 0)
        l_4_12 = l_4_13 * l_4_9
    else
        l_4_13 = round
        l_4_13 = l_4_13((l_4_10) * armorConstantSlotCoefficient[l_4_2] * armorConstantJobCoefficient[l_4_3] *
            armorConstantGradeCoefficient[l_4_1][l_4_5], 0)
        l_4_11 = l_4_13 * l_4_8
        l_4_13 = round
        l_4_13 = l_4_13((l_4_10) * armorConstantSlotCoefficient[l_4_2] * armorConstantJobCoefficient[l_4_3] *
            armorConstantGradeCoefficient[l_4_1][4], 0)
        l_4_12 = l_4_13 * l_4_9
    end
    l_4_13 = 0
    -- DECOMPILER ERROR: Confused at declaration of local variable

    if l_4_6 > 49 and l_4_5 > 3 then
        l_4_13 = round(l_4_11 + l_4_12 * (l_4_5 - 4), 0)
    end
    -- DECOMPILER ERROR: Confused at declaration of local variable

    local l_6_20 = l_4_13
    do
        local l_6_21 = 0
        return math.max(l_6_20, l_6_21)
    end
end

v1, v2 = constant_value_ndd(0, 2, 21, 0, 70, 4, 0, 0)
print(v1, v2)

