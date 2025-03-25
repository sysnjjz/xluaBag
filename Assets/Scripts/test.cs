using System.Collections;
using XLua;
using UnityEngine;
using UnityEngine.Events;

public class test : MonoBehaviour
{
    public AssetBundle ab;
    private void Start()
    {
        //ab = AssetBundle.LoadFromFile("Assets/AssetBundles/my_assest");
        LoadAsync("Heal", callBack);
    }

    public void LoadAsync(string resName, UnityAction<Object> callBack)
    {
        LuaManager.Instance.StartCoroutine(AsynLoadAB(resName, callBack));
    }

    //Э��ʵ���첽����ab��Դ
    IEnumerator AsynLoadAB(string resName, UnityAction<Object> callBack)
    {
        AssetBundleRequest request = ab.LoadAssetAsync<Object>(resName);
        yield return request;
        Object res = request.asset as Object;

        if (res == null)
        {
            Debug.Log("can not get res");
        }
        callBack?.Invoke(res);
    }

    private void callBack(Object res)
    {
        if (res == null)
            Debug.Log("can not get texture");
    }
}
