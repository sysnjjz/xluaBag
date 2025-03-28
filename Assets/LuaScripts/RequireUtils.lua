function requireModule()
    local inPathList=CS.IOUtils.luaCallRequire()

    for i=0,inPathList.Count-1 do
        require(tostring(inPathList[i]))
    end
end