using System;
using UnityEngine;

///
/// 用于整个背包界面的控制
///
public class BagController : MonoBehaviour
{
    //预制件子物体
    public GameObject HeroUIItemPrefab;
    public GameObject HeroAnimationPrefab;

    //页面
    private HeroWholePageWindow WholePageView;

    //当前选中的格子
    private string _chooseUid=Convert.ToString(-1);
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

    private void Awake()
    {
        Init();
        Initial();
    }

    private void Init()
    {
        WholePageView = GetComponent<HeroWholePageWindow>();
    }

    //初始化界面
    private void Initial()
    {
        if(HeroDeployedData.Instance.LoadDeployData().ContainsKey(1)&& HeroDeployedData.Instance.LoadDeployData()[1]!=null)
        {
            chooseUid = HeroDeployedData.Instance.LoadDeployData()[1].uid;
        }
        else if (HeroLocalData.Instance.LoadData().Count != 0)
        {
            chooseUid = HeroLocalData.Instance.LoadData()[0].uid;
        }
    }

    //方法
    private void Start()
    {
        RefreshScrollView(HeroType.All);
        RefreshDeployHero();
    }

    //刷新上阵英雄
    private void RefreshDeployHero()
    {
        if(HeroDeployedData.Instance.LoadDeployData().Count!=0)
        {
            for(int i=1;i<=5; i++)
            {
                if (HeroDeployedData.Instance.LoadDeployData().ContainsKey(i)&&HeroDeployedData.Instance.LoadDeployData()[i]!=null)
                {
                    WholePageView.UIDeployButtonArr[i - 1].Refresh(HeroDeployedData.Instance.LoadDeployData()[i]);
                }
            }
        }
    }

    //刷新英雄列表
    private void RefreshScrollView(HeroType heroType)
    {
        //清理已有项
        RectTransform scrollContent = WholePageView.UIHeroContent.content;
        for(int i=0;i<scrollContent.childCount;i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }
        //显示已获得英雄
        if (HeroLocalData.Instance.GetSortHeroLocalData() == null) return;
        //分类显示
        foreach(HeroLocalItem HeroLocalData in HeroLocalData.Instance.GetSortHeroLocalData())
        {
            switch (heroType)
            {
                case HeroType.All:
                    Transform HeroUIItem = Instantiate(HeroUIItemPrefab.transform, scrollContent) as Transform;
                    HeroLeftSideWindow heroCell = HeroUIItem.GetComponent<HeroLeftSideWindow>();
                    heroCell.Refresh(HeroLocalData, this);
                    break;
                case HeroType.Force:
                    if(HeroStaticData.Instance.GetHeroById(HeroLocalData.id).type==HeroType.Force)
                    {
                        Transform HeroUIItem1 = Instantiate(HeroUIItemPrefab.transform, scrollContent) as Transform;
                        HeroLeftSideWindow heroCell1 = HeroUIItem1.GetComponent<HeroLeftSideWindow>();
                        heroCell1.Refresh(HeroLocalData, this);
                    }
                    break;
                case HeroType.InnerForce:
                    if (HeroStaticData.Instance.GetHeroById(HeroLocalData.id).type == HeroType.InnerForce)
                    {
                        Transform HeroUIItem1 = Instantiate(HeroUIItemPrefab.transform, scrollContent) as Transform;
                        HeroLeftSideWindow heroCell1 = HeroUIItem1.GetComponent<HeroLeftSideWindow>();
                        heroCell1.Refresh(HeroLocalData, this);
                    }
                    break;
                case HeroType.Heal:
                    if (HeroStaticData.Instance.GetHeroById(HeroLocalData.id).type == HeroType.Heal)
                    {
                        Transform HeroUIItem1 = Instantiate(HeroUIItemPrefab.transform, scrollContent) as Transform;
                        HeroLeftSideWindow heroCell1 = HeroUIItem1.GetComponent<HeroLeftSideWindow>();
                        heroCell1.Refresh(HeroLocalData, this);
                    }
                    break;
                case HeroType.Sword:
                    if (HeroStaticData.Instance.GetHeroById(HeroLocalData.id).type == HeroType.Sword)
                    {
                        Transform HeroUIItem1 = Instantiate(HeroUIItemPrefab.transform, scrollContent) as Transform;
                        HeroLeftSideWindow heroCell1 = HeroUIItem1.GetComponent<HeroLeftSideWindow>();
                        heroCell1.Refresh(HeroLocalData, this);
                    }
                    break;
                case HeroType.Skill:
                    if (HeroStaticData.Instance.GetHeroById(HeroLocalData.id).type == HeroType.Skill)
                    {
                        Transform HeroUIItem1 = Instantiate(HeroUIItemPrefab.transform, scrollContent) as Transform;
                        HeroLeftSideWindow heroCell1 = HeroUIItem1.GetComponent<HeroLeftSideWindow>();
                        heroCell1.Refresh(HeroLocalData, this);
                    }
                    break;
            }
        }
    }

    //刷新详情页
    private void RefreshDetail()
    {
        //找到对应的动态数据
        HeroLocalItem heroLocalData = HeroLocalData.Instance.GetHeroLocalDataByUId(chooseUid);
        //刷新展示页
        WholePageView.RefreshHeroDetail(heroLocalData);
    }

    //按键方法
    public void OnClickClose()
    {
        UIManager.Instance.ClosePanel(UIConst.HeroBackPack);
    }

    public void OnClickHero()
    {
        WholePageView.UICodex.gameObject.SetActive(false);
        WholePageView.UIHeroBag.gameObject.SetActive(true);
    }

    public void OnClickCodex()
    {
        WholePageView.UIHeroBag.gameObject.SetActive(false);
        WholePageView.UICodex.gameObject.SetActive(true);
    }

    public void OnClickPlus()
    {
        print("plus capcity");
    }

    public void OnClickReferrer()
    {
        print("show referrer");
    }

    public void OnClickAll()
    {
        print("filter all");
        RefreshScrollView(HeroType.All);
    }

    public void OnClickForce()
    {
        print("filter force");
        RefreshScrollView(HeroType.Force);
    }

    public void OnClickInnerForce()
    {
        print("filter innerforce");
        RefreshScrollView(HeroType.InnerForce);
    }

    public void OnClickHeal()
    {
        print("filter heal");
        RefreshScrollView(HeroType.Heal);
    }

    public void OnClickSword()
    {
        print("filter sword");
        RefreshScrollView(HeroType.Sword);
    }

    public void OnClickSkill()
    {
        print("filter skill");
        RefreshScrollView(HeroType.Skill);
    }

    public void UpdateDeployHero()
    {
        //添加英雄
        if(HeroLocalData.Instance.GetHeroLocalDataByUId(chooseUid) != null && HeroLocalData.Instance.GetHeroLocalDataByUId(chooseUid).IsDeploy!=true)
        {
            int oriID = HeroDeployedData.Instance.AddDeployHero(chooseBid, HeroLocalData.Instance.GetHeroLocalDataByUId(chooseUid));
            //找到对应按键 替换显示页
            if(oriID==0)
            {
                WholePageView.UIDeployButtonArr[chooseBid - 1].Refresh(HeroLocalData.Instance.GetHeroLocalDataByUId(chooseUid));
            }
            else
            {
                WholePageView.UIDeployButtonArr[oriID - 1].Refresh(HeroLocalData.Instance.GetHeroLocalDataByUId(chooseUid));
                chooseBid = oriID;
            }
        }

    }
}
