///
/// 游戏管理器 主要是用于统一管理游戏中的数据
///
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    //单例
    private static GameManager instance;
    //英雄设定表
    private HeroTable heroTable;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UIManager.Instance.OpenPanel(UIConst.MainMenu);
    }

    /// 拿数据
    //加载静态数据
    public HeroTable GetHeroTable()
    {
        if (heroTable == null)
        {
            heroTable = Resources.Load<HeroTable>("Table/HeroTable");
        }
        return heroTable;
    }

    //加载动态数据
    public List<HeroLocalItem> GetHeroLocalData()
    {
        //LoadGame();
        //return HeroLocalData.Instance.LocalDataList;
        return HeroLocalData.Instance.LoadData();

    }

    //根据id拿到指定静态数据
    public Hero GetHeroById(int id)
    {
        List<Hero> HeroList = GetHeroTable().HeroList;
        foreach(Hero hero in HeroList)
        {
            if(hero.id==id)
            {
                return hero;
            }
        }
        return null;
    }

    //根据uid拿到指定动态数据
    public HeroLocalItem GetHeroLocalDataByUId(string uid)
    {
        List<HeroLocalItem> HeroLocalDataList = GetHeroLocalData();
        foreach(HeroLocalItem HeroLocalData in HeroLocalDataList)
        {
            if(HeroLocalData.uid==uid)
            {
                return HeroLocalData;
            }
        }
        return null;
    }

    //得到上阵英雄数据
    public Dictionary<int, HeroLocalItem> GetDeployHeroDic()
    {
        //LoadGame();
        //return DeployedHero.Instance.DeployHeroesDic;
        return DeployedHero.Instance.LoadDeployData();
    }

    //添加上阵英雄
    public int AddDeployHero(int DeployID, HeroLocalItem AddHero)
    {
        Dictionary<int, HeroLocalItem> DeployHeroes = GetDeployHeroDic();
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

    //抽卡逻辑
    //抽一次
    public HeroLocalItem GetRandomHero()
    {
        int index = UnityEngine.Random.Range(0, GetHeroTable().HeroList.Count);
        Hero newHero = GetHeroTable().HeroList[index];
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
        for(int i=0;i<10;i++)
        {
            HeroLocalItem newHero = GetRandomHero();
            newHeroLocalData.Add(newHero);
        }
        return newHeroLocalData;
    }

    ///获得排好序的数据，显示在滚动框中，这里是按星级和id排序，这个id排序是为了有更多英雄拓展用的
    public List<HeroLocalItem> GetSortHeroLocalData()
    {
        List<HeroLocalItem> HeroLocalDatas = HeroLocalData.Instance.LoadData();
        if(HeroLocalDatas.Count==0)
        {
            Debug.Log("CANNOT GET DATA");
            return null;
        }
        HeroLocalDatas.Sort(new HeroItemComparer());
        return HeroLocalDatas;
    }

    //保存游戏
    public Save CreateSave()
    {
        Save save = new Save();
        save.HeroLocalDataSave = HeroLocalData.Instance.LocalDataList;
        save.DeployHeroesSave = DeployedHero.Instance.DeployHeroesDic;

        return save;
    }

    public void SaveGame()
    {
        Save save = CreateSave();

        // 写文件
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log(save.HeroLocalDataSave[1]);
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            //读文件
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();


            //赋值
            HeroLocalData.Instance.LocalDataList = save.HeroLocalDataSave;
            DeployedHero.Instance.DeployHeroesDic = save.DeployHeroesSave;
        }
        else
        {
            Debug.Log("No game saved!");
        }
        Debug.Log(HeroLocalData.Instance.LocalDataList[1]);
    }
}

public class HeroItemComparer:IComparer<HeroLocalItem>
{
    public int Compare(HeroLocalItem a,HeroLocalItem b)
    {
        Hero x = GameManager.Instance.GetHeroById(a.id);
        Hero y = GameManager.Instance.GetHeroById(b.id);

        //按照稀有度排序
        int rarityComparision=y.rarity.CompareTo(x.rarity);

        //同级英雄按id排序
        if(rarityComparision==0)
        {
            int idComparision=y.id.CompareTo(x.id);
            return idComparision;
        }

        return rarityComparision;
    }
}
