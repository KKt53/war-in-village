using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Villager : MonoBehaviour
{
    private float strengh;
    private float speed;
    private float reaction_rate;
    private float attack_frequency;
    private float size;
    private float attack_scope;

    private int direction_flag = 1;

    // キャラクターの移動速度を引数で設定
    public void Initialize(float c_strengh, float c_speed, float c_reaction_rate, float c_attack_frequency, float c_size, float c_attack_scope)
    {
        strengh = c_strengh;
        speed = c_speed;
        reaction_rate = c_reaction_rate;
        attack_frequency = c_attack_frequency;
        size = c_size;
        attack_scope = c_attack_scope;
    }

    void Update()
    {
        GameObject boss = GameObject.FindWithTag("Boss");

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction_flag = -1;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction_flag = 1;
        }

        Vector3 enemyPosition = boss.transform.position;

        if (transform.position.x + attack_scope <= boss.transform.position.x)
        {
            // 横方向にまっすぐ進む動作
            transform.Translate(Vector2.right * direction_flag * speed * Time.deltaTime);
        }

        
    }
}
