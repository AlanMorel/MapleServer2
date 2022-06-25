function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(10001023) then
        return 20
    elseif ScriptManager.GetPlayer().HasQuestStarted(60100040) then
        return 50
    end
    return 30
end
