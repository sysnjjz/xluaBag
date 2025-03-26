--关于Unity
GameObject = CS.UnityEngine.GameObject
Object = CS.UnityEngine.Object
Random=CS.UnityEngine.Random
Vector2=CS.UnityEngine.Vector2
Vector3=CS.UnityEngine.Vector3
Rect=CS.UnityEngine.Rect
LuaBridge=CS.LuaBridge
UI = CS.UnityEngine.UI
Texture2D=CS.UnityEngine.Texture2D
Color=CS.UnityEngine.Color
Sprite=CS.UnityEngine.Sprite
Image = UI.Image
Button = UI.Button
GetName=CS.System.Enum.GetName
HeroGrade=CS.HeroGrade
HeroType=CS.HeroType

--全局函数
function deepCopy(object)
    --记录已拷贝的对象 避免重复拷贝
    local savedTable = {}
    --检查对象类型 不是表的和已拷贝的对象不拷贝
    local function copyObj(object)
        if type(object) ~= "table" then
            return object
        elseif savedTable[object] then
            return savedTable[object]
        end

        --创建新表做深拷贝
        local newTable = {}
        savedTable[object] = newTable
        for key, value in pairs(object) do
            --递归调用 对每一张表做深拷贝s
            newTable[copyObj(key)] = copyObj(value)
        end
        --新表与原表元表一致
        return setmetatable(newTable, getmetatable(object))
    end
    return copyObj(object)
end

function BaseClass(name,base)
    --类本体
    local class={}
    --实现继承关系
    class.__className=name
    class.__base=base
    if base then
        setmetatable(class,{__index=base})
    end
    --构造函数
    function class:New(...)
        local instance={}
        --setmetatable(instance,{__index=class})
        setmetatable(instance, {__index = deepCopy(class)})
        --递归实现初始化
        local initialize
        initialize=function(ins,...)
            if ins.__base then
                initialize(ins.__base,...)
            end
            if ins.__init then
                ins:__init(...)
            end
        end
        --初始化
        initialize(instance,...)
        return instance
    end
    return class
end

function RefreshStars(star,heroData)
    for i=0,star.childCount-1 do
        local uiStar=star:GetChild(i)
        if heroData.rarity:GetHashCode()>i then
            uiStar.gameObject:SetActive(true)
        else
            uiStar.gameObject:SetActive(false)
        end
    end
end