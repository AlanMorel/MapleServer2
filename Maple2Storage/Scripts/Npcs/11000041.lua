function getFirstScriptId()
    local jobId = Helper.GetPlayerJobId()
    if jobId == 1 then
        return 10
    elseif jobId == 10 then
        return 30
    elseif jobId == 20 then
        return 20
    end
    return jobId + 10
end
