using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Special_Step : MonoBehaviour
{
    public bool skill_flag = false;

    public Button targetButton; // 対象のボタン
    public float cooldownTime = 15.0f; // クールダウン時間（秒）

    private float timeofeffect = 3.0f;
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
            Debug.Log("step");
            skill_flag = true;
            StartCooldown();
            StartCoroutine(effect_time());
        }
    }
    IEnumerator effect_time()
    {
        yield return new WaitForSeconds(timeofeffect);
        skill_flag = false;
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
