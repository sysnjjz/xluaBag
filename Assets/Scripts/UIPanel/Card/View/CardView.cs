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
    public CardController controller;

    public void Init()
    {
        CloseButton = transform.Find("Panel/CloseButton");
        OneButton = transform.Find("Panel/One");
        TenButton = transform.Find("Panel/Ten");
        CardList = transform.Find("Panel/Card");

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
}
