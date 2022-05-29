TokenItemId = 30000875;

function handleGoto(nextScript)
    local itemCount = ScriptManager.GetPlayer().GetItemCount(TokenItemId);
    if nextScript == 31 and itemCount >= 5 then
        return 31
    elseif nextScript == 10 and itemCount >= 50 then
        return 10
    elseif nextScript == 100 and itemCount >= 500 then
        return 100
    else
        return 32
    end
    return 0
end

function rouletteSpin(scriptId)
    local tokenSpinCost = 5
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

function preTalkActions()
    return 4
end

function actionWindow()
    return "RouletteDialog", "22"
end
