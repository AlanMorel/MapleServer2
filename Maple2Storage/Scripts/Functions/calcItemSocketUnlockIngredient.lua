-- Calculates cost and ingredient type to unlock gem socket
calcItemSocketUnlockIngredient = function(rarity, socketSlot, itemTab)
    local ingredientCost = 0
    if itemTab == 0 then
        ingredientCost = (200 + (math.max(((math.min(50, socketSlot)) - 50), 0) * 20)) * math.max((rarity - 3), 1)
        return 'CrystalPiece', ingredientCost
    elseif itemTab > 0 then
        return 'SkinCrystal', 690
    end
end