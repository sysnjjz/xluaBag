using System.Collections;
using XLua;
using UnityEngine;
using UnityEngine.Events;

[LuaCallCSharp]
public class AsyncMgr
{
    public AssetBundle ab;
    //单例
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

    //协程实现异步加载ab资源
    IEnumerator AsynLoadAB(string resName, UnityAction<Object> callBack)
    {
        AssetBundleRequest request = ab.LoadAssetAsync<Object>(resName);

        yield return request;
        Object res = request.asset as Object;
        callBack?.Invoke(res);
    }

    //等待协程完成
    public static IEnumerator WaitForCoroutine(IEnumerator coroutine)
    {
        yield return LuaManager.Instance.StartCoroutine(coroutine);
    }
}
