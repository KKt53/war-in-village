using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Attack_Object : MonoBehaviour
{
    public float attack_point;//攻撃力
    public List<string> features_point;//かかりやすい状態
    public void Initialize(float c_attack_point, List<string> c_features_point)
    {
        attack_point = c_attack_point;
        features_point = c_features_point;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
