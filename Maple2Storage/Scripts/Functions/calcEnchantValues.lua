-- Server only scripts
function calcEnchantBoostValues(enchantLevel, itemSlot, itemLevel)
    local bonusFactorType1 = EnchantingBonusFactor[enchantLevel]
    local bonusFactorType2 = bonusFactorType1 -- Flexibility for different stat calculations
    local statType1 = 0 -- values used here are the StatAttributes ids
    local statType2 = 0
    local equipType = getSlotType(itemSlot)
    if equipType == Weapon then
        statType1 = 27
        statType2 = 28
    else
        if itemLevel > 70 then
            statType1 = 28
        else
            statType1 = 20
        end
    end
    return statType1, bonusFactorType1, statType2, bonusFactorType2
end

function calcEnchantRates(enchantLevel)
    local successRate = EnchantingSuccessRate[enchantLevel] * 100
    local additionalCatalystsRate = AdditionalCatalystSuccessRate[enchantLevel] * 100
    local pityCharges = EnchantingPityCharges[enchantLevel]
    local chargeRate = ChargeRate[enchantLevel] * 100
    return successRate, additionalCatalystsRate, pityCharges, chargeRate
end

function calcEnchantIngredients(enchantLevel, rarity, itemSlot, itemLevel)
    local ingredient1Tag = 'Onix'
    local ingredient2Tag = 'CrystalPiece'
    local ingredient3Tag = 'ChaosOnix'
    local catalystCost = 0
    local ingredient1Cost = 100 * enchantLevel -- temp value
    local ingredient2Cost = 100 * enchantLevel -- temp value
    local ingredient3Cost = 100 * enchantLevel -- temp value
    -- TODO: // figure out ingredient cost calculation
    if enchantLevel > 10 and rarity > 3 then
        catalystCost = 1
    end
    return catalystCost, ingredient1Tag, ingredient1Cost, ingredient2Tag, ingredient2Cost, ingredient3Tag, ingredient3Cost
end

EnchantingBonusFactor = { 0.02, 0.04, 0.07, 0.1, 0.14, 0.19, 0.25, 0.32, 0.4, 0.5, 0.64, 0.84, 1.12, 1.5, 2 }
EnchantingSuccessRate = { 1, 1, 1, 0.95, 0.90, 0.80, 0.70, 0.60, 0.50, 0.40, 0.30, 0.20, 0.15, 0.10, 0.05 }

EnchantingPityCharges = {}
for i = 0, 15 do
    EnchantingPityCharges[i] = 0
end
EnchantingPityCharges[11] = 2
EnchantingPityCharges[12] = 3
EnchantingPityCharges[13] = 4
EnchantingPityCharges[14] = 4
EnchantingPityCharges[15] = 5

AdditionalCatalystSuccessRate = {}
for i = 0, 15 do
    AdditionalCatalystSuccessRate[i] = 0
end
AdditionalCatalystSuccessRate[12] = 0.07
AdditionalCatalystSuccessRate[13] = 0.05
AdditionalCatalystSuccessRate[14] = 0.04
AdditionalCatalystSuccessRate[15] = 0.02

ChargeRate = {}
for i = 0, 15 do
    ChargeRate[i] = 0.01
end

-- Slot Type values
Weapon = 0
Armor = 1
Accessory = 2

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

function getSlotType(itemSlot)
    local slotType = Armor
    if itemSlot >= 30 and itemSlot <= 56 then
        slotType = Weapon
        return slotType
    elseif itemSlot == Necklace or itemSlot == Ring or itemSlot == Belt or itemSlot == Cape or itemSlot == Earring then
        slotType = Accessory
        return slotType
    else
        return slotType
    end
end