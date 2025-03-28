btnView = BaseClass("btnView")
-- 初始化函数
function btnView:__init(panel,type,icon)
    --基本属性
    self.controlPanel=panel
    self.transform=panel.transform
    self.eventListeners={}

    --自身属性
    self.type=type

    --UI控件
    self.image=self.transform:GetComponent("Image")
    self.btn=self.transform:GetComponent("Button")

    --按键事件
    self.btn.onClick:AddListener(function()
        self:__triggerEvent("changeShowHero",self.type)
    end)

    --显示图像
    CS.AsyncMgr.Instance:LoadAsync(tostring(icon),function(res)    
        return self:__callBack(res)
    end)
end
--回调
function btnView:__callBack(res)
    self.image.sprite= Sprite.Create(res, Rect(0, 0, res.width, res.height), Vector2(0, 0))
end

-- 注册事件监听器
function btnView:AddEventListener(event, callback)
    self.eventListeners[event] = callback
end

-- 触发事件
function btnView:__triggerEvent(event, data)
    local callback = self.eventListeners[event]
    if callback then
        callback(data)
    end
end