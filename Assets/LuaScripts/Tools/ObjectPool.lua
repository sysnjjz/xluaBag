ObjectPool = BaseClass("ObjectPool")
-- 初始化函数
function ObjectPool:__init(class)
    --需要实例化的对象类
    self.class=class
    -- 活跃对象列表
    self.activeObjects = {}
    -- 不活跃对象列表
    self.inactiveObjects = {}
end

-- 从对象池中获取一个对象
function ObjectPool:GetObject(...)
    print(Lens(self.inactiveObjects))
    if #self.inactiveObjects > 0 then
        -- 如果有非活跃对象，直接从非活跃列表中取出
        local obj = table.remove(self.inactiveObjects, 1)
        table.insert(self.activeObjects, obj)
        return obj
    else
        -- 如果没有非活跃对象，创建一个新的对象
        local obj = self.class:New(...)
        table.insert(self.activeObjects, obj)
        return obj
    end
end

-- 将对象放回对象池
function ObjectPool:ReturnObject(obj)
    for i, v in ipairs(self.activeObjects) do
        if v == obj then
            table.remove(self.activeObjects, v)
            table.insert(self.inactiveObjects, obj)
            print("insert")
            break
        end
    end
end

-- 清空对象池
function ObjectPool:Clear()
    self.activeObjects = {}
    self.inactiveObjects = {}
end
