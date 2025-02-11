using System.Collections.Generic;
using UnityEngine;

public enum HeroType
{
    Force,InnerForce,Heal,Sword,Skill,All
}

public enum HeroGrade
{
    Normal=1, 
    General=2,
    Good=3, 
    Great=4, 
    Legendary=5
}


[System.Serializable]
public class Hero 
{
    public int id;
    public HeroGrade rarity;
    public string name;
    public HeroType type;
    public string keyword;
    public string ImgPath;
    public string PrefabPath;
}

/// <summary>
/// 英雄的静态数据 是英雄的设定集
/// </summary>
public class HeroStaticData
{
    //单例
    private static HeroStaticData instance;

    public static HeroStaticData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new HeroStaticData();
            }
            return instance;
        }
    }

    //英雄设定表
    private HeroTable heroTable;

    //加载静态数据
    public HeroTable GetHeroTable()
    {
        if (heroTable == null)
        {
            heroTable = Resources.Load<HeroTable>("Table/HeroTable");
        }
        return heroTable;
    }

    //根据id拿到指定静态数据
    public Hero GetHeroById(int id)
    {
        List<Hero> HeroList = GetHeroTable().HeroList;
        foreach (Hero hero in HeroList)
        {
            if (hero.id == id)
            {
                return hero;
            }
        }
        return null;
    }
}
