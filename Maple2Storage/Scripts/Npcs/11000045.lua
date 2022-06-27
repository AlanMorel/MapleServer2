function getFirstScriptId()
    local jobId = ScriptManager.GetPlayer().JobId
    if jobId == 50 then
        return 20
    elseif jobId < 50 then
        return jobId + 20
    end
    return jobId + 10
end

function meetsJobScriptRequirement()
    return ScriptManager.GetPlayer().JobId == 1 and ScriptManager.GetPlayer().Level >= 10
end
