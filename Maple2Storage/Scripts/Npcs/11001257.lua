function meetsJobScriptRequirement()
    if ScriptManager.GetPlayer().HasQuestStarted(90000419) then
        return true;
    end
    return false
end
