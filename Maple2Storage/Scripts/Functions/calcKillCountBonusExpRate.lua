function calcKillCountBonusExpRate(killCount)
    if killCount > 100 then
        return 0.15
    elseif killCount > 50 then
        return 0.1
    elseif killCount > 40 then
        return 0.07
    elseif killCount > 30 then
        return 0.04
    elseif killCount > 20 then
        return 0.01
    end
    return 0
end
