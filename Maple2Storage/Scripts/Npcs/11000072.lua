-- Doctor Zenko
function meetsJobScriptRequirement()
    return true; -- does not have any requirement
end

function postTalkActions()
    return 3,4
end

function actionWindow()
    return "BeautyShopDialog", "skin"
end

function actionPortal()
    return 99
end