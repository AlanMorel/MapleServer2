function getFirstScriptId()
    local jobId = PlayerHelper.GetJobId()
    if jobId == 1 then
        return 10
    elseif jobId == 50 then
        return 20
    elseif jobId < 50 then
        return jobId + 20
    end
    return jobId + 10
end

