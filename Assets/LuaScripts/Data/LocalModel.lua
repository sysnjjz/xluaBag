--这里应该是接收服务端的数据 但是为了模拟就直接从本地里读了

LocalModel = BaseClass("LocalModel")
-- 初始化函数
function LocalModel:__init(params)
    --存取框架
    --玩家拥有的英雄列表
    self.localDataList={}
    --上阵英雄字典
    self.deployHeroDic={}
end

--单例
function LocalModel:Instance()  
    if self.instance == nil then  
        self.instance = self:New()  
    end  
    return self.instance  
end  

--加载玩家已有英雄
--预加载逻辑
function LocalModel:PreLoadHeroData()
    --已经有数据了不用重复读取
    if Lens(self.localDataList)~=0 then return self.localDataList end
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
function LocalModel:LoadHeroData()
    return self.localDataList
end

--保存逻辑
function LocalModel:SaveHeroData()
    LuaBridge.SaveLocalItemJson(self.localDataList)
end

--清除所有英雄
function LocalModel:ClearOwnHero()
    self.localDataList={}
    self:SaveHeroData()
end

--根据uid拿到指定动态数据
function LocalModel:GetLocalItemDataByUid(uid)
    --表中无数据 返回
    if Lens(self.localDataList)==0 then return nil end
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
    if Lens(self.localDataList)==0 then
        print("list is empty")
        return nil 
    end
    --排序
    table.sort(self.localDataList,function(a,b)
        --先按稀有度排序
        local x=self:GetLocalItemDataByUid(a.uid)
        local y=self:GetLocalItemDataByUid(b.uid)
        if x==nil or y==nil then
            print("can not get item data")
            return false
        end

        --一样的话比较id大小
        if x.rarity==y.rarity then
            return x.id>y.id
        end

        return x.rarity>y.rarity
    end)

    return self.localDataList
end

--加载玩家已有英雄
--预加载逻辑
function LocalModel:PreLoadDeployHeroData()
    --已经有数据了不用重复读取
    if Lens(self.deployHeroDic)~=0 then return self.deployHeroDic end
    --调用C#中的函数读取数据
    local deployHeroData=LuaBridge.LoadDeployHeroJson()
    --没读到数据返回
    if Lens(deployHeroData)==0 then 
        print("no deploy data")
        return
    end
    --类型转换
    for k,v in pairs(deployHeroData) do
        self.deployHeroDic[k]=deployHeroData[k]
    end
end

--获取玩家已上阵英雄
function LocalModel:LoadDeployHeroData()
    return self.deployHeroDic
end

--保存逻辑
function LocalModel:SaveDeployHeroData()
    LuaBridge.SaveDeployHeroJson(self.deployHeroDic)
end

--添加上阵英雄
function LocalModel:AddDeployHero(localData,heroData)
    --只在对应的位置上阵该类型英雄
    local place = heroData.type:GetHashCode()+1 --CS里写的枚举是从0开始的
    self.deployHeroDic[place]=localData
    self:SaveDeployHeroData()
    return place
end

--清除上阵英雄
function LocalModel:ClearDeployHero()
    self.deployHeroDic={}
    self:SaveDeployHeroData()
end
