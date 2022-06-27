function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(91000022) then
        return 40
    end
    return (math.random() > 0.7 and 20 or 10)
end
