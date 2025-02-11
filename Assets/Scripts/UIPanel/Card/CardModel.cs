using System;
using System.Collections.Generic;

public class CardModel
{
    //单例
    private static CardModel instance;

    public static CardModel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new CardModel();
            }
            return instance;
        }
    }
    //抽卡逻辑
    //抽一次
    public HeroLocalItem GetRandomHero()
    {
        int index = UnityEngine.Random.Range(0, HeroStaticData.Instance.GetHeroTable().HeroList.Count);
        Hero newHero = HeroStaticData.Instance.GetHeroTable().HeroList[index];
        HeroLocalItem newHeroLocalData = new()
        {
            uid = Guid.NewGuid().ToString(),
            id = newHero.id,
            level = 1,
            IsNew = true,
            IsDeploy = false,
            //这里应该在静态数据里写一个原始数据来着 忘掉了
            ATK = UnityEngine.Random.Range(4, 10) * 100,
            HP = UnityEngine.Random.Range(4, 8) * 100,
            Defence = UnityEngine.Random.Range(2, 6) * 100
        };
        HeroLocalData.Instance.LoadData();
        HeroLocalData.Instance.LocalDataList.Add(newHeroLocalData);
        HeroLocalData.Instance.SaveData();
        return newHeroLocalData;
    }
    //抽十次
    public List<HeroLocalItem> GetRandomHero10()
    {
        List<HeroLocalItem> newHeroLocalData = new();
        for (int i = 0; i < 10; i++)
        {
            HeroLocalItem newHero = GetRandomHero();
            newHeroLocalData.Add(newHero);
        }
        return newHeroLocalData;
    }

}
