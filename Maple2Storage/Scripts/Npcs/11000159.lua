function getFirstScriptId()
    --on vanilla the npc randomizes between the two scripts. broken behavior?
    if ScriptManager.GetPlayer().HasQuestStarted(40001050) then 
        return 30
    end
    return 20
end
