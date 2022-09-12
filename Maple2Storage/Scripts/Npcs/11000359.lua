function getFirstScriptId()
    local x = ScriptManager.GetPlayer().GetItemCount(30000098);
    if ScriptManager.GetPlayer().HasQuestStarted(20001050) and x > 0 then
        return 30
    end
    return 20
end
