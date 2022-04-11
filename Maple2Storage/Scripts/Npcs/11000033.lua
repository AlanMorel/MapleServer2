function getFirstScriptId()
    if ScriptManager.GetPlayer().HasQuestStarted({
        50001573, 50001580, 50001581, 50001582, 50001583, 50001584, 50001603, 50001604, 50001665, 50001669
    }) and ScriptManager.GetPlayer().MapId == 02000023 then
        return 40
    end
    return 50
end
