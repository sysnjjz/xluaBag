using System.Collections.Generic;
using UnityEngine;
using System.IO;
using XLua;

public class CallLua : MonoBehaviour
{
    private void Start()
    {
        LuaManager.Instance.Dostring("require 'Main'");
    }
}

