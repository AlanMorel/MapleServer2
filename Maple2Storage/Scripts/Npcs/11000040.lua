function getFirstScriptId()
    local jobId = PlayerHelper.GetJobId()
    if jobId == 1 then
        return 10
    end
    return jobId + 10
end
