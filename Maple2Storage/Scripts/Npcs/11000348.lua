function handleGoto(nextScript)
    if nextScript == 61 then
        jobId = ScriptManager.GetPlayer().JobId

        if jobId <= 80 and jobId > 1 then
            return 60 + jobId / 10
        elseif jobId >= 90 then
            return jobId - 20
        elseif jobId == 1 then
            return 69
        end
    end

    return nextScript
end
