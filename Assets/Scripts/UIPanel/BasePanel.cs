///
/// 这里是所有UI界面的基类
///
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    protected bool IsRemove = false;

    protected virtual void Awake()
    {
    }

    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }

    public virtual void OpenPanel(string name)
    {
        SetActive(true);
    }

    public virtual void ClosePanel(string name) 
    {
        IsRemove = true;
        SetActive(false);
        Destroy(gameObject);

        //移除缓存
        if(UIManager.Instance.panelDict.ContainsKey(name))
        {
            UIManager.Instance.panelDict.Remove(name);
        }
    }
}
