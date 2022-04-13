function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(91000021) then
        return 30
    end
    return -1
end
