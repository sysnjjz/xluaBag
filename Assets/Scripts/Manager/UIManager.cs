using System.Collections.Generic;
using UnityEngine;

///
/// UI管理器，统一管理游戏中会出现的各类UI
///
public class UIManager
{
    //单例
    private static UIManager instance;

    public Transform uiRoot;

    //路径配置字典
    private Dictionary<string, string> pathDict;
    //预制件缓存字典
    public Dictionary<string, GameObject> prefabDict;
    //已打开界面缓存
    public Dictionary<string, BasePanel> panelDict;
    //已打开界面堆栈
    private Stack<BasePanel> panelStack;

    private UIManager()
    {
        prefabDict = new Dictionary<string, GameObject>();
        panelDict = new Dictionary<string, BasePanel>();
        pathDict = new Dictionary<string, string>()
        {
            { UIConst.HeroBackPack,"/HeroBackPack"},
            { UIConst.DrawCard,"/DrawCard"},
            { UIConst.MainMenu,"/MainMenu"}
        };
        panelStack = new Stack<BasePanel>();

    }
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UIManager();
            }
            return instance;
        }
    }

    public Transform UIRoot
    {
        get
        {
            if (uiRoot == null)
            {
                if (GameObject.Find("Canvas"))
                {
                    uiRoot = GameObject.Find("Canvas").transform;
                }
                else
                {
                    uiRoot = new GameObject("Canvas").transform;
                }

            }
            return uiRoot;
        }
    }

    //打开界面
    public BasePanel OpenPanel(string name)
    {
        BasePanel panel = null;

        //已经打开的界面不打开
        if (panelDict.TryGetValue(name, out panel))
        {
            return null;
        }
        //找不到界面也不打开
        string path = "";
        if (!pathDict.TryGetValue(name, out path))
        {
            return null;
        }

        //生成界面
        UIFactory factory = new UIFactory();
        panel = factory.CreatePanel(name, path);

        //设置下层界面不可见
        if (panelStack.Count != 0)
        {
            panelStack.Peek().SetOnShow(false);
        }
        panelStack.Push(panel);

        return panel;
    }

    //关闭界面
    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        //没打开的不关
        if (!panelDict.TryGetValue(name, out panel))
        {
            return false;
        }

        panelStack.Peek().IsRemove = true;
        panelStack.Peek().SetOnShow(false);
        UnityEngine.Object.Destroy(panelStack.Pop().gameObject);

        //设置下层界面可见
        if (panelStack.Count != 0)
        {
            panelStack.Peek().SetOnShow(true);
        }

        //移除缓存
        if (panelDict.ContainsKey(name))
        {
            panelDict.Remove(name);
        }

        return true;
    }
}

public class UIConst
{
    public const string HeroBackPack = "HeroBackPack";
    public const string DrawCard = "DrawCard";
    public const string MainMenu = "MainMenu";
} 

public class UIFactory
{
    public BasePanel CreatePanel(string name,string path)
    {
        switch (name)
        {
            case UIConst.MainMenu:
                MainPageView mainpagepanel = new MainPageView();

                GameObject mainpagepanelPrefab = null;
                if (!UIManager.Instance.prefabDict.TryGetValue(name, out mainpagepanelPrefab))
                {
                    mainpagepanel.BeforeInit(name, "Prefab/Panel" + path);
                }
                else
                {
                    mainpagepanel.gameObject = mainpagepanelPrefab;
                }

                //打开界面
                mainpagepanel.Initial(name, UIManager.Instance.UIRoot);

                MainPageController mainPageController = new MainPageController(mainpagepanel);

                return mainpagepanel;

            case UIConst.HeroBackPack:
                HeroWholePageWindow bagPanel = new HeroWholePageWindow();

                GameObject bagPanelPrefab = null;
                if (!UIManager.Instance.prefabDict.TryGetValue(name, out bagPanelPrefab))
                {
                    bagPanel.BeforeInit(name, "Prefab/Panel" + path);
                }
                else
                {
                    bagPanel.gameObject = bagPanelPrefab;
                }

                //打开界面
                bagPanel.Initial(name, UIManager.Instance.UIRoot);

                BagController bagController = new BagController(bagPanel);

                return bagPanel;

            case UIConst.DrawCard:
                CardView cardPanel = new CardView();

                GameObject cardPanelPrefab = null;
                if (!UIManager.Instance.prefabDict.TryGetValue(name, out cardPanelPrefab))
                {
                    cardPanel.BeforeInit(name, "Prefab/Panel" + path);
                }
                else
                {
                    cardPanel.gameObject = cardPanelPrefab;
                }

                //打开界面
                cardPanel.Initial(name, UIManager.Instance.UIRoot);

                CardController cardController = new CardController(cardPanel);

                return cardPanel;
            default:
                return null;
        }
    }
}
