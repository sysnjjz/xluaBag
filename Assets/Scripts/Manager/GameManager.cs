using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

///
/// 游戏管理器 游戏入口 暂时把存储也写在里面
///
public class GameManager : MonoBehaviour
{
    //单例
    private static GameManager instance;

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

    //方法
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        UIManager.Instance.OpenPanel(UIConst.MainMenu);
    }

    //保存游戏
    public Save CreateSave()
    {
        Save save = new Save();
        save.HeroLocalDataSave = HeroLocalData.Instance.LocalDataList;
        save.DeployHeroesSave = HeroDeployedData.Instance.DeployHeroesDic;

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
            HeroDeployedData.Instance.DeployHeroesDic = save.DeployHeroesSave;
        }
        else
        {
            Debug.Log("No game saved!");
        }
        Debug.Log(HeroLocalData.Instance.LocalDataList[1]);
    }
}
