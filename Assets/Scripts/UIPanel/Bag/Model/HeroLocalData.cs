///
/// 英雄动态数据集，存储玩家有的英雄数据
///
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class HeroLocalData
{
    //单例
    private static HeroLocalData instance;

    public static HeroLocalData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new HeroLocalData();
            }
            return instance;
        }
    }

    //存取框架
    public List<HeroLocalItem> LocalDataList;

    //存到本地
    public void SaveData()
    {
        string InventoryJson = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("HeroLocalData", InventoryJson);
        PlayerPrefs.Save();
    }

    //取本地数据
    public List<HeroLocalItem> LoadData()
    {
        //已经用过
        if (LocalDataList != null)
        {
            return LocalDataList;
        }
        //已经取过
        if (PlayerPrefs.HasKey("HeroLocalData"))
        {
            string InventoryJson = PlayerPrefs.GetString("HeroLocalData");
            HeroLocalData HeroLocalData = JsonUtility.FromJson<HeroLocalData>(InventoryJson);
            LocalDataList = HeroLocalData.LocalDataList;
            return LocalDataList;
        }
        //都没有
        else
        {
            LocalDataList = new List<HeroLocalItem>();
            return LocalDataList;
        }
    }
}

//英雄的基本属性
[System.Serializable]
public class HeroLocalItem
{
    public string uid;
    public int id;
    public int level;
    public bool IsNew;
    public bool IsDeploy;
    public int ATK;
    public int HP;
    public int Defence;

    //调试用
    public override string ToString()
    {
        return string.Format("【id】：{0}；【level】：{1}",id,level);
    }
}
