local CardDetailView = BaseClass("CardDetailView")
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
    local pic=Resources.Load("Icon/"..tostring(path),typeof(Texture2D))
    self.type.sprite= Sprite.Create(pic, Rect(0, 0, pic.width, pic.height), Vector2(0, 0))
    --英雄图片
    local pic1=Resources.Load(tostring(heroData.imgPath),typeof(Texture2D))
    self.image.sprite= Sprite.Create(pic1, Rect(0, 0, pic1.width, pic1.height), Vector2(0, 0))
    --显示姓名
    self.name.text=heroData.name
    --更新星星
    RefreshStars(self.star,heroData)
end

return CardDetailView