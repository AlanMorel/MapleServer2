function getFirstScriptId()
    if PlayerHelper.HasQuestStarted(91000720) then
        return 30
    end
    return -1
end
