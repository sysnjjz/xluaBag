ObjectPool = BaseClass("ObjectPool")
-- 初始化函数
function ObjectPool:__init(class,size,...)
    --需要实例化的对象类
    self.class=class
    --对象池尺寸
    self.size=size
    -- 活跃对象列表
    self.activeObjList={}
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
        obj.controlPanel:SetActive(true)
        obj.isActive=true
        table.insert(self.activeObjList, obj)
        return obj
    else
        -- 如果没有非活跃对象，创建一个新的对象
        local obj = self.class:New(...)
        obj.controlPanel:SetActive(true)
        obj.isActive=true
        table.insert(self.activeObjList, obj)
        return obj
    end
end

-- 将对象放回对象池
function ObjectPool:ReturnObject(times)
    local time=times
    if times>Lens(self.activeObjList) then time=Lens(self.activeObjList) end
    for i=1,time do
        local obj = table.remove(self.activeObjList, 1)
        obj.controlPanel:SetActive(false)   
        obj.isActive=false
        table.insert(self.inactiveObjList, obj)
    end
end

-- 清空对象池
function ObjectPool:Clear()
    self.activeObjList = {}
    self.inactiveObjList = {}
end
