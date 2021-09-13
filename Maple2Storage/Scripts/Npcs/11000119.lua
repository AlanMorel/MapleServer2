function getFirstScriptId()
    math.randomseed(os.time())
    return (math.random() > 0.5 and 40 or 120)
end
