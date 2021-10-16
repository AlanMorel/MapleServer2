function getFirstScriptId()
    local jobId = Helper.GetPlayerJobId()
    if jobId == 1 then return 10 end
    if jobId == 80 then return 20 end
    if jobId < 80 then return jobId + 20 end
    return jobId + 10
  end