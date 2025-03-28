CardController = BaseClass("CardController")
--单例
function CardController:Instance(name)  
    if self.instance == nil then  
        self.instance = self:New(name)  
    end  
    return self.instance  
end 

-- 初始化函数
function CardController:__init(name)
    self.name=name
    
    --初始化view和module
    self.view=nil
    self.model=CardModel:New(self)
end

--页面刷新逻辑
function CardController:RefreshUI()
    if self.view.isDoneLoading == nil or self.view.isDoneLoading == false then return end
    self.view:ClearCard()
end

-- 设置事件监听器
function CardController:__setupEventListeners()
    self.view:AddEventListener("closePanel", function()
        self:__closePanel()
    end)
    self.view:AddEventListener("oneCard", function()
        self:__oneCard()
    end)
    self.view:AddEventListener("tenCard", function()
        self:__tenCard()
    end)
end

--关闭界面
function CardController:__closePanel()
    UIManager:Instance():CloseUI(self.name)
end

--抽一张卡
function CardController:__oneCard()
    self.view:ShowOneCard(self.model:OneCard())
end

--抽十张卡
function CardController:__tenCard()
    self.view:ShowTenCard(self.model:TenCard())
end

--控制UI
function CardController:ShowView()
    if self.view == nil then
        self.view=CardView:New()
        self.view.OnViewLoaded=function()            
            --监听器
            self:__setupEventListeners()
        end
    end
    self.view:OpenPanel("Card",UIManager:Instance().uiRoot)
end

--控制UI
function CardController:CloseView()
    self.view:ClosePanel()
end