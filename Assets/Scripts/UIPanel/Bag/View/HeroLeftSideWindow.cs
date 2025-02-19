using UnityEngine;
using UnityEngine.UI;

///
/// 子控件：单独一个英雄背包栏中的英雄视图逻辑
///
public class HeroLeftSideWindow : BasePanel
{
    //UI组件
    private Text Grade;
    private Image Type;
    private Transform Star;
    private Transform IsNew;
    private Image image;

    HeroLocalItem heroLocalData;
    BagController bagController;

    //构造函数
    public HeroLeftSideWindow(GameObject panel,Transform uiRoot)
    {
        gameObject = panel;
        OpenPanel(uiRoot);
        Init();
    }

    //打开逻辑
    public override void OpenPanel(Transform uiRoot)
    {
        gameObject = GameObject.Instantiate(gameObject, uiRoot, false);
        transform = gameObject.transform;
    }

    //初始化
    private void Init()
    {
        Grade = transform.Find("Grade").GetComponent<Text>();
        Type = transform.Find("Type").gameObject.GetComponent<Image>();
        Star = transform.Find("Star");
        IsNew = transform.Find("IsNew");
        image = transform.GetComponent<Image>();

        transform.GetComponent<Button>().onClick.AddListener(OnClickShowHero);
    }

    //更新预制体信息
    public void Refresh(HeroLocalItem heroLocalData,Hero hero,BagController bagController)
    {
        //显示的英雄
        this.heroLocalData = heroLocalData;
        this.bagController = bagController;

        //稀有度信息
        Grade.text = hero.rarity.ToString();
        //是否显示新获得
        IsNew.gameObject.SetActive(heroLocalData.IsNew);
        //显示人物图片
        Texture2D pic1 = (Texture2D)Resources.Load(hero.ImgPath);
        Sprite tmp1 = Sprite.Create(pic1, new Rect(0, 0, pic1.width, pic1.height), new Vector2(0, 0));
        image.sprite = tmp1;
        //显示类别图片
        Texture2D pic2 = (Texture2D)Resources.Load("Icon/"+hero.type.ToString());
        Sprite tmp2 = Sprite.Create(pic2, new Rect(0, 0, pic2.width, pic2.height), new Vector2(0, 0));
        Type.sprite = tmp2;
        //显示星级
        RefreshStars(hero);
    }

    public void RefreshStars(Hero hero)
    {
        for(int i=0;i<Star.childCount;i++)
        {
            Transform uistar=Star.GetChild(i);
            if((int)(hero.rarity)>i)
            {
                uistar.gameObject.SetActive(true);
            }
            else
            {
                uistar.gameObject.SetActive(false);
            }
        }
    }

    //更改展示的人物
    private void OnClickShowHero()
    {
        if (bagController == null || this.heroLocalData == null) return;
        //重复点击不更改
        if (bagController.chooseUid == this.heroLocalData.uid) return;
        //不同则更改
        bagController.chooseUid = this.heroLocalData.uid;
    }
}