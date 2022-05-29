-- Mirror
function meetsJobScriptRequirement()
    return true; -- does not have any requirement
end

function preTalkActions()
    return 3,4
end

function actionWindow()
    return "BeautyShopDialog", "mirror"
end

function actionPortal()
    return 99
end
