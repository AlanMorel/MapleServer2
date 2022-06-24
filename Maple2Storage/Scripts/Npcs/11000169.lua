function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(10001025) then
        return 50
    end
    return 70
end
