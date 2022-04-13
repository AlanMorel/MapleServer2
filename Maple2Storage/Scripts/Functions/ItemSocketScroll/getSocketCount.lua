function getSocketCount(scrollId)
    local socketCount = 1
    local twoSocketScrolls = { 10000002, 10000012, 10000014, 10000023, 10000025, 10000027, 10000029, 10000032, 10000034, 10000036 }
    if tableHasValue(twoSocketScrolls, scrollId) then
        socketCount = 2
    elseif scrollId == 10000003 then
        socketCount = 3
    end
    return socketCount
end

function tableHasValue(tab, val)
    for index, value in ipairs(tab) do
        if value == val then
            return true
        end
    end

    return false
end