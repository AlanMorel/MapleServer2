-- Rosetta
function meetsJobScriptRequirement()
    return true; -- does not have any requirement
end

function postTalkActions()
    return 3,4
end

function actionWindow()
    return "BeautyShopDialog", "hair,style"
end

function actionPortal()
    return 99
end