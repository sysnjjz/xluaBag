CardModel = BaseClass("CardModel")

-- 初始化函数
function CardModel:__init(ctrl)
    self.ctrl=ctrl
end

--抽一次
function CardModel:OneCard()
    --随机出一个英雄
    local index=Random.Range(1,Lens(HeroModel:Instance():GetHeroList()))
    index=CS.System.Convert.ToInt32(index)
    local newHero=HeroModel:Instance():GetHeroList()[index]
    --编造一些数据并加入玩家已有英雄列表中
    local newLocalData={
        uid=tostring(CS.System.Guid:NewGuid()),
        id=newHero.id,
        level=1,
        isNew=true,
        isDeploy=false,
        ATK=math.floor(Random.Range(4,10)*100),
        HP=math.floor(Random.Range(4,8)*100),
        Defence=math.floor(Random.Range(2,6)*100)
    }
    table.insert(LocalModel:Instance():LoadHeroData(),newLocalData)
    LocalModel:Instance():SaveHeroData()

    return newHero
end

--抽十次
function CardModel:TenCard()
    --重复抽十次一张卡
    local newHeroLocalData={}
    for i=1,10 do
        local newHero=self:OneCard()
        table.insert(newHeroLocalData,newHero)
    end
    return newHeroLocalData
end
