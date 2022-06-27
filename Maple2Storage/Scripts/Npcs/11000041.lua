function getFirstScriptId()
    local jobId = ScriptManager.GetPlayer().JobId
    if jobId == 10 then
        return 30
    elseif jobId == 20 then
        return 20
    end
    return jobId + 10
end

function meetsJobScriptRequirement()
    return ScriptManager.GetPlayer().JobId == 1 and ScriptManager.GetPlayer().Level >= 10
end