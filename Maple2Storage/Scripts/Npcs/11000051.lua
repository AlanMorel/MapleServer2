function getFirstScriptId()
    local jobId = ScriptManager.GetPlayer().JobId
    if jobId == 1 then
        return 10
    elseif jobId == 80 then
        return 20
    elseif jobId < 80 then
        return jobId + 20
    end
    return jobId + 10
end
