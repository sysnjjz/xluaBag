local BagView=require("Panel.BagPanel.View.BagView")
local HeroView=require("Panel.BagPanel.View.HeroView")
local DeployHeroView=require("Panel.BagPanel.View.DeployHeroView")
local BagModel=require("Panel.BagPanel.Model.BagModel")

local BagController = BaseClass("BagController")
--单例
function BagController:Instance(basePanel)  
    if self.instance == nil then  
        self.instance = self:New(basePanel)  
    end  
    return self.instance  
end  

--页面刷新逻辑
function BagController:InitUI()
    --设置当前选择展示的英雄的uid 默认展示第一位已上阵英雄 没有第一位已上阵英雄就展示用户背包中权重最高的英雄
    if ContainKeys(LocalModel:Instance():LoadDeployHeroData(),1) and LocalModel:Instance():LoadDeployHeroData()[1]~=nil then
        self.bagModel:SetUID(LocalModel:Instance():LoadDeployHeroData()[1].uid)
    elseif Lens(LocalModel:Instance():GetSortedHeroLocalData()) ~= 0 then
        self.bagModel:SetUID(LocalModel:Instance():GetSortedHeroLocalData()[1].uid)
    end

    --刷新上阵英雄列表
    self:__refreshDeployHero()
    --刷新英雄背包栏
    self:__refreshHero(HeroType.All)
    --刷新当前展示英雄
    self:__refreshShowHero()
end

-- 初始化函数
function BagController:__init(basePanel)
    -- view model
    self.view=BagView:New(basePanel)
    self.bagModel=BagModel:New()
    
    --监听器
    self:__setupEventListeners()

    --加载预制件子物体
    self.heroUIItemPrefab=Resources.Load("UI/HeroDetail")
    self.heroDeployItem=Resources.Load("UI/DeployHero")

    --生成上阵英雄按钮添加到页面里--需要改
    self:__addDeployHero()
end

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

--改变选中按钮回调
function BagController:__changeUid(newUID)
    self.bagModel:SetUID(newUID)
end
function BagController:__changeBid(newBID)
    self.bagModel:SetBID(newBID)
end

---响应事件
--当选择展示的英雄修改时 刷新展示英雄界面
function BagController:__onChangeUid()
    self:__refreshShowHero()
end

--当选择的按键改变时
function BagController:__onChangeBid(data)
    --设置当前选中英雄uid
    if ContainKeys(LocalModel:Instance():LoadDeployHeroData(),data.newBid) then
        self.bagModel:SetUID(LocalModel:Instance():LoadDeployHeroData()[data.newBid].uid)
    end
    --选中按钮高光
    self.view:ChangeLightingButton(data.oldBid, data.newBid)
end

--关闭界面
function BagController:__closePanel()
    UIManager:Instance():CloseUI(self.view.controlPanel.name)
end

--刷新上阵英雄栏
function BagController:__refreshDeployHero()
    --上阵英雄不为空
    local dic=LocalModel:Instance():LoadDeployHeroData()
    if Lens(dic)~=0 then
        for i=1,5 do
            if ContainKeys(dic,i) then
                local localData=dic[i]
                if localData~=nil then
                    local heroData=HeroModel:Instance():GetHeroByID(localData.id)
                    self.view.uiDeployBtnArr[i]:Refresh(localData,heroData)  
                end
            end
        end
    end
end

--刷新展示页
function BagController:__refreshShowHero()
    --找到对应的动态数据
    local localData=LocalModel:Instance():GetLocalItemDataByUid(self.bagModel:GetUID())
    --为空返回
    if localData==nil then
        return
    end
    --刷新展示页
    self.view:RefreashHeroDetail(localData,HeroModel:Instance():GetHeroByID(localData.id))
end

--更新英雄侧边滚动框--对象池修改范围
function BagController:__refreshHero(heroType)
    --清除原有的
    local scroll=self.view.uiHeroContent.content
    for i=0,scroll.childCount-1 do
        Object.Destroy(scroll:GetChild(i).gameObject)
    end

    --没数据就返回
    if LocalModel:Instance():GetSortedHeroLocalData()==nil then
        return
    end

    --分类显示 switch
    for k,v in pairs(LocalModel:Instance():GetSortedHeroLocalData()) do
        local heroData=HeroModel:Instance():GetHeroByID(v.id)
        if heroType==HeroType.All then
            self:__createHeroWindow(v.uid,heroData)
        elseif heroType==HeroType.Force then
            if heroData.type==HeroType.Force then
                self:__createHeroWindow(v.uid,heroData)
            end
        elseif heroType==HeroType.InnerForce then
            if heroData.type==HeroType.InnerForce then
                self:__createHeroWindow(v.uid,heroData)
            end
        elseif heroType==HeroType.Heal then
            if heroData.type==HeroType.Heal then
                self:__createHeroWindow(v.uid,heroData)
            end
        elseif heroType==HeroType.Sword then
            if heroData.type==HeroType.Sword then
                self:__createHeroWindow(v.uid,heroData)
            end
        elseif heroType==HeroType.Skill then
            if heroData.type==HeroType.Skill then
                self:__createHeroWindow(v.uid,heroData)
            end
        end

    end
end

--生成英雄并显示--对象池修改范围
function BagController:__createHeroWindow(uid,heroData)
    local heroCell=HeroView:New(GameObject.Instantiate(self.heroUIItemPrefab,self.view.uiHeroContent.content),uid)
    heroCell:AddEventListener("changeUid",function(newUID)
        self:__changeUid(newUID)
    end)
    heroCell:Refresh(heroData)
end

--添加上阵英雄
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

return BagController