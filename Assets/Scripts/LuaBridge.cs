using XLua;
using System.IO;
using UnityEngine;
using System;
using System.Collections.Generic;

[LuaCallCSharp]
public class LuaBridge : MonoBehaviour
{
    public static void ExitApplication()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    //获得服务端数据
    //玩家已有英雄数据
    //加载
    public static List<LuaLocalItem> LoadLocalItemJson()
    {
        string path = Application.persistentDataPath + "/localItemData.json";
        if (File.Exists(path))
        {
            var content = File.ReadAllText(path);
            List<LuaLocalItem> localItemData = JsonUtility.FromJson<Serialization<LuaLocalItem>>(content).ToList();
            return localItemData;
        }
        else
        {
            return null;
        }
    }

    //保存
    public static void SaveLocalItemJson(List<LuaLocalItem> localItemData)
    {
        string path = Application.persistentDataPath + "/localItemData.json";
        Serialization<LuaLocalItem> content = new Serialization<LuaLocalItem>(localItemData);
        var ncontent = JsonUtility.ToJson(content, true);
        File.WriteAllText(path, ncontent);
    }

    //上阵英雄数据
    //加载
    public static Dictionary<int, LuaLocalItem> LoadDeployHeroJson()
    {
        string path = Application.persistentDataPath + "/deployHeroData.json";
        if (File.Exists(path))
        {
            var content = File.ReadAllText(path);
            Dictionary<int, LuaLocalItem> deployHeroData = JsonUtility.FromJson<Serialization<int, LuaLocalItem>>(content).ToDictionary();
            return deployHeroData;
        }
        else
        {
            return null;
        }
    }

    //保存
    public static void SaveDeployHeroJson(Dictionary<int, LuaLocalItem> deployHeroData)
    {
        string path = Application.persistentDataPath + "/deployHeroData.json";
        Serialization<int,LuaLocalItem> content = new Serialization<int,LuaLocalItem>(deployHeroData);
        var ncontent = JsonUtility.ToJson(content, true);
        File.WriteAllText(path, ncontent);
    }

    //获得配置数据
    public static List<Hero> GetHeroList()
    {
        return Resources.Load<HeroTable>("Table/HeroTable").HeroList;
    }
}


// List<T>
[Serializable]
public class Serialization<T>
{
    [SerializeField]
    private List<T> target;
    public List<T> ToList() { return target; }

    public Serialization(List<T> target)
    {
        this.target = target;
    }
}
// Dictionary<TKey, TValue>
[Serializable]
public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    List<TKey> keys;
    [SerializeField]
    List<TValue> values;

    Dictionary<TKey, TValue> target;
    public Dictionary<TKey, TValue> ToDictionary() { return target; }

    public Serialization(Dictionary<TKey, TValue> target)
    {
        this.target = target;
    }

    public void OnBeforeSerialize()
    {
        keys = new List<TKey>(target.Keys);
        values = new List<TValue>(target.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Math.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for (var i = 0; i < count; ++i)
        {
            target.Add(keys[i], values[i]);
        }
    }
}

//英雄的基本属性
[System.Serializable]
public class LuaLocalItem
{
    public string uid;
    public int id;
    public int level;
    public bool isNew;
    public bool isDeploy;
    public int ATK;
    public int HP;
    public int Defence;
}


public enum HeroType
{
    Force, InnerForce, Heal, Sword, Skill, All
}

public enum HeroGrade
{
    Normal = 1,
    General = 2,
    Good = 3,
    Great = 4,
    Legendary = 5
}


[System.Serializable]
public class Hero
{
    public int id;
    public HeroGrade rarity;
    public string name;
    public HeroType type;
    public string keyword;
    public string imgPath;
    public string prefabPath;
}