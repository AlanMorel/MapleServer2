function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(10001861) then
        return 30
    end
    return 20
end
