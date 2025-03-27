MainView = BaseClass("MainView",BasePanel)
-- 初始化函数
function MainView:__init()
    self.uiRoot=nil
    self.haveLoaded=false
    self.isDoneLoading=false
    self.controlPanel=nil
    self.transform=nil
    -- 事件监听
    self.eventListeners={}
end

--重写回调
function MainView:__callBack(res)
    -- 更改属性
    self.controlPanel=GameObject.Instantiate(res,self.uiRoot)
    self.transform=self.controlPanel.transform
    self.controlPanel:SetActive(true)
    self.isDoneLoading=true

    -- 获取组件
    self.btmCard=self.transform:Find("Card"):GetComponent("Button")
    self.btmExit=self.transform:Find("Exit"):GetComponent("Button")
    self.btmBag=self.transform:Find("Bag"):GetComponent("Button")

    -- 绑定事件
    self.btmCard.onClick:AddListener(function()
        self:__triggerEvent("openCard")
    end)
    self.btmExit.onClick:AddListener(function()
        self:__triggerEvent("exit")
    end)
    self.btmBag.onClick:AddListener(function()
        self:__triggerEvent("openBag")
    end)
    
    self.isDoneLoading=true
    if self.OnViewLoaded then
        self.OnViewLoaded()
    end

    return true
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