using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using XLua;

public class LuaManager : MonoBehaviour
{
    //ͳһ����Update
    public event Action luaUpdates;

    //������
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

    //luaGC����ʱ�����
    float lastGcTime = 0;
    const float GCInterval = 1;

    //ȫ��Ψһlua����
    LuaEnv luaEnv;
    public LuaEnv MyLuaEnv
    {
        get
        {
            if (luaEnv == null)
            {
                luaEnv = new LuaEnv();
                luaEnv.AddLoader(LuaLoader);
                //����luaȫ�ֱ���
                luaEnv.DoString("require 'GlobalFunc'");
            }
            return luaEnv;
        }
    }
    //�Զ���Loader
    public static byte[] LuaLoader(ref string filename)
    {
        byte[] byArrayReturn = null;
        string luaPath = Application.dataPath + "/LuaScripts/" + filename.Replace(".", "/") + ".lua";
        string strLuaConent = File.ReadAllText(luaPath);
        byArrayReturn = System.Text.Encoding.UTF8.GetBytes(strLuaConent);
        return byArrayReturn;
    }

    //��װdostring ����
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
        //��ʱGC
        if (Time.time - lastGcTime > GCInterval)
        {
            MyLuaEnv.Tick();
            lastGcTime = Time.time;
        }
        //����luaUpdate
        if (luaUpdates != null)
        {
            luaUpdates();
        }
    }
}