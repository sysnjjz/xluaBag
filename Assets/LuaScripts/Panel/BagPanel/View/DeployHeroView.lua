local DeployHeroView = BaseClass("DeployHeroView")
-- 初始化函数
function DeployHeroView:__init(panel,buttonId)
    --基本属性
    self.controlPanel=panel
    self.transform=panel.transform
    self.eventListeners={}

    --自身属性
    self.buttonID=buttonId

    --UI控件
    self.uiImage=self.transform:GetComponent("Image");
    self.uiBtn=self.transform:GetComponent("Button");
    self.image=self.transform:Find("Image"):GetComponent("Image")
    self.ATK=self.transform:Find("ATK"):GetComponent("Text")

    --按键事件
    self.uiBtn.onClick:AddListener(function()
        self:__triggerEvent("changeBid",self.buttonID)
    end)
end

-- 注册事件监听器
function DeployHeroView:AddEventListener(event, callback)
    self.eventListeners[event] = callback
end

-- 触发事件
function DeployHeroView:__triggerEvent(event, data)
    local callback = self.eventListeners[event]
    if callback then
        callback(data)
    end
end

--设置自身发光
function DeployHeroView:SetIsLighting(bool)
    if bool then
        self.uiImage.color=Color(1, 1, 1,1)
    else
        self.uiImage.color=Color(0, 0, 0,1)
    end
end

--刷新显示对象
function DeployHeroView:Refresh(localData,heroData)
    --刷新显示图像
    local pic=Resources.Load(tostring(heroData.imgPath),typeof(Texture2D))
    self.image.sprite=Sprite.Create(pic,Rect(0, 0, pic.width, pic.height), Vector2(0, 0))
    --发光
    local newColor=self.image.color
    newColor.a=1
    self.image.color=newColor
    self.ATK.text=tostring(localData.ATK)
end

--恢复默认设置
function DeployHeroView:RefreshNull()
    --不显示图像
    local newColor=self.image.color
    newColor.a=0
    self.image.color=newColor
    self.ATK.text=""
end

return DeployHeroView