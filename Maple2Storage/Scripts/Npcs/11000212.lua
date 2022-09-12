function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(10001171) then
        return 40
    end
    return 20
end
