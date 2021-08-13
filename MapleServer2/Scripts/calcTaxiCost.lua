function calcTaxiCost(maps, level)
    local cost = 0

    if level <= 24 then
        cost = 0.35307 * level ^ 2 + -1.4401 * level + 34.075
    else
        cost = 0.23451 * (level - 24) ^ 2 + 24.221 * (level - 24) + 265.66
    end

    return math.floor((cost) * maps / 2 + 0.5)
end
