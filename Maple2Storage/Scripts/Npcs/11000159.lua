function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(40001050) then
        return 30
    end
    return 20
end
