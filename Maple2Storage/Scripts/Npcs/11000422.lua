function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(10001804) then
        return 20
    end
    return 10
end
