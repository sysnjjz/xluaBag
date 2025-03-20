local CardView=require("Panel.CardPanel.View.CardView")
local CardModel=require("Panel.CardPanel.Model.CardModel")

local CardController = BaseClass("CardController")
--单例
function CardController:Instance(basePanel)  
    if self.instance == nil then  
        self.instance = self:New(basePanel)  
    end  
    return self.instance  
end 

-- 初始化函数
function CardController:__init(basePanel)
    --初始化view和module
    self.view=CardView:New(basePanel)
    self.model=CardModel:New()
    --监听器
    self:__setupEventListeners()
end

--页面刷新逻辑
function CardController:InitUI()
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
    UIManager:Instance():CloseUI(self.view.controlPanel.name)
end

--抽一张卡
function CardController:__oneCard()
    self.view:ShowOneCard(self.model:OneCard())
end

--抽十张卡
function CardController:__tenCard()
    self.view:ShowTenCard(self.model:TenCard())
end

return CardController