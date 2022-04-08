function getFirstScriptId()
    if PlayerHelper.HasQuestStarted(91000022) then
        return 70
    elseif PlayerHelper.HasQuestStarted(80000614) then
        return 80
    end
    return 60
end
