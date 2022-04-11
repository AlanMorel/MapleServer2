function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(91000700) then
        return 30
    end
    return -1
end
