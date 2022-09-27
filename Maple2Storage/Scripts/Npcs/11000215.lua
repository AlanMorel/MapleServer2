-- Evan
local nextScriptIds = { [21] = true, [31] = true, [41] = true, [51] = true }
local posterGiveFunctions = { [1] = true, [2] = true, [3] = true, [4] = true }

function getFirstScriptId()
    local player = ScriptManager.GetPlayer()

    --poster quest 1
    if player.HasQuestStarted(10001222) then
        return 20
    end
    --poster quest 2
    if player.HasQuestStarted(10001223) then
        return 30
    end
    --poster quest 3
    if player.HasQuestStarted(10001224) then
        return 40
    end
    --poster quest 4
    if player.HasQuestStarted(10001225) then
        return 50
    end
    --Misdirection
    if player.HasQuestStarted(40002676) then
        return 80
    end
    return 60
end

function handleGoto(nextScript)
    local posterCount = ScriptManager.GetPlayer().GetItemCount(30000038);
    local x = nextScript

    if nextScriptIds[nextScript] and posterCount >= 1 then
        return x + 1 --already has item
    else
        --TO DO: a function for checking whether inventory tab is full is needed! 
        --nextScript will be x + 2 if inventory is full
        return x --giving item
    end
end

function preTalkActions(functionId)
    if posterGiveFunctions[functionId] then
        return 5
    elseif functionId == 5 then
        return 99
    end
end

function actionItemReward()
    return 30000038, 1, 1
end

function actionMoveMap()
    return 2000208, 1 -- Humblis' Hideout, portal id 1
end
