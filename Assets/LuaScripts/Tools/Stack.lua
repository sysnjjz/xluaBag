local Stack = BaseClass("Stack")
 
function Stack:Create()
    local t = {}
    setmetatable(t, {__index = self})
    self.dataTb = self.dataTb or {}
    return t
end
 
function Stack:Push(...)
    local arg = {...}
    if next(arg) then
        for i = 1, #arg do
            table.insert(self.dataTb, arg[i])
        end
    end
end
 
function Stack:Pop(num)
    num = num or 1
    assert(num > 0, "num必须为正整数")
    local popTb = {}
    for i = 1, num do
        table.insert(popTb, self.dataTb[#self.dataTb])
        table.remove(self.dataTb)
    end
    --return popTb
    return table.unpack(popTb)
end
 
function Stack:List()
    for i = 1, #self.dataTb do
        print(i, self.dataTb[i])
    end
end
 
function Stack:Peek()
    if #self.dataTb~=0 then
        return self.dataTb[#self.dataTb]
    else
        return nil
    end
end

function Stack:Count()
    return #self.dataTb
end
 
return Stack