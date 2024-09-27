using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossAttackPattern", menuName = "B_AttackPattern/BossAttackPattern")]
public class BossAttackPattern : ScriptableObject
{
    public List<string> b_attacksequence;
}
