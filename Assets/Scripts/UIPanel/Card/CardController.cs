using System.Collections.Generic;
using UnityEngine;

///
/// 抽卡界面逻辑
///
public class CardController : MonoBehaviour
{
    //子控件
    public CardDetailView cardDetail;
    private CardView cardView;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        cardView=GetComponent<CardView>();
    }

    public void OnClickClose()
    {
        UIManager.Instance.ClosePanel(UIConst.DrawCard);
    }

    //抽一张卡UI显示逻辑
    public void OneCard()
    {
        //销毁原有的
        for(int i=0;i<cardView.CardList.childCount;i++)
        {
            Destroy(cardView.CardList.GetChild(i).gameObject);
        }
        //抽一张卡
        HeroLocalItem hero = CardModel.Instance.GetRandomHero();
        Transform newHeroDetailWindow = Instantiate(cardDetail.transform, cardView.CardList) as Transform;
        newHeroDetailWindow.transform.localScale = new Vector3(0.8f,0.8f, 0.8f);
        //显示逻辑
        newHeroDetailWindow.GetComponent<CardDetailView>().Refresh(hero);
    }

    //抽十张卡UI显示逻辑
    public void TenCard()
    {
        //销毁原有的
        for (int i = 0; i < cardView.CardList.childCount; i++)
        {
            Destroy(cardView.CardList.GetChild(i).gameObject);
        }
        //抽十张卡
        List<HeroLocalItem> heroList= CardModel.Instance.GetRandomHero10();
        for(int i=0;i<heroList.Count;i++)
        {
            Transform newHeroDetailWindow = Instantiate(cardDetail.transform, cardView.CardList) as Transform;
            newHeroDetailWindow.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            //显示逻辑
            newHeroDetailWindow.GetComponent<CardDetailView>().Refresh(heroList[i]);
        }
    }
}
