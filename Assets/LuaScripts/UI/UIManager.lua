require("Tools.UIConfigDic")
local Stack = require("Tools.Stack")
local BasePanel=require("UI.BasePanel")

UIManager = BaseClass("UIManager")
-- 单例类
function UIManager:Instance()  
    if self.instance == nil then  
        self.instance = self:New()  
    end  
    return self.instance  
end  

-- 初始化函数
function UIManager:__init(params)
    ---初始化参数
    --根ui
    self.uiRoot=GameObject.Find("Canvas").transform
    --页面缓存列表
    self.uiCacheList={}
    --已打开页面列表 只存名字
    self.panelList={}
    --打开中的界面栈
    self.panelStack=Stack:Create()

    --缓存所有页面
    for key,value in pairs(UIConfigDic) do
        local panel=BasePanel:New()
        panel:Load(value.name,value.path,self.uiRoot)
        self.uiCacheList[value.name]=panel
    end

    --打开主界面    
    self:CreatePanel("Main")
end

--打开并刷新界面
function UIManager:CreatePanel(name)
    self:OpenUI(self.uiCacheList[name].name,self.uiCacheList[name].path)
    UIConfigDic[name].controller:Instance(self.uiCacheList[name]):InitUI()
end

--打开界面方法
function UIManager:OpenUI(name,path)
    local nowPanel=nil
    -- 已打开的界面不打开
    if ContainKeys(self.panelList,name) then
        return
    end
    -- 先找缓存 没有再加载
    if ContainKeys(self.uiCacheList,name) then
        nowPanel=self.uiCacheList[name]
        nowPanel:OpenPanel()
    else
        nowPanel=BasePanel:New()
        nowPanel:Load(name,path,self.uiRoot)
        self.uiCacheList[name]=nowPanel
        nowPanel:OpenPanel()
    end
    --下层界面不可见
    if self.panelStack:Peek()~=nil then
        self.panelStack:Peek().controlPanel:SetActive(false)
    end
    --压栈 加入已打开列表
    self.panelStack:Push(nowPanel)
    table.insert(self.panelList,nowPanel.name)

end

--关闭界面
function UIManager:CloseUI(name)
    -- 没有打开的界面就返回
    if self.panelStack:Count() == 1 then
        return
    else
        --要关闭的界面是第一个界面
        if self.panelStack:Peek().controlPanel.name==name then
            --关闭栈顶元素
            self.panelStack:Peek():ClosePanel()
            --退出已打开列表 出栈
            RemoveValue(self.panelList,self.panelStack:Peek().name)
            self.panelStack:Pop()

            --下方元素可见
            if self.panelStack:Count() ~=0 then
                self.panelStack:Peek().controlPanel:SetActive(true)
            end
        end
    end
    
end
