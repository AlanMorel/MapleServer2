-- Lotachi
function meetsJobScriptRequirement()
    return ScriptManager.GetPlayer().HasQuestStarted(10001840)
end
