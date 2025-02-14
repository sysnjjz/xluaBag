///
/// 这里是所有UI界面的基类
///
using UnityEngine;

public class BasePanel
{
    public bool IsRemove = false;
    public GameObject gameObject = null;
    public Transform transform = null;

    protected virtual void Awake()
    {
    }

    public void BeforeInit(string name,string path)
    {
        gameObject = Resources.Load<GameObject>(path);
        transform = gameObject.transform;
        UIManager.Instance.prefabDict.Add(name, gameObject);
    }

    public void Initial(string name,Transform uiRoot)
    {
        gameObject = GameObject.Instantiate(gameObject, uiRoot, false);
        transform = gameObject.transform;
        gameObject.SetActive(true);
        UIManager.Instance.panelDict.Add(name, this);
    }

    public virtual void SetOnShow(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void OpenPanel(string name)
    {
    }

    public virtual void ClosePanel(string name) 
    {
    }
}
