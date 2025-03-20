local MainController=require("Panel.MainPanel.Controller.MainController")
local CardController=require("Panel.CardPanel.Controller.CardController")
local BagController=require("Panel.BagPanel.Controller.BagController")
--name resources下的path controller
UIConfigDic = 
{
    ["Main"]={name="Main",path="Prefab/Panel/Main",controller=MainController},
    ["Card"]={name="Card",path="Prefab/Panel/Card",controller=CardController},
    ["Bag"]={name="Bag",path="Prefab/Panel/Bag",controller=BagController}
}