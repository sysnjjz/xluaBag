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
    self.view=nil
    self.model=MainModel:New(self)
end

--页面刷新逻辑
function MainController:RefreshUI()

end

--控制UI
function MainController:ShowView()
    if self.view == nil then
        self.view=MainView:New()
    end
    self.view:OpenPanel("Main",UIManager:Instance().uiRoot)
end

--控制UI
function MainController:CloseView()
    self.view:ClosePanel()
end