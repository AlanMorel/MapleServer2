function getFirstScriptId()
    if PlayerHelper.HasQuestStarted(91000020) then
        return 50
    end
    return -1
end
