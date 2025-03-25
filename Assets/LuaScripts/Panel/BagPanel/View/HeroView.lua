HeroView = BaseClass("HeroView")

-- 初始化函数
function HeroView:__init(resource,uiRoot)
    --基本属性
    self.controlPanel=GameObject.Instantiate(resource,uiRoot)
    self.transform=self.controlPanel.transform
    self.eventListeners={}

    --存储的对象uid
    self.uid=0
    --是否已激活
    self.isActice=false
    --UI组件
    self.grade = self.transform:Find("Grade"):GetComponent("Text")
    self.type = self.transform:Find("Type").gameObject:GetComponent("Image")
    self.star = self.transform:Find("Star")
    self.image = self.transform:GetComponent("Image")
    self.btn = self.transform:GetComponent("Button")
    --按键事件
    self.btn.onClick:AddListener(function()
        self:__triggerEvent("changeUid",self.uid)
        self:__triggerEvent("changeBid",0)
    end)
end

-- 注册事件监听器
function HeroView:AddEventListener(event,callback)
    self.eventListeners[event] = callback
end

-- 触发事件
function HeroView:__triggerEvent(event, data)
    local callback = self.eventListeners[event]
    if callback then
        callback(data)
    end
end

--更新预制体信息
function HeroView:Refresh(uid,heroData)
    self.uid=uid
    --稀有度信息
    self.grade.text = GetName(typeof(HeroGrade),heroData.rarity)

    --显示类别图片
    local path=GetName(typeof(HeroType),heroData.type)
    CS.AsyncMgr.Instance:LoadAsync(tostring(path),function(res)    
        return self:__typeCallBack(res)
    end)
    --显示人物图片
    CS.AsyncMgr.Instance:LoadAsync(tostring(heroData.imgPath),function(res)    
        return self:__heroCallBack(res)
    end)
    --显示星级
    RefreshStars(self.star,heroData);
end
function HeroView:__typeCallBack(res)
    self.type.sprite= Sprite.Create(res, Rect(0, 0, res.width, res.height), Vector2(0, 0))
end

function HeroView:__heroCallBack(res)
    self.image.sprite= Sprite.Create(res, Rect(0, 0, res.width, res.height), Vector2(0, 0))
end