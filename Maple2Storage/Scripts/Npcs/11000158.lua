function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(91000061) then
        return 40
    end
    return (math.random() > 0.5 and 20 or 30)
end
