using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Special_Gravity : MonoBehaviour
{
    public bool skill_flag = false;

    public Button targetButton; // 対象のボタン
    public float cooldownTime = 30.0f; // クールダウン時間（秒）

    private float timer;
    private bool isOnCooldown;

    GameObject boss;//ボス用変数

    GameObject[] Enemy;//敵認識

    void Start()
    {
        boss = GameObject.Find("Boss");

        if (targetButton != null)
        {
            targetButton.onClick.AddListener(OnButtonClick);
        }
        ResetCooldown();
    }

    void Update()
    {
        if (isOnCooldown)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                ResetCooldown();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnButtonClick();
        }
    }

    void OnButtonClick()
    {
        if (!isOnCooldown)
        {
            IAttackable attackable = boss.GetComponent<IAttackable>();
            attackable.ApplyDamage(10);
            Debug.Log("gravity");

            Enemy = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject ally in Enemy)
            {
                if (ally)
                {
                    attackable = ally.GetComponent<IAttackable>();
                    attackable.ApplyDamage(10);
                }
            }

            skill_flag = true;
            StartCooldown();
        }
    }

    void StartCooldown()
    {
        isOnCooldown = true;
        timer = cooldownTime;
        if (targetButton != null)
        {
            targetButton.interactable = false; // ボタンを押せなくする
        }
    }

    void ResetCooldown()
    {
        isOnCooldown = false;
        timer = 0f;
        if (targetButton != null)
        {
            targetButton.interactable = true; // ボタンを再び押せるようにする
        }
    }
}
