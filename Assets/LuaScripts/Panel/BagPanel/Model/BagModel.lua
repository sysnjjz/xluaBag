local BagModel = BaseClass("BagModel")
-- 初始化函数
function BagModel:__init(params)
    --私有属性
    self.chooseUid=0
    self.chooseBid=1

    self.eventListeners={}
end

-- 注册事件监听器
function BagModel:AddEventListener(event, callback)
    self.eventListeners[event] = callback
end

-- 触发事件
function BagModel:__triggerEvent(event, data)
    local callback = self.eventListeners[event]
    if callback then
        callback(data)
    end
end

--获取uid
function BagModel:GetUID()
    return self.chooseUid
end
--设置uid
function BagModel:SetUID(newUid)
    if self.chooseUid~=newUid then
        self.chooseUid=newUid
        self:__triggerEvent("onChangeUid")
    end
end

--获取bid
function BagModel:GetBID()
    return self.chooseBid
end
--设置bid
function BagModel:SetBID(newBid)
    if self.chooseBid~=newBid then
        self:__triggerEvent("onChangeBid",{oldBid=self.chooseBid,newBid=newBid})
        self.chooseBid=newBid
    end
end

return BagModel