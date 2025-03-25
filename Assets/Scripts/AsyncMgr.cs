using System.Collections;
using XLua;
using UnityEngine;
using UnityEngine.Events;

[LuaCallCSharp]
public class AsyncMgr
{
    public AssetBundle ab;
    //����
    private static AsyncMgr instance;

    public static AsyncMgr Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AsyncMgr();
            }
            return instance;
        }
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
        callBack?.Invoke(res);
    }

    //�ȴ�Э�����
    public static IEnumerator WaitForCoroutine(IEnumerator coroutine)
    {
        yield return LuaManager.Instance.StartCoroutine(coroutine);
    }
}
