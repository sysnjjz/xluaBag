CardDetailView = BaseClass("CardDetailView")
-- 初始化函数
function CardDetailView:__init(resource,root)
    --基本属性
    self.controlPanel=GameObject.Instantiate(resource,root,false)
    self.transform=self.controlPanel.transform
    self.transform.localScale=Vector3(0.8,0.8,0.8)

    --是否激活
    self.isActive=false
    --获取组件
    self.star = self.transform:Find("Star");
    self.type = self.transform:Find("Type"):GetComponent("Image");
    self.name = self.transform:Find("Name"):GetComponent("Text");
    self.image = self.transform:GetComponent("Image");
end

--显示逻辑
function CardDetailView:Refresh(heroData)
    --类别图片
    local path=GetName(typeof(HeroType),heroData.type)
    CS.AsyncMgr.Instance:LoadAsync(tostring(path),function(res)    
        return self:__typeCallBack(res)
    end)
    CS.AsyncMgr.Instance:LoadAsync(tostring(heroData.imgPath),function(res)    
        return self:__heroCallBack(res)
    end)
    --显示姓名
    self.name.text=heroData.name
    --更新星星
    self:__refreshStars(self.star,heroData);
end

function CardDetailView:__typeCallBack(res)
    self.type.sprite= Sprite.Create(res, Rect(0, 0, res.width, res.height), Vector2(0, 0))
end

function CardDetailView:__heroCallBack(res)
    self.image.sprite= Sprite.Create(res, Rect(0, 0, res.width, res.height), Vector2(0, 0))
end

function CardDetailView:__refreshStars(star,heroData)
    for i=0,star.childCount-1 do
        local uiStar=star:GetChild(i)
        if heroData.rarity:GetHashCode()>i then
            uiStar.gameObject:SetActive(true)
        else
            uiStar.gameObject:SetActive(false)
        end
    end
end