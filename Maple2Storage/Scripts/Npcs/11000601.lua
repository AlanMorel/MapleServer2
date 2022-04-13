function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(91000022) then
        return 40
    end
    return -1
end
