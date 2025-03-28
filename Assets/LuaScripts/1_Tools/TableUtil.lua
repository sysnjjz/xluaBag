TableUtil = BaseClass("TableUtil")
-- 初始化函数
function TableUtil:__init()
end

--工具函数
--表格是否有key
function TableUtil.ContainKeys(Itable,fkey)
    if Itable==nil then return end
    for key,value in pairs(Itable) do
        if key==fkey then
            return true
        end
    end
    return false
end

--删除表格指定value
function TableUtil.RemoveValue(Itable,fvalue)
    for i=TableUtil.Lens(Itable),1,-1 do
        if Itable[i].value==fvalue then
            table.remove(Itable,i)
        end
    end
end

--计算表长
function TableUtil.Lens(Itable)
    if Itable==nil then return 0 end
    local count=0
    for key, value in pairs(Itable) do
        if value~=nil then
            count=count+1
        end
    end
    return count
end

--清除表的内容
function TableUtil.ClearTable(Itable)
    if Itable==nil or TableUtil.Lens(Itable)==0  then return end
    for k in pairs(Itable) do
        Itable[k] = nil
    end
end