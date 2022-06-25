function getFirstScriptId()
    local jobId = ScriptManager.GetPlayer().JobId
    return jobId + 10
end

function meetsJobScriptRequirement()
    return ScriptManager.GetPlayer().JobId == 1 and ScriptManager.GetPlayer().Level >= 10
end