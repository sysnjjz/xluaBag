require("Tools.Gtable")

--数据初始化
HeroModel:Instance():LoadHeroList()
LocalModel:Instance():PreLoadHeroData()
LocalModel:Instance():PreLoadDeployHeroData()

--打开主界面
UIManager:Instance():OpenUI("Main")
