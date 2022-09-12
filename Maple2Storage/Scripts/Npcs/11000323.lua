function getFirstScriptId()
    local x = math.random()
    if x >= 0.75 then
        return 40
    elseif x >= 0.5 and x < 0.75 then
        return 50
    elseif x > 0.25 and x < 0.5 then
        return 60
    end
    return 70
end

