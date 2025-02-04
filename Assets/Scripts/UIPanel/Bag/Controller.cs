///
/// 此代码用于整个背包界面的控制
///
using System;
using UnityEngine;
using UnityEngine.UI;

public class BackPackCtrl : BasePanel
{
    ///UI控件 侧边栏按钮和关闭按钮
    private Transform UICloseButton;
    private Transform UIHeroButton;
    private Transform UICodexButton;
    //UI控件 英雄展示界面
    private Transform UIShowHero;
    private Transform UIHeroInfo_Grade;
    private Transform UIHeroInfo_Name;
    private Transform UIHeroInfo_Keyword;
    //UI控件 英雄背包界面
    private Transform UIHeroBag;
    private Transform UIHeroContent;
    //上栏
    private Transform UIPlusButton;
    private Transform UIReferrerButton;
    //下栏
    private Transform UIAllButton;
    private Transform UIForceButton;
    private Transform UIInnerForceButton;
    private Transform UIHealButton;
    private Transform UISwordButton;
    private Transform UISkillButton;
    //英雄上阵栏
    private Transform UIUPButton;
    private Transform UIDeployHero1;
    private Transform UIDeployHero2;
    private Transform UIDeployHero3;
    private Transform UIDeployHero4;
    private Transform UIDeployHero5;
    private Transform[] UIDeployButtonArr;
    //图鉴
    private Transform UICodex;
    //预制件子物体
    public GameObject HeroUIItemPrefab;
    public GameObject HeroAnimationPrefab;
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
            UIDeployButtonArr[_chooseBid - 1].GetComponent<DeployButtonView>().StopLighting();
            _chooseBid = value;
            if (GameManager.Instance.GetDeployHeroDic().ContainsKey(_chooseBid))
            {
                chooseUid = GameManager.Instance.GetDeployHeroDic()[_chooseBid].uid;
            }
            //找到对应的按键并使其发光
            UIDeployButtonArr[_chooseBid - 1].GetComponent<DeployButtonView>().Lighting();
        }
    }

    //方法
    override protected void Awake()
    {
        base.Awake();
        Init();
    }

    private void Start()
    {
        RefreshUI();
        RefreshDeployHero();
    }

    private void Init()
    {
        UIInit();
        ClickInit();
    }

    private void UIInit()
    {
        ///UI控件 侧边栏按钮和关闭按钮
        UICloseButton = transform.Find("CloseButton");
        UIHeroButton = transform.Find("ButtonList/HeroButton");
        UICodexButton = transform.Find("ButtonList/CodexButton");
        //UI控件 英雄展示界面
        UIShowHero = transform.Find("ShowHero");
        UIHeroInfo_Grade = transform.Find("ShowHero/HeroInfo/Grade");
        UIHeroInfo_Name = transform.Find("ShowHero/HeroInfo/Name");
        UIHeroInfo_Keyword = transform.Find("ShowHero/HeroInfo/KeyWord");
        //UI控件 英雄背包界面
        UIHeroBag = transform.Find("HeroBag");
        UIHeroContent = transform.Find("HeroBag/HeroContent");
        //上栏
        UIPlusButton = transform.Find("HeroBag/UpperWindow/Capcity/PlusButton");
        UIReferrerButton = transform.Find("HeroBag/UpperWindow/ReferrerButton");
        //下栏
        UIAllButton = transform.Find("HeroBag/DownWindow/All");
        UIForceButton = transform.Find("HeroBag/DownWindow/Force");
        UIInnerForceButton = transform.Find("HeroBag/DownWindow/InnerForce");
        UIHealButton = transform.Find("HeroBag/DownWindow/Heal");
        UISwordButton = transform.Find("HeroBag/DownWindow/Sword");
        UISkillButton = transform.Find("HeroBag/DownWindow/Skill");
        //图鉴
        UICodex = transform.Find("Codex");
        //上阵英雄栏
        UIUPButton = transform.Find("Deploy/DeployButton");
        UIDeployHero1 = transform.Find("Deploy/DeployList/DeployHero1");
        UIDeployHero2 = transform.Find("Deploy/DeployList/DeployHero2");
        UIDeployHero3 = transform.Find("Deploy/DeployList/DeployHero3");
        UIDeployHero4 = transform.Find("Deploy/DeployList/DeployHero4");
        UIDeployHero5 = transform.Find("Deploy/DeployList/DeployHero5");
        //添加到数组中方便查找
        UIDeployButtonArr = new Transform[5];
        UIDeployButtonArr[0] = UIDeployHero1;
        UIDeployButtonArr[1] = UIDeployHero2;
        UIDeployButtonArr[2] = UIDeployHero3;
        UIDeployButtonArr[3] = UIDeployHero4;
        UIDeployButtonArr[4] = UIDeployHero5;
        //页面初始化
        UICodex.gameObject.SetActive(false);
        UIHeroBag.gameObject.SetActive(true);
    }

    //以显示全部做初始化
    private void RefreshUI()
    {
        RefreshScrollView(HeroType.All);
    }

    private void RefreshDeployHero()
    {
        if(GameManager.Instance.GetDeployHeroDic().Count!=0)
        {
            for(int i=1;i<=5; i++)
            {
                if (GameManager.Instance.GetDeployHeroDic()[i]!=null)
                {
                    UIDeployButtonArr[i - 1].GetComponent<DeployButtonView>().Refresh(GameManager.Instance.GetDeployHeroDic()[i]);
                }
            }
        }
    }

    //刷新英雄列表
    private void RefreshScrollView(HeroType heroType)
    {
        //清理已有项
        RectTransform scrollContent = UIHeroContent.GetComponent<ScrollRect>().content;
        for(int i=0;i<scrollContent.childCount;i++)
        {
            Destroy(scrollContent.GetChild(i).gameObject);
        }
        //显示已获得英雄
        if (GameManager.Instance.GetSortHeroLocalData() == null) return;
        //分类显示
        foreach(HeroLocalItem HeroLocalData in GameManager.Instance.GetSortHeroLocalData())
        {
            switch (heroType)
            {
                case HeroType.All:
                    Transform HeroUIItem = Instantiate(HeroUIItemPrefab.transform, scrollContent) as Transform;
                    HeroCtrl heroCell = HeroUIItem.GetComponent<HeroCtrl>();
                    heroCell.Refresh(HeroLocalData, this);
                    break;
                case HeroType.Force:
                    if(GameManager.Instance.GetHeroById(HeroLocalData.id).type==HeroType.Force)
                    {
                        Transform HeroUIItem1 = Instantiate(HeroUIItemPrefab.transform, scrollContent) as Transform;
                        HeroCtrl heroCell1 = HeroUIItem1.GetComponent<HeroCtrl>();
                        heroCell1.Refresh(HeroLocalData, this);
                    }
                    break;
                case HeroType.InnerForce:
                    if (GameManager.Instance.GetHeroById(HeroLocalData.id).type == HeroType.InnerForce)
                    {
                        Transform HeroUIItem1 = Instantiate(HeroUIItemPrefab.transform, scrollContent) as Transform;
                        HeroCtrl heroCell1 = HeroUIItem1.GetComponent<HeroCtrl>();
                        heroCell1.Refresh(HeroLocalData, this);
                    }
                    break;
                case HeroType.Heal:
                    if (GameManager.Instance.GetHeroById(HeroLocalData.id).type == HeroType.Heal)
                    {
                        Transform HeroUIItem1 = Instantiate(HeroUIItemPrefab.transform, scrollContent) as Transform;
                        HeroCtrl heroCell1 = HeroUIItem1.GetComponent<HeroCtrl>();
                        heroCell1.Refresh(HeroLocalData, this);
                    }
                    break;
                case HeroType.Sword:
                    if (GameManager.Instance.GetHeroById(HeroLocalData.id).type == HeroType.Sword)
                    {
                        Transform HeroUIItem1 = Instantiate(HeroUIItemPrefab.transform, scrollContent) as Transform;
                        HeroCtrl heroCell1 = HeroUIItem1.GetComponent<HeroCtrl>();
                        heroCell1.Refresh(HeroLocalData, this);
                    }
                    break;
                case HeroType.Skill:
                    if (GameManager.Instance.GetHeroById(HeroLocalData.id).type == HeroType.Skill)
                    {
                        Transform HeroUIItem1 = Instantiate(HeroUIItemPrefab.transform, scrollContent) as Transform;
                        HeroCtrl heroCell1 = HeroUIItem1.GetComponent<HeroCtrl>();
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
        HeroLocalItem HeroLocalData = GameManager.Instance.GetHeroLocalDataByUId(chooseUid);
        //刷新展示页
        UIShowHero.GetComponent<HeroDetail>().Refresh(HeroLocalData, this);
    }

    private void ClickInit()
    {
        //注册按键事件
        UICloseButton.GetComponent<Button>().onClick.AddListener(OnClickClose);
        UIHeroButton.GetComponent<Button>().onClick.AddListener(OnClickHero);
        UICodexButton.GetComponent<Button>().onClick.AddListener(OnClickCodex);
        UIShowHero.GetComponent<Button>().onClick.AddListener(OnClickAttack);
        UIPlusButton.GetComponent<Button>().onClick.AddListener(OnClickPlus);
        UIReferrerButton.GetComponent<Button>().onClick.AddListener(OnClickReferrer);
        UIAllButton.GetComponent<Button>().onClick.AddListener(OnClickAll);
        UIForceButton.GetComponent<Button>().onClick.AddListener(OnClickForce);
        UIInnerForceButton.GetComponent<Button>().onClick.AddListener(OnClickInnerForce);
        UIHealButton.GetComponent<Button>().onClick.AddListener(OnClickHeal);
        UISwordButton.GetComponent<Button>().onClick.AddListener(OnClickSword);
        UISkillButton.GetComponent<Button>().onClick.AddListener(OnClickSkill);
        UIUPButton.GetComponent<Button>().onClick.AddListener(UpdateDeployHero);
    }

    private void OnClickClose()
    {
        UIManager.Instance.ClosePanel(UIConst.HeroBackPack);
    }

    private void OnClickHero()
    {
        UICodex.gameObject.SetActive(false);
        UIHeroBag.gameObject.SetActive(true);
    }

    private void OnClickCodex()
    {
        UIHeroBag.gameObject.SetActive(false);
        UICodex.gameObject.SetActive(true);
    }

    private void OnClickAttack()
    {
        print("animation->attack");
        UIShowHero.GetComponent<HeroDetail>().Attack();
    }

    private void OnClickPlus()
    {
        print("plus capcity");
    }

    private void OnClickReferrer()
    {
        print("show referrer");
    }

    private void OnClickAll()
    {
        print("filter all");
        RefreshScrollView(HeroType.All);
    }

    private void OnClickForce()
    {
        print("filter force");
        RefreshScrollView(HeroType.Force);
    }

    private void OnClickInnerForce()
    {
        print("filter innerforce");
        RefreshScrollView(HeroType.InnerForce);
    }

    private void OnClickHeal()
    {
        print("filter heal");
        RefreshScrollView(HeroType.Heal);
    }

    private void OnClickSword()
    {
        print("filter sword");
        RefreshScrollView(HeroType.Sword);
    }

    private void OnClickSkill()
    {
        print("filter skill");
        RefreshScrollView(HeroType.Skill);
    }

    private void UpdateDeployHero()
    {
        //添加英雄
        if(GameManager.Instance.GetHeroLocalDataByUId(chooseUid) != null && GameManager.Instance.GetHeroLocalDataByUId(chooseUid).IsDeploy!=true)
        {
            GameManager.Instance.GetHeroLocalDataByUId(chooseUid).IsDeploy = true;
            int oriID = GameManager.Instance.AddDeployHero(chooseBid, GameManager.Instance.GetHeroLocalDataByUId(chooseUid));
            //找到对应按键 替换显示页
            if(oriID==0)
            {
                UIDeployButtonArr[chooseBid - 1].GetComponent<DeployButtonView>().Refresh(GameManager.Instance.GetHeroLocalDataByUId(chooseUid));
            }
            else
            {
                UIDeployButtonArr[oriID - 1].GetComponent<DeployButtonView>().Refresh(GameManager.Instance.GetHeroLocalDataByUId(chooseUid));
                chooseBid = oriID;
            }
        }

    }
}
