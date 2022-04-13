function getFirstScriptId()
    if ScriptManager.GetPlayer().GetItemCount(30000435) > 0 then
        return 40
    end
    return -1
end
