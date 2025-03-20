--应该是导入策划配的xml表 这里用读unity表格代替
HeroModel = BaseClass("HeroModel")
-- 初始化函数
function HeroModel:__init(params)
    --存取框架
    self.heroList={}
end

--单例
function HeroModel:Instance()  
    if self.instance == nil then  
        self.instance = self:New()  
    end  
    return self.instance  
end  

--加载英雄设定数据
function HeroModel:LoadHeroList()
    --已经有数据了不用重复读取
    if Lens(self.heroList)~=0 then return self.heroList end
    --调用C#中的函数读取数据
    local heroData=LuaBridge.GetHeroList()
    --没读到数据返回
    if heroData==nil then
        print("can not get table")
        return
    end
    --类型转换
    for i=0,heroData.Count-1 do
        table.insert(self.heroList,heroData[i])
    end
end

--获取英雄设定数据
function HeroModel:GetHeroList()
    return self.heroList
end

--得到指定英雄设定数据
function HeroModel:GetHeroByID(id)
    for k,v in ipairs(self.heroList) do
        if v.id==id then
            return v
        end
    end
    return nil
end
