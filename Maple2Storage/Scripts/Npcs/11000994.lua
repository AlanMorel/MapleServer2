-- Lotachi
function meetsJobScriptRequirement()
    if ScriptManager.GetPlayer().HasQuestStarted(10001840) then
        return true
    end
end
