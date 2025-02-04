///
/// 这里写的是英雄背包中的子控件 主要是单个英雄框框的逻辑
///
using UnityEngine;
using UnityEngine.UI;

public class HeroCtrl : MonoBehaviour
{
    //UI组件
    private Transform Grade;
    private Transform Type;
    private Transform Star;
    private Transform IsNew;

    private HeroLocalItem HeroLocalData;
    private Hero HeroTableData;
    private BackPackCtrl uiParent;

    private void Awake()
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
        Grade = transform.Find("Grade");
        Type = transform.Find("Type");
        Star = transform.Find("Star");
        IsNew = transform.Find("IsNew");
    }

    //更新预制体信息
    public void Refresh(HeroLocalItem HeroLocalData,BackPackCtrl uiParent)
    {
        //数据初始化
        this.HeroLocalData=HeroLocalData;
        this.HeroTableData = GameManager.Instance.GetHeroById(HeroLocalData.id);
        this.uiParent = uiParent;

        //稀有度信息
        Grade.GetComponent<Text>().text = HeroTableData.rarity.ToString();
        //是否显示新获得
        IsNew.gameObject.SetActive(this.HeroLocalData.IsNew);
        //显示人物图片
        Texture2D pic1 = (Texture2D)Resources.Load(this.HeroTableData.ImgPath);
        Sprite tmp1 = Sprite.Create(pic1, new Rect(0, 0, pic1.width, pic1.height), new Vector2(0, 0));
        this.GetComponent<Image>().sprite = tmp1;
        //显示类别图片
        Texture2D pic2 = (Texture2D)Resources.Load("Icon/"+HeroTableData.type.ToString());
        Sprite tmp2 = Sprite.Create(pic2, new Rect(0, 0, pic2.width, pic2.height), new Vector2(0, 0));
        Type.gameObject.GetComponent<Image>().sprite = tmp2;
        //显示星级
        RefreshStars();
    }

    public void RefreshStars()
    {
        for(int i=0;i<Star.childCount;i++)
        {
            Transform uistar=Star.GetChild(i);
            if((int)(this.HeroTableData.rarity)>i)
            {
                uistar.gameObject.SetActive(true);
            }
            else
            {
                uistar.gameObject.SetActive(false);
            }
        }
    }

    private void ClickInit()
    {
        this.GetComponent<Button>().onClick.AddListener(OnClickShowHero);
    }

    //点一下 播放人物攻击动画
    private void OnClickShowHero()
    {
        print("show hero");
        //重复点击不改变动画
        if (this.uiParent.chooseUid == this.HeroLocalData.uid) return;
        //根据点击换新动画
        this.uiParent.chooseUid = this.HeroLocalData.uid;
    }
}