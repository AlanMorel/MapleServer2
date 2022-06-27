function getFirstScriptId()
    local x = math.random()
    if x >= 0.66 then
        return 50
    elseif x > 0.33 and x < 0.66 then
        return 40
    end
    return 30
end
