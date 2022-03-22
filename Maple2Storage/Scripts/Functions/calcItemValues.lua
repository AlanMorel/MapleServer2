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

function round(firstValue, secondValue)
    if secondValue == nil then
        secondValue = 0
    end
    return math.floor(firstValue * (10 ^ secondValue) + 0.5 + 1e-06) / (10 ^ secondValue)
end

function clip_value(clipMinValue, floorValue, clipMaxValue)
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

weaponCapSlotGradeValue = {}
weaponCapSlotGradeValue[1] = {}
weaponCapSlotGradeValue[1][CommonRarity] = 6
weaponCapSlotGradeValue[1][RareRarity] = 7
weaponCapSlotGradeValue[1][ExceptionalRarity] = 8
weaponCapSlotGradeValue[1][EpicRarity] = 9
weaponCapSlotGradeValue[1][LegendaryRarity] = 10
weaponCapSlotGradeValue[1][AscendantRarity] = 10
weaponCapSlotGradeValue[2] = {}
weaponCapSlotGradeValue[2][CommonRarity] = 12
weaponCapSlotGradeValue[2][RareRarity] = 14
weaponCapSlotGradeValue[2][ExceptionalRarity] = 16
weaponCapSlotGradeValue[2][EpicRarity] = 18
weaponCapSlotGradeValue[2][LegendaryRarity] = 20
weaponCapSlotGradeValue[2][AscendantRarity] = 20

weaponGradeDoombookMAPCorrection = {}
weaponGradeDoombookMAPCorrection[CommonRarity] = 0.925
weaponGradeDoombookMAPCorrection[RareRarity] = 1.0175
weaponGradeDoombookMAPCorrection[ExceptionalRarity] = 1.11
weaponGradeDoombookMAPCorrection[EpicRarity] = 1.2025
weaponGradeDoombookMAPCorrection[LegendaryRarity] = 1.2825
weaponGradeDoombookMAPCorrection[AscendantRarity] = 1.2825

staticWapmaxCoefficient = {}
staticWapmaxCoefficient[CommonRarity] = 0.1
staticWapmaxCoefficient[RareRarity] = 0.12
staticWapmaxCoefficient[ExceptionalRarity] = 0.14
staticWapmaxCoefficient[EpicRarity] = 0.16
staticWapmaxCoefficient[LegendaryRarity] = 0.18
staticWapmaxCoefficient[AscendantRarity] = 0.2

staticGradeCoefficient = {}
staticGradeCoefficient[CommonRarity] = 0.4
staticGradeCoefficient[RareRarity] = 0.55
staticGradeCoefficient[ExceptionalRarity] = 0.7
staticGradeCoefficient[EpicRarity] = 0.85
staticGradeCoefficient[LegendaryRarity] =  1
staticGradeCoefficient[AscendantRarity] = 1

upgradefactor = 0.06

function calcItemGearScore(gearScoreFactorValue, rarity, itemSlot, enchantLevel, limitBreakLevel)

    local maxEnchantValue = 15
    enchantLevel = clip_value(enchantLevel, 0, maxEnchantValue)
    local l_66_6 = 0
    local itemGearScore = 0
    local l_66_8 = 0
    if gearScoreFactorValue > 0 then
        if limitBreakLevel < 60 then
            if rarity > 3 and gearScoreFactorValue >= 50 then
                if itemLevelRarityCoefficient[itemSlot] > 0 then
                    if rarity >= 5 then
                        l_66_8 = 100
                        l_66_6 = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                            itemLevelCoefficient[itemSlot] * 2 * (math.max)(rarity - 3, 1) +
                            (math.max)((gearScoreFactorValue - 50) * l_66_8 * itemLevelCoefficient[itemSlot], 0)
                    else
                        l_66_6 = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                            itemLevelCoefficient[itemSlot] * 2 * (math.max)(rarity - 3, 1)
                    end
                else
                    if itemSlot == Earring or itemSlot == Cape or itemSlot == Necklace or itemSlot == Ring then
                        if rarity >= 5 then
                            l_66_8 = 100
                            l_66_6 = (10 * gearScoreFactorValue + (math.max)((rarity - 2) * 5, 0)) *
                                itemLevelCoefficient[itemSlot] * 2 * (math.max)(rarity - 4, 1) +
                                (math.max)((gearScoreFactorValue - 50) * l_66_8 * itemLevelCoefficient[itemSlot], 0)
                        else
                            l_66_6 = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                                itemLevelCoefficient[itemSlot]
                        end
                    else
                        l_66_6 = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                            itemLevelCoefficient[itemSlot]
                    end
                end
            else
                l_66_6 = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) * itemLevelCoefficient[itemSlot]
            end
            if rarity > 3 and gearScoreFactorValue >= 50 then
                if itemLevelRarityCoefficient[itemSlot] > 0 then
                    l_66_6 = (2 + levelScoreFactorNA[gearScoreFactorValue]) * 1030 * rarityScoreFactor[rarity] *
                        itemLevelCoefficient[itemSlot]
                else
                    if itemSlot == Earring or itemSlot == Cape or itemSlot == Necklace or itemSlot == Ring then
                        l_66_6 = (2 + levelScoreFactorNA[gearScoreFactorValue]) * 1030 * rarityScoreFactor[rarity] *
                            itemLevelCoefficient[itemSlot]
                    else
                        l_66_6 = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                            itemLevelCoefficient[itemSlot]
                    end
                end
            else
                l_66_6 = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                    itemLevelCoefficient[itemSlot]
            end
        else
            if limitBreakLevel < 70 then
                if rarity > 3 and gearScoreFactorValue >= 50 then
                    if itemLevelRarityCoefficient[itemSlot] > 0 then
                        l_66_6 = (2 + levelScoreFactorNA[gearScoreFactorValue]) * 1030 * rarityScoreFactor[rarity] *
                            itemLevelCoefficient[itemSlot]
                    else
                        if itemSlot == Earring or itemSlot == Cape or itemSlot == Necklace or itemSlot == Ring then
                            l_66_6 = (2 + levelScoreFactorNA[gearScoreFactorValue]) * 1030 * rarityScoreFactor[rarity] *
                                itemLevelCoefficient[itemSlot]
                        else
                            l_66_6 = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                                itemLevelCoefficient[itemSlot]
                        end
                    end
                else
                    l_66_6 = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                        itemLevelCoefficient[itemSlot]
                end
            else
                if rarity > 3 and gearScoreFactorValue >= 50 then
                    local rarityFactor = rarityScoreFactor[rarity]
                    if itemLevelRarityCoefficient[itemSlot] > 0 then
                        l_66_6 = (2 + levelScoreFactorNA[gearScoreFactorValue]) * 1030 * rarityFactor *
                            itemLevelCoefficient[itemSlot]
                    else
                        if itemSlot == Earring or itemSlot == Cape or itemSlot == Necklace or itemSlot == Ring then
                            l_66_6 = (2 + levelScoreFactorNA[gearScoreFactorValue]) * 1030 * rarityFactor *
                                itemLevelCoefficient[itemSlot]
                        else
                            l_66_6 = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                                itemLevelCoefficient[itemSlot]
                        end
                    end
                else
                    do
                        l_66_6 = (10 * gearScoreFactorValue + (math.max)((rarity - 1) * 5, 0)) *
                            itemLevelCoefficient[itemSlot]
                        l_66_6 = 0
                        if limitBreakLevel < 60 then
                            itemGearScore = l_66_6
                        else
                            if limitBreakLevel < 70 then
                                itemGearScore = l_66_6
                            else
                                itemGearScore = l_66_6
                            end
                        end
                        local addItemGearScore = 0
                        if limitBreakLevel < 60 then
                            if rarity >= 4 then
                                addItemGearScore = itemGearScore * limitBreakItemLevelCoefficientNA[enchantLevel]
                            else
                                addItemGearScore = itemGearScore * limitBreakItemLevelCoefficient[enchantLevel]
                            end
                        else
                            if limitBreakLevel < 70 then
                                if rarity >= 4 then
                                    addItemGearScore = itemGearScore * limitBreakItemLevelCoefficientNA[enchantLevel]
                                else
                                    addItemGearScore = itemGearScore * limitBreakItemLevelCoefficient[enchantLevel]
                                end
                            else
                                if limitBreakLevel < 80 then
                                    if rarity >= 4 then
                                        addItemGearScore = itemGearScore * limitBreakItemLevelCoefficientNA[enchantLevel]
                                    else
                                        addItemGearScore = itemGearScore * limitBreakItemLevelCoefficient[enchantLevel]
                                    end
                                else
                                    if rarity >= 4 then
                                        addItemGearScore = itemGearScore * limitBreakItemLevelCoefficientNA[enchantLevel]
                                    else
                                        addItemGearScore = itemGearScore * limitBreakItemLevelCoefficient[enchantLevel]
                                    end
                                end
                            end
                        end
                        return itemGearScore, addItemGearScore
                    end
                end
            end
        end
    end
end

function constant_value_hp(v1, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, v2)
    local hpValue = 0

    if itemSlot == Earring or itemSlot == Cape or itemSlot == Necklace or itemSlot == Ring or itemSlot == Belt then
        if optionLevelFactor > 50 then
            if globalOptionLevelFactor < 60 then
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
            hpValue = round(((1.2884 * optionLevelFactor) - ((6.56 * weaponGradeHPCorrection[1][rarity]) * weaponHPSlotCorrection[itemSlot])), 0)
        elseif optionLevelFactor < 6 then
            hpValue = 1
        else
            hpValue = 0
        end
    elseif itemSlot > 49 and itemSlot < 60 then
        if optionLevelFactor > 5 then
            hpValue = round(((1.2884 * optionLevelFactor) - (((6.56 * weaponGradeHPCorrection[1][rarity]) * weaponHPSlotCorrection[itemSlot]) * 2)), 0)
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

function constant_value_addndd(l_4_0, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_4_7)
    local itemRarityFactor
    local itemRarityFactor2
    local baseAddNddValue
    local NddValueRoundedResult
    local NddValueRoundedResult2
    local addNddValue
    itemRarityFactor = 0
    itemRarityFactor2 = 0
    if globalOptionLevelFactor < 60 then
        itemRarityFactor = weaponGradeAddndd50NA[rarity]
        itemRarityFactor2 = weaponGradeAddndd50NA[4]
    elseif globalOptionLevelFactor < 70 then
        itemRarityFactor = weaponGradeAddndd60NA[rarity]
        itemRarityFactor2 = weaponGradeAddndd60NA[4]
    elseif globalOptionLevelFactor < 80 then
        itemRarityFactor = weaponGradeAddndd70NA[rarity]
        itemRarityFactor2 = weaponGradeAddndd70NA[4]
    elseif globalOptionLevelFactor < 90 then
        itemRarityFactor = weaponGradeAddndd80NA[rarity]
        itemRarityFactor2 = weaponGradeAddndd80NA[4]
    else
        itemRarityFactor = weaponGradeAddndd90NA[rarity]
        itemRarityFactor2 = weaponGradeAddndd90NA[4]
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
                armorLevelBaseCurrentLevelPlus = addNddValue * upgradefactor
            else
                armorLevelBaseCurrentLevelPlus = math.max((1 + ((x / 10) * 4)), 0)
            end
            addNddValue = addNddValue + armorLevelBaseCurrentLevelPlus
            baseAddNddValue = addNddValue
        end
    end
    if itemSlot == Belt then
        addNddValue = round((baseAddNddValue) * armorConstantSlotCoefficient[itemSlot] * armorConstantJobCoefficient[GlobalJob] *
            armorConstantGradeCoefficient[deviationValue][rarity], 0)
        NddValueRoundedResult = addNddValue * itemRarityFactor
        addNddValue = round((baseAddNddValue) * armorConstantSlotCoefficient[itemSlot] * armorConstantJobCoefficient[GlobalJob] *
            armorConstantGradeCoefficient[deviationValue][4], 0)
        NddValueRoundedResult2 = addNddValue * itemRarityFactor2
    else
        addNddValue = round((baseAddNddValue) * armorConstantSlotCoefficient[itemSlot] * armorConstantJobCoefficient[itemJob] *
            armorConstantGradeCoefficient[deviationValue][rarity], 0)
        NddValueRoundedResult = addNddValue * itemRarityFactor
        addNddValue = round((baseAddNddValue) * armorConstantSlotCoefficient[itemSlot] * armorConstantJobCoefficient[itemJob] *
            armorConstantGradeCoefficient[deviationValue][4], 0)
        NddValueRoundedResult2 = addNddValue * itemRarityFactor2
    end
    addNddValue = 0
    if globalOptionLevelFactor > 49 and rarity > 3 then
        addNddValue = round((NddValueRoundedResult + NddValueRoundedResult2 * (rarity - 4)), 0)
    end
    do
        return math.max(addNddValue, 0)
    end
end

function constant_value_ndd(l_5_0, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_5_7)
    globalOptionLevelFactor = optionLevelFactor
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
                armorLevelBaseCurrentLevelPlus = addNddValue * upgradefactor
            else
                armorLevelBaseCurrentLevelPlus = math.max((1 + ((x / 10) * 4)), 0)
            end
            addNddValue = addNddValue + armorLevelBaseCurrentLevelPlus
        end
        baseNddValue = addNddValue
    end
    addNddValue = 0
    addNddValue = constant_value_addndd(l_5_0, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_5_7)
    if itemSlot == Belt then
        nddValue = round((baseNddValue) * armorConstantSlotCoefficient[itemSlot] * armorConstantJobCoefficient[GlobalJob] *
            armorConstantGradeCoefficient[deviationValue][rarity], 0) + addNddValue
    elseif globalOptionLevelFactor >= 70 and l_5_0 > 0 then
        nddValue = round((baseNddValue) * armorConstantSlotCoefficient[itemSlot] * armorConstantJobCoefficient[itemJob] *
            armorConstantGradeCoefficient[deviationValue][rarity] * (l_5_0 / 100), 0) + round(addNddValue * (l_5_0 / 100), 0)
    else
        nddValue = round((baseNddValue) * armorConstantSlotCoefficient[itemSlot] * armorConstantJobCoefficient[itemJob] *
            armorConstantGradeCoefficient[deviationValue][rarity], 0) + addNddValue
    end
    return nddValue
end

function constant_value_mar(l_6_0, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_6_7)
    local marValue = 0
    if itemSlot == Earring then
        marValue = round((math.max(0, ((((4.5 + (1 * (optionLevelFactor - 12)))) / 1.5) * accConstantGradeCoefficient[rarity]))), 0)
    elseif itemSlot == Ring then
        marValue = round((math.max(0, (((4 + (1.5 * ((optionLevelFactor - 12)))) / 1.5) * accConstantGradeCoefficient[rarity]))), 0)
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

function constant_value_par(l_7_0, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_7_7)
    local parValue = 0
    if itemSlot == Cape then
        parValue = round(math.max(0, (3 + (((1 * (optionLevelFactor - 12)) / 1.5) * accConstantGradeCoefficient[rarity]))), 0)
    elseif itemSlot == Necklace then
        parValue = round(math.max(0, (3 + (((1.5 * (optionLevelFactor - 12)) / 1.5) * accConstantGradeCoefficient[rarity]))), 0)
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

function constant_value_map(l_9_0, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_9_7)
    local mapValue
    local fractionedFactor
    mapValue = 0
    if optionLevelFactor < 58 then
        mapValue = round(((round(((0.5502 * optionLevelFactor) + 0.1806), 0)) * weaponGradeDoombookMAPCorrection[rarity]), 0)
    elseif optionLevelFactor > 57 then
        fractionedFactor = optionLevelFactor % 2
        if fractionedFactor == 1 then
            mapValue = round(((((0.2751 * (optionLevelFactor - 1)) + 16.136)) * weaponGradeDoombookMAPCorrection[rarity]), 0)
        else
            mapValue = round(((((0.2751 * optionLevelFactor) + 16.136)) * weaponGradeDoombookMAPCorrection[rarity]), 0)
        end
    else
        mapValue = 0
    end
    return mapValue
end

function constant_value_cap(l_13_0, l_13_1, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_13_7)
    local capValue = 0
    if itemSlot > 29 and itemSlot < 40 then
        capValue = (weaponCapSlotGradeValue[1])[rarity]
    else
        if itemSlot > 39 and itemSlot < 60 then
            capValue = (weaponCapSlotGradeValue[2])[rarity]
        else
            capValue = 0
        end
    end
    return capValue
end

function constant_value_str(l_14_0, l_14_1, itemSlot, itemJob, l_14_4, l_14_5, globalOptionLevelFactor, l_14_7)
    local strValue = 0
    if globalOptionLevelFactor < 51 then
        if itemJob == Knight or itemJob == Berserker or itemJob == RuneBlader then
            if itemSlot == Overall then
                strValue = round(1.8 * (globalOptionLevelFactor - 20) / 3, 0)
            else
                strValue = round((globalOptionLevelFactor - 20) / 3, 0)
            end
        elseif itemJob == GlobalJob then
            if itemSlot == Overall then
                strValue = round(1.8 * (globalOptionLevelFactor - 20) / 6, 0)
            else
                strValue = round((globalOptionLevelFactor - 20) / 6, 0)
            end
        else
            strValue = 0
        end
    else
        if itemJob == Knight or itemJob == Berserker or itemJob == RuneBlader then
            if itemSlot == Overall then
                strValue = round(1.8 * (2 * globalOptionLevelFactor - 90) / 2, 0)
            else
                strValue = round(2 * globalOptionLevelFactor - 90, 0)
            end
        elseif itemJob == GlobalJob then
            if itemSlot == Overall then
                strValue = round(1.8 * (2 * globalOptionLevelFactor - 90) / 6, 0)
            else
                strValue = round((2 * globalOptionLevelFactor - 90) / 3, 0)
            end
        else
            strValue = 0
        end
    end
    return strValue
end

function constant_value_int(l_15_0, l_15_1, itemSlot, itemJob, l_15_4, l_15_5, globalOptionLevelFactor, l_15_7)
    local intValue = 0
    if globalOptionLevelFactor < 51 then
        if itemJob == Wizard or itemJob == Priest or itemJob == SoulBinder or itemJob == Striker and l_14_7 == 1 then
            if itemSlot == Overall then
                intValue = round(1.8 * (globalOptionLevelFactor - 20) / 3, 0)
            else
                intValue = round((globalOptionLevelFactor - 20) / 3, 0)
            end
        elseif itemJob == GlobalJob then
            if itemSlot == Overall then
                intValue = round(1.8 * (globalOptionLevelFactor - 20) / 6, 0)
            else
                intValue = round((globalOptionLevelFactor - 20) / 6, 0)
            end
        else
            intValue = 0
        end
    else
        if itemJob == Wizard or itemJob == Priest or itemJob == SoulBinder or itemJob == Striker and l_14_7 == 1 then
            if itemSlot == Overall then
                intValue = round(1.8 * (2 * globalOptionLevelFactor - 90) / 2, 0)
            else
                intValue = round(2 * globalOptionLevelFactor - 90, 0)
            end
        elseif itemJob == GlobalJob then
            if itemSlot == Overall then
                intValue = round(1.8 * (2 * globalOptionLevelFactor - 90) / 6, 0)
            else
                intValue = round((2 * globalOptionLevelFactor - 90) / 3, 0)
            end
        else
            intValue = 0
        end
    end
    return intValue
end

function constant_value_dex(l_16_0, l_16_1, itemSlot, itemJob, l_16_4, l_16_5, globalOptionLevelFactor, l_16_7)
    local dexValue = 0
    if globalOptionLevelFactor < 51 then
        if itemJob == Archer or itemJob == HeavyGunner or itemJob == Striker then
            if itemSlot == Overall then
                dexValue = round(1.8 * (globalOptionLevelFactor - 20) / 3, 0)
            else
                dexValue = round((globalOptionLevelFactor - 20) / 3, 0)
            end
        elseif itemJob == GlobalJob then
            if itemSlot == Overall then
                dexValue = round(1.8 * (globalOptionLevelFactor - 20) / 6, 0)
            else
                dexValue = round((globalOptionLevelFactor - 20) / 6, 0)
            end
        else
            dexValue = 0
        end
    elseif itemJob == Archer or itemJob == HeavyGunner or itemJob == Striker then
        if itemSlot == Overall then
            dexValue = round(1.8 * (2 * globalOptionLevelFactor - 90) / 2, 0)
        else
            dexValue = round(2 * globalOptionLevelFactor - 90, 0)
        end
    elseif itemJob == GlobalJob then
        if itemSlot == Overall then
            dexValue = round(1.8 * (2 * globalOptionLevelFactor - 90) / 6, 0)
        else
            dexValue = round((2 * globalOptionLevelFactor - 90) / 3, 0)
        end
    else
        dexValue = 0
    end
    return dexValue
end

function constant_value_luk(l_14_0, l_14_1, itemSlot, itemJob, l_14_4, l_14_5, globalOptionLevelFactor, l_14_7)
    local lukValue = 0
    if globalOptionLevelFactor < 51 then
        if itemJob == Thief or itemJob == Assassin then
            if itemSlot == Overall then
                lukValue = round(((1.8 * (globalOptionLevelFactor - 20)) / 3), 0)
            else
                lukValue = round(((globalOptionLevelFactor - 20) / 3), 0)
            end
        elseif itemJob == GlobalJob then
            if itemSlot == Overall then
                lukValue = round(((1.8 * (globalOptionLevelFactor - 20)) / 6), 0)
            else
                lukValue = round(((globalOptionLevelFactor - 20) / 6), 0)
            end
        else
            lukValue = 0
        end
    elseif itemJob == Thief or itemJob == Assassin then
        if itemSlot == Overall then
            lukValue = round(((1.8 * ((2 * globalOptionLevelFactor) - 90)) / 2), 0)
        else
            lukValue = round(((2 * globalOptionLevelFactor) - 90), 0)
        end
    elseif itemJob == GlobalJob then
        if itemSlot == Overall then
            lukValue = round(((1.8 * (((2 * globalOptionLevelFactor) - 90))) / 6), 0)
        else
            lukValue = round((((2 * globalOptionLevelFactor) - 90) / 3), 0)
        end
    else
        lukValue = 0
    end
    return lukValue
end

function constant_value_addwap(l_1_0, deviationValue, itemSlot, l_1_3, optionLevelFactor, rarity, globalOptionLevelFactor, l_1_7)
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
    if globalOptionLevelFactor < 60 then
        weaponRarityFactor = weaponGradeAddwap[rarity]
        weaponRarityFactor2 = weaponGradeAddwap[4]
    elseif globalOptionLevelFactor < 70 then
        weaponRarityFactor = weaponGradeAddwap60NA[rarity]
        weaponRarityFactor2 = weaponGradeAddwap60NA[4]
    elseif globalOptionLevelFactor < 80 then
        weaponRarityFactor = weaponGradeAddwap70NA[rarity]
        weaponRarityFactor2 = weaponGradeAddwap70NA[4]
    elseif globalOptionLevelFactor < 90 then
        weaponRarityFactor = weaponGradeAddwap80NA[rarity]
        weaponRarityFactor2 = weaponGradeAddwap80NA[4]
    else
        weaponRarityFactor = weaponGradeAddwap90NA[rarity]
        weaponRarityFactor2 = weaponGradeAddwap90NA[4]
    end
    if optionLevelFactor == 1 then
        addWapValue = 5
    else
        minAddWapValue = 5
        maxAddWapValue = 0
        for x = 2, optionLevelFactor do
            if x > 49 then
                maxAddWapValue = minAddWapValue * upgradefactor
            else
                maxAddWapValue = math.max((((x / 30) * 20) - 0.8), 0)
            end
            minAddWapValue = minAddWapValue + maxAddWapValue
            addWapValue = minAddWapValue
        end
    end
    weaponRoundedWapValue = round((((addWapValue) * weaponSlotCoefficient[itemSlot]) / weaponAttackSpeedCoefficient[itemSlot]), 1)
    roundedWeaponWapValue1 = round(((weaponRoundedWapValue * (weaponGradeCoefficient[deviationValue][rarity])) * (1 - weaponSlotDeviation[itemSlot][deviationValue])), 0)
    roundedWeaponWapValue2 = round(((weaponRoundedWapValue * weaponGradeCoefficient[deviationValue][rarity]) * (1 + weaponSlotDeviation[itemSlot][deviationValue])), 0)
    weaponRarityFactorResult = ((roundedWeaponWapValue1 + roundedWeaponWapValue2) / 2) * weaponRarityFactor
    roundedWeaponWapValue3 = round(((weaponRoundedWapValue * weaponGradeCoefficient[deviationValue][4]) * (1 - weaponSlotDeviation[itemSlot][deviationValue])), 0)
    roundedWeaponWapValue4 = round(((weaponRoundedWapValue * weaponGradeCoefficient[deviationValue][4]) * (1 + weaponSlotDeviation[itemSlot][deviationValue])), 0)
    weaponRarityFactor2Result = ((roundedWeaponWapValue3 + roundedWeaponWapValue4) / 2) * weaponRarityFactor2
    if globalOptionLevelFactor > 49 and rarity > 3 then
        minAddWapValue = round(weaponRarityFactorResult + weaponRarityFactor2Result * (rarity - 4), 0)
    end
    do
        return math.max(minAddWapValue, 0)
    end
end

function constant_value_wapmin(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_2_7)
    globalOptionLevelFactor = optionLevelFactor
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
                    minWapMinValue = addWapValue * upgradefactor
                else
                    minWapMinValue = math.max((((x / 30) * 20) - 0.8), 0)
                end
            end
            addWapValue = addWapValue + minWapMinValue
            wapMinValue = addWapValue
        end
    end
    weaponRoundedWapValue = round((((wapMinValue) * weaponSlotCoefficient[itemSlot]) / weaponAttackSpeedCoefficient[itemSlot]), 1)
    addWapValue = 0
    addWapValue = constant_value_addwap(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_2_7)
    if globalOptionLevelFactor < 60 then
        minWapMinValue = round(weaponRoundedWapValue * weaponGradeCoefficient[deviationValue][rarity] * (1 - weaponSlotDeviation[itemSlot][deviationValue]), 0)
        maxWapMinValue = minWapMinValue + addWapValue
    elseif globalOptionLevelFactor >= 70 and currentStatValue > 0 then
        maxWapMinValue = round(weaponRoundedWapValue * weaponGradeCoefficient[deviationValue][rarity] * (1 - weaponSlotDeviation[itemSlot][deviationValue]) * (currentStatValue / 100), 0) +
            round(addWapValue * (1 - weaponSlotDeviation[itemSlot][deviationValue]) * (currentStatValue / 100), 0)
    else
        maxWapMinValue = round(weaponRoundedWapValue * weaponGradeCoefficient[deviationValue][rarity] * (1 - weaponSlotDeviation[itemSlot][deviationValue]), 0) +
            round(addWapValue * (1 - weaponSlotDeviation[itemSlot][deviationValue]), 0)
    end
    minWapMinValue = maxWapMinValue
    return maxWapMinValue
end

function constant_value_wapmax(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_3_7)
    globalOptionLevelFactor = optionLevelFactor
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
                    minWapMaxValue = addWapValue * upgradefactor
                else
                    minWapMaxValue = math.max((((x / 30) * 20) - 0.8), 0)
                end
            end
            addWapValue = addWapValue + minWapMaxValue
            wapMaxValue = addWapValue
        end
    end
    weaponRoundedWapValue = round((((wapMaxValue) * weaponSlotCoefficient[itemSlot]) / weaponAttackSpeedCoefficient[itemSlot]), 1)
    addWapValue = 0
    addWapValue = constant_value_addwap(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_3_7)
    if globalOptionLevelFactor < 60 then
        minWapMaxValue = round(weaponRoundedWapValue * weaponGradeCoefficient[deviationValue][rarity] * (1 + weaponSlotDeviation[itemSlot][deviationValue]), 0)
        maxWapMaxValue = minWapMaxValue + addWapValue
    elseif globalOptionLevelFactor >= 70 and currentStatValue > 0 then
        minWapMaxValue = round(weaponRoundedWapValue * weaponGradeCoefficient[deviationValue][rarity] * (1 + weaponSlotDeviation[itemSlot][deviationValue]) * (currentStatValue / 100), 0)
        maxWapMaxValue = minWapMaxValue + round(addWapValue * (1 + weaponSlotDeviation[itemSlot][deviationValue]) * (currentStatValue / 100), 0)
    else
        minWapMaxValue = round(weaponRoundedWapValue * weaponGradeCoefficient[deviationValue][rarity] * (1 + weaponSlotDeviation[itemSlot][deviationValue]), 0)
        maxWapMaxValue = minWapMaxValue + round(addWapValue * (1 + weaponSlotDeviation[itemSlot][deviationValue]), 0)
    end
    minWapMaxValue = maxWapMaxValue
    return minWapMaxValue
end

function static_value_hp(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_21_7)
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

function static_value_addndd(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_6_7)
    local itemRarityFactor = 0
    local rarityFactor = 0
    local baseNddValue = 0
    local maxAddNddValue = 0
    local minAddNddValue = 0

    if globalOptionLevelFactor < 60 then
        itemRarityFactor = weaponGradeAddndd50NA[rarity]
        rarityFactor = weaponGradeAddndd50NA[4]
    elseif globalOptionLevelFactor < 70 then
        itemRarityFactor = weaponGradeAddndd60NA[rarity]
        rarityFactor = weaponGradeAddndd60NA[4]
    elseif globalOptionLevelFactor < 80 then
        itemRarityFactor = weaponGradeAddndd70NA[rarity]
        rarityFactor = weaponGradeAddndd70NA[4]
    elseif globalOptionLevelFactor < 90 then
        itemRarityFactor = weaponGradeAddndd80NA[rarity]
        rarityFactor = weaponGradeAddndd80NA[4]
    else
        itemRarityFactor = weaponGradeAddndd90NA[rarity]
        rarityFactor = weaponGradeAddndd90NA[4]
    end
    if optionLevelFactor == 1 then
        baseNddValue = 9
    else
        local addNddValue = 9
        for x = 2, optionLevelFactor do
            if x > 49 then
                armorLevelBaseCurrentLevelPlus = addNddValue * upgradefactor
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
            maxAddNddValue = round((baseNddValue) * armorConstantSlotCoefficient[itemSlot] * armorConstantJobCoefficient[JC_Global] * (armorConstantGradeCoefficient[deviationValue])[rarity], 0) * itemRarityFactor
            minAddNddValue = round((baseNddValue) * armorConstantSlotCoefficient[itemSlot] * armorConstantJobCoefficient[JC_Global] * (armorConstantGradeCoefficient[deviationValue])[4], 0) * rarityFactor
        else
            maxAddNddValue = round((baseNddValue) * armorConstantSlotCoefficient[itemSlot] * armorConstantJobCoefficient[itemJob] * (armorConstantGradeCoefficient[deviationValue])[rarity], 0) * itemRarityFactor
            minAddNddValue = round((baseNddValue) * armorConstantSlotCoefficient[itemSlot] * armorConstantJobCoefficient[itemJob] * (armorConstantGradeCoefficient[deviationValue])[4], 0) * rarityFactor
        end
        local addNddValue = 0
        if globalOptionLevelFactor > 49 and rarity > 3 then
            addNddValue = round(maxAddNddValue + minAddNddValue * (rarity - 4), 0)
        end
        do
            return math.max(addNddValue, 0)
        end
    end
end

function static_value_ndd(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_20_7)
    globalOptionLevelFactor = optionLevelFactor
    local minNddValue = 0
    local maxNddValue = 0
    local baseNddValue = 0
    local addNddValue = 0
    local staticMinAddValue = 0
    local staticMaxAddValue = 0

    if optionLevelFactor == 1 then
        baseNddValue = 9
    else
        addNddValue = 9
        for x = 2, optionLevelFactor do
            if x > 49 then
                armorLevelBaseCurrentLevelPlus = addNddValue * upgradefactor
            else
                armorLevelBaseCurrentLevelPlus = math.max((1 + ((x / 10) * 4)), 0)
            end
        end
        addNddValue = addNddValue + armorLevelBaseCurrentLevelPlus
        baseNddValue = addNddValue
    end
    addNddValue = 0
    addNddValue = static_value_addndd(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_20_7)
    if itemSlot == 21 then
        maxNddValue = math.max(round((baseNddValue) * armorConstantSlotCoefficient[itemSlot] * armorConstantJobCoefficient[GlobalJob] *
            staticArmorGradeCoefficient[rarity], 0), 4)
    elseif itemSlot == 12 or itemSlot == 18 or itemSlot == 19 or itemSlot == 20 then
        maxNddValue = math.max(round((baseNddValue) * armorConstantSlotCoefficient[itemSlot] * staticAccGradeCoefficient[rarity], 0), 0)
    else
        maxNddValue = math.max(round((baseNddValue) * armorConstantSlotCoefficient[itemSlot] * armorConstantJobCoefficient[itemJob] *
            staticArmorGradeCoefficient[rarity], 0), 4)
    end
    if maxNddValue < 466 then
        minNddValue = round(maxNddValue * math.max(0.0598 * math.log(maxNddValue) + 0.432, 0.5), 0)
    else
        minNddValue = math.max(round(maxNddValue * 0.8, 0), 1)
    end
    if currentStatValue == 0 then
        staticMinAddValue = minNddValue + addNddValue
        staticMaxAddValue = maxNddValue + addNddValue
    else
        staticMinAddValue = round(maxNddValue * (currentStatValue / 100), 0) + addNddValue
        staticMaxAddValue = round(maxNddValue * (currentStatValue / 100), 0) + addNddValue
    end
    print(staticMinAddValue, staticMaxAddValue)
    return staticMinAddValue, staticMaxAddValue
end

function static_value_mar(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_22_7)
    local minMarValue = 0
    local maxVarValue = 0
    local staticMinMarValue = 0
    local staticMaxMarValue = 0

    if itemSlot == SpellBook then
        minMarValue = round(((((1e-005 * (optionLevelFactor ^ 3)) - (0.003 * optionLevelFactor ^ 2)) + (0.367 * optionLevelFactor)) + 4.8841), 0)
    else
        minMarValue = round((((((1e-005 * (optionLevelFactor ^ 3)) - (0.003 * (optionLevelFactor ^ 2))) + (0.367 * optionLevelFactor)) + 4.8841) * staticGradeCoefficient[rarity]), 0)
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

function static_value_par(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_23_7)
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

function static_value_map(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_18_7)
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

function static_value_pap(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_17_7)
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

function static_rate_abp(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_24_7)
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

function static_value_addwap(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_15_7)
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
    if globalOptionLevelFactor < 60 then
        itemRarityFactor = weaponGradeAddwap50NA[rarity]
        rarityFactor = weaponGradeAddwap50NA[4]
    elseif globalOptionLevelFactor < 70 then
        itemRarityFactor = weaponGradeAddwap60NA[rarity]
        rarityFactor = weaponGradeAddwap60NA[4]
    elseif globalOptionLevelFactor < 80 then
        itemRarityFactor = weaponGradeAddwap70NA[rarity]
        rarityFactor = weaponGradeAddwap70NA[4]
    elseif globalOptionLevelFactor < 90 then
        itemRarityFactor = weaponGradeAddwap80NA[rarity]
        rarityFactor = weaponGradeAddwap80NA[4]
    else
        itemRarityFactor = weaponGradeAddwap90NA[rarity]
        rarityFactor = weaponGradeAddwap90NA[4]
    end
    if optionLevelFactor == 1 then
        baseAddWapValue = 5
    else
        addWapValueResult = 5
        WapUpgradeFactor = 0
        for x = 2, optionLevelFactor do
            if x > 49 then
                WapUpgradeFactor = addWapValueResult * upgradefactor
            else
                WapUpgradeFactor = math.max((((x / 30) * 20) - 0.8), 0)
            end
            addWapValueResult = addWapValueResult + WapUpgradeFactor
            baseAddWapValue = addWapValueResult
        end
    end
    if itemSlot == Blade then
        addWapValue = round((((baseAddWapValue) * weaponSlotCoefficient[Staff]) / weaponAttackSpeedCoefficient[Staff]), 1)
    else
        addWapValue = round((((baseAddWapValue) * weaponSlotCoefficient[itemSlot]) / weaponAttackSpeedCoefficient[itemSlot]), 1)
    end
    maxAddWapValue = math.max(((addWapValue * staticWapmaxCoefficient[rarity]) * (1 + weaponSlotDeviation[itemSlot][1])), 2)
    minAddWapValue = math.max(((addWapValue * staticWapmaxCoefficient[4]) * (1 + weaponSlotDeviation[itemSlot][1])), 2)
    addWapValueResult = 0
    if globalOptionLevelFactor > 49 and rarity > 3 then
        addWapValueResult = round(maxAddWapValue + minAddWapValue * (rarity - 4), 0)
    end
    do
        return math.max(addWapValueResult, 0)
    end
end

function static_value_wapmax(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_15_7)
    globalOptionLevelFactor = optionLevelFactor
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
                wapUpgradeFactor = addWapValueResult * upgradefactor
            else
                wapUpgradeFactor = (math.max)(x / 30 * 20 - 0.8, 0)
            end
            addWapValueResult = addWapValueResult + wapUpgradeFactor
            baseWapValue = addWapValueResult
        end
    end
    do
        addWapValue = static_value_addwap(currentStatValue, deviationValue, itemSlot, itemJob, optionLevelFactor, rarity, globalOptionLevelFactor, l_15_7)
        if itemSlot == Blade then
            wapValueResult = round((baseWapValue) * weaponSlotCoefficient[Staff] / weaponAttackSpeedCoefficient[Staff], 1)
            staticWapMaxValue = (math.max)(wapValueResult * staticWapmaxCoefficient[rarity] * (1 + (weaponSlotDeviation[itemSlot])[1]), 2) + addWapValue
        else
            wapValueResult = round((baseWapValue) * weaponSlotCoefficient[itemSlot] / weaponAttackSpeedCoefficient[itemSlot], 1)
            staticWapMaxValue = (math.max)(wapValueResult * staticWapmaxCoefficient[rarity] * (1 + (weaponSlotDeviation[itemSlot])[1]), 2) + addWapValue
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