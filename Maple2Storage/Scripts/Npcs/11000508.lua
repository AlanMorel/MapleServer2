function getBeginEventId(itemRarity, enchantLevel)
    if enchantLevel == 0 then
        if itemRarity <= 4 then
            return 1100
        elseif itemRarity == 5 then
            return 1000
        elseif itemRarity == 6 then
            return 1001
        end
    end
    return 1198 + enchantLevel
end

function getProcessEventId(hasEnoughMaterials, hasEnoughCatalysts)
    if not hasEnoughMaterials then
        return 3
    elseif not hasEnoughCatalysts then
        return 4
    else
        return 0
    end
end

function getExcessCatalystEventId()
    return 6
end

function getResultEventId(enchantLevel, success)
    if success then
        if enchantLevel <= 4 then
            return 2000
        elseif enchantLevel >= 5 and enchantLevel <= 7 then
            return 2001
        elseif enchantLevel == 8 or enchantLevel == 9 then
            return 2002
        elseif enchantLevel == 10 then
            return 2003
        elseif enchantLevel == 11 then
            return 2004
        elseif enchantLevel == 12 then
            return 2005
        elseif enchantLevel == 13 then
            return 2006
        elseif enchantLevel == 14 then
            return 2007
        else
            return 2008
        end
    else
        if enchantLevel <= 5 then
            return 2300
        else
            return 2203
        end
    end
end