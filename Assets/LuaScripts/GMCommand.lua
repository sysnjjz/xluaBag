GMCommand = BaseClass("GMCommand")
-- 初始化函数
function GMCommand:__init()

end

function GMCommand.clearOwnHeroInfo()
    LocalModel:Instance():ClearOwnHero()
    LocalModel:Instance():ClearDeployHero()
end

function GMCommand.clearDeployHeroInfo()
    LocalModel:Instance():ClearDeployHero()
end

return GMCommand

