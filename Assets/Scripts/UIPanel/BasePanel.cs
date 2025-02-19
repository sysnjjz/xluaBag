///
/// 这里是所有UI界面的基类
///
using UnityEngine;

public class BasePanel
{
    public bool IsRemove = false;
    public GameObject gameObject = null;
    public Transform transform = null;

    public void BeforeInit(string name,string path)
    {
        gameObject = Resources.Load<GameObject>(path);
        transform = gameObject.transform;
        UIManager.Instance.prefabDict.Add(name, gameObject);
    }

    public virtual void SetOnShow(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void OpenPanel(Transform uiRoot)
    {
        gameObject = GameObject.Instantiate(gameObject, uiRoot, false);
        transform = gameObject.transform;
        UIManager.Instance.panelStack.Push(this);
    }

    public virtual void ClosePanel() 
    {
        this.IsRemove = true;
        this.gameObject.SetActive(false);
        UnityEngine.GameObject.Destroy(this.gameObject);
    }
}
