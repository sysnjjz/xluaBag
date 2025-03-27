CardView = BaseClass("CardView",BasePanel)

--重载加载函数
function CardView:AsyncLoad(name)
    coroutine.wrap(function()
        -- 异步加载界面
        self:__waitResource(name,function(res)    
            return self:__callBack(res)
        end)

        -- 异步加载资源
        self:__waitResource("Image",function(res)    
            return self:__loadCallBack(res)
        end)
    end)()
end

function CardView:__waitResource(name,callBack)
    local co = coroutine.running() -- 获取当前协程

    -- 启动异步加载
    CS.AsyncMgr.Instance:LoadAsync(name, function(res)
        callBack(res)
        coroutine.resume(co) -- 恢复协程执行
    end)

    -- 暂停协程，直到异步加载完成
    return coroutine.yield()
end

function CardView:__callBack(res)
    self.controlPanel=GameObject.Instantiate(res,self.uiRoot)
    self.transform=self.controlPanel.transform
    self.controlPanel:SetActive(true)

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

    self.isDoneLoading=true
    if self.OnViewLoaded then
        self.OnViewLoaded()
    end
end

function CardView:__loadCallBack(res)
    self.cardDetail = res
    --对象池
    self.cardPool=ObjectPool:New(CardDetailView,10,self.cardDetail,self.cardList)
end

--清除卡牌
function CardView:ClearCard()
    --清除原有的卡
    for i=0,self.cardList.childCount-1 do
        self.cardPool:ReturnObject(self.cardList.childCount)
    end
end

--一张卡显示逻辑
function CardView:ShowOneCard(heroData)
    --销毁原有的
    self:ClearCard()
    --抽张新的
    local newCard=self.cardPool:GetObject(self.cardDetail,self.cardList)
    newCard:Refresh(heroData)
end

--十张卡显示逻辑
function CardView:ShowTenCard(heroList)
    --销毁原有的
    self:ClearCard()
    --抽张新的
    for k,v in ipairs(heroList) do
        --抽张新的
        local newCard=self.cardPool:GetObject(self.cardDetail,self.cardList)
        newCard:Refresh(v)
    end

end