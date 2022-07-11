function meetsJobScriptRequirement()
    if ScriptManager.GetPlayer().HasQuestStarted(90000418) then
        return true;
    end
    return false
end
