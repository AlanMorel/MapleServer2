function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(10001662) then
        return 40
    end
    return 30
end
