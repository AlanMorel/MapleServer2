-- Calculates ingredient cost for changing attributes
function calcGetItemRemakeIngredientNew(itemSlot, changeCount, rarity, itemLevel)
    local colorCrystalTag = getItemRemakeUsecrystalTag(itemSlot)
    local colorCrystalCost = 1
    local crystalFragmentCost = 1
    local metaCellCost = 1
    local crystalCostFactor = 1.24
    local slotScore = getSlotScore(itemSlot)
    local slotType = getSlotType(itemSlot)
    local crystalFragmentBaseCost = 1
    local itemLevelCalcLimit = 0
    do
        if slotType == Accessory then
            crystalFragmentBaseCost = 5
        else
            crystalFragmentBaseCost = 1
        end
        itemLevelCalcLimit = math.min(50, itemLevel)
        crystalFragmentBaseCost = math.floor((crystalFragmentBaseCost * (1 + ((itemLevelCalcLimit - 50) / 10))))
        if math.min(changeCount, 14) < 10 then
            crystalCostFactor = 1.25
        end
        if rarity <= 4 then
            crystalFragmentCost = (math.floor(200 * (crystalCostFactor ^ changeCount))) * slotScore
        elseif rarity == 5 then
            crystalFragmentCost = (400 * ((changeCount + 1))) * slotScore
        elseif rarity >= 6 then
            crystalFragmentCost = (600 * ((changeCount + 1))) * slotScore
        end
        metaCellCost = (((11 + changeCount) + (math.max(0, itemLevelCalcLimit - 50)))) * slotScore
        colorCrystalCost = crystalFragmentBaseCost * slotScore
        if rarity >= 5 then
            colorCrystalCost = math.max(1, colorCrystalCost * 5)
            crystalFragmentCost = math.max(1, crystalFragmentCost)
            metaCellCost = math.max(1, metaCellCost * 15)
        end
        return colorCrystalTag, colorCrystalCost, 'CrystalPiece', crystalFragmentCost, 'MetaCell', metaCellCost
    end
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

getSlotScore = function(itemSlot)
    local slotScore = 1
    if itemSlot == Overall or itemSlot == Bludgeon or itemSlot == Longsword or itemSlot == Scepter or itemSlot == Greatsword or itemSlot == Bow or
        itemSlot == Staff or itemSlot == Cannon or itemSlot == Blade or itemSlot == Knuckle or itemSlot == Orb then
        slotScore = 2
    end
    return slotScore
end

function getItemRemakeUsecrystalTag(itemSlot)
    local crystalTypeTag = ''
    local isWeapon = false
    local isArmor = false
    local isAccessory = false
    isWeapon = isItemRemakeWeapon(itemSlot)
    if isWeapon == true then
        crystalTypeTag = 'RedCrystal'
    else
        isArmor = isItemRemakeArmor(itemSlot)
        if isArmor == true then
            crystalTypeTag = 'BlueCrystal'
        else
            isAccessory = isItemRemakeAccessory(itemSlot)
            if isAccessory == true then
                crystalTypeTag = 'GreenCrystal'
            end
        end
    end
    return crystalTypeTag
end

function isItemRemakeWeapon(itemSlot)
    local isWeapon = false
    if itemSlot >= 30 and itemSlot <= 56 then
        isWeapon = true
        return isWeapon
    end
    return isWeapon
end

function isItemRemakeArmor(itemSlot)
    local isArmor = false
    if itemSlot == Overall or itemSlot == Clothes or itemSlot == Pants or itemSlot == Hat or itemSlot == Gloves or itemSlot == Shoes then
        isArmor = true
        return isArmor
    end
    return isArmor
end

function isItemRemakeAccessory(itemSlot)
    local isAccessory = false
    if itemSlot == Necklace or itemSlot == Ring or itemSlot == Belt or itemSlot == Cape or itemSlot == Earring then
        isAccessory = true
        return isAccessory
    end
    return isAccessory
end