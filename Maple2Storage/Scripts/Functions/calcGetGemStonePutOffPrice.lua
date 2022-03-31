function calcGetGemStonePutOffPrice(gemStoneLevel, itemTab)
    local ingredientTag = 'CrystalPiece'
    local lumiStoneCost = 0
    local gemStoneCost = 0
    if itemTab > 0 then
        return ingredientTag, lumiStoneCost
    end
    gemStoneCost = 8 + gemStoneLevel * 2
    return ingredientTag, gemStoneCost
end