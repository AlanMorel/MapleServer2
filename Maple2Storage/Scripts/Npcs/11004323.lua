function getFirstScriptId()
    if Helper.HasQuestStarted(91000690) then
        return 10
    end
    return 40
end