function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted(91000061) then
        return 80
    end
    return 90
end

function postTalkActions()
    return 99
end

function actionMoveMap()
    return 2000062, 13 -- Lith Harbor, portal 13
end
