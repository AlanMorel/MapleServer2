function getFirstScriptId()
    if Helper.HasQuestStarted(91000022) then return 70 end
    if Helper.HasQuestStarted(80000614) then return 80 end
    return 60
end