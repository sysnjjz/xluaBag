local MainView = BaseClass("MainView",BasePanel)
-- 初始化函数
function MainView:__init(basePanel)
    --基本属性
    self.controlPanel=basePanel.controlPanel
    self.transform=basePanel.transform
    self.eventListeners={}

    --获取组件
    self.btmCard=self.transform:Find("Card"):GetComponent("Button")
    self.btmExit=self.transform:Find("Exit"):GetComponent("Button")
    self.btmBag=self.transform:Find("Bag"):GetComponent("Button")

    --注册监听器
    self.btmCard.onClick:AddListener(function()
        self:__triggerEvent("openCard")
    end)
    self.btmExit.onClick:AddListener(function()
        self:__triggerEvent("exit")
    end)
    self.btmBag.onClick:AddListener(function()
        self:__triggerEvent("openBag")
    end)
end

-- 注册事件监听器
function MainView:AddEventListener(event, callback)
    self.eventListeners[event] = callback
end

-- 触发事件
function MainView:__triggerEvent(event, data)
    local callback = self.eventListeners[event]
    if callback then
        callback(data)
    end
end

return MainView