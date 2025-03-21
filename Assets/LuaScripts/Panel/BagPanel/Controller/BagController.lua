local BagView=require("Panel.BagPanel.View.BagView")
local HeroView=require("Panel.BagPanel.View.HeroView")
local DeployHeroView=require("Panel.BagPanel.View.DeployHeroView")
local BagModel=require("Panel.BagPanel.Model.BagModel")
local util=require("util")

local BagController = BaseClass("BagController")
--����
function BagController:Instance(basePanel)  
    if self.instance == nil then  
        self.instance = self:New(basePanel)  
    end  
    return self.instance  
end  

-- ��ʼ������
function BagController:__init(basePanel)
    -- view model
    self.view=BagView:New(basePanel)
    self.bagModel=BagModel:New()
    
    --����Ԥ�Ƽ�������
    self.heroUIItemPrefab=Resources.Load("UI/HeroDetail")
    self.heroDeployItem=Resources.Load("UI/DeployHero")

    --�Ѽ�������ֵ�
    self.uidAndPanelDict={}

    --������
    self:__setupEventListeners()
    --��������Ӣ�۰�ť��ӵ�ҳ����
    self:__addDeployHero()
    --�����
    self.heroPool=ObjectPool:New(HeroView,12,self.heroUIItemPrefab,self.view.uiHeroContent.content)
end

------------------------------------------------��ʼ������--------------------------------------------------
-- �����¼�������
function BagController:__setupEventListeners()
    --model 
    --��ѡ��չʾ��Ӣ���޸�ʱ ˢ��չʾӢ�۽���
    self.bagModel:AddEventListener("onChangeUid",function()
        self:__onChangeUid()
    end)
    --��ѡ��İ����ı�ʱ ˢ��չʾӢ�۽��� ��ѡ�а�ť�߹�
    self.bagModel:AddEventListener("onChangeBid",function(data)
        self:__onChangeBid(data)
    end)

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

--�����ӿؼ����������Ӣ�۰�ť ��ע��ص�
function BagController:__addDeployHero()
    for i=1,5 do
        local deployHeroView=DeployHeroView:New(GameObject.Instantiate(self.heroDeployItem,self.view.uiDeployList),i)
        deployHeroView:AddEventListener("changeBid",function(newButtonID)
            self:__changeBid(newButtonID)
        end)
        self.view:AddDeployHero(deployHeroView)
    end
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
-----------------------------------------------��ʼ�����ý���-------------------------------------------------

------------------------------------------------ҳ��ˢ���߼�-------------------------------------------------
--ҳ��ˢ���߼� ÿ�����ɽ���ʱ����
function BagController:InitUI()
    --���õ�ǰѡ��չʾ��Ӣ�۵�uid Ĭ��չʾ��һλ������Ӣ�� û�е�һλ������Ӣ�۾�չʾ�û�������Ȩ����ߵ�Ӣ��
    if ContainKeys(LocalModel:Instance():LoadDeployHeroData(),1) and LocalModel:Instance():LoadDeployHeroData()[1]~=nil then
        self.bagModel:SetUID(LocalModel:Instance():LoadDeployHeroData()[1].uid)
        self.bagModel:SetBID(1)
    elseif Lens(LocalModel:Instance():GetSortedHeroLocalData()) ~= 0 then
        self.bagModel:SetUID(LocalModel:Instance():GetSortedHeroLocalData()[1].uid)
    end

    -- ��ȡ Unity �� MonoBehaviour ʵ��
    local monoInstance = CS.UnityEngine.Object.FindObjectOfType(typeof(CS.UnityEngine.MonoBehaviour))
    -- Ҫ���ص���Դ�ֵ�
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

--��ʼ������Ӣ����
function BagController:__refreshDeployHero()
    --����Ӣ�۲�Ϊ��
    local dic=LocalModel:Instance():LoadDeployHeroData()
    if Lens(dic)~=0 then
        for i=1,5 do
            --�����λ����������Ӣ�� ˢ����ͼ
            if ContainKeys(dic,i) then
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

--��ʼ����ʾӢ�۱���
function BagController:__initHero()
    --���ԭ�е�
    local scroll=self.view.uiHeroContent.content
    for i=0,scroll.childCount-1 do
        self.heroPool:ReturnObject(scroll:GetChild(i))
    end

    --û���ݾͷ���
    if LocalModel:Instance():GetSortedHeroLocalData()==nil then
        return
    end

    --��ʾ����Ӣ��
    for k,v in pairs(LocalModel:Instance():GetSortedHeroLocalData()) do
        local heroData=HeroModel:Instance():GetHeroByID(v.id)
        self:__createHeroWindow(v.uid,heroData)
    end
end

--���Ѽ���������uid������
function BagController:__initialDict()
    --���Ѽ���������uid��ӳ���
    local scroll=self.view.uiHeroContent.content
    --���ڻ�ȡ��ÿһ��Ӣ�� ���
    for i=0,scroll.childCount-1 do
        for k,v in ipairs(self.heroPool.activeObjList) do
            --����Ǽ���ı� �����uid�����ֵ���
            if v.transform == scroll:GetChild(i) then
                self.uidAndPanelDict[v.uid]=v
            end
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
function BagController:__onChangeUid()
    --����ı���ϸչʾӢ��
    self:__refreshShowHero()
end
function BagController:__onChangeBid(data)
    --���õ�ǰѡ��Ӣ��uid
    if ContainKeys(LocalModel:Instance():LoadDeployHeroData(),data.newBid) then
        self.bagModel:SetUID(LocalModel:Instance():LoadDeployHeroData()[data.newBid].uid)
    end
    --ѡ�а�ť�߹�
    self.view:ChangeLightingButton(data.oldBid, data.newBid)
end

--3.�رս���
function BagController:__closePanel()
    self:__closeInit()
    UIManager:Instance():CloseUI(self.view.controlPanel.name)
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

---��������·�Ӣ�۷������ˢ����ʾӢ������
function BagController:__refreshHero(heroType)
    --��Ϊ�����Ѽ������������ֵ�������ֱ�Ӷ��ֵ�������
    for k,v in pairs(self.uidAndPanelDict) do 
        --�����ȫ��ʾ �����ж�ֱ����ʾ
        if heroType==HeroType.All then
            v.controlPanel:SetActive(true)
        else
            --����ȫ��ʾ �жϺ�����ʾ
            local type=HeroModel:Instance():GetHeroByID(LocalModel:Instance():GetLocalItemDataByUid(k).id).type
            if type~=heroType then
                v.controlPanel:SetActive(false)
            else
                v.controlPanel:SetActive(true)
            end
        end
        
    end
end
------------------------------------------------��Ӧ�¼�����------------------------------------------------
--�ر�ʱ��ԭ�����߼�
function BagController:__closeInit()
    self.bagModel:SetBID(0)
    self.uid ={}
end

return BagController