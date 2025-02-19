using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 英雄背包的主要视图逻辑
/// </summary>
public class HeroWholePageWindow : BasePanel
{
    //最左侧按键界面
    //UI控件 侧边栏按钮和关闭按钮
    private Transform UICloseButton;
    private Transform UIHeroButton;
    private Transform UICodexButton;

    //次左侧图鉴或背包界面
    //图鉴
    public Transform UICodex;
    //英雄背包界面
    public Transform UIHeroBag;
    //上栏
    private Transform UIPlusButton;
    private Transform UIReferrerButton;
    //中间 英雄背包界面
    public ScrollRect UIHeroContent;
    //下栏
    private Transform UIAllButton;
    private Transform UIForceButton;
    private Transform UIInnerForceButton;
    private Transform UIHealButton;
    private Transform UISwordButton;
    private Transform UISkillButton;

    //右上侧英雄展示页面
    public Button UIShowHero;
    public Transform UIHero;
    private Text UIHeroInfo_GradeText;
    private Text UIHeroInfo_NameText;
    private Text UIHeroInfo_KeyWordText;
    private Text UIATKText;
    private Transform Star;
    //子控件生成位置
    Vector3 ChildPosition;
    //当前显示预制体
    private GameObject NowPrefab;
    //右下侧英雄上阵栏
    private Transform UIUPButton;
    public Transform UIDeployList;

    public HeroDownWindow[] UIDeployButtonArr;

    //预制件子物体
    public GameObject HeroUIItemPrefab;
    public GameObject HeroDeployItem;

    //控制器
    public BagController controller;
    
    //打开界面
    public override void OpenPanel(Transform uiRoot)
    {
        base.OpenPanel(uiRoot);
        UIManager.Instance.panelDict.Add(UIConst.HeroBackPack, this);
    }

    //初始化
    public void Init()
    {
        UIInit();
        ClickInit();
    }

    private void UIInit()
    {
        //最左侧按键界面
        //UI控件 侧边栏按钮和关闭按钮
        UICloseButton = transform.Find("CloseButton");
        UIHeroButton = transform.Find("ButtonList/HeroButton");
        UICodexButton = transform.Find("ButtonList/CodexButton");

        //次左侧图鉴或背包界面
        //图鉴
        UICodex = transform.Find("Codex");
        //英雄背包界面
        UIHeroBag = transform.Find("HeroBag");
        //上栏
        UIPlusButton = transform.Find("HeroBag/UpperWindow/Capcity/PlusButton");
        UIReferrerButton = transform.Find("HeroBag/UpperWindow/ReferrerButton");
        //中间 英雄背包界面
        UIHeroContent = transform.Find("HeroBag/HeroContent").GetComponent<ScrollRect>();
        //下栏
        UIAllButton = transform.Find("HeroBag/DownWindow/All");
        UIForceButton = transform.Find("HeroBag/DownWindow/Force");
        UIInnerForceButton = transform.Find("HeroBag/DownWindow/InnerForce");
        UIHealButton = transform.Find("HeroBag/DownWindow/Heal");
        UISwordButton = transform.Find("HeroBag/DownWindow/Sword");
        UISkillButton = transform.Find("HeroBag/DownWindow/Skill");

        //右上侧英雄展示页面
        UIShowHero = transform.Find("ShowHero").GetComponent<Button>();
        UIHero = transform.Find("ShowHero/Hero");
        UIHeroInfo_GradeText = transform.Find("ShowHero/HeroInfo/Grade").GetComponent<Text>();
        UIHeroInfo_NameText = transform.Find("ShowHero/HeroInfo/Name").GetComponent<Text>();
        UIHeroInfo_KeyWordText = transform.Find("ShowHero/HeroInfo/KeyWord").GetComponent<Text>();
        Star = transform.Find("ShowHero/HeroInfo/Star");
        UIATKText = transform.Find("ShowHero/ATK").GetComponent<Text>();
        ChildPosition = UIHero.gameObject.GetComponent<RectTransform>().transform.position;

        //右下侧英雄上阵栏
        UIUPButton = transform.Find("Deploy/DeployButton");
        UIDeployList= transform.Find("Deploy/DeployList");

        //预制件初始化
        HeroUIItemPrefab = Resources.Load<GameObject>("UI/HeroDetail");
        HeroDeployItem = Resources.Load<GameObject>("UI/DeployHero");

        //初始化上阵栏数组
        UIDeployButtonArr = new HeroDownWindow[5];
        //生成五个按钮并加入队列
        for (int i = 0; i < 5; i++)
        {
            HeroDownWindow heroDownWindow = new HeroDownWindow(HeroDeployItem, UIDeployList, i + 1);
            heroDownWindow.bagController = controller;
            UIDeployButtonArr[i] = heroDownWindow;
        }

        //页面初始化
        UICodex.gameObject.SetActive(false);
        UIHeroBag.gameObject.SetActive(true);
    }

    private void ClickInit()
    {
        //注册按键事件
        //关闭整个页面
        UICloseButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            controller.ClosePanel();
        });

        //内部显示逻辑
        UIHeroButton.GetComponent<Button>().onClick.AddListener(OnClickHero);
        UICodexButton.GetComponent<Button>().onClick.AddListener(OnClickCodex);

        //内部交互逻辑
        //动画展示
        UIShowHero.onClick.AddListener(Attack);
        //内部界面显示逻辑
        UIPlusButton.GetComponent<Button>().onClick.AddListener(OnClickPlus);
        UIReferrerButton.GetComponent<Button>().onClick.AddListener(OnClickReferrer);
        //内部按键交互逻辑
        UIAllButton.GetComponent<Button>().onClick.AddListener(OnClickAll);
        UIForceButton.GetComponent<Button>().onClick.AddListener(OnClickForce);
        UIInnerForceButton.GetComponent<Button>().onClick.AddListener(OnClickInnerForce);
        UIHealButton.GetComponent<Button>().onClick.AddListener(OnClickHeal);
        UISwordButton.GetComponent<Button>().onClick.AddListener(OnClickSword);
        UISkillButton.GetComponent<Button>().onClick.AddListener(OnClickSkill);
        //使用model层数据刷新英雄
        UIUPButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            controller.UpdateDeployHero();
        });
    }

    public void RefreshHeroDetail(HeroLocalItem heroLocalItem,Hero hero)
    {
        //稀有度信息
        UIHeroInfo_GradeText.text = hero.rarity.ToString();
        //攻击力
        UIATKText.text = "ATK:" + heroLocalItem.ATK.ToString();
        //显示名称
        UIHeroInfo_NameText.text = hero.name.ToString();
        //显示关键词
        UIHeroInfo_KeyWordText.text = hero.keyword.ToString();

        ///显示预制体
        //清除原有预制体
        for (int i = 0; i < UIHero.childCount; i++)
        {
            GameObject.Destroy(UIHero.GetChild(i).gameObject);
        }
        //添加新的预制体
        NowPrefab = (GameObject)Resources.Load(hero.PrefabPath);
        quaternion ChildRotation = new quaternion(0, 180, 0, 0);
        GameObject ShowHero = UnityEngine.Object.Instantiate(NowPrefab, ChildPosition, ChildRotation, UIHero);
        ShowHero.transform.position=new Vector3 (0, 0, 0);
        ShowHero.transform.localScale = new Vector3(900, 500, 450);
        //显示星级
        RefreshStars(hero);
    }

    public void RefreshStars(Hero hero)
    {
        for (int i = 0; i < Star.childCount; i++)
        {
            Transform uistar = Star.GetChild(i);
            if ((int)(hero.rarity) > i)
            {
                uistar.gameObject.SetActive(true);
            }
            else
            {
                uistar.gameObject.SetActive(false);
            }
        }
    }

    //攻击动画
    public void Attack()
    {
        if (UIHero.childCount == 0) return;
        PlayerObj HeroPrefab = UIHero.GetChild(0).GetComponent<PlayerObj>();
        HeroPrefab.SetStateAnimationIndex(PlayerState.ATTACK);
        HeroPrefab.PlayStateAnimation(PlayerState.ATTACK);
    }

    public void OnClickHero()
    {
        UICodex.gameObject.SetActive(false);
        UIHeroBag.gameObject.SetActive(true);
    }

    public void OnClickCodex()
    {
        UIHeroBag.gameObject.SetActive(false);
        UICodex.gameObject.SetActive(true);
    }

    public void OnClickPlus()
    {
        Debug.Log("plus capcity");
    }

    public void OnClickReferrer()
    {
        Debug.Log("show referrer");
    }

    public void OnClickAll()
    {
        Debug.Log("filter all");
        controller.RefreshScrollView(HeroType.All);
    }

    public void OnClickForce()
    {
        Debug.Log("filter force");
        controller.RefreshScrollView(HeroType.Force);
    }

    public void OnClickInnerForce()
    {
        Debug.Log("filter innerforce");
        controller.RefreshScrollView(HeroType.InnerForce);
    }

    public void OnClickHeal()
    {
        Debug.Log("filter heal");
        controller.RefreshScrollView(HeroType.Heal);
    }

    public void OnClickSword()
    {
        Debug.Log("filter sword");
        controller.RefreshScrollView(HeroType.Sword);
    }

    public void OnClickSkill()
    {
        Debug.Log("filter skill");
        controller.RefreshScrollView(HeroType.Skill);
    }

}
