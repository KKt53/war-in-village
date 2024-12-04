using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Special_Guard : MonoBehaviour
{
    public bool skill_flag = false;

    public Button targetButton; // �Ώۂ̃{�^��
    public float cooldownTime = 20.0f; // �N�[���_�E�����ԁi�b�j

    private float timer;
    private bool isOnCooldown;

    void Start()
    {
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
    }

    void OnButtonClick()
    {
        if (!isOnCooldown)
        {
            Debug.Log("guard");
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
