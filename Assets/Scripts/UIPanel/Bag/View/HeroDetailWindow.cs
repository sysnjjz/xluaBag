///
/// 这里是英雄展示页面的逻辑
///
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

public class HeroDetail : MonoBehaviour
{
    //UI控件
    private Transform UIHero;
    private Transform UIHeroInfo_Grade;
    private Transform UIHeroInfo_Name;
    private Transform UIHeroInfo_KeyWord;
    private Transform UIATK;
    private Transform Star;

    private HeroLocalItem HeroLocalData;
    private Hero HeroTableData;
    private BackPackCtrl uiParent;

    private GameObject NowPrefab;

    private void Awake()
    {
        InitUI();
        test();
    }

    //初始化界面用的
    private void test()
    {
        if (GameManager.Instance.GetHeroLocalData().Count!=0)
        {
            Refresh(GameManager.Instance.GetHeroLocalData()[0], null);
            transform.parent.GetComponent<BackPackCtrl>().chooseUid = GameManager.Instance.GetHeroLocalData()[0].uid;
        }
    }

    private void InitUI()
    {
        UIHero = transform.Find("Hero");
        UIHeroInfo_Grade = transform.Find("HeroInfo/Grade");
        UIHeroInfo_Name = transform.Find("HeroInfo/Name");
        UIHeroInfo_KeyWord = transform.Find("HeroInfo/KeyWord");
        UIATK = transform.Find("ATK");
        Star = transform.Find("HeroInfo/Star");
    }

    public void Refresh(HeroLocalItem HeroLocalData, BackPackCtrl uiParent)
    {
        //数据初始化
        this.HeroLocalData = HeroLocalData;
        this.HeroTableData = GameManager.Instance.GetHeroById(HeroLocalData.id);
        this.uiParent = uiParent;

        //稀有度信息
        UIHeroInfo_Grade.GetComponent<Text>().text = HeroTableData.rarity.ToString();
        //攻击力
        UIATK.GetComponent<Text>().text="ATK:"+HeroLocalData.ATK.ToString();
        //显示名称
        UIHeroInfo_Name.GetComponent<Text>().text= HeroTableData.name.ToString();
        //显示关键词
        UIHeroInfo_KeyWord.GetComponent<Text>().text=HeroTableData.keyword.ToString();
        ///显示预制体
        //清除原有预制体
        for (int i = 0; i < UIHero.childCount; i++)
        {
            GameObject.Destroy(UIHero.GetChild(i).gameObject);
        }
        //添加新的预制体
        NowPrefab = (GameObject)Resources.Load(HeroTableData.PrefabPath);
        Vector3 ChildPosition = UIHero.gameObject.GetComponent<RectTransform>().transform.position;
        quaternion ChildRotation = new quaternion(0, 180, 0, 0);
        GameObject ShowHero = Instantiate(NowPrefab, ChildPosition, ChildRotation, UIHero);
        ShowHero.transform.localScale = new Vector3(1100, 550, 550);
        ShowHero.transform.SetSiblingIndex(UIHero.childCount - 1);
        //显示星级
        RefreshStars();
    }

    public void RefreshStars()
    {
        for (int i = 0; i < Star.childCount; i++)
        {
            Transform uistar = Star.GetChild(i);
            if ((int)(this.HeroTableData.rarity) > i)
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
        PlayerObj HeroPrefab=this.transform.Find("Hero").GetChild(0).GetComponent<PlayerObj>();
        HeroPrefab.SetStateAnimationIndex(PlayerState.ATTACK);
        HeroPrefab.PlayStateAnimation(PlayerState.ATTACK);
    }

    public HeroLocalItem GetHeroLocalData()
    {
        return this.HeroLocalData;
    }
}
