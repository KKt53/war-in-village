using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class Special_Gravity : MonoBehaviour
{
    public bool skill_flag = false;

    public Button targetButton; // �Ώۂ̃{�^��
    public float cooldownTime = 30.0f; // �N�[���_�E�����ԁi�b�j

    private float timer;
    private bool isOnCooldown;

    public GameObject canvas;

    public GameObject sq;

    GameObject sq_instance;

    GameObject boss;//�{�X�p�ϐ�

    GameObject[] Enemy;//�G�F��

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

            sq_instance = Instantiate(sq, canvas.transform);
            RectTransform rectTransform = sq_instance.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(30, -250);
        }
    }

    void StartCooldown()
    {
        isOnCooldown = true;
        timer = cooldownTime;
        if (targetButton != null)
        {
            targetButton.interactable = false; // �{�^���������Ȃ�����
        }
    }

    void ResetCooldown()
    {
        isOnCooldown = false;
        timer = 0f;
        if (targetButton != null)
        {
            targetButton.interactable = true; // �{�^�����Ăщ�����悤�ɂ���
        }
    }
}
