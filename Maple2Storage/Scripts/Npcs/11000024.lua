function getFirstScriptId()
    if PlayerHelper.HasQuestStarted(91000020) then
        return 40
    end
    return -1
end
