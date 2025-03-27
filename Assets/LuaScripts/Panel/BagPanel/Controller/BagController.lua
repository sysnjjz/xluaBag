BagController = BaseClass("BagController")
--单例
function BagController:Instance(name)  
    if self.instance == nil then  
        self.instance = self:New(name)  
    end  
    return self.instance  
end  

-- 初始化函数
function BagController:__init(name)
    self.name=name

    -- view model
    self.view=nil
    self.bagModel=BagModel:New(self)
end

--控制UI
function BagController:ShowView()
    if self.view == nil then
        self.view=BagView:New()
        --页面加载完成后加载子控件
        self.view.OnViewLoaded=function()
            --英雄背包子控件
            CS.AsyncMgr.Instance:LoadAsync("HeroDetail", function(res)
                return self:__heroCallBack(res)
            end)
    
            --上阵英雄子控件
            CS.AsyncMgr.Instance:LoadAsync("DeployHero", function(res)
                return self:__deployHeroCallBack(res)
            end)
            --英雄类型子控件
            CS.AsyncMgr.Instance:LoadAsync("TypeBtn", function(res)
                return self:__createHeroTypeBtn(res)
            end)
    
            --监听器
            self:__setupEventListeners()
            self:__refreshShowHero()
        end
    end
    self.view:OpenPanel("Bag",UIManager:Instance().uiRoot)
end

--控制UI
function BagController:CloseView()
    self.view:ClosePanel()
end

------------------------------------------------初始化设置--------------------------------------------------
-- 设置事件监听器
function BagController:__setupEventListeners()
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

--生成子控件：英雄背包子控件
function BagController:__heroCallBack(res)
    self.heroUIItemPrefab=res
    --对象池
    self.heroPool=ObjectPool:New(HeroView,12,self.heroUIItemPrefab,self.view.uiHeroContent.content)
    self:__refreshHero(HeroType.All)
end

--生成子控件：上阵英雄子控件
function BagController:__deployHeroCallBack(res)
    self.heroDeployItem=res
    --生成上阵英雄按钮添加到页面里
    for i=1,5 do
        local deployHeroView=DeployHeroView:New(GameObject.Instantiate(self.heroDeployItem,self.view.uiDeployList),i)
        deployHeroView:AddEventListener("changeBid",function(newButtonID)
            self:__changeBid(newButtonID)
        end)
        self.view:AddDeployHero(deployHeroView)
    end
    self:__initID()
    self:__refreshDeployHero()
end

--生成子控件：英雄分类视图
function BagController:__createHeroTypeBtn(res)
    self.btn=res
    --示例配置表
    local config=
    {
        [1]={name="All",heroType=HeroType.All,icon="All"},
        [2]={name="Force",heroType=HeroType.Force,icon="Force"},
        [3]={name="InnerForce",heroType=HeroType.InnerForce,icon="InnerForce"},
        [4]={name="Skill",heroType=HeroType.Skill,icon="Skill"},
        [5]={name="Sword",heroType=HeroType.Sword,icon="Sword"},
        [6]={name="Heal",heroType=HeroType.Heal,icon="Heal"}
    }
    --生成上阵英雄按钮添加到页面里
    for k,v in pairs(config) do
        local newBtn=btnView:New(GameObject.Instantiate(self.btn,self.view.uiBtnList),v.heroType,v.icon)
        newBtn:AddEventListener("changeShowHero",function(heroType)
            self:__refreshHero(heroType)
        end)
    end
end

--初始化选中id
function BagController:__initID()
    --设置当前选择展示的英雄的uid 默认展示第一位已上阵英雄 没有第一位已上阵英雄就展示用户背包中权重最高的英雄
    if TableUtil.ContainKeys(LocalModel:Instance():LoadDeployHeroData(),1) and LocalModel:Instance():LoadDeployHeroData()[1]~=nil then
        self.bagModel:SetUID(LocalModel:Instance():LoadDeployHeroData()[1].uid)
        self.bagModel:SetBID(1)
    elseif TableUtil.Lens(LocalModel:Instance():GetSortedHeroLocalData()) ~= 0 then
        self.bagModel:SetUID(LocalModel:Instance():GetSortedHeroLocalData()[1].uid)
    end
end
-----------------------------------------------初始化设置结束-------------------------------------------------

------------------------------------------------页面刷新逻辑-------------------------------------------------
--页面刷新逻辑 每次生成界面时调用
function BagController:RefreshUI()
    if self.view.isDoneLoading == nil or self.view.isDoneLoading == false then return end
    self:__initID()
    self:__refreshDeployHero()
    self:__refreshHero(HeroType.All)
    self:__refreshShowHero()
    self.view:InitPage()
end

--刷新上阵英雄栏
function BagController:__refreshDeployHero()
    --上阵英雄不为空
    local dic=LocalModel:Instance():LoadDeployHeroData()
    if TableUtil.Lens(dic)~=0 then
        for i=1,5 do
            --如果该位置上有上阵英雄 刷新视图
            if TableUtil.ContainKeys(dic,i) then
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
function BagController:onChangeUid()
    --点击改变详细展示英雄
    self:__refreshShowHero()
end
function BagController:onChangeBid(data)
    --设置当前选中英雄uid
    if TableUtil.ContainKeys(LocalModel:Instance():LoadDeployHeroData(),data.newBid) then
        self.bagModel:SetUID(LocalModel:Instance():LoadDeployHeroData()[data.newBid].uid)
    end
    --选中按钮高光
    self.view:ChangeLightingButton(data.oldBid, data.newBid)
end

--3.关闭界面
function BagController:__closePanel()
    self:__closeInit()
    UIManager:Instance():CloseUI(self.name)
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

--点击背包下方英雄分类键：刷新显示英雄类型
function BagController:__refreshHero(heroType)
    --清除原有对象
    local scroll=self.view.uiHeroContent.content
    for i=0,scroll.childCount-1 do
        self.heroPool:ReturnObject(scroll.childCount)
    end

    --获取对应数据   
    local typeHero=nil
    if heroType==HeroType.All then
        typeHero=LocalModel:Instance():GetSortedHeroLocalData()
    else
        typeHero=LocalModel:Instance():GetTypeHeroLocalData(heroType)
    end
    if typeHero ==nil then return end

    --用数据刷新
    for k,v in ipairs(typeHero) do
        --显示新的
        local heroCell=self.heroPool:GetObject(self.heroUIItemPrefab,self.view.uiHeroContent.content)
        heroCell:AddEventListener("changeUid",function(newUID)
            self:__changeUid(newUID)
        end)
        heroCell:AddEventListener("changeBid",function(newBID)
            self:__changeBid(newBID)
        end)
        local heroData=HeroModel:Instance():GetHeroByID(v.id)
        heroCell:Refresh(v.uid,heroData)
    end
end
------------------------------------------------响应事件结束------------------------------------------------
--关闭时还原数据逻辑
function BagController:__closeInit()
    self.bagModel:SetBID(0)
    self.bagModel:SetUID(0)
end