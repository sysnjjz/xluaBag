ObjectPool = BaseClass("ObjectPool")
-- 初始化函数
function ObjectPool:__init(class,size,...)
    --需要实例化的对象类
    self.class=class
    --对象池尺寸
    self.size=size
    -- 活跃对象列表
    self.activeObjList = {}
    -- 不活跃对象列表
    self.inactiveObjList = {}

    --初始化
    for i=1,size do
        local obj = self.class:New(...)
        obj.controlPanel:SetActive(false)
        obj.isActive=false
        table.insert(self.inactiveObjList, obj)
    end
end

-- 从对象池中获取一个对象
function ObjectPool:GetObject(...)
    if Lens(self.inactiveObjList)> 0 then
        -- 如果有非活跃对象，直接从非活跃列表中取出
        local obj = table.remove(self.inactiveObjList, 1)
        table.insert(self.activeObjList, obj)
        obj.controlPanel:SetActive(true)
        obj.isActive=true
        return obj
    else
        -- 如果没有非活跃对象，创建一个新的对象
        local obj = self.class:New(...)
        table.insert(self.activeObjList, obj)
        obj.controlPanel:SetActive(true)
        obj.isActive=true
        return obj
    end
end

-- 将对象放回对象池
function ObjectPool:ReturnObject(obj)
    for i, v in ipairs(self.activeObjList) do
        if v.transform == obj then     
            v.controlPanel:SetActive(false)   
            v.isActive=false
            table.insert(self.inactiveObjList, v)
            table.remove(self.activeObjList, i)
            break
        end
    end
end

-- 清空对象池
function ObjectPool:Clear()
    self.activeObjList = {}
    self.inactiveObjList = {}
end
