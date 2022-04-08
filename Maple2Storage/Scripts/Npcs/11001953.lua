function handleGoto(nextScript)
    if nextScript == 31 and PlayerHelper.GetItemCount(30000610) >= 1 then
        return 31
    elseif nextScript == 10 and PlayerHelper.GetItemCount(30000610) >= 10 then
        return 10
    elseif nextScript == 100 and PlayerHelper.GetItemCount(30000610) >= 100 then
        return 100
    else
        return 32
    end
    return 0
end
