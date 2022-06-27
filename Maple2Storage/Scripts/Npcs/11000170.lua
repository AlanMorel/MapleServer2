function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(60100020) then
        return 40
    end
    return 20
end
