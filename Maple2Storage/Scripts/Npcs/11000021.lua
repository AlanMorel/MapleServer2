function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(10001000) then
        return 40
    end
    return -1
end
