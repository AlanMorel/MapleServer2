function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(91000022) then
        return 70
    end
    return (math.random() > 0.7 and 40 or 120)
end
