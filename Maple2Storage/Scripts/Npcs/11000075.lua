function getFirstScriptId()
    --[[In GMS2, the original behavior of this NPC was that it randomly picked between scripts 10 and 20, and seen in the commented code below.
    math.randomseed(os.time())
    return (math.random() > 0.5 and 10 or 20)
    However, script 10 directly references the old pre-restart and, therefore, harms the game's lore, worldbuilding and UX.
    Due to these issues, I have personally decided to change the NPC's behavior, even if it means contradicting the original game's.
    If you'd rather use the original NPC's script, please uncomment the code above and comment the code below.]]
    return 20
end
