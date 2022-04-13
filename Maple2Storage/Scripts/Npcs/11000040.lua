function getFirstScriptId()
    local jobId = ScriptManager.GetPlayer().JobId
    if jobId == 1 then
        return 10
    end
    return jobId + 10
end
