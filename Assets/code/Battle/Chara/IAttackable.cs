using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable
{
    int hp { get; set; }
    void ApplyDamage(int damage);
}
