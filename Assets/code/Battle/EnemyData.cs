using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public int strengh;
    public int hp;
    public float speed;
    public int attack_scope;
    public float attack_frequency;
    public int wait;
}
[System.Serializable]
public class EnemyDataList
{
    public List<EnemyData> enemies;
}