function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(91000061) then
        return 80
    end
    return 90
end
