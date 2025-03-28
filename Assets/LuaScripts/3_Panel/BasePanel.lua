BasePanel = BaseClass("BasePanel")

-- 初始化函数
function BasePanel:__init()
    self.uiRoot=nil
    self.haveLoaded=false
    self.isDoneLoading=false
    self.controlPanel=nil
    self.transform=nil
    self.eventListeners={}
end

-- 打开界面
function BasePanel:Load(name,uiRoot)
    self.haveLoaded=true
    self.uiRoot=uiRoot

    --加载界面
    self:AsyncLoad(name)
end

-- 加载界面
function BasePanel:AsyncLoad(name)
    --加载预制件
    CS.AsyncMgr.Instance:LoadAsync(name,function(res)    
        return self:__callBack(res)
    end)
end

--回调
function BasePanel:__callBack(res)
    self.controlPanel=GameObject.Instantiate(res,self.uiRoot)
    self.transform=self.controlPanel.transform
    self.controlPanel:SetActive(true)
    self.isDoneLoading=true
end

--打开界面
function BasePanel:OpenPanel(name,uiRoot)
    --既没加载过也没加载完
    if self.haveLoaded == false and self.isDoneLoading == false then
        --加载
        self:Load(name,uiRoot)
        return
    --加载了但没加载完 什么都不做
    elseif self.haveLoaded == true and self.isDoneLoading == false then
        return
    --加载过也加载完了
    else
        self.controlPanel:SetActive(true)
    end
end

--关闭界面
function BasePanel:ClosePanel()
    if self.isDoneLoading == false then
        return
    else
        self.controlPanel:SetActive(false)
    end
end

-- 注册事件监听器
function BasePanel:AddEventListener(event, callback)
    self.eventListeners[event] = callback
end

-- 触发事件
function BasePanel:__triggerEvent(event, data)
    local callback = self.eventListeners[event]
    if callback then
        callback(data)
    end
end