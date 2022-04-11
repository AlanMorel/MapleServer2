function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(91000020) then
        return 60
    end
    return -1
end
