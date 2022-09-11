function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(20001061) then
        return (math.random() > 0.5 and 41 or 40)
    end
    return 30
end
