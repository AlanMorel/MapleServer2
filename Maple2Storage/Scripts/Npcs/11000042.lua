function getFirstScriptId()
    if Helper.GetPlayerJobId() == 1 then
        return 10
    elseif Helper.GetPlayerJobId() == 10 then
        return 30
    elseif Helper.GetPlayerJobId() == 20 then
        return 40
    elseif Helper.GetPlayerJobId() == 30 then
        return 20
    elseif Helper.GetPlayerJobId() >= 40 then
        return Helper.GetPlayerJobId() + 10;
    end
    return -1
end
