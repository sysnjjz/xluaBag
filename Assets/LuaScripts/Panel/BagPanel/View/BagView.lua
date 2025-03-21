﻿local BagView = BaseClass("BagView",BasePanel)
-- 初始化函数
function BagView:__init(basePanel)
    --基本属性
    self.controlPanel=basePanel.controlPanel
    self.transform=basePanel.transform
    self.eventListeners={}

    --控件
    --最左侧按键界面
    --UI控件 侧边栏按钮和关闭按钮
    self.uiCloseBtn=self.transform:Find("CloseButton"):GetComponent("Button")
    self.uiHeroBtn=self.transform:Find("ButtonList/HeroButton"):GetComponent("Button")
    self.uiCodexBtn=self.transform:Find("ButtonList/CodexButton"):GetComponent("Button")

    --次左侧图鉴或背包界面
    --图鉴
    self.uiCodex=self.transform:Find("Codex")
    --英雄背包界面
    self.uiHeroBag=self.transform:Find("HeroBag")
    --上栏
    self.uiPlusBtn=self.transform:Find("HeroBag/UpperWindow/Capcity/PlusButton"):GetComponent("Button")
    self.uiReferrerBtn=self.transform:Find("HeroBag/UpperWindow/ReferrerButton"):GetComponent("Button")
    --中间 英雄背包界面
    self.uiHeroContent=self.transform:Find("HeroBag/HeroContent"):GetComponent("ScrollRect")
    --下栏
    self.uiAllBtn=self.transform:Find("HeroBag/DownWindow/All"):GetComponent("Button")
    self.uiForceBtn=self.transform:Find("HeroBag/DownWindow/Force"):GetComponent("Button")
    self.uiInnerForceBtn=self.transform:Find("HeroBag/DownWindow/InnerForce"):GetComponent("Button")
    self.uiHealBtn=self.transform:Find("HeroBag/DownWindow/Heal"):GetComponent("Button")
    self.uiSwordBtn=self.transform:Find("HeroBag/DownWindow/Sword"):GetComponent("Button")
    self.uiSkillBtn=self.transform:Find("HeroBag/DownWindow/Skill"):GetComponent("Button")

    --右上侧英雄展示页面
    self.uiShowHeroBtn= self.transform:Find("ShowHero"):GetComponent("Button")
    self.uiHero=self.transform:Find("ShowHero/Hero")
    self.uiHeroInfo_GradeText=self.transform:Find("ShowHero/HeroInfo/Grade"):GetComponent("Text")
    self.uiHeroInfo_NameText=self.transform:Find("ShowHero/HeroInfo/Name"):GetComponent("Text")
    self.uiHeroInfo_KeyWordText=self.transform:Find("ShowHero/HeroInfo/KeyWord"):GetComponent("Text")
    self.uiATKText=self.transform:Find("ShowHero/ATK"):GetComponent("Text")
    self.star=self.transform:Find("ShowHero/HeroInfo/Star")
    --子控件生成位置
    self.childPosition=self.transform:Find("ShowHero/Hero"):GetComponent("RectTransform").transform.position;

    --右下侧英雄上阵栏
    self.uiUPBtn=self.transform:Find("Deploy/DeployButton"):GetComponent("Button")
    self.uiDeployList=self.transform:Find("Deploy/DeployList")
    self.uiDeployBtnArr={}

    --页面初始化
    self.uiCodex.gameObject:SetActive(false)
    self.uiHeroBag.gameObject:SetActive(true)

    --注册按键事件 需要controller做的
    --关闭界面
    self.uiCloseBtn.onClick:AddListener(function()
        self:__triggerEvent("closePanel")
    end)

    --调用数据做更新
    self.uiAllBtn.onClick:AddListener(function()
        self:__triggerEvent("changeShowHero",HeroType.All)
    end)
    self.uiForceBtn.onClick:AddListener(function()
        self:__triggerEvent("changeShowHero",HeroType.Force)
    end)
    self.uiInnerForceBtn.onClick:AddListener(function()
        self:__triggerEvent("changeShowHero",HeroType.InnerForce)
    end)
    self.uiHealBtn.onClick:AddListener(function()
        self:__triggerEvent("changeShowHero",HeroType.Heal)
    end)
    self.uiSwordBtn.onClick:AddListener(function()
        self:__triggerEvent("changeShowHero",HeroType.Sword)
    end)
    self.uiSkillBtn.onClick:AddListener(function()
        self:__triggerEvent("changeShowHero",HeroType.Skill)
    end)

    --更新上阵英雄
    self.uiUPBtn.onClick:AddListener(function()
        self:__triggerEvent("updateDeployHero")
    end)

    --注册按键事件 不需要controller做的
    --内部显示逻辑
    self.uiCodexBtn.onClick:AddListener(function()
        self:__showCodex()
    end)
    self.uiHeroBtn.onClick:AddListener(function()
        self:__showHeroBag()
    end)

    --内部交互逻辑
    --播动画
    self.uiShowHeroBtn.onClick:AddListener(function()
        self:__attackAnim()
    end)
    --没做的背包扩容和上阵推荐 现在只能打印
    self.uiPlusBtn.onClick:AddListener(function()
        self:__onClickPlus()
    end)
    self.uiReferrerBtn.onClick:AddListener(function()
        self:__onClickReferrer()
    end)
end

-- 注册事件监听器
function BagView:AddEventListener(event, callback)
    self.eventListeners[event] = callback
end

-- 触发事件
function BagView:__triggerEvent(event, data)
    local callback = self.eventListeners[event]
    if callback then
        callback(data)
    end
end

--添加子控件到按钮队列中
function BagView:AddDeployHero(deployHeroButton)
    table.insert(self.uiDeployBtnArr,deployHeroButton)
end

--按键逻辑
function BagView:__showCodex()
    self.uiCodex.gameObject:SetActive(true)
    self.uiHeroBag.gameObject:SetActive(false)
end
function BagView:__showHeroBag()
    self.uiCodex.gameObject:SetActive(false)
    self.uiHeroBag.gameObject:SetActive(true) 
end

--攻击动画
function BagView:__attackAnim()
    if self.uiHero.childCount==0 then
        return
    end
    local heroPrefab=self.uiHero:GetChild(0):GetComponent("PlayerObj")
    heroPrefab:SetStateAnimationIndex(CS.PlayerState.ATTACK)
    heroPrefab:PlayStateAnimation(CS.PlayerState.ATTACK)
end

--没做的背包扩容和上阵推荐
function BagView:__onClickPlus()
    print("click plus")
end
function BagView:__onClickReferrer()
    print("click referrer")
end

--刷新英雄展示界面
function BagView:RefreashHeroDetail(localData,heroData)
    --稀有度信息
    self.uiHeroInfo_GradeText.text=CS.System.Enum.GetName(typeof(HeroGrade),heroData.rarity)
    --攻击力
    self.uiATKText.text="ATK:"..tostring(localData.ATK)
    --名称
    self.uiHeroInfo_NameText.text=tostring(heroData.name)
    --关键词
    self.uiHeroInfo_KeyWordText.text=tostring(heroData.keyword)

    --显示预制体
    --清除原有的
    for i=0,self.uiHero.childCount-1 do
        Object.Destroy(self.uiHero:GetChild(i).gameObject)
    end
    --添加新的预制体
    local nowPrefab=Resources.Load(heroData.prefabPath)
    local showHero=GameObject.Instantiate(nowPrefab,self.uiHero)
    showHero.transform.position=Vector3(0,0,0)
    showHero.transform:Rotate(0, 180, 0)
    showHero.transform.localScale=Vector3(900,500,450)

    --显示星级
    RefreshStars(self.star,heroData)
end

-- 恢复初始视图
function BagView:ClearHeroDetail()
    --稀有度信息
    self.uiHeroInfo_GradeText.text=""
    --攻击力
    self.uiATKText.text=""
    --名称
    self.uiHeroInfo_NameText.text=""
    --关键词
    self.uiHeroInfo_KeyWordText.text=""

    --清除原有的预制体
    for i=0,self.uiHero.childCount-1 do
        Object.Destroy(self.uiHero:GetChild(i).gameObject)
    end

    --清除星星
    for i=0,self.star.childCount-1 do
        self.star:GetChild(i).gameObject:SetActive(false)
    end
end

--改变发光按钮
function BagView:ChangeLightingButton(oldBid,newBid)
    if oldBid~=0 then 
        --前一个按钮不亮
        self.uiDeployBtnArr[oldBid]:SetIsLighting(false)
    end
    if newBid~=0 then 
        --后一个亮
        self.uiDeployBtnArr[newBid]:SetIsLighting(true)
    end
end

return BagView