using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public List<HeroLocalItem> HeroLocalDataSave;
    public Dictionary<int, HeroLocalItem> DeployHeroesSave;
}
