function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(80000613) then
        return 30
    end
    return 20
end
