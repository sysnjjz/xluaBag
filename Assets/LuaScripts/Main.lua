require("UI.UIManager")
require("Data.HeroModel")
require("Data.LocalModel")
require("GMCommand")
require("Tools.ObjectPool")

--数据初始化
HeroModel:Instance():LoadHeroList()
LocalModel:Instance():PreLoadHeroData()
LocalModel:Instance():PreLoadDeployHeroData()

--打开主界面
UIManager:Instance()
