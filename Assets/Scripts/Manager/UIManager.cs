using System.Collections.Generic;
using UnityEngine;

///
/// UI管理器，统一管理游戏中会出现的各类UI
///
public class UIManager
{
    //单例
    private static UIManager instance;

    private Transform uiRoot;

    //路径配置字典
    public Dictionary<string, string> pathDict;
    //预制件缓存字典
    public Dictionary<string, GameObject> prefabDict;
    //已打开界面缓存
    public Dictionary<string, BasePanel> panelDict;
    //已打开界面堆栈
    public Stack<BasePanel> panelStack;

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
    public void OpenPanel(string name)
    {
        //已经打开的界面不打开
        BasePanel panel = null;
        if (panelDict.TryGetValue(name, out panel))
        {
            return;
        }

        //找不到界面也不打开
        string path = "";
        if (!pathDict.TryGetValue(name, out path))
        {
            return;
        }

        //设置下层界面不可见
        if (panelStack.Count != 0)
        {
            panelStack.Peek().SetOnShow(false);
        }

        //生成对应控制器
        UIFactory factory = new UIFactory();
        factory.CreateController(name);
    }

    //关闭界面
    public bool ClosePanel(string name)
    {
        //没打开的不关
        BasePanel panel = null;
        if (!panelDict.TryGetValue(name, out panel))
        {
            return false;
        }

        //关闭对应界面
        BasePanel nowPanel= panelStack.Pop();
        nowPanel.ClosePanel();

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
    public void CreateController(string name)
    {
        switch (name)
        {
            case UIConst.MainMenu:
                MainPageController mainPageController = new MainPageController();
                break;

            case UIConst.HeroBackPack:
                BagController bagController = new BagController();
                break;

            case UIConst.DrawCard:
                CardController cardController = new CardController();
                break;
            default:
                break;
        }
    }
}
