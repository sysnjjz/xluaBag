using UnityEngine;
using UnityEngine.UI;

public class MainPageView : BasePanel
{
    //UI控件
    private Button BagButton;
    private Button CardButton;
    private Button ExitButton;

    //控制器
    public MainPageController controller;

    //打开逻辑
    public override void OpenPanel(Transform uiRoot)
    {
        base.OpenPanel(uiRoot);
        UIManager.Instance.panelDict.Add(UIConst.MainMenu, this);
    }

    //初始化
    public void Init()
    {
        BagButton = transform.Find("Bag").GetComponent<Button>();
        CardButton = transform.Find("Card").GetComponent<Button>();
        ExitButton = transform.Find("Exit").GetComponent<Button>();

        BagButton.onClick.AddListener(OnClickBag);
        CardButton.onClick.AddListener(OnClickCard);
        ExitButton.onClick.AddListener(OnClickExit);
    }

    //按键操作
    public void OnClickBag()
    {
        UIManager.Instance.OpenPanel(UIConst.HeroBackPack);
    }

    public void OnClickCard()
    {
        UIManager.Instance.OpenPanel(UIConst.DrawCard);
    }

    public void OnClickExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
