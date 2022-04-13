function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(91000022) then
        return 70
    elseif ScriptManager.GetPlayer().HasQuestStarted(80000614) then
        return 80
    end
    return 60
end
