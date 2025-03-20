using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using XLua;

[CSharpCallLua]
public class GMCommand
{
    [MenuItem("GMCommand/�����ӵ��Ӣ������")]
    public static void ClearOwnHeroData()
    {
        ILuaGM luaFunc = LuaManager.Instance.MyLuaEnv.Global.Get<ILuaGM>("GMCommand");
        luaFunc.clearOwnHeroInfo();
    }

    [MenuItem("GMCommand/���������Ӣ������")]
    public static void ClearDeployHeroData()
    {
        ILuaGM luaFunc = LuaManager.Instance.MyLuaEnv.Global.Get<ILuaGM>("GMCommand");
        luaFunc.clearDeployHeroInfo();
    }
}