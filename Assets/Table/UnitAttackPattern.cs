using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAttackPattern", menuName = "AttackPattern/UnitAttackPattern")]
public class UnitAttackPattern : ScriptableObject
{
    public List<string> attackSequence;
}
