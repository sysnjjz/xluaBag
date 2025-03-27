BagController = BaseClass("BagController")
--����
function BagController:Instance(name)  
    if self.instance == nil then  
        self.instance = self:New(name)  
    end  
    return self.instance  
end  

-- ��ʼ������
function BagController:__init(name)
    self.name=name

    -- view model
    self.view=nil
    self.bagModel=BagModel:New(self)
end

--����UI
function BagController:ShowView()
    if self.view == nil then
        self.view=BagView:New()
        --ҳ�������ɺ�����ӿؼ�
        self.view.OnViewLoaded=function()
            --Ӣ�۱����ӿؼ�
            CS.AsyncMgr.Instance:LoadAsync("HeroDetail", function(res)
                return self:__heroCallBack(res)
            end)
    
            --����Ӣ���ӿؼ�
            CS.AsyncMgr.Instance:LoadAsync("DeployHero", function(res)
                return self:__deployHeroCallBack(res)
            end)
            --Ӣ�������ӿؼ�
            CS.AsyncMgr.Instance:LoadAsync("TypeBtn", function(res)
                return self:__createHeroTypeBtn(res)
            end)
    
            --������
            self:__setupEventListeners()
            self:__refreshShowHero()
        end
    end
    self.view:OpenPanel("Bag",UIManager:Instance().uiRoot)
end

--����UI
function BagController:CloseView()
    self.view:ClosePanel()
end

------------------------------------------------��ʼ������--------------------------------------------------
-- �����¼�������
function BagController:__setupEventListeners()
    --view
    --�رս���
    self.view:AddEventListener("closePanel",function()
        self:__closePanel()
    end)
    --��������ʾ��ͬ���Ե�Ӣ��
    self.view:AddEventListener("changeShowHero",function(heroType)
        self:__refreshHero(heroType)
    end)
    --�������ť ˢ������Ӣ����չʾ
    self.view:AddEventListener("updateDeployHero",function()
        self:__updateDeployHero()
    end)
end

--�����ӿؼ���Ӣ�۱����е�Ӣ�ۿ�Ƭ ��ע��ص�
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

--�����ӿؼ���Ӣ�۱����ӿؼ�
function BagController:__heroCallBack(res)
    self.heroUIItemPrefab=res
    --�����
    self.heroPool=ObjectPool:New(HeroView,12,self.heroUIItemPrefab,self.view.uiHeroContent.content)
    self:__refreshHero(HeroType.All)
end

--�����ӿؼ�������Ӣ���ӿؼ�
function BagController:__deployHeroCallBack(res)
    self.heroDeployItem=res
    --��������Ӣ�۰�ť��ӵ�ҳ����
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

--�����ӿؼ���Ӣ�۷�����ͼ
function BagController:__createHeroTypeBtn(res)
    self.btn=res
    --ʾ�����ñ�
    local config=
    {
        [1]={name="All",heroType=HeroType.All,icon="All"},
        [2]={name="Force",heroType=HeroType.Force,icon="Force"},
        [3]={name="InnerForce",heroType=HeroType.InnerForce,icon="InnerForce"},
        [4]={name="Skill",heroType=HeroType.Skill,icon="Skill"},
        [5]={name="Sword",heroType=HeroType.Sword,icon="Sword"},
        [6]={name="Heal",heroType=HeroType.Heal,icon="Heal"}
    }
    --��������Ӣ�۰�ť��ӵ�ҳ����
    for k,v in pairs(config) do
        local newBtn=btnView:New(GameObject.Instantiate(self.btn,self.view.uiBtnList),v.heroType,v.icon)
        newBtn:AddEventListener("changeShowHero",function(heroType)
            self:__refreshHero(heroType)
        end)
    end
end

--��ʼ��ѡ��id
function BagController:__initID()
    --���õ�ǰѡ��չʾ��Ӣ�۵�uid Ĭ��չʾ��һλ������Ӣ�� û�е�һλ������Ӣ�۾�չʾ�û�������Ȩ����ߵ�Ӣ��
    if TableUtil.ContainKeys(LocalModel:Instance():LoadDeployHeroData(),1) and LocalModel:Instance():LoadDeployHeroData()[1]~=nil then
        self.bagModel:SetUID(LocalModel:Instance():LoadDeployHeroData()[1].uid)
        self.bagModel:SetBID(1)
    elseif TableUtil.Lens(LocalModel:Instance():GetSortedHeroLocalData()) ~= 0 then
        self.bagModel:SetUID(LocalModel:Instance():GetSortedHeroLocalData()[1].uid)
    end
end
-----------------------------------------------��ʼ�����ý���-------------------------------------------------

------------------------------------------------ҳ��ˢ���߼�-------------------------------------------------
--ҳ��ˢ���߼� ÿ�����ɽ���ʱ����
function BagController:RefreshUI()
    if self.view.isDoneLoading == nil or self.view.isDoneLoading == false then return end
    self:__initID()
    self:__refreshDeployHero()
    self:__refreshHero(HeroType.All)
    self:__refreshShowHero()
    self.view:InitPage()
end

--ˢ������Ӣ����
function BagController:__refreshDeployHero()
    --����Ӣ�۲�Ϊ��
    local dic=LocalModel:Instance():LoadDeployHeroData()
    if TableUtil.Lens(dic)~=0 then
        for i=1,5 do
            --�����λ����������Ӣ�� ˢ����ͼ
            if TableUtil.ContainKeys(dic,i) then
                local localData=dic[i]
                if localData~=nil then
                    local heroData=HeroModel:Instance():GetHeroByID(localData.id)
                    self.view.uiDeployBtnArr[i]:Refresh(localData,heroData)  
                end
            else
                --���û������Ӣ�� �ָ�Ĭ��ͼ��
                self.view.uiDeployBtnArr[i]:RefreshNull()                
            end
        end
    else
        --�ָ�Ĭ��ͼ��
        for i=1,5 do
            self.view.uiDeployBtnArr[i]:RefreshNull()
        end
    end
end

----------------------------------------------ҳ��ˢ���߼�����---------------------------------------------------

-------------------------------------------------��Ӧ�¼�-------------------------------------------------
--1.�ı�ѡ�а�ť�ص�
function BagController:__changeUid(newUID)
    self.bagModel:SetUID(newUID)
end
function BagController:__changeBid(newBID)
    self.bagModel:SetBID(newBID)
end

--2.ѡ��İ����ı�ʱ�ص�
function BagController:onChangeUid()
    --����ı���ϸչʾӢ��
    self:__refreshShowHero()
end
function BagController:onChangeBid(data)
    --���õ�ǰѡ��Ӣ��uid
    if TableUtil.ContainKeys(LocalModel:Instance():LoadDeployHeroData(),data.newBid) then
        self.bagModel:SetUID(LocalModel:Instance():LoadDeployHeroData()[data.newBid].uid)
    end
    --ѡ�а�ť�߹�
    self.view:ChangeLightingButton(data.oldBid, data.newBid)
end

--3.�رս���
function BagController:__closePanel()
    self:__closeInit()
    UIManager:Instance():CloseUI(self.name)
end

--4.ˢ�»ص�
--������󰴼����������Ӣ��
function BagController:__updateDeployHero()
    --��õ�ǰѡ��Ӣ����Ϣ
    local localData=LocalModel:Instance():GetLocalItemDataByUid(self.bagModel:GetUID())
    if localData ~=nil and localData.isDeploy~=true then
        --���Ӣ����Ϣ ��ӵ�������в�ˢ����ͼ   
        local heroData=HeroModel:Instance():GetHeroByID(localData.id)
        local place = LocalModel:Instance():AddDeployHero(localData,heroData)
        self.view.uiDeployBtnArr[place]:Refresh(localData,heroData)
        self.bagModel:SetBID(place)
    end
end

--���Ӣ�ۿ�Ƭ��ˢ��չʾҳ
function BagController:__refreshShowHero()
    --�ҵ���Ӧ�Ķ�̬����
    local localData=LocalModel:Instance():GetLocalItemDataByUid(self.bagModel:GetUID())
    --Ϊ�ջָ�Ĭ�����
    if localData==nil then
        self.view:ClearHeroDetail()
        return
    end
    --ˢ��չʾҳ
    self.view:RefreashHeroDetail(localData,HeroModel:Instance():GetHeroByID(localData.id))
end

--��������·�Ӣ�۷������ˢ����ʾӢ������
function BagController:__refreshHero(heroType)
    --���ԭ�ж���
    local scroll=self.view.uiHeroContent.content
    for i=0,scroll.childCount-1 do
        self.heroPool:ReturnObject(scroll.childCount)
    end

    --��ȡ��Ӧ����   
    local typeHero=nil
    if heroType==HeroType.All then
        typeHero=LocalModel:Instance():GetSortedHeroLocalData()
    else
        typeHero=LocalModel:Instance():GetTypeHeroLocalData(heroType)
    end
    if typeHero ==nil then return end

    --������ˢ��
    for k,v in ipairs(typeHero) do
        --��ʾ�µ�
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
------------------------------------------------��Ӧ�¼�����------------------------------------------------
--�ر�ʱ��ԭ�����߼�
function BagController:__closeInit()
    self.bagModel:SetBID(0)
    self.bagModel:SetUID(0)
end