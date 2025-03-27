BagView = BaseClass("BagView",BasePanel)

function BagView:__callBack(res)
    self.controlPanel=GameObject.Instantiate(res,self.uiRoot)
    self.transform=self.controlPanel.transform
    self.controlPanel:SetActive(true)

    self:__initUI()
    self:__initButton()
    self:__addListener()

    self.isDoneLoading=true
    if self.OnViewLoaded then
        self.OnViewLoaded()
    end
end

--打开界面
function BagView:OpenPanel(name,uiRoot,ctrl)
    self.ctrl=ctrl
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

-- ui初始化
function BagView:__initUI()
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
    self.uiBtnList=self.transform:Find("HeroBag/DownWindow")

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

    self:InitPage()
end

function BagView:InitPage()
    --页面初始化
    self.uiCodex.gameObject:SetActive(false)
    self.uiHeroBag.gameObject:SetActive(true)
end

function BagView:__initButton()
    --注册按键事件 需要controller做的
    --关闭界面
    self.uiCloseBtn.onClick:AddListener(function()
        self:__triggerEvent("closePanel")
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

function BagView:__addListener()
    --关闭界面
    self:AddEventListener("closePanel",function()
        self.ctrl:__closePanel()
    end)
    --背包栏显示不同属性的英雄
    self:AddEventListener("changeShowHero",function(heroType)
        self.ctrl:__refreshHero(heroType)
    end)
    --点击上阵按钮 刷新上阵英雄栏展示
    self:AddEventListener("updateDeployHero",function()
        self.ctrl:__updateDeployHero()
    end)
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
    CS.AsyncMgr.Instance:LoadAsync(tostring(heroData.prefabPath),function(res)    
        return self:__heroPrefabCallBack(res)
    end)

    --显示星级
    self:__refreshStars(self.star,heroData)
end
--回调
function BagView:__heroPrefabCallBack(res)
    local showHero=GameObject.Instantiate(res,self.uiHero)
    showHero.transform.position=Vector3(0,0,0)
    showHero.transform:Rotate(0, 180, 0)
    showHero.transform.localScale=Vector3(900,500,450)
end

function BagView:__refreshStars(star,heroData)
    for i=0,star.childCount-1 do
        local uiStar=star:GetChild(i)
        if heroData.rarity:GetHashCode()>i then
            uiStar.gameObject:SetActive(true)
        else
            uiStar.gameObject:SetActive(false)
        end
    end
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