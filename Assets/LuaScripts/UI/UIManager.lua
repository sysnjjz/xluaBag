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
    --页面控制器缓存字典
    self.controllerDict={}
    --已打开页面列表 只存名字
    self.panelList={}
    --打开中的界面栈
    self.panelStack=Stack:Create()
end

-- 打开界面
--打开界面方法
function UIManager:OpenUI(name)
    -- 已打开的界面不打开
    if TableUtil.ContainKeys(self.panelList,name) then
        return
    end
    -- 先找缓存 没有再加载
    if TableUtil.ContainKeys(self.controllerDict,name) then
        self.controllerDict[name]:ShowView()
    else
        self.controllerDict[name]=UIConfigDic[name].controller:New(name)
    end
    --刷新界面
    self.controllerDict[name]:RefreshUI()
    --下层界面不可见
    if self.panelStack:Peek()~=nil then
        self.panelStack:Peek():CloseView()
    end
    --压栈 加入已打开列表
    self.panelStack:Push(self.controllerDict[name])
    table.insert(self.panelList,name)
end

--关闭界面
function UIManager:CloseUI()
    -- 没有打开的界面就返回
    if self.panelStack:Count() == 1 then
        return
    else
        --关闭栈顶元素
        self.panelStack:Peek():CloseView()
        --退出已打开列表 出栈
        TableUtil.RemoveValue(self.panelList,self.panelStack:Peek().name)
        self.panelStack:Pop()

        --下方元素可见
        if self.panelStack:Count() ~=0 then
            self.panelStack:Peek():ShowView()
        end
    end
    
end