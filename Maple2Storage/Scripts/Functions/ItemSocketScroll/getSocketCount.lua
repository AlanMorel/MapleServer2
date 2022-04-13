function getSocketCount(scrollId)
    local socketCount = 1
    if scrollId == 10000002 or scrollId == 10000012 or scrollId == 10000014 or scrollId == 10000023 or
        scrollId == 10000025 or scrollId == 10000027 or scrollId == 10000029 or scrollId == 10000032 or
        scrollId == 10000034 or scrollId == 10000036 then
        socketCount = 2
    elseif scrollId == 10000003 then
        socketCount = 3
    end
    return socketCount
end