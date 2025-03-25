BasePanel = BaseClass("BasePanel")

-- 初始化函数
function BasePanel:__init(params)
    self.uiRoot=nil
    self.isDoneLoading=false
    self.controlPanel=nil
    self.transform=nil
end

-- 打开界面
function BasePanel:Load(name,uiRoot)
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
function BasePanel:OpenPanel()
    if self.isDoneLoading == false then
        return
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