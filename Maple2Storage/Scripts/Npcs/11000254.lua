-- Jane
function handleGoto(nextScript)
    local itemCount = ScriptManager.GetPlayer().GetItemCount(39000014);
    if nextScript == 61 and itemCount >= 1 then
        return 62
    else
        return 61
    end
end

function preTalkActions(functionId)
    if functionId == 1 then
        return 5
    end
end

function actionItemReward()
    return 39000014, 1, 1
end
