using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using XLua;

public class LuaManager : MonoBehaviour
{
    //统一管理Update
    public event Action luaUpdates;

    //单例类
    private static readonly object padlock = new object();
    private static LuaManager instance;
    public static LuaManager Instance
    {
        get
        {
            if (instance == null)
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        GameObject go = new GameObject("LuaManager");
                        DontDestroyOnLoad(go);
                        instance = go.AddComponent<LuaManager>();
                    }
                }

            }
            return instance;
        }
    }

    //luaGC回收时间见隔
    float lastGcTime = 0;
    const float GCInterval = 1;

    //全局唯一lua环境
    LuaEnv luaEnv;
    public LuaEnv MyLuaEnv
    {
        get
        {
            if (luaEnv == null)
            {
                luaEnv = new LuaEnv();
                luaEnv.AddLoader(LuaLoader);
                //加载lua全局变量
                luaEnv.DoString("require 'GlobalFunc'");
            }
            return luaEnv;
        }
    }
    //自定义Loader
    public static byte[] LuaLoader(ref string filename)
    {
        byte[] byArrayReturn = null;
        string luaPath = Application.dataPath + "/LuaScripts/" + filename.Replace(".", "/") + ".lua";
        string strLuaConent = File.ReadAllText(luaPath);
        byArrayReturn = System.Text.Encoding.UTF8.GetBytes(strLuaConent);
        return byArrayReturn;
    }

    //封装dostring 方法
    public object[] Dostring(string str, string chunkName = "chunk")
    {
        var ret = MyLuaEnv.DoString(str, chunkName);
        return ret;
    }

    private void Awake()
    {
        AsyncMgr.Instance.ab = AssetBundle.LoadFromFile("Assets/AssetBundles/my_assest");
    }
    private void Update()
    {
        //定时GC
        if (Time.time - lastGcTime > GCInterval)
        {
            MyLuaEnv.Tick();
            lastGcTime = Time.time;
        }
        //管理luaUpdate
        if (luaUpdates != null)
        {
            luaUpdates();
        }
    }
}