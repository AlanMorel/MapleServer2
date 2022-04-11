function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(91000021) then
        return 60
    end
    return 50
end
