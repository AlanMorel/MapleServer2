function handleGoto(nextScript)
    if nextScript == 61 then
        jobId = Helper.GetPlayerJobId()

        if jobId <= 80 and jobId > 1 then
            return 60 + math.floor(jobId / 10)
        elseif jobId >= 90 then
            if jobId % 10 == 1 then
                jobId = jobId - 1
            end
            return jobId - 20
        elseif jobId == 1 then
            return 69
        end
    end

    return nextScript
end
