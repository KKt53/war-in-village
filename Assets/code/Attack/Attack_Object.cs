using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Attack_Object : MonoBehaviour
{
    public int attack_point;//�U����
    public List<string> features_point;//������₷�����
    public void Initialize(int c_attack_point, List<string> c_features_point)
    {
        attack_point = c_attack_point;
        features_point = c_features_point;
    }
}
