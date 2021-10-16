function getFirstScriptId()
    local jobId = Helper.GetPlayerJobId()
    if jobId == 1 then
        return 10
    elseif jobId == 10 then
        return 30
    elseif jobId == 20 then
        return 40
    elseif jobId == 30 then
        return 20
    elseif jobId >= 40 then
        return Helper.GetPlayerJobId() + 10;
    end
    return -1
end
