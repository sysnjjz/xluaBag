using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

///
/// UI管理器，统一管理游戏中会出现的各类UI
///
public class UIManager
{
    //单例
    private static UIManager instance;

    private Transform uiRoot;

    //路径配置字典
    private Dictionary<string, string> pathDict;
    //预制件缓存字典
    private Dictionary<string, GameObject> prefabDict;
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
            if(instance==null)
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
            if(uiRoot==null)
            {
                if(GameObject.Find("Canvas"))
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
        //已经打开的界面
        if(panelDict.TryGetValue(name,out panel))
        {
            return null;
        }

        //找不到界面
        string path = "";
        if(!pathDict.TryGetValue(name,out path))
        {
            return null;
        }

        //使用预制体创造界面
        GameObject panelPrefab = null;
        if(!prefabDict.TryGetValue(name,out panelPrefab))
        {
            string realPath = "Prefab/Panel" + path;
            panelPrefab = Resources.Load<GameObject>(realPath) as GameObject;
            prefabDict.Add(name, panelPrefab );
        }

        //打开界面
        GameObject panelObject = GameObject.Instantiate(panelPrefab, UIRoot, false);
        //设置UI缩放模式
        if (panelObject.GetComponent<CanvasScaler>()==null)
        {
            panelObject.AddComponent<CanvasScaler>();
        }
        panelObject.GetComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        panelObject.GetComponent<CanvasScaler>().referenceResolution = new Vector2(1920, 1080);
        panelObject.GetComponent<CanvasScaler>().matchWidthOrHeight = 0.5f;
        panel = panelObject.GetComponent<BasePanel>();
        panelDict.Add(name, panel );

        //设置下层界面不可见
        if (panelStack.Count != 0)
        {
            panelStack.Peek().SetActive(false);
        }
        panelStack.Push(panel);

        return panel;
    }

    //关闭界面
    public bool ClosePanel(string name)
    {
        BasePanel panel = null;
        //没打开的不关
        if(!panelDict.TryGetValue(name,out panel))
        {
            return false;
        }

        panelStack.Pop();
        //设置下层界面可见
        if (panelStack.Count != 0)
        {
            panelStack.Peek().SetActive(true);
        }
        panel.ClosePanel(name);

        return true;
    }
}

public class UIConst
{
    public const string HeroBackPack = "HeroBackPack";
    public const string DrawCard = "DrawCard";
    public const string MainMenu = "MainMenu";
}



