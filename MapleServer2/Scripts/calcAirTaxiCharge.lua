function calcAirTaxiCharge(playerLevel)
    return 30000 + math.max(playerLevel - 10, 0) * 500
end
