MainController = BaseClass("MainController")
--单例
function MainController:Instance(name)  
    if self.instance == nil then  
        self.instance = self:New(name)  
    end  
    return self.instance  
end  

-- 初始化函数
function MainController:__init(name)
    --初始化view和module
    self.view=MainView:New(name)
    self.model=MainModel:New()

    --设置监听器
    self:__setupEventListeners()
end

--页面刷新逻辑
function MainController:RefreshUI()

end

-- 设置监听器
function MainController:__setupEventListeners()
    --界面
    self.view:AddEventListener("openCard", function()
        self:__openCard()
    end)
    self.view:AddEventListener("exit", function()
        self:__exit()
    end)
    self.view:AddEventListener("openBag", function()
        self:__openBag()
    end)
end

--按键方法：打开抽卡界面
function MainController:__openCard()
    UIManager:Instance():OpenUI("Card")
end

--按键方法：打开背包界面
function MainController:__openBag()
    UIManager:Instance():OpenUI("Bag")
end

--按键方法：退出游戏
function MainController:__exit()
    LuaBridge.ExitApplication()
end

--控制UI
function MainController:ShowView()
    self.view:OpenPanel()
end

--控制UI
function MainController:CloseView()
    self.view:ClosePanel()
end