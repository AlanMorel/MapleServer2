function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(90000180) then
        return 20
    end
    return 10
end
