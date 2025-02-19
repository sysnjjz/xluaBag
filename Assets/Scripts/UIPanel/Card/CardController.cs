using System.Collections.Generic;
using UnityEngine;

///
/// 抽卡界面逻辑
///
public class CardController
{
    private CardView cardView;

    public CardController()
    {
        CreatePanel();
        ShowPanel();
    }

    private void CreatePanel()
    {
        cardView = new CardView();
        GameObject cardPanelPrefab = null;
        if (!UIManager.Instance.prefabDict.TryGetValue(UIManager.Instance.pathDict[UIConst.DrawCard], out cardPanelPrefab))
        {
            cardView.BeforeInit(UIManager.Instance.pathDict[UIConst.DrawCard], "Prefab/Panel" + UIManager.Instance.pathDict[UIConst.DrawCard]);
        }
        else
        {
            cardView.gameObject = cardPanelPrefab;
        }
    }

    private void ShowPanel()
    {
        cardView.OpenPanel(UIManager.Instance.UIRoot);
        cardView.controller = this;
        cardView.Init();
    }

    public void OnClickClose()
    {
        UIManager.Instance.ClosePanel(UIConst.DrawCard);
    }

    //抽一张卡UI
    public void OneCard()
    {
        //抽一张卡
        HeroLocalItem hero = CardModel.Instance.GetRandomHero();
        //显示一张卡
        cardView.RefreshOneCard(hero, HeroStaticData.Instance.GetHeroById(hero.id));
    }

    //抽十张卡UI显示逻辑
    public void TenCard()
    {
        //抽十张卡
        List<HeroLocalItem> heroList = CardModel.Instance.GetRandomHero10();
        //显示十张卡
        cardView.TenCard(heroList);
    }
}
