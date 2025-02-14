///
/// Ö÷½çÃæÂß¼­
///

public class MainPageController
{
    private MainPageView mainPageView;

    public MainPageController(MainPageView view)
    {
        mainPageView = view;
        view.controller = this;
        mainPageView.Init();
    }

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
