-- Limit Break stat calculations. Based on KMS2
function calcLimitBreakStatRateValues(limitBreakLevel)
    local nextLevel = limitBreakLevel + 1
    local bonusFactor = math.ceil(nextLevel / 10)
    -- values used here represent stat id
    return 27, LimitBreakBonusFactor[bonusFactor], 28, LimitBreakBonusFactor[bonusFactor],
    11024, LimitBreakAddDamage[nextLevel], 11025, LimitBreakAddDamage[nextLevel], 31, LimitBreakAddPiercing[nextLevel]
end

function calcLimitBreakStatFlatValues(limitBreakLevel)
    local nextLevel = limitBreakLevel + 1
    return 4, LimitBreakAddHp[nextLevel]
end

function calcLimitBreakCost(limitBreakLevel)
    local ingredient1Tag = "ChaosOnix"
    local ingredient2Tag = "Onix"
    local ingredient3Tag = "PrismShard"
    local ingredient4Tag = "PrismStone"
    local levelSet = math.ceil((limitBreakLevel + 1) / 10)
    return MesoCost[levelSet], ingredient1Tag, Ingredient1Cost[levelSet], ingredient2Tag, Ingredient2Cost[levelSet],
    ingredient3Tag, Ingredient3Cost[levelSet], ingredient4Tag, Ingredient4Cost[levelSet]
end

LimitBreakBonusFactor = { 0.02, 0.02, 0.03, 0.03, 0.04, 0.05, 0.06, 0.07, 0.08, 0.10 } -- each value represents 10 levels
LimitBreakAddHp = {}
for i = 1, 99 do
    LimitBreakAddHp[i] = 10
end
LimitBreakAddHp[1] = 380
LimitBreakAddHp[10] = 100
LimitBreakAddHp[20] = 120
LimitBreakAddHp[30] = 140
LimitBreakAddHp[40] = 160
LimitBreakAddHp[50] = 180
LimitBreakAddHp[60] = 200
LimitBreakAddHp[70] = 220
LimitBreakAddHp[80] = 240
LimitBreakAddHp[90] = 260

LimitBreakAddDamage = {}
for i = 1, 99 do
    LimitBreakAddDamage[i] = 0
end
LimitBreakAddDamage[1] = 0.02
LimitBreakAddDamage[10] = 0.02
LimitBreakAddDamage[20] = 0.02
LimitBreakAddDamage[30] = 0.02
LimitBreakAddDamage[40] = 0.02
LimitBreakAddDamage[50] = 0.02
LimitBreakAddDamage[60] = 0.02
LimitBreakAddDamage[70] = 0.02
LimitBreakAddDamage[80] = 0.02
LimitBreakAddDamage[90] = 0.02

LimitBreakAddPiercing = {}
for i = 1, 99 do
    LimitBreakAddPiercing[i] = 0
end
LimitBreakAddPiercing[1] = 0.06
LimitBreakAddPiercing[30] = 0.06
LimitBreakAddPiercing[60] = 0.06
LimitBreakAddPiercing[90] = 0.06

MesoCost = { 1171000, 1951000, 2731000, 4198500, 6228500, 8258500, 16069000, 28609000, 41157600, 46878000 }
Ingredient1Cost = { 54, 74, 94, 158, 258, 358, 601, 961, 1321, 1483 }
Ingredient2Cost = { 3510, 4810, 6110, 10270, 16770, 23270, 39065, 62465, 85865, 96395 }
Ingredient3Cost = { 64, 128, 192, 256, 320, 384, 448, 512, 576, 640 }
Ingredient4Cost = { 4, 8, 12, 16, 20, 24, 28, 32, 36, 40 }