function getFirstScriptId()
    if Helper.HasQuestStarted(91000022) then return 70 end
    math.randomseed(os.time())
    return (math.random() > 0.7 and 40 or 120)
end