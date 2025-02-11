using UnityEngine;
using UnityEngine.UI;

public class CardView:BasePanel
{
    //UI¿Ø¼þ
    private Transform CloseButton;
    private Transform OneButton;
    private Transform TenButton;
    public Transform CardList;

    //¿ØÖÆÆ÷
    CardController cardController;

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

        cardController = GetComponent<CardController>();

        CloseButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            cardController.OnClickClose();
        });
        OneButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            cardController.OneCard();
        }); 
        TenButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            cardController.TenCard();
        }); 
    }
}
