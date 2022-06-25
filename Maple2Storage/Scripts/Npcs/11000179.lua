function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestCompleted(10001005) then
        return 70
    end
    return 30
end
