using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployedHero
{
    //单例
    private static DeployedHero instance;

    public static DeployedHero Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DeployedHero();
            }
            return instance;
        }
    }

    //存取框架
    public Dictionary<int, HeroLocalItem> DeployHeroesDic;

    //是否需要本地存取逻辑？
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
            DeployedHero depolyedHero= JsonUtility.FromJson<DeployedHero>(InventoryJson);
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
}
