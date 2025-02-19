using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardView:BasePanel
{
    //UI控件
    private Transform CloseButton;
    private Transform OneButton;
    private Transform TenButton;
    public Transform CardList;

    //子控件
    public GameObject cardDetail;

    //控制器
    public CardController controller;

    //打开逻辑
    public override void OpenPanel(Transform uiRoot)
    {
        base.OpenPanel(uiRoot);
        UIManager.Instance.panelDict.Add(UIConst.DrawCard, this);
    }

    //初始化
    public void Init()
    {
        CloseButton = transform.Find("Panel/CloseButton");
        OneButton = transform.Find("Panel/One");
        TenButton = transform.Find("Panel/Ten");
        CardList = transform.Find("Panel/Card");

        cardDetail = Resources.Load<GameObject>("UI/Image");

        CloseButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            controller.OnClickClose();
        });
        OneButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            controller.OneCard();
        }); 
        TenButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            controller.TenCard();
        }); 
    }

    //抽一张卡UI显示逻辑
    public void RefreshOneCard(HeroLocalItem heroLocalItem,Hero hero)
    {
        //销毁原有的
        for (int i = 0; i < CardList.childCount; i++)
        {
            UnityEngine.Object.Destroy(CardList.GetChild(i).gameObject);
        }
        //抽一张卡
        CardDetailView newCardDetailView = new CardDetailView(cardDetail,CardList);
        newCardDetailView.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        //显示逻辑
        newCardDetailView.Refresh(heroLocalItem,hero);
    }

    //抽十张卡UI显示逻辑
    public void TenCard(List<HeroLocalItem> heroList)
    {
        //销毁原有的
        for (int i = 0; i < CardList.childCount; i++)
        {
            UnityEngine.Object.Destroy(CardList.GetChild(i).gameObject);
        }
        //抽十张卡
        for (int i = 0; i < heroList.Count; i++)
        {
            CardDetailView newCardDetailView = new CardDetailView(cardDetail, CardList);
            newCardDetailView.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            //显示逻辑
            newCardDetailView.Refresh(heroList[i], HeroStaticData.Instance.GetHeroById(heroList[i].id));
        }
    }
}
