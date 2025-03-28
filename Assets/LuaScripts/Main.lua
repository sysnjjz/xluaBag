-- require("Tools.Gtable")

--加载模块
require("RequireUtils")
requireModule()
-- CS.IOUtils.luaCallRequire()

--数据初始化
HeroModel:Instance():LoadHeroList()
LocalModel:Instance():LoadHeroData()
LocalModel:Instance():LoadDeployHeroData()

--打开并缓存所有controller
controllerDict={}
for k,v in pairs(UIConfigDic) do
    controllerDict[k]=v.controller:Instance(k)
end

--打开主界面
UIManager:Instance():OpenUI("Main")
