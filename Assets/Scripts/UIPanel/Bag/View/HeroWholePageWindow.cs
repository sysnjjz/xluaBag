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
    private HeroDownWindow UIDeployHero1;
    private HeroDownWindow UIDeployHero2;
    private HeroDownWindow UIDeployHero3;
    private HeroDownWindow UIDeployHero4;
    private HeroDownWindow UIDeployHero5;
    public HeroDownWindow[] UIDeployButtonArr;

    //控制器
    BagController bagController;

    //方法
    override protected void Awake()
    {
        Init();
    }

    private void Init()
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
        UIDeployHero1 = transform.Find("Deploy/DeployList/DeployHero1").GetComponent<HeroDownWindow>();
        UIDeployHero2 = transform.Find("Deploy/DeployList/DeployHero2").GetComponent<HeroDownWindow>();
        UIDeployHero3 = transform.Find("Deploy/DeployList/DeployHero3").GetComponent<HeroDownWindow>();
        UIDeployHero4 = transform.Find("Deploy/DeployList/DeployHero4").GetComponent<HeroDownWindow>();
        UIDeployHero5 = transform.Find("Deploy/DeployList/DeployHero5").GetComponent<HeroDownWindow>();
        //添加到数组中方便查找
        UIDeployButtonArr = new HeroDownWindow[5];
        UIDeployButtonArr[0] = UIDeployHero1;
        UIDeployButtonArr[1] = UIDeployHero2;
        UIDeployButtonArr[2] = UIDeployHero3;
        UIDeployButtonArr[3] = UIDeployHero4;
        UIDeployButtonArr[4] = UIDeployHero5;

        //页面初始化
        UICodex.gameObject.SetActive(false);
        UIHeroBag.gameObject.SetActive(true);

        //控制器
        bagController=GetComponent<BagController>();
    }

    private void ClickInit()
    {
        //注册按键事件
        UICloseButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            bagController.OnClickClose();
        }); 
        UIHeroButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            bagController.OnClickHero();
        }); 
        UICodexButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            bagController.OnClickCodex();
        }); 
        UIShowHero.onClick.AddListener(() =>
        {
            Attack();
        }); 
        UIPlusButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            bagController.OnClickPlus();
        }); 
        UIReferrerButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            bagController.OnClickReferrer();
        }); 
        UIAllButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            bagController.OnClickAll();
        }); 
        UIForceButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            bagController.OnClickForce();
        });
        UIInnerForceButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            bagController.OnClickInnerForce();
        }); 
        UIHealButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            bagController.OnClickHeal();
        }); 
        UISwordButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            bagController.OnClickSword();
        }); 
        UISkillButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            bagController.OnClickSkill();
        }); 
        UIUPButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            bagController.UpdateDeployHero();
        }); 
    }

    public void RefreshHeroDetail(HeroLocalItem heroLocalItem)
    {
        //显示英雄信息
        Hero hero = HeroStaticData.Instance.GetHeroById(heroLocalItem.id);
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
        GameObject ShowHero = Instantiate(NowPrefab, ChildPosition, ChildRotation, UIHero);
        ShowHero.transform.localScale = new Vector3(1100, 550, 550);
        ShowHero.transform.SetSiblingIndex(UIHero.childCount - 1);
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
        PlayerObj HeroPrefab = UIHero.GetChild(0).GetComponent<PlayerObj>();
        HeroPrefab.SetStateAnimationIndex(PlayerState.ATTACK);
        HeroPrefab.PlayStateAnimation(PlayerState.ATTACK);
    }
}
