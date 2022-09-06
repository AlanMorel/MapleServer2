-- Mirror
function meetsJobScriptRequirement()
    return true; -- does not have any requirement
end

function preTalkActions(functionId)
    if functionId == 1 then
        return 3,4
    end
end

function actionWindow()
    return "BeautyShopDialog", "mirror"
end

function actionPortal()
    return 99
end
