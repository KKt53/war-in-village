using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public int hp;
    public float speed;
    public float attack_scope;
    public int attack_frequency;
    public int wait;
}
[System.Serializable]
public class EnemyDataList
{
    public List<EnemyData> enemies;
}