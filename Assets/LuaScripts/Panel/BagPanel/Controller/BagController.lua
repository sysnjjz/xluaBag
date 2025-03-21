local BagView=require("Panel.BagPanel.View.BagView")
local HeroView=require("Panel.BagPanel.View.HeroView")
local DeployHeroView=require("Panel.BagPanel.View.DeployHeroView")
local BagModel=require("Panel.BagPanel.Model.BagModel")
local util=require("util")

local BagController = BaseClass("BagController")
--单例
function BagController:Instance(basePanel)  
    if self.instance == nil then  
        self.instance = self:New(basePanel)  
    end  
    return self.instance  
end  

-- 初始化函数
function BagController:__init(basePanel)
    -- view model
    self.view=BagView:New(basePanel)
    self.bagModel=BagModel:New()
    
    --加载预制件子物体
    self.heroUIItemPrefab=Resources.Load("UI/HeroDetail")
    self.heroDeployItem=Resources.Load("UI/DeployHero")

    --已激活对象字典
    self.uidAndPanelDict={}

    --监听器
    self:__setupEventListeners()
    --生成上阵英雄按钮添加到页面里
    self:__addDeployHero()
    --对象池
    self.heroPool=ObjectPool:New(HeroView,12,self.heroUIItemPrefab,self.view.uiHeroContent.content)
end

------------------------------------------------初始化设置--------------------------------------------------
-- 设置事件监听器
function BagController:__setupEventListeners()
    --model 
    --当选择展示的英雄修改时 刷新展示英雄界面
    self.bagModel:AddEventListener("onChangeUid",function()
        self:__onChangeUid()
    end)
    --当选择的按键改变时 刷新展示英雄界面 并选中按钮高光
    self.bagModel:AddEventListener("onChangeBid",function(data)
        self:__onChangeBid(data)
    end)

    --view
    --关闭界面
    self.view:AddEventListener("closePanel",function()
        self:__closePanel()
    end)
    --背包栏显示不同属性的英雄
    self.view:AddEventListener("changeShowHero",function(heroType)
        self:__refreshHero(heroType)
    end)
    --点击上阵按钮 刷新上阵英雄栏展示
    self.view:AddEventListener("updateDeployHero",function()
        self:__updateDeployHero()
    end)
end

--生成子控件：五个上阵英雄按钮 并注册回调
function BagController:__addDeployHero()
    for i=1,5 do
        local deployHeroView=DeployHeroView:New(GameObject.Instantiate(self.heroDeployItem,self.view.uiDeployList),i)
        deployHeroView:AddEventListener("changeBid",function(newButtonID)
            self:__changeBid(newButtonID)
        end)
        self.view:AddDeployHero(deployHeroView)
    end
end

--生成子控件：英雄背包中的英雄卡片 并注册回调
function BagController:__createHeroWindow(uid,heroData)
    local heroCell=self.heroPool:GetObject(self.heroUIItemPrefab,self.view.uiHeroContent.content)
    heroCell:AddEventListener("changeUid",function(newUID)
        self:__changeUid(newUID)
    end)
    heroCell:AddEventListener("changeBid",function(newBID)
        self:__changeBid(newBID)
    end)
    heroCell:Refresh(uid,heroData)
end
-----------------------------------------------初始化设置结束-------------------------------------------------

------------------------------------------------页面刷新逻辑-------------------------------------------------
--页面刷新逻辑 每次生成界面时调用
function BagController:InitUI()
    --设置当前选择展示的英雄的uid 默认展示第一位已上阵英雄 没有第一位已上阵英雄就展示用户背包中权重最高的英雄
    if ContainKeys(LocalModel:Instance():LoadDeployHeroData(),1) and LocalModel:Instance():LoadDeployHeroData()[1]~=nil then
        self.bagModel:SetUID(LocalModel:Instance():LoadDeployHeroData()[1].uid)
        self.bagModel:SetBID(1)
    elseif Lens(LocalModel:Instance():GetSortedHeroLocalData()) ~= 0 then
        self.bagModel:SetUID(LocalModel:Instance():GetSortedHeroLocalData()[1].uid)
    end

    -- 获取 Unity 的 MonoBehaviour 实例
    local monoInstance = CS.UnityEngine.Object.FindObjectOfType(typeof(CS.UnityEngine.MonoBehaviour))
    -- 要加载的资源字典
    self.resourceDic = {
        ["deploy hero"] = function() self:__refreshDeployHero() end,
        ["hero bag"] = function() self:__initHero() end,
        ["show hero"] = function() self:__refreshShowHero() end,
        ["Dict"] = function() self:__initialDict() end
    }
    monoInstance:StartCoroutine(util.cs_generator(self.resourceDic["deploy hero"]))
    monoInstance:StartCoroutine(util.cs_generator(self.resourceDic["hero bag"]))
    monoInstance:StartCoroutine(util.cs_generator(self.resourceDic["show hero"]))
    monoInstance:StartCoroutine(util.cs_generator(self.resourceDic["Dict"]))
end

--初始化上阵英雄栏
function BagController:__refreshDeployHero()
    --上阵英雄不为空
    local dic=LocalModel:Instance():LoadDeployHeroData()
    if Lens(dic)~=0 then
        for i=1,5 do
            --如果该位置上有上阵英雄 刷新视图
            if ContainKeys(dic,i) then
                local localData=dic[i]
                if localData~=nil then
                    local heroData=HeroModel:Instance():GetHeroByID(localData.id)
                    self.view.uiDeployBtnArr[i]:Refresh(localData,heroData)  
                end
            else
                --如果没有上阵英雄 恢复默认图像
                self.view.uiDeployBtnArr[i]:RefreshNull()                
            end
        end
    else
        --恢复默认图像
        for i=1,5 do
            self.view.uiDeployBtnArr[i]:RefreshNull()
        end
    end
end

--初始化显示英雄背包
function BagController:__initHero()
    --清除原有的
    local scroll=self.view.uiHeroContent.content
    for i=0,scroll.childCount-1 do
        self.heroPool:ReturnObject(scroll:GetChild(i))
    end

    --没数据就返回
    if LocalModel:Instance():GetSortedHeroLocalData()==nil then
        return
    end

    --显示所有英雄
    for k,v in pairs(LocalModel:Instance():GetSortedHeroLocalData()) do
        local heroData=HeroModel:Instance():GetHeroByID(v.id)
        self:__createHeroWindow(v.uid,heroData)
    end
end

--将已激活对象和其uid存起来
function BagController:__initialDict()
    --做已激活对象和其uid的映射表
    local scroll=self.view.uiHeroContent.content
    --对于获取的每一个英雄 查表
    for i=0,scroll.childCount-1 do
        for k,v in ipairs(self.heroPool.activeObjList) do
            --如果是激活的表 将其的uid存在字典里
            if v.transform == scroll:GetChild(i) then
                self.uidAndPanelDict[v.uid]=v
            end
        end
    end
end
----------------------------------------------页面刷新逻辑结束---------------------------------------------------

-------------------------------------------------响应事件-------------------------------------------------
--1.改变选中按钮回调
function BagController:__changeUid(newUID)
    self.bagModel:SetUID(newUID)
end
function BagController:__changeBid(newBID)
    self.bagModel:SetBID(newBID)
end

--2.选择的按键改变时回调
function BagController:__onChangeUid()
    --点击改变详细展示英雄
    self:__refreshShowHero()
end
function BagController:__onChangeBid(data)
    --设置当前选中英雄uid
    if ContainKeys(LocalModel:Instance():LoadDeployHeroData(),data.newBid) then
        self.bagModel:SetUID(LocalModel:Instance():LoadDeployHeroData()[data.newBid].uid)
    end
    --选中按钮高光
    self.view:ChangeLightingButton(data.oldBid, data.newBid)
end

--3.关闭界面
function BagController:__closePanel()
    self:__closeInit()
    UIManager:Instance():CloseUI(self.view.controlPanel.name)
end

--4.刷新回调
--点击上阵按键：添加上阵英雄
function BagController:__updateDeployHero()
    --获得当前选择英雄信息
    local localData=LocalModel:Instance():GetLocalItemDataByUid(self.bagModel:GetUID())
    if localData ~=nil and localData.isDeploy~=true then
        --获得英雄信息 添加到上阵队列并刷新视图   
        local heroData=HeroModel:Instance():GetHeroByID(localData.id)
        local place = LocalModel:Instance():AddDeployHero(localData,heroData)
        self.view.uiDeployBtnArr[place]:Refresh(localData,heroData)
        self.bagModel:SetBID(place)
    end
end

--点击英雄卡片：刷新展示页
function BagController:__refreshShowHero()
    --找到对应的动态数据
    local localData=LocalModel:Instance():GetLocalItemDataByUid(self.bagModel:GetUID())
    --为空恢复默认设计
    if localData==nil then
        self.view:ClearHeroDetail()
        return
    end
    --刷新展示页
    self.view:RefreashHeroDetail(localData,HeroModel:Instance():GetHeroByID(localData.id))
end

---点击背包下方英雄分类键：刷新显示英雄类型
function BagController:__refreshHero(heroType)
    --因为所有已激活的组件都在字典里所以直接对字典做操作
    for k,v in pairs(self.uidAndPanelDict) do 
        --如果是全显示 不做判断直接显示
        if heroType==HeroType.All then
            v.controlPanel:SetActive(true)
        else
            --不是全显示 判断后再显示
            local type=HeroModel:Instance():GetHeroByID(LocalModel:Instance():GetLocalItemDataByUid(k).id).type
            if type~=heroType then
                v.controlPanel:SetActive(false)
            else
                v.controlPanel:SetActive(true)
            end
        end
        
    end
end
------------------------------------------------响应事件结束------------------------------------------------
--关闭时还原数据逻辑
function BagController:__closeInit()
    self.bagModel:SetBID(0)
    self.uid ={}
end

return BagController