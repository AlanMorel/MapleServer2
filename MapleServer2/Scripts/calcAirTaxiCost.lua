function calcAirTaxiCost(playerLevel)
    local baseFee = 30000;
    return baseFee + math.max(playerLevel - 10, 0) * 500
end
