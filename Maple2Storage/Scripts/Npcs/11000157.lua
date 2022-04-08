function getFirstScriptId()
    if PlayerHelper.HasQuestStarted(91000061) then
        return 30
    end
    return -1
end
