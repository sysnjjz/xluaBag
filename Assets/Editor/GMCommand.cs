using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using XLua;

[CSharpCallLua]
public class GMCommand
{
    [MenuItem("GMCommand/清除已拥有英雄数据")]
    public static void ClearOwnHeroData()
    {
        ILuaGM luaFunc = LuaManager.Instance.MyLuaEnv.Global.Get<ILuaGM>("GMCommand");
        luaFunc.clearOwnHeroInfo();
    }

    [MenuItem("GMCommand/清除已上阵英雄数据")]
    public static void ClearDeployHeroData()
    {
        ILuaGM luaFunc = LuaManager.Instance.MyLuaEnv.Global.Get<ILuaGM>("GMCommand");
        luaFunc.clearDeployHeroInfo();
    }
}