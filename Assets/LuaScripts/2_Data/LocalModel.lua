--这里应该是接收服务端的数据 但是为了模拟就直接从本地里读了
LocalModel = BaseClass("LocalModel")

-- 初始化函数
function LocalModel:__init()
    --存取框架
    --玩家拥有的英雄列表
    self.localDataList={}
    --上阵英雄字典
    self.deployHeroDic={}
    --临时表
    self.tmpTypeList={}
    --监听器
    self.eventListeners={}
end

--单例
function LocalModel:Instance()  
    if self.instance == nil then  
        self.instance = self:New()  
    end  
    return self.instance  
end  

-- 注册事件监听器
function LocalModel:AddEventListener(event, callback)
    self.eventListeners[event] = callback
end

-- 触发事件
function LocalModel:__triggerEvent(event, data)
    local callback = self.eventListeners[event]
    if callback then
        callback(data)
    end
end

--加载玩家已有英雄
--预加载逻辑
function LocalModel:LoadHeroData()
    --已经有数据了不用重复读取
    if TableUtil.Lens(self.localDataList)~=0 then return self.localDataList end
    --调用C#中的函数读取数据
    local heroLocalData=LuaBridge.LoadLocalItemJson()
    --没读到数据返回
    if heroLocalData==nil then 
        print("can not read data")
        return
    end
    --类型转换
    for i=0,heroLocalData.Count-1 do
        table.insert(self.localDataList,heroLocalData[i])
    end
end

--获取玩家已有英雄
function LocalModel:GetHeroData()
    return self.localDataList
end

--保存逻辑
function LocalModel:SaveHeroData()
    LuaBridge.SaveLocalItemJson(self.localDataList)
end

--清除所有英雄
function LocalModel:ClearOwnHero()
    TableUtil.ClearTable(self.localDataList)
    self:SaveHeroData()
end

--根据uid拿到指定动态数据
function LocalModel:GetLocalItemDataByUid(uid)
    --表中无数据 返回
    if TableUtil.Lens(self.localDataList)==0 then  
        print("no local data")
        return nil 
    end
    --遍历查找
    for k,v in ipairs(self.localDataList) do
        if v.uid==uid then
            return v
        end
    end
    return nil
end

--获得排好序的数据
function LocalModel:GetSortedHeroLocalData()
    --表中无数据 返回
    if TableUtil.Lens(self.localDataList)==0 then
        print("list is empty")
        return nil 
    end
    --排序
    table.sort(self.localDataList,function(a,b)
        --先按稀有度排序
        local x=HeroModel:Instance():GetHeroByID(a.id)
        local y=HeroModel:Instance():GetHeroByID(b.id)
        if x==nil or y==nil then
            print("can not get item data")
            return false
        end

        --一样的话比较ATK大小
        if x.rarity:GetHashCode()==y.rarity:GetHashCode() then
            return a.ATK>b.ATK
        end

        return x.rarity:GetHashCode()>y.rarity:GetHashCode()
        end)
    return self.localDataList
end

--获得指定类型的数据
function LocalModel:GetTypeHeroLocalData(heroType)
    --表中无数据 返回
    if TableUtil.Lens(self.localDataList)==0 then
        print("list is empty")
        return nil 
    end
    --清空表
    TableUtil.ClearTable(self.tmpTypeList)
    --查找对应数据
    for k,v in ipairs(self.localDataList) do
        if HeroModel:Instance():GetHeroByID(v.id).type==heroType then
            table.insert(self.tmpTypeList,v)
        end
    end
    --排序
    table.sort(self.tmpTypeList,function(a,b)
        --先按稀有度排序
        local x=HeroModel:Instance():GetHeroByID(a.id)
        local y=HeroModel:Instance():GetHeroByID(b.id)
        if x==nil or y==nil then
            print("can not get item data")
            return false
        end

        --一样的话比较ATK大小
        if x.rarity:GetHashCode()==y.rarity:GetHashCode() then
            return a.ATK>b.ATK
        end

        return x.rarity:GetHashCode()>y.rarity:GetHashCode()
    end)
    return self.tmpTypeList
end

--加载玩家已有英雄
--预加载逻辑
function LocalModel:LoadDeployHeroData()
    --已经有数据了不用重复读取
    if TableUtil.Lens(self.deployHeroDic)~=0 then return self.deployHeroDic end
    --调用C#中的函数读取数据
    local deployHeroData=LuaBridge.LoadDeployHeroJson()
    --没读到数据返回
    if TableUtil.Lens(deployHeroData)==0 then 
        print("no deploy data")
        return
    end
    --类型转换
    for k,v in pairs(deployHeroData) do
        self.deployHeroDic[k]=deployHeroData[k]
    end
end

--获取玩家已上阵英雄
function LocalModel:GetDeployHeroData()
    return self.deployHeroDic
end

--保存逻辑
function LocalModel:SaveDeployHeroData()
    LuaBridge.SaveDeployHeroJson(self.deployHeroDic)
end

--添加上阵英雄
function LocalModel:AddDeployHero(uid)
    local localData=self:GetLocalItemDataByUid(uid)
    if localData ==nil or localData.isDeploy ==true then
        self:__triggerEvent("refreshDeployHero",{["success"]=false})
        return
    end
    local heroData=HeroModel:Instance():GetHeroByID(localData.id)
    --只在对应的位置上阵该类型英雄
    local place = heroData.type:GetHashCode()+1 --CS里写的枚举是从0开始的
    self.deployHeroDic[place]=localData
    self:SaveDeployHeroData()
    --给view发事件
    self:__triggerEvent("refreshDeployHero",{["success"]=true,["place"]=place,["localData"]=localData,["heroData"]=heroData})
end

--清除上阵英雄
function LocalModel:ClearDeployHero()
    TableUtil.ClearTable(self.deployHeroDic)
    self:SaveDeployHeroData()
end
