function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(10001742) then
        return 20
    end
    return 30
end
