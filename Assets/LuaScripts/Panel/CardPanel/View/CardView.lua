local CardDetailView=require("Panel.CardPanel.View.CardDetailView")

local CardView = BaseClass("CardView")
-- 初始化函数
function CardView:__init(basePanel)
    --基本属性
    self.controlPanel=basePanel.controlPanel
    self.transform=basePanel.transform
    self.eventListeners={}

    --获取组件
    self.closeBtn = self.transform:Find("Panel/CloseButton"):GetComponent("Button")
    self.oneBtn = self.transform:Find("Panel/One"):GetComponent("Button")
    self.tenBtn = self.transform:Find("Panel/Ten"):GetComponent("Button")
    self.cardList = self.transform:Find("Panel/Card")

    --注册监听器
    self.closeBtn.onClick:AddListener(function()
        self:__triggerEvent("closePanel")
    end)
    self.oneBtn.onClick:AddListener(function()
        self:__triggerEvent("oneCard")
    end)
    self.tenBtn.onClick:AddListener(function()
        self:__triggerEvent("tenCard")
    end)

    --加载资源
    self.cardDetail = Resources.Load("UI/Image");
end

-- 注册事件监听器
function CardView:AddEventListener(event, callback)
    self.eventListeners[event] = callback
end

-- 触发事件
function CardView:__triggerEvent(event, data)
    local callback = self.eventListeners[event]
    if callback then
        callback(data)
    end
end

--清除卡牌
function CardView:ClearCard()
    --清除原有的卡
    for i=0,self.cardList.childCount-1 do
        Object.Destroy(self.cardList:GetChild(i).gameObject)
    end
end

--一张卡显示逻辑
function CardView:ShowOneCard(heroData)
    --销毁原有的
    self:ClearCard()
    --抽张新的
    local newCard= CardDetailView:New(self.cardDetail,self.cardList)
    newCard:Refresh(heroData)
end

--十张卡显示逻辑
function CardView:ShowTenCard(heroList)
    --销毁原有的
    self:ClearCard()
    --抽张新的--对象池修改范围
    for k,v in ipairs(heroList) do
        --抽张新的
        local newCard= CardDetailView:New(self.cardDetail,self.cardList)
        newCard:Refresh(v)
    end

end

return CardView