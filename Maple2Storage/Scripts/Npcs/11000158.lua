function getFirstScriptId()
    if PlayerHelper.HasQuestStarted(91000061) then
        return 40
    end
    math.randomseed(os.time())
    return (math.random() > 0.5 and 20 or 30)
end
