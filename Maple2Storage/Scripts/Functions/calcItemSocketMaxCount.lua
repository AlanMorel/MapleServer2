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

-- Calculates amount of gemstone sockets an item can have
function calcItemSocketMaxCount(itemSlot, rarity, optionLevelFactor, inventoryTab)
    local socketCount = 0
    if inventoryTab > 0 then
        return socketCount
    end
    if itemSlot ~= Necklace and itemSlot ~= Ring and itemSlot ~= Earring then
        return socketCount
    end
    if optionLevelFactor < 50 then
        return socketCount
    end
    if rarity < 3 then
        return socketCount
    end
    if rarity == 3 then
        socketCount = 1
    elseif rarity >= 4 then
        socketCount = 3
    else
        socketCount = 0
    end
    return socketCount
end