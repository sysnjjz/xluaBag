using System;
using UnityEngine;

///
/// 用于整个背包界面的控制
/// 我对于MVC模式的理解是，Controller是沟通view和model的，view和model之间不直接调用对方，而是提供方法等controller调用
/// 根据对于反馈的理解，controller还要负责view的创建和显示
/// 所以下述controller中有创建和显示view的方法，以及页面的初始化方法中需要调用model中数据的方法
///
public class BagController
{
    //页面
    private HeroWholePageWindow WholePageView;

    //当前选中的格子
    private string _chooseUid = Convert.ToString(-1);
    public string chooseUid
    {
        get
        {
            return _chooseUid;
        }
        set
        {
            _chooseUid = value;
            RefreshDetail();
        }
    }
    //当前选中的上阵英雄栏
    private int _chooseBid = 1;
    public int chooseBid
    {
        get
        {
            return _chooseBid;
        }
        set
        {
            //使原来的按键不发光
            WholePageView.UIDeployButtonArr[_chooseBid - 1].StopLighting();
            _chooseBid = value;
            if (HeroDeployedData.Instance.LoadDeployData().ContainsKey(_chooseBid))
            {
                chooseUid = HeroDeployedData.Instance.LoadDeployData()[_chooseBid].uid;
            }
            //找到对应的按键并使其发光
            WholePageView.UIDeployButtonArr[_chooseBid - 1].Lighting();
        }
    }

    public BagController()
    {
        //缓存及创建界面
        CreatePanel();
        ShowPanel();
        //初始化
        Init();
        //依赖于model中数据的界面初始化
        RefreshScrollView(HeroType.All);
        RefreshDeployHero();
        RefreshDetail();
    }

    //控制器初始化
    public void Init()
    {
        if (HeroDeployedData.Instance.LoadDeployData().ContainsKey(1) && HeroDeployedData.Instance.LoadDeployData()[1] != null)
        {
            chooseUid = HeroDeployedData.Instance.LoadDeployData()[1].uid;
        }
        else if (HeroLocalData.Instance.LoadData().Count != 0)
        {
            chooseUid = HeroLocalData.Instance.LoadData()[0].uid;
        }
    }

    //缓存界面
    public void CreatePanel()
    {
        WholePageView=new HeroWholePageWindow();
        GameObject bagPagePrefab = null;
        if (!UIManager.Instance.prefabDict.TryGetValue(UIManager.Instance.pathDict[UIConst.HeroBackPack], out bagPagePrefab))
        {
            WholePageView.BeforeInit(UIManager.Instance.pathDict[UIConst.HeroBackPack], "Prefab/Panel" + UIManager.Instance.pathDict[UIConst.HeroBackPack]);
        }
        else
        {
            WholePageView.gameObject = bagPagePrefab;
        }
    }

    //创建界面
    public void ShowPanel()
    {
        WholePageView.OpenPanel(UIManager.Instance.UIRoot);
        WholePageView.controller = this;
        WholePageView.Init();
    }

    //关闭界面
    public void ClosePanel()
    {
        UIManager.Instance.ClosePanel(UIConst.HeroBackPack);
    }

    //刷新上阵英雄
    //使用model中的数据刷新上阵英雄按钮的view显示
    public void RefreshDeployHero()
    {
        //找到对应按钮
        if (HeroDeployedData.Instance.LoadDeployData().Count!=0)
        {
            for(int i=1;i<=5; i++)
            {
                if (HeroDeployedData.Instance.LoadDeployData().ContainsKey(i)&&HeroDeployedData.Instance.LoadDeployData()[i]!=null)
                {
                    //刷新界面
                    WholePageView.UIDeployButtonArr[i - 1].Refresh(HeroDeployedData.Instance.LoadDeployData()[i], HeroStaticData.Instance.GetHeroById(HeroDeployedData.Instance.LoadDeployData()[i].id));
                }
            }
        }
    }

    //刷新上阵英雄
    //使用model中的数据替换上阵英雄的view显示
    public void UpdateDeployHero()
    {
        //添加英雄
        if (HeroLocalData.Instance.GetHeroLocalDataByUId(chooseUid) != null && HeroLocalData.Instance.GetHeroLocalDataByUId(chooseUid).IsDeploy != true)
        {
            int oriID = HeroDeployedData.Instance.AddDeployHero(chooseBid, HeroLocalData.Instance.GetHeroLocalDataByUId(chooseUid));
            //找到对应按键 替换显示页
            if (oriID == 0)
            {
                WholePageView.UIDeployButtonArr[chooseBid - 1].Refresh(HeroLocalData.Instance.GetHeroLocalDataByUId(chooseUid), HeroStaticData.Instance.GetHeroById(HeroLocalData.Instance.GetHeroLocalDataByUId(chooseUid).id));
            }
            else
            {
                WholePageView.UIDeployButtonArr[oriID - 1].Refresh(HeroLocalData.Instance.GetHeroLocalDataByUId(chooseUid), HeroStaticData.Instance.GetHeroById(HeroLocalData.Instance.GetHeroLocalDataByUId(chooseUid).id));
                chooseBid = oriID;
            }
        }
    }

    //刷新详情页
    //使用model中的数据刷新上阵英雄详情页的view显示
    public void RefreshDetail()
    {
        //找到对应的动态数据
        HeroLocalItem heroLocalData = HeroLocalData.Instance.GetHeroLocalDataByUId(chooseUid);
        //为空返回
        if (heroLocalData == null) return;
        //刷新展示页
        WholePageView.RefreshHeroDetail(heroLocalData, HeroStaticData.Instance.GetHeroById(heroLocalData.id));
    }

    //刷新英雄列表(这里因为model和view的调用都有 拆不太开 显得很复杂 不知道要不要调整一下)
    //使用model中的数据刷新英雄列表的view显示
    public void RefreshScrollView(HeroType heroType)
    {
        //清理已有项
        RectTransform scrollContent = WholePageView.UIHeroContent.content;
        for (int i = 0; i < scrollContent.childCount; i++)
        {
            UnityEngine.Object.Destroy(scrollContent.GetChild(i).gameObject);
        }
        //为空返回
        if (HeroLocalData.Instance.GetSortHeroLocalData() == null) return;
        //分类显示
        foreach (HeroLocalItem HeroLocalData in HeroLocalData.Instance.GetSortHeroLocalData())
        {
            switch (heroType)
            {
                case HeroType.All:
                    HeroLeftSideWindow heroCell = new HeroLeftSideWindow(WholePageView.HeroUIItemPrefab, scrollContent);
                    heroCell.Refresh(HeroLocalData, HeroStaticData.Instance.GetHeroById(HeroLocalData.id),this);
                    break;
                case HeroType.Force:
                    if (HeroStaticData.Instance.GetHeroById(HeroLocalData.id).type == HeroType.Force)
                    {
                        HeroLeftSideWindow heroCellForce = new HeroLeftSideWindow(WholePageView.HeroUIItemPrefab, scrollContent);
                        heroCellForce.Refresh(HeroLocalData, HeroStaticData.Instance.GetHeroById(HeroLocalData.id), this);
                    }
                    break;
                case HeroType.InnerForce:
                    if (HeroStaticData.Instance.GetHeroById(HeroLocalData.id).type == HeroType.InnerForce)
                    {
                        HeroLeftSideWindow heroCellInner = new HeroLeftSideWindow(WholePageView.HeroUIItemPrefab, scrollContent);
                        heroCellInner.Refresh(HeroLocalData, HeroStaticData.Instance.GetHeroById(HeroLocalData.id), this);
                    }
                    break;
                case HeroType.Heal:
                    if (HeroStaticData.Instance.GetHeroById(HeroLocalData.id).type == HeroType.Heal)
                    {
                        HeroLeftSideWindow heroCellHeal = new HeroLeftSideWindow(WholePageView.HeroUIItemPrefab, scrollContent);
                        heroCellHeal.Refresh(HeroLocalData, HeroStaticData.Instance.GetHeroById(HeroLocalData.id), this);
                    }
                    break;
                case HeroType.Sword:
                    if (HeroStaticData.Instance.GetHeroById(HeroLocalData.id).type == HeroType.Sword)
                    {
                        HeroLeftSideWindow heroCellSword = new HeroLeftSideWindow(WholePageView.HeroUIItemPrefab, scrollContent);
                        heroCellSword.Refresh(HeroLocalData, HeroStaticData.Instance.GetHeroById(HeroLocalData.id), this);
                    }
                    break;
                case HeroType.Skill:
                    if (HeroStaticData.Instance.GetHeroById(HeroLocalData.id).type == HeroType.Skill)
                    {
                        HeroLeftSideWindow heroCellSkill = new HeroLeftSideWindow(WholePageView.HeroUIItemPrefab, scrollContent);
                        heroCellSkill.Refresh(HeroLocalData, HeroStaticData.Instance.GetHeroById(HeroLocalData.id), this);
                    }
                    break;
            }
        }
    }
}
