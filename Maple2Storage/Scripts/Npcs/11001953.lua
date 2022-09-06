TokenItemId = 30000610;

function handleGoto(nextScript)
    local itemCount = ScriptManager.GetPlayer().GetItemCount(TokenItemId);
    if nextScript == 31 and itemCount >= 1 then
        return 31
    elseif nextScript == 10 and itemCount >= 10 then
        return 10
    elseif nextScript == 100 and itemCount >= 100 then
        return 100
    else
        return 32
    end
    return 0
end

function rouletteSpin(scriptId)
    local tokenSpinCost = 1
    local spinCount = 0
    if scriptId == 31 then
        spinCount = 1
    elseif scriptId == 10 then
        spinCount = 10
    elseif scriptId == 100 then
        spinCount = 100
    else
        spinCount = 0
    end
    return TokenItemId, spinCount, tokenSpinCost
end

function preTalkActions(functionId)
    if functionId == 1 then
        return 4
    end
end

function actionWindow()
    return "RouletteDialog", "13"
end
