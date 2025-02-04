///
/// 抽卡界面逻辑
///
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : BasePanel
{
    //UI控件
    private Transform CloseButton;
    private Transform OneButton;
    private Transform TenButton;
    private Transform CardList;

    public CardDetail cardDetail;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        CloseButton = transform.Find("Panel/CloseButton");
        OneButton = transform.Find("Panel/One");
        TenButton = transform.Find("Panel/Ten");
        CardList = transform.Find("Panel/Card");

        CloseButton.GetComponent<Button>().onClick.AddListener(OnClickClose);
        OneButton.GetComponent<Button>().onClick.AddListener(OneCard);
        TenButton.GetComponent<Button>().onClick.AddListener(TenCard);
    }

    private void OnClickClose()
    {
        UIManager.Instance.ClosePanel(UIConst.DrawCard);
    }

    //抽一张卡UI显示逻辑
    private void OneCard()
    {
        //销毁原有的
        for(int i=0;i<CardList.childCount;i++)
        {
            Destroy(CardList.GetChild(i).gameObject);
        }
        //抽一张卡
        HeroLocalItem hero = GameManager.Instance.GetRandomHero();
        Transform newHeroDetail = Instantiate(cardDetail.transform, CardList) as Transform;
        newHeroDetail.transform.localScale = new Vector3(0.8f,0.8f, 0.8f);
        //显示逻辑
        CardDetail cardInfo = newHeroDetail.GetComponent<CardDetail>();
        cardInfo.Refresh(hero,this);
    }

    //抽十张卡UI显示逻辑
    private void TenCard()
    {
        //销毁原有的
        for (int i = 0; i < CardList.childCount; i++)
        {
            Destroy(CardList.GetChild(i).gameObject);
        }
        //抽十张卡
        List<HeroLocalItem> heroList= GameManager.Instance.GetRandomHero10();
        for(int i=0;i<heroList.Count;i++)
        {
            Transform newHeroDetail = Instantiate(cardDetail.transform, CardList) as Transform;
            newHeroDetail.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            //显示逻辑
            CardDetail cardInfo = newHeroDetail.GetComponent<CardDetail>();
            cardInfo.Refresh(heroList[i], this);
        }
    }
}
