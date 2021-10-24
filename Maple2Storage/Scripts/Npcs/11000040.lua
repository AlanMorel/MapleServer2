function getFirstScriptId()
    local jobId = Helper.GetPlayerJobId()
    if jobId == 1 then
        return 10
    end
    return jobId + 10
end
