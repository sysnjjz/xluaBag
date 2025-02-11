///
/// 主界面逻辑
///
using UnityEngine;
using UnityEngine.UI;

public class MainPageController:BasePanel
{
    //UI控件
    private Button BagButton;
    private Button CardButton;
    private Button ExitButton;

    private void Awake()
    {
        base.Awake();
        Init();
    }

    private void Init()
    {
        BagButton=transform.Find("Bag").GetComponent<Button>();
        CardButton=transform.Find("Card").GetComponent <Button>();
        ExitButton = transform.Find("Exit").GetComponent<Button>();

        BagButton.onClick.AddListener(OnClickBag);
        CardButton.onClick.AddListener(OnClickCard);
        ExitButton.onClick.AddListener(OnClickExit);
    }

    private void OnClickBag()
    {
        UIManager.Instance.OpenPanel(UIConst.HeroBackPack);
    }

    private void OnClickCard()
    {
        UIManager.Instance.OpenPanel(UIConst.DrawCard);
    }

    private void OnClickExit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
