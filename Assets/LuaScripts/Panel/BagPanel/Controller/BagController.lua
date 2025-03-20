local BagView=require("Panel.BagPanel.View.BagView")
local HeroView=require("Panel.BagPanel.View.HeroView")
local DeployHeroView=require("Panel.BagPanel.View.DeployHeroView")
local BagModel=require("Panel.BagPanel.Model.BagModel")

local BagController = BaseClass("BagController")
--����
function BagController:Instance(basePanel)  
    if self.instance == nil then  
        self.instance = self:New(basePanel)  
    end  
    return self.instance  
end  

--ҳ��ˢ���߼�
function BagController:InitUI()
    --���õ�ǰѡ��չʾ��Ӣ�۵�uid Ĭ��չʾ��һλ������Ӣ�� û�е�һλ������Ӣ�۾�չʾ�û�������Ȩ����ߵ�Ӣ��
    if ContainKeys(LocalModel:Instance():LoadDeployHeroData(),1) and LocalModel:Instance():LoadDeployHeroData()[1]~=nil then
        self.bagModel:SetUID(LocalModel:Instance():LoadDeployHeroData()[1].uid)
    elseif Lens(LocalModel:Instance():GetSortedHeroLocalData()) ~= 0 then
        self.bagModel:SetUID(LocalModel:Instance():GetSortedHeroLocalData()[1].uid)
    end

    --ˢ������Ӣ���б�
    self:__refreshDeployHero()
    --ˢ��Ӣ�۱�����
    self:__refreshHero(HeroType.All)
    --ˢ�µ�ǰչʾӢ��
    self:__refreshShowHero()
end

-- ��ʼ������
function BagController:__init(basePanel)
    -- view model
    self.view=BagView:New(basePanel)
    self.bagModel=BagModel:New()
    
    --������
    self:__setupEventListeners()

    --����Ԥ�Ƽ�������
    self.heroUIItemPrefab=Resources.Load("UI/HeroDetail")
    self.heroDeployItem=Resources.Load("UI/DeployHero")

    --��������Ӣ�۰�ť��ӵ�ҳ����--��Ҫ��
    self:__addDeployHero()
end

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

--�ı�ѡ�а�ť�ص�
function BagController:__changeUid(newUID)
    self.bagModel:SetUID(newUID)
end
function BagController:__changeBid(newBID)
    self.bagModel:SetBID(newBID)
end

---��Ӧ�¼�
--��ѡ��չʾ��Ӣ���޸�ʱ ˢ��չʾӢ�۽���
function BagController:__onChangeUid()
    self:__refreshShowHero()
end

--��ѡ��İ����ı�ʱ
function BagController:__onChangeBid(data)
    --���õ�ǰѡ��Ӣ��uid
    if ContainKeys(LocalModel:Instance():LoadDeployHeroData(),data.newBid) then
        self.bagModel:SetUID(LocalModel:Instance():LoadDeployHeroData()[data.newBid].uid)
    end
    --ѡ�а�ť�߹�
    self.view:ChangeLightingButton(data.oldBid, data.newBid)
end

--�رս���
function BagController:__closePanel()
    UIManager:Instance():CloseUI(self.view.controlPanel.name)
end

--ˢ������Ӣ����
function BagController:__refreshDeployHero()
    --����Ӣ�۲�Ϊ��
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

--ˢ��չʾҳ
function BagController:__refreshShowHero()
    --�ҵ���Ӧ�Ķ�̬����
    local localData=LocalModel:Instance():GetLocalItemDataByUid(self.bagModel:GetUID())
    --Ϊ�շ���
    if localData==nil then
        return
    end
    --ˢ��չʾҳ
    self.view:RefreashHeroDetail(localData,HeroModel:Instance():GetHeroByID(localData.id))
end

--����Ӣ�۲�߹�����--������޸ķ�Χ
function BagController:__refreshHero(heroType)
    --���ԭ�е�
    local scroll=self.view.uiHeroContent.content
    for i=0,scroll.childCount-1 do
        Object.Destroy(scroll:GetChild(i).gameObject)
    end

    --û���ݾͷ���
    if LocalModel:Instance():GetSortedHeroLocalData()==nil then
        return
    end

    --������ʾ switch
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

--����Ӣ�۲���ʾ--������޸ķ�Χ
function BagController:__createHeroWindow(uid,heroData)
    local heroCell=HeroView:New(GameObject.Instantiate(self.heroUIItemPrefab,self.view.uiHeroContent.content),uid)
    heroCell:AddEventListener("changeUid",function(newUID)
        self:__changeUid(newUID)
    end)
    heroCell:Refresh(heroData)
end

--�������Ӣ��
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

return BagController