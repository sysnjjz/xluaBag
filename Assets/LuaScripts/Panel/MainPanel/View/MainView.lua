MainView = BaseClass("MainView",BasePanel)

--重写回调
function MainView:__callBack(res)
    -- 更改属性
    self.controlPanel=GameObject.Instantiate(res,self.uiRoot)
    self.transform=self.controlPanel.transform
    self.controlPanel:SetActive(true)
    self.isDoneLoading=true

    -- 获取组件
    self.btmCard=self.transform:Find("Card"):GetComponent("Button")
    self.btmExit=self.transform:Find("Exit"):GetComponent("Button")
    self.btmBag=self.transform:Find("Bag"):GetComponent("Button")

    -- 绑定事件
    self.btmCard.onClick:AddListener(function()
        UIManager:Instance():OpenUI("Card")
    end)
    self.btmExit.onClick:AddListener(function()
        LuaBridge.ExitApplication()
    end)
    self.btmBag.onClick:AddListener(function()
        UIManager:Instance():OpenUI("Bag")
    end)

    return true
end