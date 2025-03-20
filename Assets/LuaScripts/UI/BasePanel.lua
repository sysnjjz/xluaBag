local BasePanel = BaseClass("BasePanel")

-- 初始化函数
function BasePanel:__init(params)
    self.name=nil
    self.isRemove=true
    self.controlPanel=nil
    self.transform=nil
end

-- 打开界面
function BasePanel:Load(name,path,uiRoot)
    --加载
    local prefab=Resources.Load(path)
    --实例化并缓存
    self.controlPanel=GameObject.Instantiate(prefab,uiRoot)
    self.name=name
    self.transform=self.controlPanel.transform
    self.controlPanel:SetActive(false)
end

--打开界面
function BasePanel:OpenPanel()
    self.isRemove=false
    self.controlPanel:SetActive(true)
end

--关闭界面
function BasePanel:ClosePanel()
    self.isRemove=true
    self.controlPanel:SetActive(false)
end

return BasePanel