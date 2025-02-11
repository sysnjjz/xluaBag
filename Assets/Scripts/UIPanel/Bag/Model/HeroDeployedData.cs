using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 上阵英雄数据
/// </summary>
public class HeroDeployedData
{
    //单例
    private static HeroDeployedData instance;

    public static HeroDeployedData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new HeroDeployedData();
            }
            return instance;
        }
    }

    //存取框架
    public Dictionary<int, HeroLocalItem> DeployHeroesDic;

    //存到本地
    public void SaveDeployData()
    {
        string InventoryJson = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("DeployHeroData", InventoryJson);
        PlayerPrefs.Save();
    }

    //取本地数据
    public Dictionary<int, HeroLocalItem> LoadDeployData()
    {
        //已经用过
        if (DeployHeroesDic != null)
        {
            return DeployHeroesDic;
        }
        //已经取过
        if (PlayerPrefs.HasKey("DeployHeroData"))
        {
            string InventoryJson = PlayerPrefs.GetString("DeployHeroData");
            HeroDeployedData depolyedHero= JsonUtility.FromJson<HeroDeployedData>(InventoryJson);
            DeployHeroesDic=depolyedHero.DeployHeroesDic;
            return DeployHeroesDic;
        }
        //都没有
        else
        {
            DeployHeroesDic=new Dictionary<int, HeroLocalItem>();
            return DeployHeroesDic;
        }
    }

    //添加上阵英雄
    public int AddDeployHero(int DeployID, HeroLocalItem AddHero)
    {
        Dictionary<int, HeroLocalItem> DeployHeroes = LoadDeployData();
        int OriDeployID = 0;
        bool replace = false;
        foreach (var DeployHero in DeployHeroes)
        {
            //如果已经有同一个英雄出阵中，则新英雄是替换掉旧英雄，而不是直接上阵
            if (DeployHero.Value.id == AddHero.id)
            {
                OriDeployID = DeployHero.Key;
                DeployHeroes[OriDeployID] = AddHero;
                DeployHero.Value.IsDeploy = false;
                replace = true;
                break;
            }
        }
        if (!replace)
        {
            if (DeployHeroes.ContainsKey(DeployID))
            {
                DeployHeroes[DeployID].IsDeploy = false;
                DeployHeroes[DeployID] = AddHero;
            }
            else
            {
                DeployHeroes.Add(DeployID, AddHero);
            }
        }
        return OriDeployID;
    }
}
