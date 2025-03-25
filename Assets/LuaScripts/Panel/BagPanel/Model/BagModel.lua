BagModel = BaseClass("BagModel")
-- 初始化函数
function BagModel:__init(ctrl)
    -- 控制器
    self.ctrl=ctrl

    --私有属性
    self.chooseUid=0
    self.chooseBid=0
end

--获取uid
function BagModel:GetUID()
    return self.chooseUid
end
--设置uid
function BagModel:SetUID(newUid)
    if self.chooseUid~=newUid then
        self.chooseUid=newUid
        self.ctrl:onChangeUid()    
    end
end

--获取bid
function BagModel:GetBID()
    return self.chooseBid
end
--设置bid
function BagModel:SetBID(newBid)
    if self.chooseBid~=newBid then
        self.ctrl:onChangeBid({oldBid=self.chooseBid,newBid=newBid})
        self.chooseBid=newBid
    end
end