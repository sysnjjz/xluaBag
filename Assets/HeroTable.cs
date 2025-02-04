using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Table/HeroTable", fileName = "HeroTable")]
public class HeroTable : ScriptableObject
{
    public List<Hero> HeroList = new List<Hero>();
}
